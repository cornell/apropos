using System.Collections.Generic;
using Xunit;
using Apropos.Domain;


namespace Apropos.Test.Domain
{
    public class FileReaderTest
    {
        [Fact]
        public void ReadTest()
        {
            string articleBrut = FileReader.Read(@"_data\\articles\Formation\formation-2016-03.md");
            Assert.NotNull(articleBrut);
        }

        [Fact]
        public void GetArticlesTest()
        {
            List<string> cheminArticles = FileReader.GetArticles("_data\\articles");

            Assert.Equal(4, cheminArticles.Count);
        }

        [Fact]
        public void GetArticles_When_filtré_par_type_d_article()
        {
            List<string> cheminArticles = FileReader.GetArticles("_data\\articles", @"\prevention\");

            Assert.Equal(2, cheminArticles.Count);
        }
    }
}
