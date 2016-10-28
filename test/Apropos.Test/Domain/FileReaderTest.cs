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
            string articleBrut = new FileReader().Read(@"_data\\articles\Formation\formation-2016-03.md");
            Assert.NotNull(articleBrut);
        }

        [Fact]
        public void GetArticlesTest()
        {
            List<string> cheminArticles = new FileReader().GetArticles("_data\\articles");

            Assert.Equal(4, cheminArticles.Count);
        }        
    }
}
