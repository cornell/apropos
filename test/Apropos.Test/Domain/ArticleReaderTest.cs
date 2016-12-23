using Apropos.Domain;
using Microsoft.Extensions.Logging;
using NSubstitute;
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
            var logger = Substitute.For<ILogger>();            

            var articleReader = ArticleReader.CreateForTest(articleBrut, logger);
            Article article = articleReader.Read();
            Assert.Equal("<p>lorem ipsum</p>\n", article.ContenuHtml);
        }

        [Fact]
        public void Read_When_Yaml_parsing_exception()
        {
            string articleBrut = @"---
titre: monTitre
sous-titre
---
lorem ipsum";
            var logger = Substitute.For<ILogger>();
            var articleReader = ArticleReader.CreateForTest(articleBrut, logger);
            Article article = articleReader.Read();

            logger.Received().LogError($"coucou");
            Assert.Equal("<p>lorem ipsum</p>\n", article.ContenuHtml);
        }
    }
}
