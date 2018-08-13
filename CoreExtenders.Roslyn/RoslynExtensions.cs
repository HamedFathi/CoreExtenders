using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.Formatting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace Roslyn
{
    public static class RoslynExtensions
    {
        public static string ToRoslynAPI(this string source, bool useDefaultFormatting = true, bool openParenthesisOnNewLine = false
            , bool closingParenthesisOnNewLine = false, bool removeRedundantModifyingCalls = false, bool shortenCodeWithUsingStatic = false)
        {
            var qouter = new Quoter()
            {
                UseDefaultFormatting = useDefaultFormatting,
                ShortenCodeWithUsingStatic = shortenCodeWithUsingStatic,
                RemoveRedundantModifyingCalls = removeRedundantModifyingCalls,
                OpenParenthesisOnNewLine = openParenthesisOnNewLine,
                ClosingParenthesisOnNewLine = closingParenthesisOnNewLine
            };
            return qouter.Quote(source).ToString();
        }

        public static IEnumerable<string> ToRoslynAPI(this IEnumerable<string> sources, bool useDefaultFormatting = true, bool openParenthesisOnNewLine = false
            , bool closingParenthesisOnNewLine = false, bool removeRedundantModifyingCalls = false, bool shortenCodeWithUsingStatic = false)
        {

            var list = new List<string>();
            foreach (var source in sources)
                list.Add(ToRoslynAPI(source, useDefaultFormatting, openParenthesisOnNewLine, closingParenthesisOnNewLine, removeRedundantModifyingCalls, shortenCodeWithUsingStatic));
            return list;
        }

        public static Type ToType(this string source, string classWithNamespace, out List<string> failures, params Assembly[] assemblies)
        {
            failures = new List<string>();
            var metadata = new List<MetadataReference>();
            if (assemblies == null || assemblies.Length == 0)
            {
                metadata.Add(MetadataReference.CreateFromFile(typeof(object).Assembly.Location));
            }
            else
            {
                foreach (var asm in assemblies)
                {
                    metadata.Add(MetadataReference.CreateFromFile(asm.Location));
                }
            }
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(source);
            string assemblyName = Path.GetRandomFileName();
            CSharpCompilation compilation = CSharpCompilation.Create(
                assemblyName,
                new[] { syntaxTree },
                metadata,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
            using (var ms = new MemoryStream())
            {
                EmitResult result = compilation.Emit(ms);
                if (!result.Success)
                {
                    var diagnostics = result.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == DiagnosticSeverity.Error);
                    foreach (Diagnostic diagnostic in diagnostics)
                        failures.Add(string.Format("{0}: {1}", diagnostic.Id, diagnostic.GetMessage()));
                }
                else
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    Assembly assembly = AssemblyLoadContext.Default.LoadFromStream(new MemoryStream(ms.ToArray()));
                    Type type = assembly.GetType(classWithNamespace);
                    if (type == null)
                        failures.Add($"'{classWithNamespace}' class could not be found. Did you forget 'namespace'?");
                    else
                        failures = null;
                    return type;
                }
            }

            return null;
        }

        public static string FormatCSharpCode(this string sourceCode)
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(sourceCode);
            var root = (CompilationUnitSyntax)tree.GetRoot();
            var workspace = new AdhocWorkspace();
            var newSyntax = Formatter.Format(root, workspace);
            var newSourceCode = newSyntax.GetText().ToString();
            return newSourceCode;
        }
        public static SyntaxNode FormatCSharpCode(this SyntaxNode syntaxNode)
        {
            var workspace = new AdhocWorkspace();
            var newSyntax = Formatter.Format(syntaxNode, workspace);
            return newSyntax;
        }

    }
}
