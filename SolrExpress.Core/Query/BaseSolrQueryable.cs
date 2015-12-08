﻿using SolrExpress.Core.Builder;
using SolrExpress.Core.Entity;
using SolrExpress.Core.Exception;
using SolrExpress.Core.Helper;
using SolrExpress.Core.Parameter;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SolrExpress.Core.Query
{
    /// <summary>
    /// SOLR queryable
    /// </summary>
    public abstract class BaseSolrQueryable<TDocument>
        where TDocument : IDocument
    {
        /// <summary>
        /// Configurations about SolrQueriable behavior
        /// </summary>
        private readonly SolrQueryConfiguration _configuration;

        /// <summary>
        /// Provider used to resolve the expression
        /// </summary>
        private readonly IProvider _provider;

        /// <summary>
        /// List of the parameters arranged in the queryable class
        /// </summary>
        private readonly List<IParameter> _parameters = new List<IParameter>();

        /// <summary>
        /// List of the query interceptors arranged in the queryable class
        /// </summary>
        private readonly List<IQueryInterceptor> _queryInterceptors = new List<IQueryInterceptor>();

        /// <summary>
        /// List of the result interceptors arranged in the queryable class
        /// </summary>
        private readonly List<IResultInterceptor> _resultInterceptors = new List<IResultInterceptor>();

        /// <summary>
        /// Factory used to resolve builder creation in Linq facilities
        /// </summary>
        protected readonly IBuilderFactory<TDocument> _builderFactory;

        /// <summary>
        /// Factory used to resolve parameter creation in Linq facilities
        /// </summary>
        protected readonly IParameterFactory<TDocument> _paramaterFactory;

        /// <summary>
        /// Default constructor of the class
        /// </summary>
        /// <param name="provider">Provider used to resolve the expression</param>
        /// <param name="paramaterFactory">Factory used to resolve parameter creation in Linq facilities</param>
        /// <param name="builderFactory">Factory used to resolve builder creation in Linq facilities</param>
        /// <param name="configuration">Configurations about SolrQueriable behavior</param>
        protected BaseSolrQueryable(IProvider provider, IParameterFactory<TDocument> paramaterFactory, IBuilderFactory<TDocument> builderFactory, SolrQueryConfiguration configuration = null)
        {
            ThrowHelper<ArgumentNullException>.If(provider == null);
            ThrowHelper<ArgumentNullException>.If(paramaterFactory == null);
            ThrowHelper<ArgumentNullException>.If(builderFactory == null);

            this._provider = provider;
            this._paramaterFactory = paramaterFactory;
            this._builderFactory = builderFactory;
            this._configuration = configuration ?? new SolrQueryConfiguration
            {
                FailFast = true
            };
        }

        /// <summary>
        /// Add a parameter to the query
        /// </summary>
        /// <param name="parameter">The parameter to add in the query</param>
        /// <returns>Itself</returns>
        public BaseSolrQueryable<TDocument> Parameter(IParameter parameter)
        {
            ThrowHelper<ArgumentNullException>.If(parameter == null);

            if (this._parameters.Any(q => q.GetType() == parameter.GetType()) && !parameter.AllowMultipleInstances)
            {
                throw new AllowMultipleInstanceOfParameterTypeException(parameter.ToString());
            }

            var parameterValidation = parameter as IValidation;

            if (this._configuration.FailFast && parameterValidation != null)
            {
                bool isValid;
                string errorMessage;

                parameterValidation.Validate(out isValid, out errorMessage);

                if (!isValid)
                {
                    throw new InvalidParameterTypeException(parameterValidation.ToString(), errorMessage);
                }
            }

            this._parameters.Add(parameter);

            return this;
        }

        /// <summary>
        /// Add a query interceptor to the queryable
        /// </summary>
        /// <param name="interceptor">The query interceptor to add in the queryable</param>
        /// <returns>Itself</returns>
        public BaseSolrQueryable<TDocument> QueryInterceptor(IQueryInterceptor interceptor)
        {
            ThrowHelper<ArgumentNullException>.If(interceptor == null);

            this._queryInterceptors.Add(interceptor);

            return this;
        }

        /// <summary>
        /// Add a result interceptor to the queryable
        /// </summary>
        /// <param name="interceptor">The result interceptor to add in the queryable</param>
        /// <returns>Itself</returns>
        public BaseSolrQueryable<TDocument> ResultInterceptor(IResultInterceptor interceptor)
        {
            ThrowHelper<ArgumentNullException>.If(interceptor == null);

            this._resultInterceptors.Add(interceptor);

            return this;
        }

        /// <summary>
        /// Execute the search in the solr with informed parameters
        /// </summary>
        /// <returns>Solr result</returns>
        public SolrQueryResult<TDocument> Execute()
        {
            var query = this._provider.GetQuery(this._parameters);

            this._queryInterceptors.ForEach(q => q.Execute(ref query));

            var json = this._provider.Execute(query);

            this._resultInterceptors.ForEach(q => q.Execute(ref json));

            return new SolrQueryResult<TDocument>(this._builderFactory, json);
        }
    }
}