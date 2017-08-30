using CoreExtensions;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Mustache
{
    public static class MustasheSharpExtensions
    {
        private const string TemplateRegex = @"{{#template\s+.+\s*}}";
        private const string OpenBraceReplacement = @"$@$@$***___@$%$";
        private const string TemplateReplacement = @"$@$@$***___@$%$#template";
        private const string TemplateTag = @"{{#template";

        public static Generator CompileInMemoryNestedTemplates(this FormatCompiler compiler, string format, Dictionary<string, string> templates)
        {
            var text = compiler.ResolveInMemoryNestedTemplates(format, templates);
            Generator generator = compiler.Compile(text);
            return generator;
        }

        public static Generator CompileInMemoryNestedTemplates(this HtmlFormatCompiler compiler, string format, Dictionary<string, string> templates)
        {
            var text = compiler.ResolveInMemoryNestedTemplates(format, templates);
            Generator generator = compiler.Compile(text);
            return generator;
        }

        public static Generator CompileNestedTemplates(this FormatCompiler compiler, string format)
        {
            var text = compiler.ResolveNestedTemplates(format, true);
            Generator generator = compiler.Compile(text);
            return generator;
        }

        public static Generator CompileNestedTemplates(this HtmlFormatCompiler compiler, string format)
        {
            var text = compiler.ResolveNestedTemplates(format, true);
            Generator generator = compiler.Compile(text);
            return generator;
        }

        public static void ResigterCustomTags(this HtmlFormatCompiler compiler)
        {
            compiler.RegisterTag(new TemplateDefinition(), true);
            compiler.RegisterTag(new IsNullOrEmptyTagDefinition(), true);
            compiler.RegisterTag(new AnyTagDefinition(), true);
            compiler.RegisterTag(new CamelizeTagDefinition(), true);
            compiler.RegisterTag(new LowerTagDefinition(), true);
            compiler.RegisterTag(new UpperTagDefinition(), true);
            compiler.RegisterTag(new TabTagDefinition(), true);
            compiler.RegisterTag(new CommentTagDefinition(), true);
        }

        public static void ResigterCustomTags(this FormatCompiler compiler)
        {
            compiler.RegisterTag(new TemplateDefinition(), true);
            compiler.RegisterTag(new IsNullOrEmptyTagDefinition(), true);
            compiler.RegisterTag(new AnyTagDefinition(), true);
            compiler.RegisterTag(new CamelizeTagDefinition(), true);
            compiler.RegisterTag(new LowerTagDefinition(), true);
            compiler.RegisterTag(new UpperTagDefinition(), true);
            compiler.RegisterTag(new TabTagDefinition(), true);
            compiler.RegisterTag(new CommentTagDefinition(), true);

        }

        private static string ResolveInMemoryNestedTemplates(this FormatCompiler compiler, string format, Dictionary<string, string> templates)
        {
            if (templates != null && templates.Count > 0)
            {
                Dictionary<string, object> obj = new Dictionary<string, object>();
                foreach (var tuple in templates)
                    obj.Add(tuple.Key, tuple.Value);
                var anonymos = obj.ToDynamicObject();
                while (true)
                {

                    if (Regex.IsMatch(format, TemplateRegex))
                    {
                        format = format.Replace("{{", OpenBraceReplacement);
                        format = format.Replace(TemplateReplacement, TemplateTag);
                        Generator generator = compiler.Compile(format);
                        format = generator.Render(anonymos);
                    }
                    else
                    {
                        format = format.Replace(OpenBraceReplacement, "{{");
                        return format;
                    }
                }
            }
            return format;

        }

        private static string ResolveInMemoryNestedTemplates(this HtmlFormatCompiler compiler, string format, Dictionary<string, string> templates)
        {
            if (templates != null && templates.Count > 0)
            {
                Dictionary<string, object> obj = new Dictionary<string, object>();
                foreach (var tuple in templates)
                    obj.Add(tuple.Key, tuple.Value);
                var anonymos = obj.ToDynamicObject();
                while (true)
                {

                    if (Regex.IsMatch(format, TemplateRegex))
                    {
                        format = format.Replace("{{", OpenBraceReplacement);
                        format = format.Replace(TemplateReplacement, TemplateTag);
                        Generator generator = compiler.Compile(format);
                        format = generator.Render(anonymos);
                    }
                    else
                    {
                        format = format.Replace(OpenBraceReplacement, "{{");
                        return format;
                    }
                }
            }
            return format;
        }

        private static string ResolveNestedTemplates(this FormatCompiler compiler, string format, bool searchDirectory = false)
        {
            var assembly = Assembly.GetEntryAssembly();
            Dictionary<string, object> resources = new Dictionary<string, object>();
            var files = assembly.GetResources(new Regex(@".*\.mustache", RegexOptions.IgnoreCase));
            foreach (var file in files)
            {
                var fName = Path.GetFileNameWithoutExtension(file.Name).ToLowerInvariant();
                if (fName.Contains("/"))
                {
                    var lastIndex = fName.LastIndexOf('/');
                    fName = fName.Substring(lastIndex);
                }
                if (!resources.ContainsKey(fName))
                    resources.Add(fName, file.CreateReadStream().ToString());
            }

            if (searchDirectory)
            {
                var dirFiles = Directory.EnumerateFiles(assembly.Location, "*.mustache", SearchOption.AllDirectories);
                foreach (var file in dirFiles)
                {
                    var name = Path.GetFileNameWithoutExtension(file).ToLowerInvariant();
                    if (!resources.ContainsKey(name))
                        resources.Add(name, File.ReadAllText(file));
                }
            }
            if (resources.Count > 0)
            {
                var anonymos = resources.ToDynamicObject();
                while (true)
                {

                    if (Regex.IsMatch(format, TemplateRegex))
                    {
                        format = format.Replace("{{", OpenBraceReplacement);
                        format = format.Replace(TemplateReplacement, TemplateTag);
                        Generator generator = compiler.Compile(format);
                        format = generator.Render(anonymos);
                    }
                    else
                    {
                        format = format.Replace(OpenBraceReplacement, "{{");
                        return format;
                    }
                }
            }
            return format;

        }

        private static string ResolveNestedTemplates(this HtmlFormatCompiler compiler, string format, bool searchDirectory = false)
        {
            var assembly = Assembly.GetEntryAssembly();
            Dictionary<string, object> resources = new Dictionary<string, object>();
            var files = assembly.GetResources(new Regex(@".*\.mustache", RegexOptions.IgnoreCase));
            foreach (var file in files)
            {
                var fName = Path.GetFileNameWithoutExtension(file.Name).ToLowerInvariant();
                if (fName.Contains("/"))
                {
                    var lastIndex = fName.LastIndexOf('/');
                    fName = fName.Substring(lastIndex);
                }
                if (!resources.ContainsKey(fName))
                    resources.Add(fName, file.CreateReadStream().ToString());
            }

            if (searchDirectory)
            {
                var dirFiles = Directory.EnumerateFiles(assembly.Location, "*.mustache", SearchOption.AllDirectories);
                foreach (var file in dirFiles)
                {
                    var name = Path.GetFileNameWithoutExtension(file).ToLowerInvariant();
                    if (!resources.ContainsKey(name))
                        resources.Add(name, File.ReadAllText(file));
                }
            }
            if (resources.Count > 0)
            {
                var anonymos = resources.ToDynamicObject();
                while (true)
                {

                    if (Regex.IsMatch(format, TemplateRegex))
                    {
                        format = format.Replace("{{", OpenBraceReplacement);
                        format = format.Replace(TemplateReplacement, TemplateTag);
                        Generator generator = compiler.Compile(format);
                        format = generator.Render(anonymos);
                    }
                    else
                    {
                        format = format.Replace(OpenBraceReplacement, "{{");
                        return format;
                    }
                }
            }
            return format;
        }
    }
}
