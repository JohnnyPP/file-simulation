using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSimulation
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Opening file dialog");
            FileSystemHelper.LoadImages();
            Console.WriteLine("Done!");
            Console.ReadKey();
        }
    }
}
