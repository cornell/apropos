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

namespace Apropos.Web.Controllers
{
    public class PreventionController : Controller
    {
        ILogger<PreventionController> _logger;
        private readonly IHostingEnvironment _hostingEnvironment;
        ArticleService _service;
        private ArticleViewService _viewService;
        IServiceProvider _serviceProvider;

        public PreventionController(IHostingEnvironment hostingEnvironment, ILogger<PreventionController> logger, ArticleService service, ArticleViewService viewService, IServiceProvider serviceProvider)
        {
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
            _service = service;
            _viewService = viewService;
            _serviceProvider = serviceProvider;
        }

        public ViewResult Index(string url, string annee)
        {
            string contentRootPath = _hostingEnvironment.ContentRootPath;
            if (string.IsNullOrEmpty(url))
            {

                var articles = _service.GetArticles(Axe.Prevention);
                var vm = ArticleView.CreateList(articles);

                return View(vm);
            }
            else
            {
                string webRootPath = _hostingEnvironment.WebRootPath;
                var articles = _service.GetArticles(Axe.Prevention);
                Article article = articles.FirstOrDefault(s => s.Url == url && s.Annee == annee);

                var vm = _viewService.GetArticle(article);

                return View("Article", vm);
            }
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
