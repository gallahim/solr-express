using Newtonsoft.Json.Linq;
using SolrExpress.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolrExpress.Core.Search.Result
{
    public class HighlightingResult<TDocument> : IHighlightingResult<TDocument>, IConvertJsonObject
        where TDocument : IDocument
    {
        void IConvertJsonObject.Execute(IEnumerable<ISearchParameter> parameters, JObject jsonObject)
        {
            Checker.IsNull(parameters);
            Checker.IsTrue<UnexpectedJsonFormatException>(jsonObject["highlighting"] == null, new[] { jsonObject.ToString() });

            this.Data = jsonObject["highlighting"].ToObject<Dictionary<string, Dictionary<string, IEnumerable<string>>>>();
        }

        /// <summary>
        /// Facet data
        /// </summary>
        public Dictionary<string, Dictionary<string, IEnumerable<string>>> Data { get; set; }
    }
}
