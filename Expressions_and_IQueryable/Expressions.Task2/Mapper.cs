using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Expressions.Task2
{
    internal static class Mapper
    {
        private static readonly IList<CompiledExpression> CompiledExpressions = new List<CompiledExpression>();

        public static TDestination Map<TSource, TDestination>(TSource source, IDictionary<string, string> bindings = null)
            where TSource : class, new()
            where TDestination : class, new()
        {
            var compiledExpression = new CompiledExpression(typeof(TSource), typeof(TDestination));
            int i = CompiledExpressions.IndexOf(compiledExpression);
            if (i != -1)
            {
                var func = CompiledExpressions[i].MappingFunc as Func<TSource, TDestination>;
                return func.Invoke(source);
            }

            bindings = bindings ?? new Dictionary<string, string>();
            var expression = CreateExpression<TSource, TDestination>(bindings);
            var mappingFunc = expression.Compile();
            compiledExpression.MappingFunc = mappingFunc;
            CompiledExpressions.Add(compiledExpression);
            return mappingFunc.Invoke(source);
        }

        private static Expression<Func<TSource, TDestination>> CreateExpression<TSource, TDestination>(IDictionary<string, string> bindings)
            where TSource : class, new()
            where TDestination : class, new()
        {
            var from = typeof(TSource);
            var to = typeof(TDestination);

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
            var membersFrom = fieldsFrom.Concat(propertiesFrom).ToList();

            // All members that can be written to.
            var fieldsTo = to.GetFields().Select(f => f.Name);
            var propertiesTo = to.GetProperties().Where(p => p.CanWrite).Select(p => p.Name);
            var membersTo = fieldsTo.Concat(propertiesTo).ToList();

            // Mapped members.
            var mappings = membersFrom.ToDictionary(name => name, _ => string.Empty);
            var commonMembers = membersFrom.Intersect(membersTo).ToArray();
            foreach (var name in commonMembers)
            {
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

        private class CompiledExpression : IComparable, IComparable<CompiledExpression>, IEquatable<CompiledExpression>
        {
            private Type _source;
            private Type _destination;

            public object MappingFunc { get; set; }

            public CompiledExpression(Type source, Type destination)
            {
                _source = source ?? throw new ArgumentNullException(nameof(source));
                _destination = destination ?? throw new ArgumentNullException(nameof(destination));
            }

            public int CompareTo(object obj)
            {
                if (ReferenceEquals(obj, null)) return 1;
                if (obj.GetType() != GetType()) return 1;
                return CompareTo((CompiledExpression)obj);
            }

            public int CompareTo(CompiledExpression other)
            {
                if (ReferenceEquals(other, null)) return 1;
                if (string.Equals(_source.FullName, other._source.FullName, StringComparison.Ordinal) &&
                    string.Equals(_destination.FullName, other._destination.FullName, StringComparison.Ordinal))
                {
                    return 0;
                }

                return string.Compare(_source.FullName, other._source.FullName, StringComparison.Ordinal);
            }

            public bool Equals(CompiledExpression other)
            {
                return CompareTo(other) == 0;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(obj, null)) return false;
                if (obj.GetType() != GetType()) return false;
                return Equals((CompiledExpression)obj);
            }

            public override int GetHashCode() => ToString().GetHashCode();
        }
    }
}
