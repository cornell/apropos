using Apropos.Domain;
using System;
using System.IO;
using System.Text.RegularExpressions;
using Xunit;

namespace Apropos.Test.Domain
{
    public class ArticleTest
    {
        [Fact]
        public void CreateTest()
        {
            var metadonnees = @"titre: Le bilan et la rééducation vocale-le timbre en question   
sous-titre:  Niveau 1
axe: formation
layout: post.html
ville: Pontivy
departement: 56
date: 
    - 2016-04-11
    - 2016-04-12
dateAffichage: 11-12 mars 2016
organisateur: organisateur-kb
tarif-adherent-salarie: 300
tarif-non-adherent-salarie: 350
tarif-adherent-liberal: 250
tarif-non-adherent-liberal: 300
ogdpc-reference: 32621500013 session 1
duree: 2 jours en présentiel avec le formateur
datePdf: vendredi 11 et samedi 12 mars 2016
horaire: 9h à 17h30 (repas inclus)
lieu: Auberge de jeunesse - 5 rue de Kerbriant - 29000 Brest
effectif: 20
animation:
    - Kristell Bourdin, orthophoniste
    - Natacha Roginski, orthophoniste
financement:
    - dpc
    - horsdpc
    - salarie
photos:
    - 01-formation.jpg
    - 02-formation.jpg
    - 03-formation.jpg
    - 04-formation.jpg          
";

            var sut = Article.CreateTest(metadonnees, null);

            Assert.Equal("Le bilan et la rééducation vocale-le timbre en question", sut.Titre);
            Assert.Equal("Niveau 1", sut.SousTitre);
            Assert.Equal(Axe.Formation, sut.Axe);
            Assert.Equal("post.html", sut.Layout);
            Assert.Equal("Pontivy", sut.Ville);
            Assert.Equal("56", sut.Departement);
            Assert.Equal(new DateTime(2016, 04, 11), sut.Date[0]);
            Assert.Equal(new DateTime(2016, 04, 12), sut.Date[1]);
            Assert.Equal("11-12 mars 2016", sut.DateAffichage);
            Assert.Equal("organisateur-kb", sut.Organisateur);
            Assert.Equal(300, sut.TarifAdherentSalarie);
            Assert.Equal(350, sut.TarifNonAdherentSalarie);
            Assert.Equal(250, sut.TarifAdherentLiberal);
            Assert.Equal(300, sut.TarifNonAdherentLiberal);
            Assert.Equal("32621500013 session 1", sut.OgdpcReference);
            Assert.Equal("2 jours en présentiel avec le formateur", sut.Duree);
            Assert.Equal("vendredi 11 et samedi 12 mars 2016", sut.DatePdf);
            Assert.Equal("9h à 17h30 (repas inclus)", sut.Horaire);
            Assert.Equal("Auberge de jeunesse - 5 rue de Kerbriant - 29000 Brest", sut.Lieu);
            Assert.Equal(20, sut.Effectif);
            Assert.Equal(2, sut.Animation.Count);
            Assert.Equal(3, sut.Financement.Count);
            Assert.Equal(4, sut.Photos.Count);
            Assert.Equal("le-bilan-et-la-reeducation-vocale-le-timbre-en-question", sut.Url);
        }

        [Fact]
        public void Create_When_metadonnee_est_vide_Et_contenu_html_est_vide()
        {
            var sut = Article.CreateTest("", null);
            Assert.NotNull(sut);
        }

        [Fact]
        public void Create_When_metadonnee_est_vide_Et_contenu_html_n_est_pas_vide()
        {
            string contenuHtml = "<p>lorem impsum</p>";
            var sut = Article.CreateTest("", contenuHtml);
            Assert.NotNull(sut);
            Assert.Equal(contenuHtml, sut.ContenuHtml);
        }

        [Fact]
        public void GetResumeTest()
        {
            var sut = Article.CreateTest("", @"<p>Illud autem non dubitatur quod cum esset aliquando virtutum.</p>
<p>In his tractibus navigerum nusquam formavit.</p>");
            sut.LimiterLeNombreDeCaracteres(12);

            Assert.Equal(@"<p>Illud autem non dubitatur quod cum esset aliquando virtutum.</p><p>In his tractibus...</p>", sut.GetResume());
        }

        [Fact]
        public void GetParagraphesTest_01()
        {
            Article sut = Article.CreateTest("", "");
            MatchCollection matches = sut.GetParagraphes(@"<p>Illud autem non dubitatur quod cum esset aliquando virtutum.</p>
<p>In his tractibus navigerum nusquam formavit.</p>");

            Assert.Equal(2, matches.Count);
        }

        [Fact]
        public void GetParagraphesTest_02()
        {
            Article sut = Article.CreateTest("", "");
            MatchCollection matches = sut.GetParagraphes(@"<h2>Objectifs pédagogiques</h2>
<p>Pour le praticien, la métacognition vise tout d'abord à comprendre le fonctionnement cognitif de son patient, ses compétences comme ses
difficultés.</p>
<p>Les postulats adoptés dans cette formation sont ceux de Vygotski selon lesquels l'enfant construit sa cognition avec l'étayage de l'adulte.</p>
<p>Ainsi l'enfant s'approprie le langage oral, l'écrit, les outils logiques, qui deviennent des outils de pensée. Pour Vygotski, l'aide apportée à
l'enfant sera la plus efficace si elle se situe dans sa « Zone Proximale de Développement », c'est-à-dire au niveau de complexité immédiatement supérieur à ce qu'il peut réaliser seul.</p>
<p>Lors de cette formation, les données théoriques actuelles relatives au langage écrit seront présentées avec l'objectif d'une appropriation
par l'orthophoniste. Cette assimilation posera les fondations des connaissances pratiques nécessaires pour maîtriser l'évaluation et la
rééducation des troubles du langage écrit</p>
<h2>Programme</h2>
<h3>Jour 1</h3>
<ul>
<li><p>8h30 - 9h00 : Accueil</p>
</li>
<ul>");

            Assert.Equal(5, matches.Count);
        }

        [Fact]
        public void GetNombreDeMotsTest()
        {
            Article sut = Article.CreateTest("", "");
            Assert.Equal(3, sut.GetNombreDeMots("Illud, aliquando12 15."));
        }

        [Fact]
        public void GetMotsTest()
        {
            Article sut = Article.CreateTest("", "");
            Assert.Equal(@"In his tractibus", sut.GetMots(@"In his tractibus navigerum nusquam formavit.", 3));
        }

        //[Fact]
        //public void Axe_When_formation()
        //{
        //    Article sut = Article.CreateTest("axe: Formâtion", "");
        //    Assert.True(sut.IsAxeFormation);
        //}

        //[Fact]
        //public void Axe_When_Recherche()
        //{
        //    Article sut = Article.CreateTest("axe: RecherChé", "");
        //    Assert.True(sut.IsAxeRecherche);
        //}

        //[Fact]
        //public void Axe_When_prevention()
        //{
        //    Article sut = Article.CreateTest("axe: Préventïon", "");
        //    Assert.True(sut.IsAxePrevention);
        //}
    }
}
