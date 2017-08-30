using System;
using System.Collections.Generic;
using TypeScripter;

namespace CoreExtenders
{
    public static class TypeScripterExtensions
    {
        public static string GetTypeScripterPoco(this Type type)
        {
            var scripter = new Scripter();
            return scripter.AddType(type).ToString();
        }

        public static string GetTypeScripterPoco(this IEnumerable<Type> types)
        {
            var scripter = new Scripter();
            foreach (var type in types)
            {
                scripter.AddType(type);
            }
            return scripter.ToString();
        }
    }
}
