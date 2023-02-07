namespace NamingsResolver;

internal class Program
{
    static void Main(string[] args)
    {
        const string targetPath = @"C:\Users\x\source\repos\a\NamingsResolver\Tests\bin\Debug\net7.0\TestLibraries\TestTarget.dll";
        const string blueprintPath = @"C:\Users\x\source\repos\a\NamingsResolver\Tests\bin\Debug\net7.0\TestLibraries\TestBlueprint.dll";

        string outputPath = targetPath.Replace(".dll", "-renamed.dll");

        var renamer = new MethodRenamer(targetPath, blueprintPath, outputPath);

        Console.WriteLine("Done.");
        Console.ReadKey();
    }
}
