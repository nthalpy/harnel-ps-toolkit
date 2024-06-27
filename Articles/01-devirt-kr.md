# Devirtualization

## Introduction

PS를 하다 보면 자연스럽게 특정 알고리즘들을 구현하는 템플릿을 작성하게 되는 날이 옵니다. 구간 합을 구하는 세그먼트 트리를 템플릿으로 만든다고 생각해봅시다.

```cs
public sealed class SumSeg
{
    private long[] _tree;
    private int _leafMask;

    public SumSeg(int size)
    {
        _leafMask = (int)BitOperations.RoundUpToPowerOf2((uint)size);
        var treeSize = _leafMask << 1;

        _tree = new long[treeSize];
    }

    public void Update(int index, long val)
    {
        var curr = _leafMask | index;
        var diff = val - _tree[curr];

        while (curr != 0)
        {
            _tree[curr] += diff;
            curr >>= 1;
        }
    }

    public long Range(int stIncl, int edExcl)
    {
        var leftNode = _leafMask | stIncl;
        var rightNode = _leafMask | (edExcl - 1);

        var aggregated = 0L;

        while (leftNode <= rightNode)
        {
            if ((leftNode & 1) == 1)
                aggregated += _tree[leftNode++];
            if ((rightNode & 1) == 0)
                aggregated += _tree[rightNode--];

            leftNode >>= 1;
            rightNode >>= 1;
        }

        return aggregated;
    }
}
```

상당히 잘 최적화된 비재귀 세그먼트 트리입니다. 덧셈에 역원이 있다는 것을 이용해서 point update를 수행할 때 불필요한 자식 노드 접근을 최소화하고, 항등원이 있다는 것을 이용해서 range query를 수행할 때 불필요한 조건 분기를 최소화한 구조입니다. 그러나 이 코드는 다른 비슷한 세그먼트 트리를 만들려면 코드 대부분을 복사한 뒤 연산하는 로직을 갈아끼워야 한다는 단점이 있습니다.

이를 해결하는 방법 중 하나는 다형성을 사용하는 것입니다. 아래의 코드처럼 변화량을 계산해서 적용하는 방식을 택하면 연산자 (덧셈, 곱셈, xor, ...) 에 따라서 필요한 메서드는 4가지로 추려지고 다음과 같이 추상화할 수 있습니다:

```cs
public abstract class AbelianGroupSegTree<TElement, TUpdate, TDiff>
    where TElement : struct
{
    protected TElement[] _tree;
    protected int _leafMask;

    public AbelianGroupSegTree(int size)
    {
        _leafMask = (int)BitOperations.RoundUpToPowerOf2((uint)size);
        var treeSize = _leafMask << 1;

        _tree = new TElement[treeSize];
    }

    public void Update(int index, TUpdate val)
    {
        var curr = _leafMask | index;
        var diff = CreateDiff(_tree[curr], val);

        while (curr != 0)
        {
            _tree[curr] = ApplyDiff(_tree[curr], diff);
            curr >>= 1;
        }
    }

    public TElement Range(int stIncl, int edExcl)
    {
        var leftNode = _leafMask | stIncl;
        var rightNode = _leafMask | (edExcl - 1);

        var aggregated = Identity();

        while (leftNode <= rightNode)
        {
            if ((leftNode & 1) == 1)
                aggregated = Merge(aggregated, _tree[leftNode++]);
            if ((rightNode & 1) == 0)
                aggregated = Merge(aggregated, _tree[rightNode--]);

            leftNode >>= 1;
            rightNode >>= 1;
        }

        return aggregated;
    }

    protected abstract TElement Identity();
    protected abstract TDiff CreateDiff(TElement element, TUpdate val);
    protected abstract TElement ApplyDiff(TElement element, TDiff diff);
    protected abstract TElement Merge(TElement l, TElement r);
}
```

