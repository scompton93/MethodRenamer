using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;

namespace NamingsResolver;

internal class Program
{
    static void Main(string[] args)
    {
        const string targetPath = @"C:\Users\x\Downloads\gamepack-deob.dll";
        const string blueprintPath = @"C:\Users\x\Downloads\gamepack-deob - Copy.dll";
        string outputPath = targetPath.Replace(".dll", "-renamed.dll");

        var targetMethods = GetMethods(targetPath)
            .GroupBy(o => o.AnonymousDefinition)
            .Where(g => g.Count() == 1)
            .SelectMany(g => g);
        var blueprintMethods = GetMethods(blueprintPath)
            .GroupBy(o => o.AnonymousDefinition)
            .Where(g => g.Count() == 1)
            .SelectMany(g => g);

        var matchedMethods = targetMethods
            .Join(
                blueprintMethods,
                tagetMethod => tagetMethod.AnonymousDefinition,
                blueprintMethod => blueprintMethod.AnonymousDefinition,
                (tagetMethod, blueprintMethod) => new { tagetMethod, blueprintMethod }
            )
            .Where(x => x.blueprintMethod.MethodName != x.tagetMethod.MethodName);

        var module = ModuleDefinition.ReadModule(targetPath);
        var methodsToRename = from type in module.Types from method in type.Methods select method;

        foreach (var renamableMethod in methodsToRename)
        {
            var blueprintMethod = matchedMethods
                .Where(x => x.tagetMethod.MethodName == renamableMethod.Name)
                .Select(x => x.blueprintMethod)
                .FirstOrDefault();
            if (blueprintMethod == null)
                continue;

            Console.WriteLine($"Renaming {renamableMethod.Name} to {blueprintMethod.MethodName}");
            renamableMethod.Name = blueprintMethod.MethodName;

            if (renamableMethod.DeclaringType.Name == blueprintMethod.DeclaringType)
                continue;

            Console.WriteLine(
                $"Renaming type from {renamableMethod.DeclaringType.Name} to {blueprintMethod.DeclaringType}"
            );
            renamableMethod.DeclaringType.Name = blueprintMethod.DeclaringType;
        }
        module.Write("test.dll");
        Console.WriteLine("Done.");
        Console.ReadKey();
    }

    public static List<MethodAnonymizer> GetMethods(string target)
    {
        var module = ModuleDefinition.ReadModule(target);
        var methods = from type in module.Types from method in type.Methods select method;

        var anonymizedMethods = new List<MethodAnonymizer>();
        foreach (var method in methods)
        {
            anonymizedMethods.Add(new MethodAnonymizer(method));
        }
        return anonymizedMethods;
    }
}
