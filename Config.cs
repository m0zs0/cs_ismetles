using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace ism_console
{
    public static class Config
    {
        public static readonly string CsvFilename = "users.csv";
        public static readonly string CsvFolder = Path.Combine(GetSolutionRoot(), "data");
        public static readonly char CsvSeparator = ';';

        public static string CsvFullPath => Path.Combine(CsvFolder, CsvFilename);


        private static string GetSolutionRoot()
        {
            // A futó projekt könyvtárából visszalépünk a solution szintre
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string solutionRoot = Directory.GetParent(baseDir).Parent.Parent.Parent.Parent.FullName;
            return solutionRoot;
        }

    }
}
