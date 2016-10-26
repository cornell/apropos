using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Apropos.Domain;
using System.Linq;
using Apropos.Web.ViewModels;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Apropos.Web.Controllers
{
    public class RechercheController : Controller
    {
        ILogger<RechercheController> _logger;
        private readonly IHostingEnvironment _hostingEnvironment;
        ArticleService _service;

        public RechercheController(IHostingEnvironment hostingEnvironment, ILogger<RechercheController> logger, ArticleService service)
        {
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
            _service = service;
        }

        // GET: /<controller>/
        public ViewResult Index()
        {
            var articles = _service.GetArticles(@"\recherche\");
            var vm = ArticleView.CreateList(articles);

            return View(vm);
        }

        // GET: /<controller>/
        public ViewResult Article(string url)
        {
            var articles = _service.GetArticles();
            Article article = articles.Where(s => s.Titre == url).FirstOrDefault();
            var vm = ArticleView.Create(article);

            return View("Article", vm);
        }

    }
}
