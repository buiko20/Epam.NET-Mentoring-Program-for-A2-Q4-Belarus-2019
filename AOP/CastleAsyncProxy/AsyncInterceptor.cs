using System;
using System.Reflection;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Newtonsoft.Json;

namespace CastleAsyncProxy
{
    [Serializable]
    public class AsyncInterceptor : IInterceptor
    {
        private static readonly MethodInfo HandleAsyncMethodInfo = typeof(AsyncInterceptor)
            .GetMethod(nameof(HandleAsyncWithResult), BindingFlags.Instance | BindingFlags.NonPublic);

        public void Intercept(IInvocation invocation)
        {
            Console.WriteLine($"{DateTime.Now}: {invocation.TargetType.FullName}.{invocation.Method.Name} Arguments: {SerializeParameters(invocation)}");

            invocation.Proceed();

            var methodType = GetMethodType(invocation);
            if (methodType == MethodType.Synchronous)
            {
                PrintReturnValue(invocation);
            }

            if (methodType == MethodType.AsyncAction)
            {
                invocation.ReturnValue = HandleAsync(invocation);
            }

            if (methodType == MethodType.AsyncFunction)
            {
                InvokeHandleAsyncWithResult(invocation);
            }
        }

        private void InvokeHandleAsyncWithResult(IInvocation invocation)
        {
            var resultType = invocation.Method.ReturnType.GetGenericArguments()[0];
            var mi = HandleAsyncMethodInfo.MakeGenericMethod(resultType);
            invocation.ReturnValue = mi.Invoke(this, new[] { invocation, invocation.ReturnValue });
        }

        private async Task HandleAsync(IInvocation invocation)
        {
            var task = (Task)invocation.ReturnValue;
            await task.ConfigureAwait(false);
            PrintReturnValue(invocation);
        }

        private async Task<T> HandleAsyncWithResult<T>(IInvocation invocation, Task<T> task)
        {
            T result = await task.ConfigureAwait(false);
            PrintReturnValue(invocation, result);
            return result;
        }

        private static void PrintReturnValue(IInvocation invocation, object result = null)
        {
            if (result == null)
            {
                result = invocation.ReturnValue;
            }

            Console.WriteLine($"{DateTime.Now}: {invocation.TargetType.FullName}.{invocation.Method.Name} Return: {JsonConvert.SerializeObject(result)}");
        }

        private static string SerializeParameters(IInvocation invocation)
        {
            var methodParameters = invocation.Method.GetParameters();
            string result = string.Empty;
            for (var i = 0; i < methodParameters.Length; i++)
            {
                var name = methodParameters[i].Name;
                var value = JsonConvert.SerializeObject(invocation.Arguments[i]);
                result += $"{name}={value}, ";
            }

            return result.TrimEnd(',', ' ');
        }

        private static MethodType GetMethodType(IInvocation invocation)
        {
            Type returnType = invocation.Method.ReturnType;
            if (returnType == typeof(Task))
            {
                return MethodType.AsyncAction;
            }

            if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
            {
                return MethodType.AsyncFunction;
            }

            return MethodType.Synchronous;
        }

        private enum MethodType
        {
            Synchronous,
            AsyncAction,
            AsyncFunction
        }
    }
}
