using Moq;
using Newtonsoft.Json.Linq;
using SolrExpress.Core;
using SolrExpress.Core.Utility;
using SolrExpress.Solr5.Search.Parameter;
using System;
using Xunit;

namespace SolrExpress.Solr5.UnitTests.Search.Parameter
{
    public class HighlightParameterTests
    {
        /// <summary>
        /// Where   Using a HighlightParameter instance
        /// When    Invoking the method "Execute"
        /// What    Create a valid JSON
        /// </summary>
        [Fact]
        public void HighlightParameter001()
        {
            // Arrange
            var expected = JObject.Parse(@"
            {
              ""hl"": true,
              ""hl.fl"": ""_id_""
            }");
            string actual;
            var jObject = new JObject();
            var expressionCache = new ExpressionCache<TestDocument>();
            var expressionBuilder = (IExpressionBuilder<TestDocument>)new ExpressionBuilder<TestDocument>(expressionCache);
            var parameter = new HighlightParameter<TestDocument>(expressionBuilder);
            parameter.Configure(q => q.Id);

            // Act
            parameter.Execute(jObject);
            actual = jObject.ToString();

            // Assert
            Assert.Equal(expected.ToString(), actual);
        }

        /// <summary>
        /// Where   Using a HighlightParameter instance
        /// When    Create the instance with null
        /// What    Throws ArgumentNullException
        /// </summary>
        [Fact]
        public void HighlightParameter002()
        {
            // Arrange
            var expressionCache = new ExpressionCache<TestDocument>();
            var expressionBuilder = (IExpressionBuilder<TestDocument>)new ExpressionBuilder<TestDocument>(expressionCache);
            var parameter = new HighlightParameter<TestDocument>(expressionBuilder);

            // Act / Assert
            Assert.Throws<ArgumentNullException>(() => parameter.Configure(null));
        }

        /// <summary>
        /// Where   Using a HighlightParameter instance
        /// When    Invoking the method "Validate" using field Indexed=true
        /// What    Valid is true
        /// </summary>
        [Fact]
        public void HighlightParameter003()
        {
            // Arrange
            bool isValid;
            string errorMessage;
            var expressionCache = new ExpressionCache<TestDocumentWithAttribute>();
            var expressionBuilder = (IExpressionBuilder<TestDocumentWithAttribute>)new ExpressionBuilder<TestDocumentWithAttribute>(expressionCache);
            var parameter = new HighlightParameter<TestDocumentWithAttribute>(expressionBuilder);
            parameter.Configure(q => q.Indexed);

            // Act
            parameter.Validate(out isValid, out errorMessage);

            // Assert
            Assert.True(isValid);
        }
       
    }
}