그리고 다음과 같이 구간 합을 계산하는 세그먼트 트리를 빠르고 편하게 구현할 수 있습니다.

```cs
public sealed class SealedSumSeg : AbelianGroupSegTree<long, long, long>
{
    public SealedSumSeg(int size) : base(size)
    {
    }

    protected override long ApplyDiff(long element, long diff) => element + diff;
    protected override long CreateDiff(long element, long val) => val - element;
    protected override long Identity() => 0;
    protected override long Merge(long l, long r) => l + r;
}
```

그런데 여기서 문제가 발생합니다. .NET Core 6.0과 7.0을 기준으로 위의 `NonGenericSumSeg`와 `SealedSumSeg` 사이에는 약 1.2배의 성능 차이가 존재합니다. 다른 개발이었다면 유지보수의 편함을 위해서 실행시간의 증가를 타협했겠지만, PS에서 실행 시간을 타협하기란 쉽지 않은 일입니다. 뭔가 좋은 방법이 없을까요?

이 문서에서는 .NET의 내부 구조와 JIT compile, devirtualization 등을 설명합니다. 이 내용은 런타임마다 구현이 다르므로 다음과 같은 3가지 런타임에 대해서 분석하고자 합니다:

- .NET 6.0: Codeforces와 제가 자주 사용하는 online judge "Baekjoon Online Judge"가 사용하는 런타임입니다.
- .NET 7.0: AtCoder가 사용하는 런타임입니다.
- .NET 8.0: 가장 최신 런타임입니다.

또, 모든 분석은 Windows 10, AMD Ryzen 7 3700X에서 이뤄졌고 disassemble과 .NET 내부 객체 조회는 WinDbg와 SOS를 사용했습니다.

## Virtual everywhere (.NET 6.0, .NET 7.0)

_Virtual_, 참 매혹적인 단어입니다. Virtual method, virtual class, virtual reality, virtual youtuber 등등, 대세는 virtual이라고 해도 과언이 아닙니다. Virtual method를 사용하면 그 객체의 타입에 따라서 다르게 구현된 메서드를 호출할 수 있어서 다형성을 굉장히 편하게 구현할 수 있습니다.

그러나 모든 일에는 대가가 따르는 법. Virtual method를 내부적으로 구현하기 위해서는 `this`가 가리키는 객체가 무슨 타입인지와 어떤 메서드 본문을 가지는지 알아야만 합니다. .NET은 이 문제를 타입과 관련된 모든 정보를 갖고 있는 singleton 객체인 "메서드 테이블"이란 개념을 통해서 해결합니다. 실제 JIT compile 된 결과는 다음과 같습니다:

```asm
; _tree[curr] = ApplyDiff(_tree[curr], diff);
00007ffd`7107d1bd 4863d7       movsxd  rdx, edi
00007ffd`7107d1c0 488b54d110   mov     rdx, qword ptr [rcx+rdx*8+10h]
00007ffd`7107d1c5 488bce       mov     rcx, rsi
00007ffd`7107d1c8 488b06       mov     rax, qword ptr [rsi]
00007ffd`7107d1cb 488b5840     mov     rbx, qword ptr [rax+40h]
00007ffd`7107d1cf ff5328       call    qword ptr [rbx+28h]
00007ffd`7107d1d2 488be8       mov     rbp, rax
```

메서드 테이블은 `rsi`에 위치하게 되는데, 어떤 메서드 본문을 호출해야 하는지 알아내기 위해서 3번의 dereferencing을 해야 하며 함수 호출의 오버헤드도 발생합니다. 이 부분에서 발생한 오버헤드로 인해서 퍼포먼스 하락이 발생하게 됩니다.

Virtual method를 통해서 구현하면 오버헤드가 발생한다는 사실이 확인되었습니다. 뭔가 다른 좋은 방법이 없을까요?

## Devirtualization by Generic  (.NET 6.0, .NET 7.0)

AtCoder Library는 여기서 기막힌 아이디어를 하나 사용합니다. 바로 제네릭을 통해서 devirtualization을 노리는 것입니다. 정확히는 아래와 같이 추상화할 연산을 모아둔 interface와 그 interface를 구현하는 struct를 구현하고, type parameter를 통해서 세그먼트 트리에 추상화된 연산 정보를 전달하는 것입니다.

```cs
public interface IAbelianGroupSegOp<TElement, TUpdate, TDiff>
{
    TElement Identity();
    TDiff CreateDiff(TElement element, TUpdate val);
    TElement ApplyDiff(TElement element, TDiff diff);
    TElement Merge(TElement l, TElement r);
}

