namespace NamingsResolver;

internal class Program
{
    static void Main(string[] args)
    {
        const string targetPath = @"C:\Users\x\Downloads\gamepack-deob.dll";
        const string blueprintPath = @"C:\Users\x\Downloads\gamepack-deob - Copy.dll";

        string outputPath = targetPath.Replace(".dll", "-renamed.dll");

        var renamer = new MethodRenamer(targetPath, blueprintPath, outputPath);

        Console.WriteLine("Done.");
        Console.ReadKey();
    }
}
