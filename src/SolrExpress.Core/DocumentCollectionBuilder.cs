﻿using SolrExpress.Core.DependencyInjection;
using SolrExpress.Core.Extension.Internal;
using SolrExpress.Core.Query;
using SolrExpress.Core.Update;

namespace SolrExpress.Core
{
    /// <summary>
    /// Options builder to control SOLR Query behavior
    /// </summary>
    public class DocumentCollectionBuilder<TDocument>
        where TDocument : IDocument
    {
        /// <summary>
        /// Collection options
        /// </summary>
        private DocumentCollectionOptions<TDocument> _options;

        /// <summary>
        /// Default constructor of class
        /// </summary>
        public DocumentCollectionBuilder()
        {
            this._options = new DocumentCollectionOptions<TDocument>();
        }

        /// <summary>
        /// Use indicated host address in prepared options
        /// </summary>
        /// <param name="options">Options to be used</param>
        /// <returns>Current instance</returns>
        public DocumentCollectionBuilder<TDocument> UseOptions(DocumentCollectionOptions options)
        {
            var shadowOptions = new DocumentCollectionOptions<TDocument>();
            options.CopyOptionsTo(out shadowOptions);

            this._options = shadowOptions;

            return this;
        }

        /// <summary>
        /// Use indicated host address in prepared options
        /// </summary>
        /// <param name="hostAddress">Host address to use in options</param>
        /// <returns>Current instance</returns>
        public DocumentCollectionBuilder<TDocument> UseHostAddress(string hostAddress)
        {
            this._options.HostAddress = hostAddress;

            return this;
        }

        /// <summary>
        /// Create configured instance of DocumentCollection<TDocument>
        /// </summary>
        /// <returns>Instance of DocumentCollection<TDocument></returns>
#if NET40 || NET45
        public DocumentCollection<TDocument> Create()
#else
        internal DocumentCollection<TDocument> Create()
#endif
        {
            var documentCollection = new DocumentCollection<TDocument>(this._options);

            ApplicationServices
                .Current
                .AddTransient<ISolrQueryable<TDocument>, SolrQueryable<TDocument>>()
                .AddTransient<ISolrAtomicUpdate<TDocument>, SolrAtomicUpdate<TDocument>>()
                .AddTransient<IDocumentCollection<TDocument>, DocumentCollection<TDocument>>(documentCollection);

            return documentCollection;
        }
    }
}