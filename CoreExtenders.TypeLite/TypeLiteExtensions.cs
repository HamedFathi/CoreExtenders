using System;
using System.Collections.Generic;
using TypeLite;

namespace CoreExtenders
{
    public static class TypeLiteExtensions
    {
        public static string GetTypeLitePoco(this Type type, bool useNamespace = true, bool isExported = true)
        {
            var name = type.DeclaringType?.FullName;
            var poco = TypeScript.Definitions().For(type).Generate();
            if (isExported)
            {
                poco = poco.Replace("interface ", " export interface ");
            }
            if (useNamespace)
            {
                poco = poco.Replace("declare module ", "namespace ");
            }
            return poco;
        }

        public static string GetTypeLitePoco(this Type type, TsGeneratorOutput output, bool useNamespace = true, bool isExported = true)
        {
            var name = type.DeclaringType?.FullName;
            var poco = TypeScript.Definitions().For(type).Generate(output);
            if (isExported)
            {
                poco = poco.Replace("interface ", " export interface ");
            }
            if (useNamespace)
            {
                poco = poco.Replace("declare module ", "namespace ");
            }
            return poco;
        }

        public static string GetTypeLitePocos(this IEnumerable<Type> types, bool useNamespace = true, bool isExported = true)
        {
            var replacements = new List<string>();
            var def = TypeScript.Definitions();
            foreach (var type in types)
            {
                def.For(type);
                var r = type.DeclaringType?.FullName;
                if (r != null && string.IsNullOrEmpty(type.Namespace))
                    replacements.Add(r);

            }
            var script = def.Generate();
            if (isExported)
            {
                script = script.Replace("interface ", " export interface ");
            }
            if (useNamespace)
            {
                script = script.Replace("declare module ", "namespace ");
            }
            return script;
        }

        public static string GetTypeLitePocos(this IEnumerable<Type> types, TsGeneratorOutput output, bool useNamespace = true, bool isExported = true)
        {
            var replacements = new List<string>();
            var def = TypeScript.Definitions();
            foreach (var type in types)
            {
                def.For(type);
                var r = type.DeclaringType?.FullName;
                if (r != null && string.IsNullOrEmpty(type.Namespace))
                    replacements.Add(r);

            }
            var script = def.Generate(output);
            if (isExported)
            {
                script = script.Replace("interface ", "export interface ");
            }
            if (useNamespace)
            {
                script = script.Replace("declare module ", "namespace ");
            }
            return script;
        }
    }
}