public struct SumSegOp : IAbelianGroupSegOp<long, long, long>
{
    public long ApplyDiff(long element, long diff) => element + diff;
    public long CreateDiff(long element, long val) => val - element;
    public long Identity() => 0;
    public long Merge(long l, long r) => l + r;
}

// 사용할 때 ACLStyleAbelianGroupSegTree<long, long, long, SumSegOp> 로 사용
public class ACLStyleAbelianGroupSegTree<TElement, TUpdate, TDiff, TOp>
    where TElement : struct
    where TUpdate : struct
    where TDiff : struct
    where TOp : struct, IAbelianGroupSegOp<TElement, TUpdate, TDiff>
{
    private TOp _op = default;

    public void Update(int index, TUpdate val)
    {
        var curr = _leafMask | index;
        var diff = _op.CreateDiff(_tree[curr], val);

        while (curr != 0)
        {
            _tree[curr] = _op.ApplyDiff(_tree[curr], diff);
            curr >>= 1;
        }
    }
}
```

이렇게 하면, 무려 virtual call이 발생하지 않고 불필요한 dereferencing을 줄일 수 있으며, 어떤 메서드를 호출할지가 확실하기 때문에 `ApplyDiff`의 크기에 따라서는 method inlining을 할 수도 있게 됩니다. 대체 어떻게 이런 일이 가능한 걸까요?

이를 알기 위해서는 generic class/method의 JIT compile 방법에 대해 알아야 합니다. Generic class/method는 다음과 같은 기준에 맞춰서 JIT compile 된 본문을 사용하게 되어있습니다:

- 각 type parameter에 대해서...
    - Type parameter가 값 타입인 경우 해당 타입을 그대로 사용
    - Type parameter가 참조 타입인 경우 `System.__Canon`으로 대체

이는 값 타입은 메모리 구조가 통일되어 있지 않아 메모리 오프셋 계산을 전부 따로 해야 하지만, 참조 타입은 통일된 메모리 구조를 가져 동일한 메모리 오프셋 계산을 사용할 수 있기 때문입니다. 그래서 실제로 메서드 테이블을 참조해 보면 다음과 같이 별도의 메서드 테이블로 구성되었음을 알 수 있습니다.

(!DumpMT의 원문은 텍스트가 너무 길어져서 자명한 namespace 등은 생략했습니다.)

```plain
0:000> !DumpMT -md 00007ffd56826008
EEClass:             00007ffd56812388
Module:              00007ffd56804d78
Name:                ACLStyleSumSeg+ACLStyleAbelianGroupSegTree`4[long,long,long,SumSegOp]
mdToken:             0000000002000013
File:                ProblemSolving.TestInterfaces.dll
AssemblyLoadContext: Default ALC - The managed instance of this context doesn't exist yet.
BaseSize:            0x28
ComponentSize:       0x0
DynamicStatics:      false
ContainsPointers:    true
Slots in VTable:     10
Number of IFaces in IFaceMap: 0
--------------------------------------
MethodDesc Table
           Entry       MethodDesc    JIT Name
00007FFD56700030 00007ffd566f5618   NONE System.Object.Finalize()
00007FFD56700038 00007ffd566f5628   NONE System.Object.ToString()
00007FFD56700040 00007ffd566f5638   NONE System.Object.Equals(System.Object)
00007FFD56700058 00007ffd566f5678   NONE System.Object.GetHashCode()
00007FFD5671AC18 00007ffd56825f90    JIT ACLStyleSumSeg+ACLStyleAbelianGroupSegTree`4[long,long,long,SumSegOp]..ctor(Int32)
00007FFD5671AC20 00007ffd56825fa0   NONE ACLStyleSumSeg+ACLStyleAbelianGroupSegTree`4[long,long,long,SumSegOp].get_AllRange()
00007FFD5671AC28 00007ffd56825fb0   NONE ACLStyleSumSeg+ACLStyleAbelianGroupSegTree`4[long,long,long,SumSegOp].ElementAt(Int32)
00007FFD5671AC30 00007ffd56825fc0   NONE ACLStyleSumSeg+ACLStyleAbelianGroupSegTree`4[long,long,long,SumSegOp].Init(System.Collections.Generic.IList`1)
00007FFD5671AC38 00007ffd56825fd0    JIT ACLStyleSumSeg+ACLStyleAbelianGroupSegTree`4[long,long,long,SumSegOp].Update(Int32, Int64)
00007FFD5671AC40 00007ffd56825fe0    JIT ACLStyleSumSeg+ACLStyleAbelianGroupSegTree`4[long,long,long,SumSegOp].Range(Int32, Int32)
```

메서드 테이블이 다르기 때문에 메서드 본문 역시 다르게 되는데, 이 지점에서 서로 다른 최적화를 적용할 수 있는 경우가 있습니다. 실제로 `ApplyDiff`를 호출하는 부분을 보면 다음과 같이 최적화되었음을 알 수 있습니다.

```asm
; _tree[curr] = ApplyDiff(_tree[curr], diff);
00007ffd`5671d23b 4c63da          movsxd  r11,edx
00007ffd`5671d23e 4f8b54da10      mov     r10,qword ptr [r10+r11*8+10h]
00007ffd`5671d243 4d03d0          add     r10,r8
00007ffd`5671d246 4e8954d910      mov     qword ptr [rcx+r11*8+10h],r10
```

## Benchmarks (.NET 6.0, .NET 7.0, .NET 8.0)

이론적인 이해를 완료했으니 이제 벤치마크를 돌려볼 차례입니다. 벤치마크는 BenchmarkDotNet을 통해서 수행되었고, 크기 100000의 세그먼트 트리에서 100000번의 point update/range query를 수행하는 코드로 작성되었으며, 500번씩 실행한 것을 4차례 실행해 통계를 냈습니다.

그 결과는 다음과 같습니다:

```plain
// * Summary *

