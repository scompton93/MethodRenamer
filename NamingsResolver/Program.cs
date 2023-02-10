using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamingsResolver
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string targetPath = @"";
            const string blueprintPath = @"";

            string outputPath = targetPath.Replace(".dll", "-renamed.dll");

            var renamer = new MethodRenamer(targetPath, blueprintPath, outputPath);

            Console.WriteLine("Done.");
            Console.ReadKey();
        }
    }
}
