using Apropos.Core;
using Xunit;

namespace Apropos.Test.Core
{
    public class SluggerTest
    {
        [Fact]
        public void SlugTest()
        {
            Assert.Equal("le-bilan-et-la-reeducation-vocale-le-timbre-en-question", Slugger.GenerateSlug(" Le bilan et la rééducation vocale - le timbre en question  "));
            Assert.Equal("a-propos-d-orthophonie-2", Slugger.GenerateSlug("à propos d'orthophonie 2"));
            Assert.Equal("l-audition", Slugger.GenerateSlug("l'audition"));
            Assert.Equal("l-evaluation", Slugger.GenerateSlug("l’évaluation"));
        }

        [Fact]
        public void RemoveDiacriticsTest()
        {
            Assert.Equal("aeeuiiuuooeecaeuio", Slugger.RemoveDiacritics("àéèùîïüûôöëêçaeuio"));
        }

        [Fact]
        public void RemoveDiacritics_When_entree_est_vide()
        {
            Assert.Equal("", Slugger.RemoveDiacritics(""));
        }

        [Fact]
        public void RemoveDiacritics_When_entree_est_null()
        {
            Assert.Equal("", Slugger.RemoveDiacritics(null));
        }
    }
}