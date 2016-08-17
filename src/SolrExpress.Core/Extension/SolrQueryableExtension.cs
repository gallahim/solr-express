﻿using SolrExpress.Core.DependencyInjection;
using SolrExpress.Core.Query;
using SolrExpress.Core.Query.Parameter;
using SolrExpress.Core.Query.ParameterValue;
using System;
using System.Linq.Expressions;

namespace SolrExpress.Core.Extension
{
    public static class SolrQueryableExtension
    {
        /// <summary>
        /// Create a facet field parameter
        /// </summary>
        /// <param name="expression">Expression used to find the property name</param>
        /// <param name="sortType">Sort type of the result of the facet</param>
        /// <param name="limit">Limit of itens in facet's result</param>
        /// <param name="excludes">List of tags to exclude in facet calculation</param>
        public static ISolrQueryable<TDocument> FacetField<TDocument>(this ISolrQueryable<TDocument> queryable, Expression<Func<TDocument, object>> expression, FacetSortType? sortType = null, int? limit = null, params string[] excludes)
            where TDocument : IDocument
        {
            var parameter = ApplicationServices
                .Current
                .GetService<IFacetFieldParameter<TDocument>>()
                .Configure(expression, sortType, limit, excludes);

            return queryable.Parameter(parameter);
        }

        /// <summary>
        /// Create a facet query parameter
        /// </summary>
        /// <param name="aliasName">Name of the alias added in the query</param>
        /// <param name="query">Query used to make the facet</param>
        /// <param name="sortType">Sort type of the result of the facet</param>
        /// <param name="excludes">List of tags to exclude in facet calculation</param>
        public static ISolrQueryable<TDocument> FacetQuery<TDocument>(this ISolrQueryable<TDocument> queryable, string aliasName, IQueryParameterValue query, FacetSortType? sortType = null, params string[] excludes)
            where TDocument : IDocument
        {
            var parameter = ApplicationServices
                .Current
                .GetService<IFacetQueryParameter<TDocument>>()
                .Configure(aliasName, query, sortType, excludes);

            return queryable.Parameter(parameter);
        }

        /// <summary>
        /// Create a facet range parameter
        /// </summary>
        /// <param name="aliasName">Name of the alias added in the query</param>
        /// <param name="expression">Expression used to find the property name</param>
        /// <param name="gap">Size of each range bucket to make the facet</param>
        /// <param name="start">Lower bound to make the facet</param>
        /// <param name="end">Upper bound to make the facet</param>
        /// <param name="sortType">Sort type of the result of the facet</param>
        public static ISolrQueryable<TDocument> FacetRange<TDocument>(this ISolrQueryable<TDocument> queryable, string aliasName, Expression<Func<TDocument, object>> expression, string gap = null, string start = null, string end = null, FacetSortType? sortType = null)
            where TDocument : IDocument
        {
            var parameter = ApplicationServices
                .Current
                .GetService<IFacetRangeParameter<TDocument>>()
                .Configure(aliasName, expression, gap, start, end, sortType);

            return queryable.Parameter(parameter);
        }

        /// <summary>
        /// Create a fields parameter
        /// </summary>
        /// <param name="expressions">Expression used to find the property name</param>
        public static ISolrQueryable<TDocument> Fields<TDocument>(this ISolrQueryable<TDocument> queryable, params Expression<Func<TDocument, object>>[] expressions)
            where TDocument : IDocument
        {
            var parameter = ApplicationServices
                .Current
                .GetService<IFieldsParameter<TDocument>>()
                .Configure(expressions);

            return queryable.Parameter(parameter);
        }

        /// <summary>
        /// Create a filter parameter
        /// </summary>
        /// <param name="expression">Expression used to find the property name</param>
        /// <param name="value">Value of the filter</param>
        /// <param name="tagName">Tag name to use in facet excluding list</param>
        public static ISolrQueryable<TDocument> Filter<TDocument>(this ISolrQueryable<TDocument> queryable, Expression<Func<TDocument, object>> expression, string value, string tagName = null)
            where TDocument : IDocument
        {
            var paramaterValue = new Single<TDocument>(expression, value);

            var parameter = ApplicationServices
                .Current
                .GetService<IFilterParameter<TDocument>>()
                .Configure(paramaterValue, tagName);

            return queryable.Parameter(parameter);
        }

