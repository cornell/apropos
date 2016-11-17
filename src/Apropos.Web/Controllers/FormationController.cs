using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Apropos.Domain;
using System.Linq;
using Apropos.Web.ViewModels;

namespace Apropos.Web.Controllers
{
    public class FormationController : Controller
    {
        ILogger<FormationController> _logger;
        private readonly IHostingEnvironment _hostingEnvironment;
        ArticleService _service;

        public FormationController(IHostingEnvironment hostingEnvironment, ILogger<FormationController> logger, ArticleService service)
        {
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
            _service = service;
        }

        //// GET: /<controller>/
        //public ViewResult Index()
        //{
        //    string contentRootPath = _hostingEnvironment.ContentRootPath;

        //    var articles = _service.GetArticles(Axe.Formation);
        //    var vm = ArticleView.CreateList(articles);

        //    return View(vm);
        //}

        // GET: /<controller>/
        public ViewResult Index(string url, string annee)
        {
            string contentRootPath = _hostingEnvironment.ContentRootPath;
            if (string.IsNullOrEmpty(url))
            {

                var articles = _service.GetArticles(Axe.Formation);
                var vm = ArticleView.CreateList(articles);

                return View(vm);
            }
            else
            {
                string webRootPath = _hostingEnvironment.WebRootPath;
                var articles = _service.GetArticles(Axe.Formation);
                Article article = articles.FirstOrDefault(s => s.Url == url && s.Annee == annee);

                var vm = ArticleView.Create(article);

                return View("Article", vm);
            }
        }

        //// GET: /<controller>/
        //public ViewResult Index(string url)
        //{
        //    string webRootPath = _hostingEnvironment.WebRootPath;
        //    string contentRootPath = _hostingEnvironment.ContentRootPath;

        //    var articles = _service.GetArticles(Axe.Formation);
        //    Article article = articles.FirstOrDefault(s => s.Url == url);

        //    var vm = ArticleView.Create(article);

        //    return View("Article", vm);
        //}
    }
}
