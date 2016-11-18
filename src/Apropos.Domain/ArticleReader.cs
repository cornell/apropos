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
        private string _chemin;

        private ArticleReader(string articleBrut, string chemin, ILogger logger)
        {
            _articleBrut = articleBrut;
            _logger = logger;
            _chemin = chemin;
        }

        public Article Read()
        {
            ArticleBrut articleBrut = Parse();
            string contenuHtml = GetContenuHtml(articleBrut);
            Article result = null;
            try
            {
                ArticleDeserializer deserializer = ArticleDeserializer.Create();
                result = Article.Create(articleBrut.Metadonnees, contenuHtml, _chemin, deserializer);

            }
            catch(Exception ex)
            {
                _logger.LogError($"erreur sur la désérialisation du fichier {_chemin}: [{ex.Message} -> {ex.InnerException.Message}]", ex);
            }
            return result;
        }

        private string GetContenuHtml(ArticleBrut articleBrut)
        {
            string result = "";
            try
            {
                result = Markdown.ToHtml(articleBrut.Contenu);
            }
            catch (Exception ex)
            {
                _logger.LogError($"erreur Markdown sur le fichier {_chemin}", ex);
            }
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
            return ArticleReader.Create(articleBrut, null, null);
        }

        public static ArticleReader Create(string articleBrut, string chemin, ILogger logger)
        {
            return new ArticleReader(articleBrut, chemin, logger);
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