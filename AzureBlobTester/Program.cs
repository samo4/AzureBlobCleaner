using AzureBlobCleaner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureBlobTester
{
    class Program
    {
        static void Main(string[] args)
        {
            var cleaner = new Cleaner();
            cleaner.Execute();
            Console.WriteLine("Done... press a key to end.");
            Console.ReadKey();
        }
    }
}
