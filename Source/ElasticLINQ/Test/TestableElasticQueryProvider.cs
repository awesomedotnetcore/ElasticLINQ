﻿// Licensed under the Apache 2.0 License. See LICENSE.txt in the project root for more information.

using System.Linq;
using System.Linq.Expressions;

namespace ElasticLinq.Test
{
    /// <summary>
    /// Provides an <see cref="IQueryProvider"/> that can be used for unit tests.
    /// </summary>
    public class TestableElasticQueryProvider : IQueryProvider
    {
        readonly TestableElasticContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestableElasticQueryProvider"/> class.
        /// </summary>
        /// <param name="context">The <see cref="TestableElasticContext"/> used to execute the queries.</param>
        public TestableElasticQueryProvider(TestableElasticContext context)
        {
            this.context = context;
        }

        /// <inheritdoc/>
        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new TestableElasticQuery<TElement>(context, expression);
        }

        /// <inheritdoc/>
        public IQueryable CreateQuery(Expression expression)
        {
            return CreateQuery<object>(expression);
        }

        /// <inheritdoc/>
        public TResult Execute<TResult>(Expression expression)
        {
            return (TResult)Execute(expression);
        }

        /// <inheritdoc/>
        public object Execute(Expression expression)
        {
            return Expression.Lambda(expression).Compile().DynamicInvoke();
        }
    }
}
