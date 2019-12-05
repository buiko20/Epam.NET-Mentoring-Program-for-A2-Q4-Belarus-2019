using System;
using Castle.DynamicProxy;
using Newtonsoft.Json;

namespace CastleProxy
{
    [Serializable]
    public class CustomInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            Console.WriteLine($"{DateTime.Now}: {invocation.TargetType.FullName}.{invocation.Method.Name} Arguments: {SerializeParameters(invocation)}");
            try
            {
                invocation.Proceed();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                Console.WriteLine($"{DateTime.Now}: {invocation.TargetType.FullName}.{invocation.Method.Name} Return: {JsonConvert.SerializeObject(invocation.ReturnValue)}");
            }
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
    }
}