BenchmarkDotNet v0.13.12, Windows 10 (10.0.19045.4412/22H2/2022Update)
AMD Ryzen 7 3700X, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.301
  [Host]               : .NET 6.0.26 (6.0.2623.60508), X64 RyuJIT AVX2
  VeryLongRun-.NET 6.0 : .NET 6.0.26 (6.0.2623.60508), X64 RyuJIT AVX2
  VeryLongRun-.NET 7.0 : .NET 7.0.15 (7.0.1523.57226), X64 RyuJIT AVX2
  VeryLongRun-.NET 8.0 : .NET 8.0.6 (8.0.624.26715), X64 RyuJIT AVX2

InvocationCount=1  IterationCount=500  LaunchCount=4  
UnrollFactor=1  WarmupCount=30  

| Method           | Job                  | Runtime  | Mean      | StdDev    | Ratio |
|----------------- |--------------------- |--------- |----------:|----------:|------:|
| ACLStyleSumSeg   | VeryLongRun-.NET 6.0 | .NET 6.0 | 10.173 ms | 0.5054 ms |  1.00 |
| NonGenericSumSeg | VeryLongRun-.NET 6.0 | .NET 6.0 | 10.192 ms | 0.5680 ms |  1.00 |
| NonSealedSumSeg  | VeryLongRun-.NET 6.0 | .NET 6.0 | 12.470 ms | 0.5861 ms |  1.23 |
| SealedSumSeg     | VeryLongRun-.NET 6.0 | .NET 6.0 | 12.054 ms | 0.3557 ms |  1.19 |
|                  |                      |          |           |           |       |
| ACLStyleSumSeg   | VeryLongRun-.NET 7.0 | .NET 7.0 |  9.586 ms | 0.2298 ms |  1.01 |
| NonGenericSumSeg | VeryLongRun-.NET 7.0 | .NET 7.0 |  9.474 ms | 0.1830 ms |  1.00 |
| NonSealedSumSeg  | VeryLongRun-.NET 7.0 | .NET 7.0 | 11.881 ms | 0.1997 ms |  1.25 |
| SealedSumSeg     | VeryLongRun-.NET 7.0 | .NET 7.0 | 12.171 ms | 0.5539 ms |  1.28 |
|                  |                      |          |           |           |       |
| ACLStyleSumSeg   | VeryLongRun-.NET 8.0 | .NET 8.0 |  8.080 ms | 0.3175 ms |  1.01 |
| NonGenericSumSeg | VeryLongRun-.NET 8.0 | .NET 8.0 |  8.019 ms | 0.2649 ms |  1.00 |
| NonSealedSumSeg  | VeryLongRun-.NET 8.0 | .NET 8.0 |  8.226 ms | 0.2678 ms |  1.03 |
| SealedSumSeg     | VeryLongRun-.NET 8.0 | .NET 8.0 |  8.339 ms | 0.4166 ms |  1.04 |
```

.NET 6.0과 .NET 7.0에는 예상한 대로의 결과가 나왔습니다. AtCoder Library 스타일로 devirtualization을 꾀한 코드는 비 제네릭 세그먼트 트리와 0.5σ 안쪽의 차이로 거의 동일한 퍼포먼스를 보여주고 있고, virtual method를 사용한 제네릭 세그먼트 트리는 4σ 이상의 명백한 퍼포먼스 저하를 보여주고 있습니다.

그런데, .NET 8.0에서는 비슷한 퍼포먼스를 보여주고 있습니다. 심지어 sealed를 붙이지 않아도 비슷한 퍼포먼스를 보여주고 있습니다. 이게 대체 어떻게 된 일일까요?

## Guarded devirtualization (.NET 8.0)

직관에 반하는 결과를 제쳐두고 침착하게 .NET 8.0의 런타임으로 실행해서 덤프를 살펴봅시다.

```plain
0:000> !DumpMT -md 00007ffd6d5c5000
EEClass:             00007ffd6d5adf20
Module:              00007ffd6d5c4860
Name:                AbelianGroupSegTree`3[long,long,long]
mdToken:             0000000002000013
File:                ProblemSolving.Templates.dll
AssemblyLoadContext: Default ALC - The managed instance of this context doesn't exist yet.
BaseSize:            0x20
ComponentSize:       0x0
DynamicStatics:      false
ContainsPointers:    true
Slots in VTable:     14
Number of IFaces in IFaceMap: 0
--------------------------------------
MethodDesc Table
           Entry       MethodDesc    JIT Name
00007FFD6D3C0048 00007ffd6d3b5f38   NONE System.Object.Finalize()
00007FFD6D3C0060 00007ffd6d3b5f48   NONE System.Object.ToString()
00007FFD6D3C0078 00007ffd6d3b5f58   NONE System.Object.Equals(System.Object)
00007FFD6D3C00C0 00007ffd6d3b5f98   NONE System.Object.GetHashCode()
00007FFD6D5BBA08 00007ffd6d5c4fa8   NONE AbelianGroupSegTree`3[long,long,long].Identity()
00007FFD6D5BBA20 00007ffd6d5c4fb8   NONE AbelianGroupSegTree`3[long,long,long].CreateDiff(Int64, Int64)
00007FFD6D5BBA38 00007ffd6d5c4fc8   NONE AbelianGroupSegTree`3[long,long,long].ApplyDiff(Int64, Int64)
00007FFD6D5BBA50 00007ffd6d5c4fd8   NONE AbelianGroupSegTree`3[long,long,long].Merge(Int64, Int64)
00007FFD6D5BB978 00007ffd6d5c4f48    JIT AbelianGroupSegTree`3[long,long,long]..ctor(Int32)
00007FFD6D5BB990 00007ffd6d5c4f58   NONE AbelianGroupSegTree`3[long,long,long].get_AllRange()
00007FFD6D5BB9A8 00007ffd6d5c4f68   NONE AbelianGroupSegTree`3[long,long,long].ElementAt(Int32)
00007FFD6D5BB9C0 00007ffd6d5c4f78   NONE AbelianGroupSegTree`3[long,long,long].Init(System.Collections.Generic.IList`1)
00007FFD6D5BB9D8 00007ffd6d5c4f88    JIT AbelianGroupSegTree`3[long,long,long].Update(Int32, Int64)
00007FFD6D5BB9F0 00007ffd6d5c4f98    JIT AbelianGroupSegTree`3[long,long,long].Range(Int32, Int32)

