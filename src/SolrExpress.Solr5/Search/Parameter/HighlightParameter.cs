using Newtonsoft.Json.Linq;
using SolrExpress.Core;
using SolrExpress.Core.Search;
using SolrExpress.Core.Search.Parameter;
using SolrExpress.Core.Utility;

namespace SolrExpress.Solr5.Search.Parameter
{
    public sealed class HighlightParameter<TDocument> : BaseHighlightParameter<TDocument>, ISearchParameterExecute<JObject>
        where TDocument : IDocument
    {
        public HighlightParameter(IExpressionBuilder<TDocument> expressionBuilder) : base(expressionBuilder)
        {
        }

        /// <summary>
        /// Execute the creation of the parameter "highlight"
        /// </summary>
        /// <param name="jObject">JSON object with parameters to request to SOLR</param>
        public void Execute(JObject jObject)
        {
            var fieldName = this._expressionBuilder.GetFieldNameFromExpression(this.Expression);

            // Set switch to turn on highlighting
            var jValue = (JValue)jObject["hl"] ?? new JValue(true);
            jObject["hl"] = jValue;

            // Set fieldname
            var jValueFieldName = (JValue)jObject["hl.fl"] ?? new JValue((string)null);
            var value = $"{fieldName}";

            if (jValueFieldName.Value != null)
            {
                jValueFieldName.Value += $", {value}";
            }
            else
            {
                jValueFieldName.Value = value;
            }

            jObject["hl.fl"] = jValueFieldName;
        }
    }
}
