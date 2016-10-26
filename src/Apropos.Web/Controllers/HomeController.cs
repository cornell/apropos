using Apropos.Domain;
using Apropos.Web.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Apropos.Web.Controllers
{
    public class HomeController : Controller
    {
        ILogger<HomeController> _logger;
        private readonly IHostingEnvironment _hostingEnvironment;
        private ArticleService _service;

        public HomeController(IHostingEnvironment hostingEnvironment, ILogger<HomeController> logger, ArticleService service)
        {
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
            _service = service;
        }

        // GET: /<controller>/
        public ViewResult Index()
        {
            var articles = _service.GetArticles();

            var vm = ArticleView.CreateList(articles);
            return View(vm);
        }
    }
}
