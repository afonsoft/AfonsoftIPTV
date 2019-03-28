using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Afonsoft.m3u;

namespace ConsoleAppTestM3U
{
    class Program
    {
        static void Main(string[] args)
        {
            string caminhoPath = @"C:\Users\anogueira\Downloads\Lista\";

            string[] files = Directory.GetFiles(caminhoPath, "*.m3u");


            IList<M3UFile> m3uFiles = new List<M3UFile>();

            foreach (string file in files)
            {
                M3UFile m3u = new M3UFile(file);
                m3uFiles.Add(m3u);
                Console.WriteLine($"Arquivo: {file} Total da lista: {m3u.Count}");
            }

            if (m3uFiles.Count > 0)
            {
                int count = 0;
                foreach (var m3u in m3uFiles)
                {
                    m3u.Save(caminhoPath + count + ".m3u");
                    count++;
                }
            }

            Console.ReadKey();
        }
    }
}
