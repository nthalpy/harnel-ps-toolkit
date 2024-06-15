using Microsoft.CodeAnalysis.CSharp.Syntax;
using ProblemSolving.Templates.Merger;
using System;

namespace ProblemSolving.Templates
{
    [IncludeIfReferenced]
    public class Deque<T>
    {
        public bool IsEmpty => Count == 0;
        public int Count => _count;

        public T Front => _arr[_stIncl];
        public T Back => _edExcl == 0 ? _arr[_arr.Length - 1] : _arr[_edExcl - 1];

        private int _stIncl;
        private int _edExcl;
        private int _count;

        private T[] _arr;

        public Deque()
        {
            _arr = new T[1024];
        }

        public T this[int idx]
        {
            get => _arr[(_stIncl + idx) % _arr.Length];
            set => _arr[(_stIncl + idx) % _arr.Length] = value;
        }

        public void PushFront(T v)
        {
            EnsureSize(1 + _count);
            _count++;

            _stIncl--;
            if (_stIncl == -1)
                _stIncl = _arr.Length - 1;

            _arr[_stIncl] = v;
        }
        public T PopFront()
        {
            if (_count == 0)
                throw new InvalidOperationException();

            _count--;

            var val = _arr[_stIncl];

            _stIncl++;
            if (_stIncl == _arr.Length)
                _stIncl = 0;

            return val;
        }
        public void PushBack(T v)
        {
            EnsureSize(1 + _count);
            _count++;

            _arr[_edExcl] = v;

            _edExcl++;
            if (_edExcl == _arr.Length)
                _edExcl = 0;
        }
        public T PopBack()
        {
            if (_count == 0)
                throw new InvalidOperationException();

            _count--;

            _edExcl--;
            if (_edExcl == -1)
                _edExcl += _arr.Length;

            return _arr[_edExcl];
        }

        private void EnsureSize(int size)
        {
            if (1 + _arr.Length == size)
            {
                var newarr = new T[2 * _arr.Length];

                Array.Copy(_arr, _stIncl, newarr, 0, _arr.Length - _stIncl);
                Array.Copy(_arr, 0, newarr, _arr.Length - _stIncl, _stIncl);

                _stIncl = 0;
                _edExcl = _arr.Length;

                _arr = newarr;
            }
        }
    }
}
