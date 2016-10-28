using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Apropos.Domain
{
    public class FileReader
    {
        public List<string> GetArticles(string path)
        {
            var cheminArticles = new List<string>();
            FindPathArticlesFromDirectory(path, cheminArticles);

            return cheminArticles;
        }

        private List<string> FindPathArticlesFromDirectory(string path, List<string> cheminArticles)
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

        public string Read(string filePath)
        {
            return File.ReadAllText(filePath);
        }
    }
}