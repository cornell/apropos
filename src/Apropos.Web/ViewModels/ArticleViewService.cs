using System;
using Apropos.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Apropos.Web.ViewModels
{
    public class ArticleViewService
    {
        private ILogger<ArticleViewService> _logger;
        private string _path;
        private List<KeyValuePair<string, ArticleView>> _articles = new List<KeyValuePair<string, ArticleView>>();

        public ArticleViewService(IHostingEnvironment hostingEnvironment, ILogger<ArticleViewService> logger)
        {
            _logger = logger;
            _path = hostingEnvironment.WebRootPath + "/articles";
            _logger.LogInformation($"chemin racine des articles: {_path}");
        }

        public ArticleView GetArticle(Article article)
        {
            ArticleView result;
            if(_articles.Exists(s => s.Key == article.UrlComplete))
            {
                KeyValuePair<string, ArticleView> keyValue = _articles.Find(s => s.Key == article.UrlComplete);
                result = keyValue.Value;
            }
            else
            {
                result = ArticleView.Create(article);
                _articles.Add(new KeyValuePair<string, ArticleView>(result.UrlComplete, result));
            }
            return result;
        }
    }
}
