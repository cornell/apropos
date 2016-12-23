using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;

namespace Apropos.Domain
{
    public class ArticleService
    {
        private ILogger<ArticleService> _logger;
        private string _path;
        private List<Article> _articles = null;
        private List<Article> _articlePreventionList = null;
        private List<Article> _articleRechercheList = null;
        private List<Article> _articleFormationList = null;
        private FileReader _fileReader;

        public ArticleService(IHostingEnvironment hostingEnvironment, ILogger<ArticleService> logger, FileReader fileReader)
        {
            _logger = logger;
            _path = hostingEnvironment.ContentRootPath + "/wwwroot/articles";
            _fileReader = fileReader;
            _logger.LogInformation($"chemin racine des articles: {_path}");
        }

        public List<Article> GetArticles()
        {
            List<string> cheminArticles = null;
            if (_articles == null)
            {
                cheminArticles = _fileReader.GetArticles(_path);
                _articles = GetArticles(cheminArticles);
            }
            return _articles;
        }

        public List<Article> GetArticles(Axe axeArticle)
        {
            List<Article> result = null;
            List<string> cheminArticles = null;
            if (_articles == null)
            {
                cheminArticles = _fileReader.GetArticles(_path);
                _logger.LogInformation("nombre d'articles: {0}", cheminArticles.Count);
                _articles = GetArticles(cheminArticles);                
            }
            result = GetArticlesParAxe(axeArticle);

            return result;
        }

        private List<Article> GetArticlesParAxe(Axe axeArticle)
        {
            List<Article> result = null;
            switch (axeArticle)
            {
                case Axe.Prevention:
                    if (_articlePreventionList == null)
                    {
                        _articlePreventionList = _articles.Where(a => a.Axe == axeArticle).ToList();
                    }
                    result = _articlePreventionList;
                    break;
                case Axe.Recherche:
                    if (_articleRechercheList == null)
                    {
                        _articleRechercheList = _articles.Where(a => a.Axe == axeArticle).ToList();
                    }
                    result = _articleRechercheList;
                    break;
                case Axe.Formation:
                    if (_articleFormationList == null)
                    {
                        _articleFormationList = _articles.Where(a => a.Axe == axeArticle).ToList();
                    }
                    result = _articleFormationList;
                    break;
                default:
                    break;
            }
            return result;
        }

        private List<Article> GetArticles(List<string> cheminArticles)
        {
            var articles = new List<Article>();
            cheminArticles.ForEach(chemin =>
            {
                try
                {
                    string articleBrut = _fileReader.Read(chemin);
                    ArticleReader articleReader = ArticleReader.Create(articleBrut, chemin, _logger);
                    Article article = articleReader.Read();
                    articles.Add(article);
                }
                catch (Exception ex)
                {
                    _logger.LogError("{messageError} {chemin}", ex, chemin);
                }
            });

            var articleOrdonnes = articles.OrderByDescending(s => s.Date[0]).ToList();
            _articles = articleOrdonnes;

            return articleOrdonnes;
        }
    }
}