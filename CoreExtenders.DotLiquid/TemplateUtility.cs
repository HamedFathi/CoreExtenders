using CoreExtenders.DotLiquid.Filters;
using DotLiquid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CoreExtenders
{
    public static class TemplateUtility
    {
        public static void RegisterCustomFilters()
        {
            Template.RegisterFilter(typeof(CustomFilters));
        }

        public static void RegisterSafeTypes(params Type[] types)
        {
            if (types != null && types.Length > 0)
            {
                foreach (var type in types)
                {
                    var props = type.GetTypeInfo().GetProperties().Select(x => x.Name).ToArray();
                    Template.RegisterSafeType(type, props);
                }
            }
        }

        public static void RegisterSafeTypes(bool withReferencedAssemblies = false)
        {
            var types = new List<Type>();
            types.AddRange(Assembly.GetEntryAssembly().GetTypes());

            if (withReferencedAssemblies)
            {
                var refAssm = Assembly.GetEntryAssembly().GetReferencedAssemblies();
                foreach (var referencedAssembly in refAssm)
                {
                    var loadedAssembly = Assembly.Load(referencedAssembly);
                    types.AddRange(loadedAssembly.GetTypes());
                }
            }

            foreach (var type in types)
            {
                var props = type.GetTypeInfo().GetProperties().Select(x => x.Name).ToArray();
                Template.RegisterSafeType(type, props);
            }
        }

        public static void RegisterSafeTypes(Assembly assembly)
        {

            var types = assembly.GetTypes();
            foreach (var type in types)
            {
                var props = type.GetTypeInfo().GetProperties().Select(x => x.Name).ToArray();
                Template.RegisterSafeType(type, props);
            }
        }

        public static void RegisterSafeTypes(List<Assembly> assemblies)
        {
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes();
                foreach (var type in types)
                {
                    var props = type.GetTypeInfo().GetProperties().Select(x => x.Name).ToArray();
                    Template.RegisterSafeType(type, props);
                }
            }
        }
    }
}
