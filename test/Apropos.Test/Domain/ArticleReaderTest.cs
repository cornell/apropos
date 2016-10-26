using Apropos.Domain;
using Xunit;

namespace Apropos.Test.Domain
{
    public class ArticleReaderTest
    {
        [Fact]
        public void ReadTest_01()
        {
            string articleBrut = @"---         
---
lorem ipsum";
            var articleReader = ArticleReader.CreateForTest(articleBrut);
            Article article = articleReader.Read();
            Assert.Equal("<p>lorem ipsum</p>\n", article.ContenuHtml);
        }        
    }
}
