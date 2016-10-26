using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Apropos.Domain
{
    public static class FileReader
    {
        public static List<string> GetArticles(string path, string filterPattern = null)
        {
            var cheminArticles = new List<string>();
            FindPathArticlesFromDirectory(path, cheminArticles);

            var result = (filterPattern == null) ? cheminArticles : cheminArticles.Where(s => s.Contains(filterPattern)).ToList();
            return result;
        }

        private static List<string> FindPathArticlesFromDirectory(string path, List<string> cheminArticles)
        {
            try
            {
                foreach (string directory in Directory.GetDirectories(path))
                {
                    foreach (string f in Directory.GetFiles(directory, "*.md"))
                    {
                        cheminArticles.Add(f);
                    }
                    FindPathArticlesFromDirectory(directory, cheminArticles);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return cheminArticles;
        }

        public static string Read(string filePath)
        {
            return File.ReadAllText(filePath);
        }

    }
}