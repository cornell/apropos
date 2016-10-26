using System.ComponentModel.DataAnnotations;

namespace Apropos.Web.ViewModels
{
    public class ArticleEditionView
    {
        [Required]
        public string Metadonnees { get; set; }

        [Required]
        public string Contenu { get; set; }
    }
}
