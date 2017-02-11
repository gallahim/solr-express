using Newtonsoft.Json.Linq;
using SolrExpress.Core.Search;
using SolrExpress.Core.Search.Result;
using SolrExpress.Solr5.Search.Parameter;
using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;

namespace SolrExpress.Solr5.UnitTests.Search.Result
{
    public class HighlightingResultTests
    {
        /// <summary>
        /// Where   Using a InformationResult instance
        /// When    Invoking the method "Execute" using a valid JSON
        /// What    Parse to informed concret classes
        /// </summary>
        [Fact]
        public void HighlightingResult001()
        {
            // Arrange
            var parameters = new List<ISearchParameter>();
            var jsonStr = @"
            {
              ""responseHeader"":{
                ""status"":0,
                ""QTime"":10},
                ""response"":{""numFound"":1000,""start"":0,""maxScore"":1.0},
                ""highlighting"":{""0001"":{""_id_"":[""<em>Some<em> Text""]}}
            }";
            var jsonObject = JObject.Parse(jsonStr);
            var result = (IConvertJsonObject)new HighlightingResult<TestDocumentWithAnyPropertyTypes>();

            string highlighting;

            // Act
            result.Execute(parameters, jsonObject);
            highlighting = ((IHighlightingResult<TestDocumentWithAnyPropertyTypes>)result).Data["0001"]?["_id_"].FirstOrDefault();

            // Assert
            Assert.Equal("<em>Some<em> Text", highlighting);
        }
    }
}
