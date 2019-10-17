using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Expressions.Task1
{
    internal class TaskVisitor2<T> : ExpressionVisitor
    {
        private readonly IDictionary<string, T> _wildcardData;

        public TaskVisitor2(IDictionary<string, T> wildcardData)
        {
            _wildcardData = wildcardData ?? throw new ArgumentNullException(nameof(wildcardData));
        }

        protected override Expression VisitLambda<T1>(Expression<T1> node)
        {
            return Expression.Lambda(Visit(node.Body), node.Parameters.Where(p => !_wildcardData.ContainsKey(p.Name)));
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (_wildcardData.TryGetValue(node.Name, out T value))
            {
                return Expression.Constant(value, node.Type);
            }

            return base.VisitParameter(node);
        }
    }
}
