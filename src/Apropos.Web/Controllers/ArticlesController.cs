using Apropos.Domain;
using Apropos.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Apropos.Web.Controllers
{
    public class ArticlesController: Controller
    {
        private ArticleService _service;
        private ILogger _logger;

        public ArticlesController(ILogger<ArticlesController> logger, ArticleService service)
        {
            _logger = logger;
            _service = service;
        }

        // GET: Movies
        public ActionResult Index()
        {
            var articles = _service.GetArticles();
            var vm = ArticleView.CreateList(articles);

            return View(vm);
        }

        // GET: Movies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null) return NotFound();
            
            Article article = _service.GetArticles().Find(s => s.Id == id);
            if (article == null)
            {
                return NotFound();
            }

            var vm = ArticleView.Create(article);
            return View(article);
        }

        // GET: Movies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Id,Titre,Axe")]Article article)
        {
            if (ModelState.IsValid)
            {
                _service.GetArticles(article.Axe).Add(article);
                //_service.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(article);
        }

        // GET: Movies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Article article = _service.GetArticles().Find(s => s.Id == id);
            var vm = ArticleView.Create(article);
            if (article == null)
            {
                return NotFound();
            }
            return View(vm);
        }


        // POST: Movies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind("Id,Titre,Axe")]Article article)
        {
            //if (id != article.Id) return NotFound();

            if (ModelState.IsValid)
            {
                //_repository.Entry(movie).State = EntityState.Modified;
                //_service.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(article);
        }

        // GET: Movies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Article article = _service.GetArticles().Find(s => s.Id == id);
            if (article == null)
            {
                return NotFound();
            }
            return View(article);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var articles = _service.GetArticles();
            Article article = articles.Find(s => s.Id == id);
            articles.Remove(article);
            //_service.SaveChanges();
            return RedirectToAction("Index");
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        _repository.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
