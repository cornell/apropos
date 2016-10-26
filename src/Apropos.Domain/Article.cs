using Apropos.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using YamlDotNet.Serialization;

namespace Apropos.Domain
{
    public class Article
    {
        private List<string> _photos;
        private List<Financement> _financement;
        private List<string> _animation;
        private static int NOMBRE_DE_MOTS = 20;
        private int _id;

        public int Id
        {
            get
            {
                if(_id == 0)
                {
                    _id = new Random().Next();
                }
                return _id;
            }
            set { _id = value; }
        }

        public string Titre { get; set; }

        [YamlMember(Alias = "sous-titre")]
        public string SousTitre { get; set; }

        public Axe Axe { get; set; }

        public string Layout { get; set; }

        public string Ville { get; set; }

        public string Departement { get; set; }

        public List<DateTime> Date { get; set; }

        public string DateAffichage { get; set; }

        public string Organisateur { get; set; }

        [YamlMember(Alias = "tarif-adherent-salarie", SerializeAs = typeof(int))]
        public int TarifAdherentSalarie { get; set; }

        [YamlMember(Alias = "tarif-non-adherent-salarie")]
        public int TarifNonAdherentSalarie { get; set; }

        [YamlMember(Alias = "tarif-adherent-liberal")]
        public int TarifAdherentLiberal { get; set; }

        [YamlMember(Alias = "tarif-non-adherent-liberal")]

        public int TarifNonAdherentLiberal { get; set; }

        [YamlMember(Alias = "ogdpc-reference")]
        public string OgdpcReference { get; set; }

        public string Duree { get; set; }

        public string DatePdf { get; set; }

        public string Horaire { get; set; }

        public string Lieu { get; set; }

        public int Effectif { get; set; }

        public List<string> Animation
        {
            get { return _animation ?? new List<string>(); }
            set { _animation = value; }
        }

        public List<Financement> Financement
        {
            get { return _financement ?? new List<Financement>(); }
            set { _financement = value; }
        }

        public List<string> Photos
        {
            get { return _photos ?? new List<string>(); }
            set { _photos = value; }
        }        

        public string ContenuHtml { get; set; }

        public string Resume { get; private set; }       

        public string Url
        {
            get { return Slugger.GenerateSlug(Titre); }
        }

        public void LimiterLeNombreDeCaracteres(int limite)
        {
            NOMBRE_DE_MOTS = limite;
        }

        public Article(){}

        private Article(int nombreDeMots)
        {
            NOMBRE_DE_MOTS = nombreDeMots;
        }

        /// <summary>
        /// Obtient le résumé d'un article.
        /// </summary>
        public string GetResume()
        {
            StringBuilder result = new StringBuilder();
            MatchCollection paragraphes = GetParagraphes(ContenuHtml);
            if (paragraphes?.Count > 0)
            {
                int nombreDeMotsPrecedent = 0;
                int nombreDeMots = 0;
                foreach (Match paragraphe in paragraphes)
                {
                    string paragrapheHtml = "";
                    string paragrapheBrut = paragraphe.Groups[1].Value;
                    nombreDeMots = GetNombreDeMots(paragrapheBrut);
                    if(nombreDeMotsPrecedent + nombreDeMots <= NOMBRE_DE_MOTS)
                    {
                        paragrapheHtml = paragraphe.Groups[0].Value;
                        result.Append(paragrapheHtml);
                    }
                    else
                    {
                        int nombreDeMotsACapturer = NOMBRE_DE_MOTS - nombreDeMotsPrecedent;
                        string paragrapheTronque = GetMots(paragrapheBrut, nombreDeMotsACapturer);
                        paragrapheHtml = AjouterBaliseHtml(paragrapheTronque);
                        result.Append(paragrapheHtml);
                        break;
                    }
                    nombreDeMotsPrecedent += nombreDeMots;
                }
                // match the first seven items separated by spaces / tabs / newlines, ignoring any trailing punctuation or non-word characters.
               
            }
            return result.ToString();
        }

        private string AjouterBaliseHtml(string paragrapheTronque)
        {
            return "<p>" + paragrapheTronque + "...</p>";
        }

        public string GetMots(string paragrapheBrut, int nombreDeMotsACapturer)
        {
            if (string.IsNullOrEmpty(paragrapheBrut)) return "";

            string result =  "";
            nombreDeMotsACapturer -= 1;
            Match match = Regex.Match(paragrapheBrut, @"^((?:\S+\s+){" + nombreDeMotsACapturer + @"}\S+).*");
            if(match.Success)
            {
                result = match.Groups[1].Value;
            }
            return result;
        }

        public MatchCollection GetParagraphes(string paragraphesHtml)
        {
            if (string.IsNullOrEmpty(paragraphesHtml)) return null;

            MatchCollection result = null;
            string pattern = @"<p>(.+?)<\/p>";
            Regex regex = new Regex(pattern, RegexOptions.Singleline);
            if(regex.IsMatch(paragraphesHtml))
            {
                result = regex.Matches(paragraphesHtml);
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paragrapheBrut">paragraphe sans les balises html</param>
        /// <returns></returns>
        public int GetNombreDeMots(string paragrapheBrut)
        {
            if (string.IsNullOrEmpty(paragrapheBrut)) return 0;

            int result = 0;
            MatchCollection collection = Regex.Matches(paragrapheBrut, @"[\w]+");
            result = collection.Count;
            return result;
        }

        public static Article Create(string metadonnees, string contenuHtml, ArticleDeserializer deserializer)
        {
            var input = new StringReader(metadonnees);
            //var yaml = new Deserializer(namingConvention: new CamelCaseNamingConvention());
            Article article = deserializer.Deserialize(input);
            if (article != null)
            {
                article.ContenuHtml = contenuHtml;
                article.Resume = article.GetResume();
            }
            if(article == null)
            {
                article = new Article
                {
                    ContenuHtml = contenuHtml
                };
            }
            return article;
        }

        public static Article CreateTest(string metadonnees, string contenuHtml)
        {
            Article article = Create(metadonnees, contenuHtml, ArticleDeserializer.Create());

            article.LimiterLeNombreDeCaracteres(12);
            return article;
        }
    }
}