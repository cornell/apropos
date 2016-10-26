using Apropos.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Apropos.Test.Domain
{
    public class ArticleDeserializerTest
    {
        [Fact]
        public void Deserialize_When_une_date()
        {
            string metadonnees = @"titre: Le bilan et la rééducation vocale - le timbre en question  
date: 
    - 2015-11-13";
            var input = new StringReader(metadonnees);
            var deserializer = ArticleDeserializer.Create();
            Article article = deserializer.Deserialize(input);

            Assert.Equal(new DateTime(2015, 11, 13), article.Date[0]);
        }

        [Fact]
        public void Deserialize_When_plusieurs_dates()
        {
            string metadonnees = @"titre: Le bilan et la rééducation vocale - le timbre en question  
date: 
    - 2015-11-13
    - 2015-11-14";
            var input = new StringReader(metadonnees);
            var deserializer = ArticleDeserializer.Create();
            Article article = deserializer.Deserialize(input);

            Assert.Equal(new DateTime(2015, 11, 13), article.Date[0]);
            Assert.Equal(new DateTime(2015, 11, 14), article.Date[1]);
        }
    }
}
