using System;
using Apropos.Domain;

namespace Apropos.Web.ViewModels
{
    public class ContratFormationView
    {
        public string Duree { get; private set; }
        public int Effectif { get; private set; }
        public string Lieu { get; private set; }
        public string Titre { get; private set; }
        public Financement Financement { get; set; }
        public int TarifAdherentLiberal { get; private set; }
        public int TarifAdherentSalarie { get; private set; }
        public int TarifNonAdherentLiberal { get; private set; }
        public int TarifNonAdherentSalarie { get; private set; }
        public string DatePdf { get; private set; }
        public string Horaire { get; private set; }
        public int TarifAdherentLiberalMoinsAccompte { get; private set; }
        public int TarifNonAdherentLiberalMoinsAccompte { get; private set; }
        public bool AfficherInscriptionEtTarif { get; private set; }
        public int TarifDpc { get; private set; }

        private ContratFormationView(Article article, Financement financement)
        {
            Titre = article.Titre;
            Duree = article.Duree;
            Lieu = article.Lieu;
            Effectif = article.Effectif;
            DatePdf = article.DatePdf;
            Horaire = article.Horaire;
            Financement = financement;
            TarifAdherentLiberal = article.TarifAdherentLiberal;
            TarifAdherentSalarie = article.TarifAdherentSalarie;
            TarifNonAdherentLiberal = article.TarifNonAdherentLiberal;
            TarifNonAdherentSalarie = article.TarifNonAdherentSalarie;
            TarifAdherentLiberalMoinsAccompte = article.TarifAdherentLiberal - 50;
            TarifNonAdherentLiberalMoinsAccompte = article.TarifNonAdherentLiberal - 50;
            TarifDpc = article.TarifDpc;
        }

        internal static ContratFormationView Create(Article article, Financement financement)
        {
            return new ContratFormationView(article, financement);
        }
    }
}
