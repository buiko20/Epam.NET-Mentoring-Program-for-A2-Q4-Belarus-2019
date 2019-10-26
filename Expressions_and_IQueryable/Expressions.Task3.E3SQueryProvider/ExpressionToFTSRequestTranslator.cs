using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Expressions.Task3.E3SQueryProvider
{
    public class ExpressionToFtsRequestTranslator : ExpressionVisitor
    {
        private readonly StringBuilder _resultStringBuilder;
        private SupportedOperations _operation = SupportedOperations.NotSupported;

        public const string AndSeparator = "&&";

        public ExpressionToFtsRequestTranslator()
        {
            _resultStringBuilder = new StringBuilder();
        }

        public string Translate(Expression exp)
        {
            Visit(exp);
            return _resultStringBuilder.ToString();
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.DeclaringType == typeof(string) && node.Method.Name.Equals(nameof(SupportedOperations.StartsWith), StringComparison.Ordinal))
            {
                _operation = SupportedOperations.StartsWith;
            }

            if (node.Method.DeclaringType == typeof(string) && node.Method.Name.Equals(nameof(SupportedOperations.EndsWith), StringComparison.Ordinal))
            {
                _operation = SupportedOperations.EndsWith;
            }

            if (node.Method.DeclaringType == typeof(string) && node.Method.Name.Equals(nameof(SupportedOperations.Contains), StringComparison.Ordinal))
            {
                _operation = SupportedOperations.Contains;
            }

            if (node.Method.DeclaringType == typeof(string) && node.Method.Name.Equals(nameof(SupportedOperations.Equals), StringComparison.Ordinal))
            {
                _operation = SupportedOperations.Equals;
            }

            return base.VisitMethodCall(node);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            switch (node.NodeType)
            {
                case ExpressionType.Equal:
                    if (node.Left.NodeType != ExpressionType.MemberAccess && node.Left.NodeType != ExpressionType.Constant
                                                                          &&
                        node.Right.NodeType != ExpressionType.Constant && node.Right.NodeType != ExpressionType.MemberAccess)
                    {
                        throw new NotSupportedException("Left or Right operand should be property, field or constant.");
                    }

                    Expression memberAccess = node.Left;
                    Expression constant = node.Right;
                    if (node.Left.NodeType == ExpressionType.Constant)
                    {
                        memberAccess = node.Right;
                        constant = node.Left;
                    }

                    // Redirect to string.Equals call.
                    _operation = SupportedOperations.Equals;
                    MethodInfo equalsMethod = typeof(string).GetMethod(nameof(SupportedOperations.Equals), new[] { typeof(string) });
                    var equalsCall = Expression.Call(memberAccess, equalsMethod, constant);
                    Visit(equalsCall);
                    break;

                case ExpressionType.AndAlso:
                    Visit(node.Left);
                    _resultStringBuilder.Append(AndSeparator);
                    Visit(node.Right);
                    break;

                default:
                    throw new NotSupportedException($"Operation {node.NodeType} is not supported");
            }

            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (_operation != SupportedOperations.NotSupported)
            {
                _resultStringBuilder.Append(node.Member.Name).Append(":(");
                if (_operation == SupportedOperations.EndsWith || _operation == SupportedOperations.Contains)
                {
                    _resultStringBuilder.Append("*");
                }
            }

            return base.VisitMember(node);
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            if (_operation != SupportedOperations.NotSupported)
            {
                _resultStringBuilder.Append(node.Value);
                if (_operation == SupportedOperations.StartsWith || _operation == SupportedOperations.Contains)
                {
                    _resultStringBuilder.Append("*");
                }
                _resultStringBuilder.Append(")");
            }

            return node;
        }

        private enum SupportedOperations
        {
            NotSupported,
            Equals,
            StartsWith,
            EndsWith,
            Contains
        }
    }
}
