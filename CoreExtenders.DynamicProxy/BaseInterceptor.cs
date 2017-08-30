using Castle.DynamicProxy;
using System;
using System.Linq;
using System.Reflection;

namespace CoreExtenders
{
    public class BaseInterceptor : IInterceptor
    {
        public string[] Methods { get; set; }

        public BaseInterceptor(string[] methods = null)
        {
            Methods = methods;
        }

        private object GetDefaultValue(Type t)
        {
            if (t.GetTypeInfo().IsValueType && Nullable.GetUnderlyingType(t) == null)
                return Activator.CreateInstance(t);
            return null;
        }

        public void Intercept(IInvocation invocation)
        {
            try
            {
                var method = invocation.Method.Name;
                if (Methods != null)
                {
                    if (!Methods.Contains(method))
                    {
                        invocation.Proceed();
                        return;
                    }
                }
                OnEntry(invocation);
                invocation.Proceed();
                OnSuccess(invocation);
            }
            catch (Exception ex)
            {
                invocation.ReturnValue = GetDefaultValue(invocation.Method.ReturnType);
                OnException(invocation, ex);
            }
            finally
            {
                OnExit(invocation);
            }
        }

        protected virtual void OnEntry(IInvocation invocation)
        {
        }

        protected virtual void OnException(IInvocation invocation, Exception ex)
        {
        }

        protected virtual void OnExit(IInvocation invocation)
        {
        }

        protected virtual void OnSuccess(IInvocation invocation)
        {
        }
    }
}
