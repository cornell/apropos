using Apropos.Domain;
using System;
using System.Collections.Generic;

namespace Apropos.Web.Models
{
    public class ArticleRepository
    {
        public List<Article> Articles { get; set; }

        public ArticleRepository()
        {
            Articles = new List<Article>
        {
            new Article { Id = new Random().Next(), Titre = "La grande vadrouille" },
            new Article { Id = new Random().Next(), Titre = "Le corniaud" }
        };
        }


        internal void SaveChanges()
        {

        }
    }
}
