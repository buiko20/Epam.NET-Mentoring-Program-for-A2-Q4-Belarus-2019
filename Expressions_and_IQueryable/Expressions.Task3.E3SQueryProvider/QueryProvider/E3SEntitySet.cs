using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Expressions.Task3.E3SQueryProvider.Models.Entitites;
using Expressions.Task3.E3SQueryProvider.Services;

namespace Expressions.Task3.E3SQueryProvider.QueryProvider
{
    public class E3SEntitySet<T> : IQueryable<T>, IQueryable, IEnumerable<T>, IEnumerable, IOrderedQueryable<T>, IOrderedQueryable where T : BaseE3SEntity
    {
        public E3SEntitySet(E3SSearchService client)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            Expression = Expression.Constant(this);
            Provider = new E3SLinqProvider(client);
        }

        public Type ElementType => typeof(T);

        public Expression Expression { get; }

        public IQueryProvider Provider { get; }

        public IEnumerator<T> GetEnumerator()
        {
            return Provider.Execute<IEnumerable<T>>(Expression).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}