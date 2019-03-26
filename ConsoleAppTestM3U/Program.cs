using System;
using System.Collections.Generic;
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
            string caminhoM3U = @"C:\Users\anogueira\Downloads\Lista\LISTA 07-01-19.m3u";

            M3UFile m3u = new M3UFile(caminhoM3U);
            
            if (m3u.Count != 0)
            {
                Console.WriteLine($"Total da lista: {m3u.Count}");
            }

            Console.ReadKey();
        }
    }
}
