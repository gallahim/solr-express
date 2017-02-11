using System;
using System.Linq.Expressions;

namespace SolrExpress.Core.Search.Parameter
{
    /// <summary>
    /// Signatures to use in sort parameter
    /// </summary>
    public interface IHighlightParameter<TDocument> : ISearchParameter
        where TDocument : IDocument
    {
        /// <summary>
        /// Configure current instance
        /// </summary>
        /// <param name="expression">Expression used to find the property name</param>
        IHighlightParameter<TDocument> Configure(Expression<Func<TDocument, object>> expression);

        /// <summary>
        /// Expression used to find the property name
        /// </summary>
        Expression<Func<TDocument, object>> Expression { get; }
    }
}
