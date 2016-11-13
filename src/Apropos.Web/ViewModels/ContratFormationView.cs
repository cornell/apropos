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
        public int Tarif { get; private set; }
        public string DatePdf { get; private set; }
        public string Horaire { get; private set; }

        private ContratFormationView(Article article, Financement financement)
        {
            Titre = article.Titre;
            Duree = article.Duree;
            Lieu = article.Lieu;
            Effectif = article.Effectif;
            DatePdf = article.DatePdf;
            Horaire = article.Horaire;
            Financement = financement;
            if(financement == Financement.Dpc)
            {
                Tarif = article.TarifAdherentLiberal;
            }
        }

        internal static ContratFormationView Create(Article article, Financement financement)
        {
            return new ContratFormationView(article, financement);
        }
    }
}
