using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;

namespace NamingsResolver;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        const string targetPath = @"C:\Users\x\Downloads\gamepack-deob.dll";
        const string blueprintPath = @"C:\Users\x\Downloads\gamepack-deob - Copy.dll";
        string outputPath = targetPath.Replace(".dll", "-renamed.dll");

        var targetMethods = GetMethods(targetPath)
            .GroupBy(o => o.AnonymousDefinition)
            .Where(g => g.Count() == 1)
            .SelectMany(g => g)
            .Where(g => g.DeclaringType == "class35");
        var blueprintMethods = GetMethods(blueprintPath)
            .GroupBy(o => o.AnonymousDefinition)
            .Where(g => g.Count() == 1)
            .SelectMany(g => g);

        var matchedMethods = targetMethods.Join(
            blueprintMethods,
            tagetMethod => tagetMethod.AnonymousDefinition,
            blueprintMethod => blueprintMethod.AnonymousDefinition,
            (tagetMethod, blueprintMethod) => new { tagetMethod, blueprintMethod }
        );

        var module = ModuleDefinition.ReadModule(targetPath);
        var methodsToRename = from type in module.Types from method in type.Methods select method;

        foreach (var matchedMethod in matchedMethods)
        {
            var methodToRename = methodsToRename
                .Where(g => g.FullName == matchedMethod.tagetMethod.MethodFullName)
                .First();
            methodToRename.Name = matchedMethod.blueprintMethod.MethodName;
            methodToRename.DeclaringType.Name = matchedMethod.blueprintMethod.DeclaringType;
        }
        module.Write(outputPath);
    }

    public static List<AnonymizedMethod> GetMethods(string target)
    {
        var module = ModuleDefinition.ReadModule(target);
        var methods = from type in module.Types from method in type.Methods select method;

        var anonymizedMethods = new List<AnonymizedMethod>();
        foreach (var method in methods)
        {
            anonymizedMethods.Add(new AnonymizedMethod(method));
        }
        return anonymizedMethods;
    }
}
