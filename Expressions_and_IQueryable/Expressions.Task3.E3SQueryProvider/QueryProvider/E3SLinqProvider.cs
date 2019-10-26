using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Expressions.Task3.E3SQueryProvider.Helpers;
using Expressions.Task3.E3SQueryProvider.Services;

namespace Expressions.Task3.E3SQueryProvider.QueryProvider
{
    public class E3SLinqProvider : IQueryProvider
    {
        private readonly E3SSearchService _e3SClient;

        public E3SLinqProvider(E3SSearchService client)
        {
            _e3SClient = client ?? throw new ArgumentNullException(nameof(client));
        }

        public IQueryable CreateQuery(Expression expression)
        {
            Type elementType = TypeHelper.GetElementType(expression.Type);
            try
            {
                return (IQueryable)Activator.CreateInstance(typeof(E3SQuery<>).MakeGenericType(elementType), this, expression);
            }
            catch (TargetInvocationException tie)
            {
                throw tie.InnerException ?? tie;
            }
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new E3SQuery<TElement>(expression, this);
        }

        public object Execute(Expression expression)
        {
            Type itemType = TypeHelper.GetElementType(expression.Type);

            var translator = new ExpressionToFtsRequestTranslator();
            string queryString = translator.Translate(expression);

            return _e3SClient.SearchFTS(itemType, queryString);
        }

        public TResult Execute<TResult>(Expression expression) => (TResult)Execute(expression);
    }
}
