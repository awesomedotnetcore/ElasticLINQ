﻿// Licensed under the Apache 2.0 License. See LICENSE.txt in the project root for more information.

using System;
using ElasticLinq.Mapping;
using ElasticLinq.Response.Model;
using ElasticLinq.Utility;
using System.Linq.Expressions;

namespace ElasticLinq.Request.Visitors
{
    /// <summary>
    /// Expression visitor that substitutes references to <see cref="ElasticFields"/>
    /// with the desired underlying special reserved name.
    /// </summary>
    internal class ElasticFieldsRebindingExpressionVisitor : RebindingExpressionVisitor
    {
        public ElasticFieldsRebindingExpressionVisitor(ParameterExpression bindingParameter, IElasticMapping mapping)
            : base(bindingParameter, mapping)
        {
        }

        internal static Tuple<Expression, ParameterExpression> Rebind(IElasticMapping mapping, Expression selector)
        {
            var parameter = Expression.Parameter(typeof(Hit), "h");
            var visitor = new ElasticFieldsRebindingExpressionVisitor(parameter, mapping);
            Argument.EnsureNotNull("selector", selector);
            return Tuple.Create(visitor.Visit(selector), parameter);
        }

        protected override Expression VisitMember(MemberExpression m)
        {
            return m.Member.DeclaringType == typeof(ElasticFields)
                ? VisitElasticField(m)
                : base.VisitMember(m);
        }

        protected virtual Expression VisitElasticField(MemberExpression m)
        {
            return Expression.Convert(Expression.PropertyOrField(BindingParameter, Mapping.GetFieldName(m.Member)), m.Type);
        }
    }
}