        /// <summary>
        /// Create a filter parameter
        /// </summary>
        /// <param name="expression">Expression used to find the property name</param>
        /// <param name="from">From value in a range filter</param>
        /// <param name="to">To value in a range filter</param>
        /// <param name="tagName">Tag name to use in facet excluding list</param>
        public static ISolrQueryable<TDocument> Filter<TDocument, TValue>(this ISolrQueryable<TDocument> queryable, Expression<Func<TDocument, object>> expression, TValue? from, TValue? to, string tagName = null)
            where TDocument : IDocument
            where TValue : struct
        {
            var paramaterValue = new Range<TDocument, TValue>(expression, from, to);

            var parameter = ApplicationServices
                .Current
                .GetService<IFilterParameter<TDocument>>()
                .Configure(paramaterValue, tagName);

            return queryable.Parameter(parameter);
        }

        /// <summary>
        /// Create a limit parameter
        /// </summary>
        /// <param name="value">Value of limit</param>
        public static ISolrQueryable<TDocument> Limit<TDocument>(this ISolrQueryable<TDocument> queryable, int value)
            where TDocument : IDocument
        {
            var parameter = ApplicationServices
                .Current
                .GetService<ILimitParameter>()
                .Configure(value);

            return queryable.Parameter(parameter);
        }

        /// <summary>
        /// Create a offset parameter
        /// </summary>
        /// <param name="value">Value of limit</param>
        public static ISolrQueryable<TDocument> Offset<TDocument>(this ISolrQueryable<TDocument> queryable, int value)
            where TDocument : IDocument
        {
            var parameter = ApplicationServices
                .Current
                .GetService<IOffsetParameter>()
                .Configure(value);

            return queryable.Parameter(parameter);
        }

        /// <summary>
        /// Create a query parameter
        /// </summary>
        /// <param name="value">Parameter to include in the query</param>
        public static ISolrQueryable<TDocument> Query<TDocument>(this ISolrQueryable<TDocument> queryable, IQueryParameterValue value)
            where TDocument : IDocument
        {
            var parameter = ApplicationServices
                .Current
                .GetService<IQueryParameter<TDocument>>()
                .Configure(value);

            return queryable.Parameter(parameter);
        }

        /// <summary>
        /// Create a query parameter
        /// </summary>
        /// <param name="value">Parameter to include in the query</param>
        public static ISolrQueryable<TDocument> Query<TDocument>(this ISolrQueryable<TDocument> queryable, string value)
            where TDocument : IDocument
        {
            var paramaterValue = new Any(value);

            var parameter = ApplicationServices
                .Current
                .GetService<IQueryParameter<TDocument>>()
                .Configure(paramaterValue);

            return queryable.Parameter(parameter);
        }

        /// <summary>
        /// Create a query parameter
        /// </summary>
        /// <param name="expression">Expression used to find the property name</param>
        /// <param name="value">Value of the query</param>
        public static ISolrQueryable<TDocument> Query<TDocument>(this ISolrQueryable<TDocument> queryable, Expression<Func<TDocument, object>> expression, string value)
            where TDocument : IDocument
        {
            var paramaterValue = new Single<TDocument>(expression, value);

            var parameter = ApplicationServices
                .Current
                .GetService<IQueryParameter<TDocument>>()
                .Configure(paramaterValue);

            return queryable.Parameter(parameter);
        }

        /// <summary>
        /// Create a sort parameter
        /// </summary>
        /// <param name="expression">Expression used to find the property name</param>
        /// <param name="ascendent">True to ascendent order, otherwise false</param>
        public static ISolrQueryable<TDocument> Sort<TDocument>(this ISolrQueryable<TDocument> queryable, Expression<Func<TDocument, object>> expression, bool ascendent)
            where TDocument : IDocument
        {
            var parameter = ApplicationServices
                .Current
                .GetService<ISortParameter<TDocument>>()
                .Configure(expression, ascendent);

            return queryable.Parameter(parameter);
        }

        /// <summary>
        /// Create a sort parameter
        /// </summary>
        /// <param name="ascendent">True to ascendent order, otherwise false</param>
        /// <param name="expressions">Expression used to find the property name</param>
        public static ISolrQueryable<TDocument> Sort<TDocument>(this ISolrQueryable<TDocument> queryable, bool ascendent, params Expression<Func<TDocument, object>>[] expressions)
            where TDocument : IDocument
        {
            foreach (var expression in expressions)
            {
                var parameter = ApplicationServices
                    .Current
                    .GetService<ISortParameter<TDocument>>()
                    .Configure(expression, ascendent);

                queryable.Parameter(parameter);
            }

            return queryable;
        }

