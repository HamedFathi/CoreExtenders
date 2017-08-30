using Castle.DynamicProxy;
using System.Linq;
using System.Reflection;

namespace CoreExtenders
{
    public static class New
    {
        public static TClass Of<TClass>()
                    where TClass : class, new()
        {
            var interceptors = typeof(TClass).GetTypeInfo().GetCustomAttributes(typeof(InterceptorAttribute), false);

            var customInterceptor = (InterceptorAttribute)interceptors.FirstOrDefault();
            var methods = customInterceptor.Methods?.Split(',');
            var ctorCustom = customInterceptor.Interceptor.GetTypeInfo().GetConstructor(new[] { typeof(string[]) });
            var instanceCustom = (IInterceptor)ctorCustom?.Invoke(new object[] { methods });

            var generator = new ProxyGenerator();
            var tc = generator.CreateClassProxy<TClass>(instanceCustom);
            return tc;
        }

        public static TClass Of<TClass>(ProxyGenerationOptions options, params IInterceptor[] interceptors)
                    where TClass : class, new()
        {
            var generator = new ProxyGenerator();
            var tc = generator.CreateClassProxy<TClass>(options, interceptors);
            return tc;
        }
    }
}
