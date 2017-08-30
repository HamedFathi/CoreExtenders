using System;

namespace CoreExtenders
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class InterceptorAttribute : Attribute
    {
        public Type Interceptor { get; set; }

        public string Methods { get; set; }

        public InterceptorAttribute(Type interceptor)
        {
            Interceptor = interceptor;
        }
    }
}
