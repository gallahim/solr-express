using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolrExpress.Core.Search.Result
{
    public interface IHighlightingResult<TDocument> : IResult
        where TDocument : IDocument
    {
        /// <summary>
        /// Highlighting data
        /// </summary>
        Dictionary<string, Dictionary<string, IEnumerable<string>>> Data { get; }
    }
}
