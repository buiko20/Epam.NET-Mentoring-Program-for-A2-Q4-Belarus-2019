using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Expressions.Task2
{
    internal static class Mapper
    {
        public static TDestination Map<TSource, TDestination>(TSource source, IDictionary<string, string> bindings = null)
            where TSource : class, new()
            where TDestination : class, new()
        {
            bindings = bindings ?? new Dictionary<string, string>();
            var expression = CreateExpression<TSource, TDestination>(typeof(TSource), typeof(TDestination), bindings);
            return expression.Compile().Invoke(source);
        }

        private static Expression<Func<TSource, TDestination>> CreateExpression<TSource, TDestination>(Type from, Type to, IDictionary<string, string> bindings)
            where TSource : class, new()
            where TDestination : class, new()
        {
            var mappings = GetMappings(from, to, bindings);
            var parameter = Expression.Parameter(from);

            var memberBindings = mappings.Select(mapping =>
            {
                var member = (MemberInfo)to.GetField(mapping.Value) ?? to.GetProperty(mapping.Value);
                if (member != null)
                {
                    return Expression.Bind(member, Expression.PropertyOrField(parameter, mapping.Key));
                }

                return null;
            }).Where(item => item != null).ToArray();

            var newObjectExpression = Expression.MemberInit(Expression.New(to.GetConstructor(Type.EmptyTypes)), memberBindings);
            return Expression.Lambda<Func<TSource, TDestination>>(newObjectExpression, parameter);
        }

        private static IDictionary<string, string> GetMappings(Type from, Type to, IDictionary<string, string> bindings)
        {
            // All members that can be read from.
            var fieldsFrom = from.GetFields().Select(f => f.Name);
            var propertiesFrom = from.GetProperties().Where(p => p.CanRead).Select(p => p.Name);
            var membersFrom = fieldsFrom.Concat(propertiesFrom).ToArray();

            // All members that can be written to.
            var fieldsTo = to.GetFields().Select(f => f.Name);
            var propertiesTo = to.GetProperties().Where(p => p.CanWrite).Select(p => p.Name);
            var membersTo = fieldsTo.Concat(propertiesTo).ToArray();

            var mappings = membersFrom.ToDictionary(name => name, _ => string.Empty);
            foreach (var name in membersTo)
            {
                if (mappings.ContainsKey(name))
                    mappings[name] = name;
            }

            foreach (var mapping in bindings)
            {
                if (mappings.ContainsKey(mapping.Key))
                    mappings[mapping.Key] = mapping.Value;
            }

            return mappings
                .Where(pair => !string.IsNullOrWhiteSpace(pair.Value))
                .ToDictionary(pair => pair.Key, pair => pair.Value);
        }
    }
}
