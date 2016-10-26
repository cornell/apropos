using Apropos.Domain;
using Markdig;
using Microsoft.Extensions.Logging;
using System;
using System.Text.RegularExpressions;

namespace Apropos.Domain
{
    public class ArticleReader
    {
        private string _articleBrut;
        private ILogger _logger;

        private ArticleReader(string articleBrut, ILogger logger)
        {
            _articleBrut = articleBrut;
            _logger = logger;
        }

        public Article Read()
        {
            ArticleBrut articleBrut = Parse();
            string contenuHtml = Markdown.ToHtml(articleBrut.Contenu);
            var result = Article.Create(articleBrut.Metadonnees, contenuHtml, ArticleDeserializer.Create());
            return result;
        }

        private ArticleBrut Parse()
        {
            string pattern = string.Format("^---(.*)---(.*)", Environment.NewLine);
            var regex = new Regex(pattern, RegexOptions.Singleline);
            var v = regex.Match(_articleBrut);
            string metadonnees = v.Groups[1].ToString().Trim();
            string contenu = v.Groups[2].ToString().Trim();

            return new ArticleBrut(metadonnees, contenu);
        }

        public static ArticleReader CreateForTest(string articleBrut)
        {
            return ArticleReader.Create(articleBrut, null);
        }

        public static ArticleReader Create(string articleBrut, ILogger logger)
        {
            return new ArticleReader(articleBrut, logger);
        }

        public class ArticleBrut
        {
            public string Metadonnees { get; }
            public string Contenu { get; }

            public ArticleBrut(string metadonneesBrut, string contenu)
            {
                Metadonnees = metadonneesBrut;
                Contenu = contenu;
            }
        }
    }
}