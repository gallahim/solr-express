﻿using Newtonsoft.Json.Linq;
using SolrExpress.Core;
using SolrExpress.Core.Search;
using SolrExpress.Core.Search.Result;
using SolrExpress.Core.Utility;
using System.Collections.Generic;
using System.Linq;

namespace SolrExpress.Solr4.Search.Result
{
    /// <summary>
    /// Facet query data builder
    /// </summary>
    public sealed class FacetQueryResult<TDocument> : IFacetQueryResult<TDocument>, IConvertJsonObject
        where TDocument : IDocument
    {
        void IConvertJsonObject.Execute(IEnumerable<ISearchParameter> parameters, JObject jsonObject)
        {
            Checker.IsNull(parameters);
            Checker.IsTrue<UnexpectedJsonFormatException>(jsonObject["facet_counts"]?["facet_queries"] == null, jsonObject.ToString());

            if (!jsonObject["facet_counts"]["facet_queries"].Children().Any())
            {
                this.Data = new Dictionary<string, long>();
            }
            else
            {
                this.Data = jsonObject["facet_counts"]["facet_queries"]
                    .Children()
                    .ToDictionary(
                        k => ((JProperty)k).Name,
                        v => ((JProperty)v).Value.ToObject<long>());
            }
        }

        public IDictionary<string, long> Data { get; set; }

        public object Tag { get; set; }
    }
}
