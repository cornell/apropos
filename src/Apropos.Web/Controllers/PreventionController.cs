using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Apropos.Domain;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.IO;
using System;
using Apropos.Web.ViewModels;
using System.Collections.Generic;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Apropos.Web.Controllers
{
    public class PreventionController : Controller
    {
        ILogger<PreventionController> _logger;
        private readonly IHostingEnvironment _hostingEnvironment;
        ArticleService _service;
        IServiceProvider _serviceProvider;

        public PreventionController(IHostingEnvironment hostingEnvironment, ILogger<PreventionController> logger, ArticleService service, IServiceProvider serviceProvider)
        {
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
            _service = service;
            _serviceProvider = serviceProvider;
        }

        // GET: /<controller>/
        public ViewResult Index()
        {
            var articles = _service.GetArticles(Axe.Prevention);
            var vm = ArticleView.CreateList(articles);

            return View(vm);
        }

        // GET: /<controller>/
        public ViewResult Article(string url)
        {            
            var articles = _service.GetArticles(Axe.Prevention);
            Article article = articles.FirstOrDefault(s => s.Url == url);

            ArticleView viewModel = ArticleView.Create(article);
            return View("Article", viewModel);

            //string formationDpc = RenderPartialViewToString("ContratFormationDpcHorsDpc", viewModel);
            //using (StreamWriter htmlWriter = System.IO.File.CreateText("./contrat-formation-dpc.html"))
            //{
            //    htmlWriter.WriteLine(formationDpc);
            //}
            //return v;
        }

        protected string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.ActionDescriptor.ActionName;

            ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                var engine = _serviceProvider.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine; // Resolver.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
                ViewEngineResult viewResult = engine.FindView(ControllerContext, viewName, false);

                ViewContext viewContext = new ViewContext(
                    ControllerContext,
                    viewResult.View,
                    ViewData,
                    TempData,
                    sw,
                    new HtmlHelperOptions() //Added this parameter in
                );

                //Everything is async now!
                var t = viewResult.View.RenderAsync(viewContext);
                t.Wait();

                return sw.GetStringBuilder().ToString();
            }
        }

        //protected string RenderPartialViewToString(string viewName, object model)
        //{
        //    if (string.IsNullOrEmpty(viewName))
        //        viewName = ActionContext.ActionDescriptor.Name;

        //    ViewData.Model = model;

        //    using (StringWriter sw = new StringWriter())
        //    {
        //        var engine = Resolver.GetService(typeof(ICompositeViewEngine))
        //            as ICompositeViewEngine;
        //        ViewEngineResult viewResult = engine.FindPartialView(ActionContext, viewName);

        //        ViewContext viewContext = new ViewContext(
        //            ActionContext,
        //            viewResult.View,
        //            ViewData,
        //            TempData,
        //            sw,
        //            new HtmlHelperOptions() //Added this parameter in
        //        );

        //        //Everything is async now!
        //        var t = viewResult.View.RenderAsync(viewContext);
        //        t.Wait();

        //        return sw.GetStringBuilder().ToString();
        //    }
        //}
    }
}
