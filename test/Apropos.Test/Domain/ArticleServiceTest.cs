using Apropos.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Collections.Generic;
using Xunit;

namespace Apropos.Test.Domain
{
    public class ArticleServiceTest
    {
        [Fact]
        public void GetArticles_prevention()
        {
            ILogger<ArticleService> logger = Substitute.For<ILogger<ArticleService>>();
            IHostingEnvironment hostingEnvironment = Substitute.For<IHostingEnvironment>();
            hostingEnvironment.ContentRootPath = @"D:\Projets\apropos\src\Apropos.Web";
            FileReader fileReader = new FileReader();

            var service = new ArticleService(hostingEnvironment, logger, fileReader);
            List<Article> articles = service.GetArticles(Axe.Prevention);

            Assert.Equal(4, articles.Count);
        }

        [Fact]
        public void GetArticles_recherche()
        {
            ILogger<ArticleService> logger = Substitute.For<ILogger<ArticleService>>();
            IHostingEnvironment hostingEnvironment = Substitute.For<IHostingEnvironment>();
            hostingEnvironment.ContentRootPath = @"D:\Projets\apropos\src\Apropos.Web";
            FileReader fileReader = new FileReader();

            var service = new ArticleService(hostingEnvironment, logger, fileReader);
            List<Article> articles = service.GetArticles(Axe.Recherche);

            Assert.Equal(0, articles.Count);
        }

        [Fact]
        public void GetArticles_formation()
        {
            ILogger<ArticleService> logger = Substitute.For<ILogger<ArticleService>>();
            IHostingEnvironment hostingEnvironment = Substitute.For<IHostingEnvironment>();
            hostingEnvironment.ContentRootPath = @"D:\Projets\apropos\src\Apropos.Web";
            FileReader fileReader = new FileReader();

            var service = new ArticleService(hostingEnvironment, logger, fileReader);
            List<Article> articles = service.GetArticles(Axe.Formation);

            Assert.Equal(9, articles.Count);
        }
    }
}