0:000> !DumpMD /d 00007ffd6d5c4f88
Method Name:          AbelianGroupSegTree`3[long,long,long].Update(Int32, Int64)
Class:                00007ffd6d5adf20
MethodTable:          00007ffd6d5c5000
mdToken:              0000000006000058
Module:               00007ffd6d5c4860
IsJitted:             yes
Current CodeAddr:     00007ffd6d4848a0
Version History:
  ILCodeVersion:      0000000000000000
  ReJIT ID:           0
  IL Addr:            000001d816c23ad4
     CodeAddr:           00007ffd6d483d40  (QuickJitted + Instrumented)
     NativeCodeVersion:  00000218AB911A30
     CodeAddr:           00007ffd6d4848a0  (OptimizedTier1)
     NativeCodeVersion:  00000218AB912530
     CodeAddr:           00007ffd6d482750  (QuickJitted)
     NativeCodeVersion:  0000000000000000
```

자세히 보면 JIT compile이 3번 발생한 것을 알 수 있습니다. 가장 위에 있는 "QuickJitted + Instrumented" 의 결과를 사용하게 되는데, 이는 런타임에 코드가 실행되면서 런타임에서 삽입한 여러 instrumentation 코드를 통해서 모은 정보를 토대로 최적화된 코드입니다. 이렇게 instrumentation을 통해서 코드를 최적화하는 것을 profile-guided optimization이라 하고, 런타임에 동적으로 삽입된 instrumentation을 통해서 최적화가 진행되면 그것을 dynamic profile-guided optimization (dynamic PGO) 라고 합니다.

