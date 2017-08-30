using System;
using System.Collections.Generic;
using TypeScriptBuilder;

namespace CoreExtenders
{
    public static class TypeScriptBuilderExtensions
    {
        public static string GetTypeScriptBuilderPoco(this Type type)
        {
            var scripter = new TypeScriptGenerator();
            return scripter.AddCSType(type).ToString();
        }

        public static string GetTypeScriptBuilderPoco(this IEnumerable<Type> types)
        {
            var scripter = new TypeScriptGenerator();
            foreach (var type in types)
            {
                scripter.AddCSType(type);
            }
            return scripter.ToString();
        }
    }
}
