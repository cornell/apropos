using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Apropos.Domain;
using System.Linq;
using Apropos.Web.ViewModels;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

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

        // GET: /<controller>/
        public ViewResult Index()
        {
            string contentRootPath = _hostingEnvironment.ContentRootPath;

            var articles = _service.GetArticles(@"\formation\");
            var vm = ArticleView.CreateList(articles);

            return View(vm);
        }

        // GET: /<controller>/
        public ViewResult Article(string url)
        {
            string webRootPath = _hostingEnvironment.WebRootPath;
            string contentRootPath = _hostingEnvironment.ContentRootPath;

            var articles = _service.GetArticles();
            Article article = articles.FirstOrDefault(s => s.Url == url);
            var vm = ArticleView.Create(article);

            return View("Article", vm);
        }
    }
}
