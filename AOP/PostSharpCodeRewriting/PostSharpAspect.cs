using System;
using Newtonsoft.Json;
using PostSharp.Aspects;

namespace PostSharpCodeRewriting
{
    [Serializable]
    internal class PostSharpAspect : OnMethodBoundaryAspect
    {
        public override void OnEntry(MethodExecutionArgs args)
        {
            if (args.Method.IsConstructor) return;

            Console.WriteLine($"{DateTime.Now}: {args.Method.DeclaringType.FullName}.{args.Method.Name} Arguments: {SerializeParameters(args)}");
        }

        public override void OnSuccess(MethodExecutionArgs args)
        {
            if (args.Method.IsConstructor) return;

            Console.WriteLine($"{DateTime.Now}: {args.Method.DeclaringType.FullName}.{args.Method.Name} Return: {JsonConvert.SerializeObject(args.ReturnValue)}");
        }

        private static string SerializeParameters(MethodExecutionArgs args)
        {
            var methodParameters = args.Method.GetParameters();
            string result = string.Empty;
            for (var i = 0; i < methodParameters.Length; i++)
            {
                var name = methodParameters[i].Name;
                var value = JsonConvert.SerializeObject(args.Arguments[i]);
                result += $"{name}={value}, ";
            }

            return result.TrimEnd(',', ' ');
        }
    }
}
