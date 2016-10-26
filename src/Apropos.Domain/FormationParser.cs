using System.Text.RegularExpressions;

namespace Apropos.Domain
{
    public class FormationParser
    {
        public string _contenu { get; set; }

        public FormationParser(string contenu)
        {
            _contenu = contenu;
        } 

        public void GetMetadonnee()
        {
            var regex = new Regex("^---(.*)---");
            var v = regex.Match("morenonxmldata<tag1>0002</tag1>morenonxmldata");
            string s = v.Groups[1].ToString();
        }

        public string GetContenu()
        {
            return "";
        }
    }
}