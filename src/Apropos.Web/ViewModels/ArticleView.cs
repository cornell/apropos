using System;
using System.Collections.Generic;
using Apropos.Domain;

namespace Apropos.Web.ViewModels
{
    public class ArticleView
    {
        public List<string> Animation { get; private set; }
        public Axe Axe { get; private set; }

        public string AxeCss
        {
            get { return this.Axe.ToString().ToLower(); }
        }

        public string ContenuHtml { get; private set; }
        public List<DateTime> Date { get; private set; }
        public string DateAffichage { get; private set; }
        public string DatePdf { get; private set; }
        public string Departement { get; private set; }
        public string Duree { get; private set; }
        public int Effectif { get; private set; }
        public List<Financement> Financement { get; private set; }
        public string Horaire { get; private set; }
        public string Lieu { get; private set; }
        public string OgdpcReference { get; private set; }
        public string Organisateur { get; private set; }
        public List<string> Photos { get; private set; }
        public string Resume { get; private set; }
        public string SousTitre { get; private set; }
        public int TarifAdherentLiberal { get; private set; }
        public int TarifAdherentSalarie { get; private set; }
        public int TarifNonAdherentLiberal { get; private set; }
        public int TarifNonAdherentSalarie { get; private set; }
        public string Titre { get; private set; }
        public string Ville { get; private set; }

        public bool HasVille
        {
            get { return string.IsNullOrEmpty(Ville) == false; }
        }

        public bool HasPhotos
        {
            get { return !(Photos == null || Photos.Count == 0); }
        }

        public bool HasSousTitre
        {
            get { return string.IsNullOrEmpty(SousTitre) == false; }
        }

        public bool IsAxePrevention
        {
            get { return Axe == Axe.Prevention; }
        }

        public bool IsAxeRecherche
        {
            get { return Axe == Axe.Recherche; }
        }

        public bool IsAxeFormation
        {
            get { return Axe == Axe.Formation; }
        }

        public bool HasFinancementDpc
        {
            get { return Financement.Contains(Domain.Financement.Dpc); }
        }

        public bool HasFinancementHorsDpc
        {
            get { return Financement.Contains(Domain.Financement.HorsDpc); }
        }
        public bool HasFinancementSalarie
        {
            get { return Financement.Contains(Domain.Financement.Salarie); }
        }

        public bool HasFinancement { get; set; }

        public bool HasAnimation { get; set; }

        public string Url { get; set; }

        public int Id { get; private set; }

        private ArticleView(Article article)
        {
            if (article == null) return;

            this.Id = article.Id;
            this.Animation = article.Animation;
            this.Axe = article.Axe;
            this.ContenuHtml = article.ContenuHtml;
            this.Date = article.Date;
            this.DateAffichage = article.DateAffichage;
            this.DatePdf = article.DatePdf;
            this.Departement = article.Departement;
            this.Duree = article.Duree;
            this.Effectif = article.Effectif;
            this.Financement = article.Financement;
            this.Horaire = article.Horaire;
            this.Lieu = article.Lieu;
            this.OgdpcReference = article.OgdpcReference;
            this.Organisateur = article.Organisateur;
            this.Photos = article.Photos;
            this.Resume = article.Resume;
            this.SousTitre = article.SousTitre;
            this.TarifAdherentLiberal = article.TarifAdherentLiberal;
            this.TarifAdherentSalarie = article.TarifAdherentSalarie;
            this.TarifNonAdherentLiberal = article.TarifNonAdherentLiberal;
            this.TarifNonAdherentSalarie = article.TarifNonAdherentSalarie;
            this.Titre = article.Titre;
            this.SousTitre = article.SousTitre;
            this.Ville = article.Ville;
            this.Url = article.Url;
        }

        internal static List<ArticleView> CreateList(List<Article> articles)
        {
            var result = new List<ArticleView>();
            foreach (Article article in articles)
            {
                ArticleView vm = ArticleView.Create(article);
                result.Add(vm);
            }
            return result;
        }

        public static ArticleView Create(Article article)
        {
            return new ArticleView(article);
        }

        public static ArticleView CreateTest(Article article)
        {
            article.LimiterLeNombreDeCaracteres(12);
            return new ArticleView(article);
        }
    }
}
