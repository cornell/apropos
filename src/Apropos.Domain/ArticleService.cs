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

        public ArticleService(IHostingEnvironment hostingEnvironment, ILogger<ArticleService> logger)
        {
            _logger = logger;
            _path = hostingEnvironment.ContentRootPath + "\\_data\\articles";
        }

        public List<Article> GetArticles(string filterPattern = null)
        {
            if (_articles != null && filterPattern == null) return _articles;

            List<string> cheminArticles = FileReader.GetArticles(_path, filterPattern);
            return GetArticles(cheminArticles);
        }

        public List<Article> GetArticles(List<string> cheminArticles)
        {            
            var articles = new List<Article>();
            cheminArticles.ForEach(chemin =>
            {
                try
                {
                    string articleBrut = FileReader.Read(chemin);
                    ArticleReader articleReader = ArticleReader.Create(articleBrut, _logger);
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