.NET 8.0 부터는 간편하게 JIT compile 관련 정보를 볼 수 있는데 확인해 보면 `while` 루프가 다음과 같이 JIT compile 되었음을 알 수 있습니다.

```asm
; Assembly listing for method ProblemSolving.Templates.SegmentTree.AbelianGroupSegTree`3[long,long,long]:Update(int,long):this (Tier1)
; Emitting BLENDED_CODE for X64 with AVX - Windows
; Tier1 code
; optimized code
; optimized using Dynamic PGO
; rsp based frame
; fully interruptible
; with Dynamic PGO: edge weights are invalid, and fgCalledCount is 6085
; 0 inlinees with PGO data; 2 single block inlinees; 0 inlinees without PGO data

G_M000_IG05:                ;; offset=0x0049
       mov      rcx, gword ptr [rbx+0x08]
       mov      r15, rcx
       mov      edx, dword ptr [rcx+0x08]
       cmp      esi, edx
       jae      SHORT G_M000_IG12
       mov      edx, esi
       mov      rax, qword ptr [rcx+8*rdx+0x10]
       add      rax, r14
       mov      qword ptr [r15+8*rdx+0x10], rax
       sar      esi, 1
       jne      SHORT G_M000_IG05

G_M000_IG06:                ;; offset=0x006A
       jmp      SHORT G_M000_IG09

