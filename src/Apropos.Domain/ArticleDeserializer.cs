using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Apropos.Domain
{
    public class ArticleDeserializer
    {
        private ArticleDeserializer(){}

        public Article Deserialize(StringReader input)
        {
            var yaml = new Deserializer(namingConvention: new CamelCaseNamingConvention());
            yaml.RegisterTagMapping("date", typeof(DateTime));
            yaml.RegisterTagMapping("axe", typeof(Axe));
            //yaml.RegisterTagMapping("tarif-adherent-salarie", typeof(int));

            Article article = yaml.Deserialize<Article>(input);

            return article;
        }

        public static ArticleDeserializer Create()
        {
            return new ArticleDeserializer();
        }
    }
}
