using Castle.Sharp2Js;
using System;
using System.Collections.Generic;

namespace CoreExtenders
{
    public static class Sharp2JsExtensions
    {
        private static string AddModels(string source)
        {
            if (string.IsNullOrEmpty(source))
                return "";
            var model = "models = [];";
            return model + Environment.NewLine + Environment.NewLine + source;
        }

        public static string GetJavaScriptPoco(this Type type, bool addModelArray = false)
        {
            var poco = JsGenerator.Generate(new[] { type }, new JsGeneratorOptions
            {
                CamelCase = true,
                IncludeMergeFunction = false,
                IncludeEqualsFunction = false
            });
            if (addModelArray)
                return AddModels(poco);
            return poco;
        }

        public static string GetJavaScriptPoco(this Type type, JsGeneratorOptions options, bool addModelArray = false)
        {
            var poco = JsGenerator.Generate(new[] { type }, options);
            if (addModelArray)
                return AddModels(poco);
            return poco;
        }

        public static string GetJavaScriptPocos(this IEnumerable<Type> types, bool addModelArray = false)
        {
            var poco = JsGenerator.Generate(types, new JsGeneratorOptions
            {
                CamelCase = true,
                IncludeMergeFunction = false,
                IncludeEqualsFunction = false
            });
            if (addModelArray)
                return AddModels(poco);
            return poco;
        }

        public static string GetTypeScriptPocos(this IEnumerable<Type> types, JsGeneratorOptions options, bool addModelArray = false)
        {
            var poco = JsGenerator.Generate(types, options);
            if (addModelArray)
                return AddModels(poco);
            return poco;
        }
    }
}