G_M000_IG08:                ;; offset=0x0087
       cmp      esi, dword ptr [r15+0x08]
       jae      SHORT G_M000_IG12
       mov      ecx, esi
       mov      qword ptr [r15+8*rcx+0x10], rax
       sar      esi, 1
       jne      SHORT G_M000_IG07

G_M000_IG11:                ;; offset=0x00B7
       mov      rcx, rbx
       mov      rdx, rax
       mov      r8, r14
       mov      rax, qword ptr [rbx]
       mov      rax, qword ptr [rax+0x40]
       call     [rax+0x30]AbelianGroupSegTree`3[long,long,long]:ApplyDiff(long,long):long:this
       jmp      SHORT G_M000_IG08

G_M000_IG12:                ;; offset=0x00CC
       call     CORINFO_HELP_RNGCHKFAIL
       int3

; Total bytes of code 210
```

중간에 메서드 테이블과 비교되는 값 `0x7FFD6D5A5180`는 concrete class `SumSeg` 의 메서드 테이블이고, 이를 토대로 코드가 다음과 같이 최적화되었음을 알 수 있습니다:

- `this`가 `SumSeg` 인지 체크
    - 만약 `this`가 `SumSeg`라면 `SumSeg.ApplyDiff` 본문을 직접 호출 (여기서는 inline 됨)
    - 그렇지 않다면 `G_M000_IG11`로 이동 후 메서드 테이블을 참조해 virtual call을 수행

이를 통해서 if 문 하나를 추가한 대신 devirtualization을 노릴 수 있습니다. 벤치마크 시나리오에서는 `SumSeg` 하나의 타입만 사용했으므로 branch prediction이 아주 잘 작동했을 것이며, 0.2ms의 시간 차이밖에 나지 않은 것입니다. 이와 같이 type을 체크해 devirtualization을 하는 것을 "Guarded devirtualization" 이라고 합니다.

## Further readings

이 글에 나온 내용들에 대해서 더 알고 싶은 분들을 위한 링크 모음집입니다.

### [Harnel's Problem-Solving Toolkits](https://github.com/nthalpy/harnel-ps-toolkit)

이 글에 나온 벤치마크를 포함해서 다양한 자잘한 코드가 있는 저장소입니다. 귀찮음으로 인해서 커밋 로그가 엉망인 건 너그러운 마음으로 봐주셨으면 합니다.

### [kzrnm/atcoder-library-csharp](https://github.com/kzrnm/ac-library-csharp)

C#으로 포팅된 AtCoder Library 입니다. 늘 감사히 읽으면서 최적화 거리를 떠올리고 있습니다.

### [Performance Improvements in .NET 8](https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-8/)

이 글에 나온 JIT compile, devirtualization, dynamic PGO, guarded devirtualization 등 대부분의 주제가 포함되어 있는 문서입니다. .NET Core 2.0 시절부터 이어져 온 유구한 전통의 문서입니다. 이 글을 읽고 재미를 느끼셨을 독자분이라면 Performance Improvements in .NET 8 역시 즐겁게 읽으실 것이라 믿어 의심치 않습니다.

### [WinDbg](https://learn.microsoft.com/ko-kr/windows-hardware/drivers/debugger/)

Assembly 레벨이나 .NET 내부 구조를 살펴봐야 할 때 제가 사용하는 디버거인 WinDbg입니다. SOS와 조합하면 정말 편하게 내부 구조를 살펴볼 수 있습니다.