        /// <summary>
        /// Create a random sort parameter
        /// </summary>
        /// <param name="ascendent">True to ascendent order, otherwise false</param>
        /// <param name="expressions">Expression used to find the property name</param>
        public static ISolrQueryable<TDocument> RandomSort<TDocument>(this ISolrQueryable<TDocument> queryable, bool ascendent)
            where TDocument : IDocument
        {
            var parameter = ApplicationServices
                .Current
                .GetService<IRandomSortParameter>()
                .Configure(ascendent);

            return queryable.Parameter(parameter);
        }

        /// <summary>
        /// Create a facet limit parameter
        /// </summary>
        /// <param name="value">Value of limit</param>
        public static ISolrQueryable<TDocument> FacetLimit<TDocument>(this ISolrQueryable<TDocument> queryable, int value)
            where TDocument : IDocument
        {
            var parameter = ApplicationServices
                .Current
                .GetService<IFacetLimitParameter>()
                .Configure(value);

            return queryable.Parameter(parameter);
        }

        /// <summary>
        /// Create a minimum should match parameter
        /// </summary>
        /// <param name="expression">Expression used to make the mm parameter</param>
        public static ISolrQueryable<TDocument> MinimumShouldMatch<TDocument>(this ISolrQueryable<TDocument> queryable, string expression)
            where TDocument : IDocument
        {
            var parameter = ApplicationServices
                .Current
                .GetService<IMinimumShouldMatchParameter>()
                .Configure(expression);

            return queryable.Parameter(parameter);
        }

        /// <summary>
        /// Create a query field parameter
        /// </summary>
        /// <param name="expression">Expression used to make the mm parameter</param>
        public static ISolrQueryable<TDocument> QueryField<TDocument>(this ISolrQueryable<TDocument> queryable, string expression)
            where TDocument : IDocument
        {
            var parameter = ApplicationServices
                .Current
                .GetService<IQueryFieldParameter>()
                .Configure(expression);

            return queryable.Parameter(parameter);
        }

        /// <summary>
        /// Create a query field parameter
        /// </summary>
        /// <param name="expression">Expression used to find the property name</param>
        /// <param name="functionType">Function used in the spatial filter</param>
        /// <param name="centerPoint">Center point to spatial filter</param>
        /// <param name="distance">Distance from the center point</param>
        public static ISolrQueryable<TDocument> SpatialFilter<TDocument>(this ISolrQueryable<TDocument> queryable, Expression<Func<TDocument, object>> expression, SolrSpatialFunctionType functionType, GeoCoordinate centerPoint, decimal distance)
            where TDocument : IDocument
        {
            var parameter = ApplicationServices
                .Current
                .GetService<ISpatialFilterParameter<TDocument>>()
                .Configure(expression, functionType, centerPoint, distance);

            return queryable.Parameter(parameter);
        }

        /// <summary>
        /// Create a any parameter
        /// </summary>
        /// <param name="name">Name of the parameter</param>
        /// <param name="value">Value of the parameter</param>
        public static ISolrQueryable<TDocument> Any<TDocument>(this ISolrQueryable<TDocument> queryable, string name, string value)
            where TDocument : IDocument
        {
            var parameter = ApplicationServices
                .Current
                .GetService<IAnyParameter>()
                .Configure(name, value);

            return queryable.Parameter(parameter);
        }

        /// <summary>
        /// Create a boost parameter
        /// </summary>
        /// <param name="query">Query used to make boost</param>
        /// <param name="boostFunctionType">Boost type used in calculation. Default is BoostFunctionType.Boost</param>
        public static ISolrQueryable<TDocument> Boost<TDocument>(this ISolrQueryable<TDocument> queryable, IQueryParameterValue query, BoostFunctionType? boostFunctionType = null)
            where TDocument : IDocument
        {
            var parameter = ApplicationServices
                .Current
                .GetService<IBoostParameter<TDocument>>()
                .Configure(query, boostFunctionType ?? BoostFunctionType.Boost);

            return queryable.Parameter(parameter);
        }
    }
}
