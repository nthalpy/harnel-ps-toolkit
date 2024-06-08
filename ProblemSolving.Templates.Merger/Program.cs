using CommandLine;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Mono.Cecil;
using System.Text;

namespace ProblemSolving.Templates.Merger
{
    public sealed class MergerOption
    {
        public string DllDirectory => Path.GetDirectoryName(DllPath)!;

        [Option("DllPath", Required = true)]
        public string DllPath { get; set; } = default!;

        [Option("EntrypointPath", Required = true)]
        public string EntrypointPath { get; set; } = default!;

        [Option("StandalonePath", Required = true)]
        public string StandalonePath { get; set; } = default!;
    }
    public static class Program
    {
        public static void Main(string[] args)
        {
            var parseResult = Parser.Default.ParseArguments<MergerOption>(args);
            if (parseResult.Errors.Any())
            {
                foreach (var err in parseResult.Errors)
                {
                    Console.WriteLine(err);
                    throw new InvalidOperationException();
                }
            }

            var option = parseResult.Value!;
            Directory.SetCurrentDirectory(option.DllDirectory);
            var module = ModuleDefinition.ReadModule(option.DllPath);

            var entrypoint = module.EntryPoint;

            var referencedTypes = new HashSet<TypeDefinition>();
            var handledMethodDefs = new HashSet<MethodDefinition>();
            var q = new Queue<MethodDefinition>();
            q.Enqueue(entrypoint);

            while (q.TryDequeue(out var curr))
            {
                if (!handledMethodDefs.Add(curr))
                    continue;

                if (curr.Body != null)
                    foreach (var inst in curr.Body.Instructions)
                    {
                        Console.WriteLine(inst);

                        if (inst.Operand is MethodReference methodRef)
                        {
                            var methodDef = methodRef.Resolve();
                            var typeRef = methodRef.DeclaringType.Resolve();

                            if (typeRef.Module.Name.Contains("ProblemSolving"))
                                q.Enqueue(methodDef);

                            if (methodDef.DeclaringType.CustomAttributes.Any(attr => attr.AttributeType.Name == nameof(IncludeIfReferenced)))
                                referencedTypes.Add(methodDef.DeclaringType);
                        }
                    }
            }

            var mergeTargets = new List<string>();
            mergeTargets.Add(option.EntrypointPath);

            foreach (var type in referencedTypes)
            {
                var attr = type.CustomAttributes.Single(attr => attr.AttributeType.Name == nameof(IncludeIfReferenced));
                var arg0 = attr.ConstructorArguments[0].Value as string;

                if (arg0 != null)
                    mergeTargets.Add(arg0);
            }

            var merged = MergeSources(mergeTargets);
            File.WriteAllText(option.StandalonePath, merged);
        }

        public static string MergeSources(IEnumerable<string> paths)
        {
            var trees = new List<SyntaxTree>();
            foreach (var path in paths)
            {
                var text = File.ReadAllText(path);
                var root = CSharpSyntaxTree.ParseText(text);
                trees.Add(root);
            }

            var usings = new SortedSet<string>();
            foreach (var tree in trees)
            {
                var comp = tree.GetRoot() as CompilationUnitSyntax;
                if (comp == null)
                    throw new InvalidOperationException();

                usings.UnionWith(comp.Usings.Select(u => u.Name.ToFullString()));
            }

            var sb = new StringBuilder();
            foreach (var u in usings)
                sb.AppendLine($"using {u};");

            foreach (var u in usings)
                sb.AppendLine($"namespace {u} {{}}");

            foreach (var tree in trees)
            {
                var comp = tree.GetRoot() as CompilationUnitSyntax;
                if (comp == null)
                    throw new InvalidOperationException();

                comp = comp.WithUsings(new SyntaxList<UsingDirectiveSyntax>());

                foreach (var classDecl in comp.DescendantNodes().OfType<ClassDeclarationSyntax>())
                {
                    foreach (var attrList in classDecl.AttributeLists)
                    {
                        if (attrList.Attributes.Any(attr => attr.Name.ToFullString() == nameof(IncludeIfReferenced)))
                        {
                            comp = comp.RemoveNode(attrList, SyntaxRemoveOptions.KeepNoTrivia);
                            if (comp == null)
                                throw new InvalidOperationException();
                        }
                    }
                }

                sb.AppendLine(comp.ToFullString());
            }

            sb.AppendLine($"// This is source code merged w/ template");
            sb.AppendLine($"// Timestamp: {DateTime.Now:yyyy-MM-dd HH:mm:ss} UTC+9");

            return sb.ToString();
        }
    }
}
