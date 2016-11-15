using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Apropos.Core
{
    public static class Slugger
    {
        public static string GenerateSlug(this string phrase)
        {
            string str = RemoveDiacritics(phrase).ToLower();
            // invalid chars           
            str = Regex.Replace(str, @"[^a-z0-9\s-'’]", "");
            // convert multiple spaces into one space   
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // cut and trim 
            str = str.Trim();//.Substring(0, str.Length <= 45 ? str.Length : 45);
            str = Regex.Replace(str, @"[\s'’]", "-"); // hyphens   
            str = Regex.Replace(str, @"---", "-");
            return str;
        }

        public static string RemoveAccent(this string txt)
        {
            byte[] bytes = Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            return Encoding.ASCII.GetString(bytes);
        }

        /// <summary>
        /// https://fr.wikipedia.org/wiki/Diacritiques_utilis%C3%A9s_en_fran%C3%A7ais
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RemoveDiacritics(string text)
        {
            if (string.IsNullOrEmpty(text)) return "";

            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
