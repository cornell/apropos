using Apropos.Domain;
using Apropos.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Apropos.Test.ViewModels
{
    public class ArticleViewTest
    {
        [Fact]
        public void Photos_When_metadonnee_est_vide()
        {
            var sut = _CreateArticleView("", "");
            Assert.False(sut.HasPhotos);
            Assert.Equal(0, sut.Photos.Count);
        }


        [Fact]
        public void Financement_When_metadonnee_est_vide()
        {
            var sut = _CreateArticleView("", "");
            Assert.False(sut.HasFinancement);
            Assert.Equal(0, sut.Financement.Count);
        }


        [Fact]
        public void Animation_When_metadonnee_est_vide()
        {
            var sut = _CreateArticleView("", "");
            Assert.False(sut.HasAnimation);
            Assert.Equal(0, sut.Animation.Count);
        }

        [Fact]
        public void Axe_When_financement_dpc()
        {
            var sut = _CreateArticleView(@"financement:
    - dpc
    - horsdpc
    - salarie", "");
            Assert.True(sut.HasFinancementDpc);
        }

        [Fact]
        public void Axe_When_financement_horsDpc()
        {
            var sut = _CreateArticleView(@"financement:
    - dpc
    - horsdpc
    - salarie", "");
            Assert.True(sut.HasFinancementHorsDpc);
        }

        [Fact]
        public void Axe_When_financement_salarie()
        {
            var sut = _CreateArticleView(@"financement:
    - dpc
    - horsdpc
    - salarie", "");
            Assert.True(sut.HasFinancementSalarie);
        }

        private ArticleView _CreateArticleView(string metadonnees, string contenuHtml)
        {
            var article = Article.CreateTest(metadonnees, contenuHtml);
            return ArticleView.CreateTest(article);
        }
    }
}
