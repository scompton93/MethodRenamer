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

        var targetMethods = GetUniqueMethodDefinitions(targetPath);
        var blueprintMethods = GetUniqueMethodDefinitions(blueprintPath);

        var matchedMethods = GetMatchedMethods(targetMethods, blueprintMethods);

        var module = ModuleDefinition.ReadModule(targetPath);
        var methodsToRename = module.Types.SelectMany(type => type.Methods);

        foreach (var renamableMethod in methodsToRename)
        {
            var blueprintMethod = matchedMethods
                .FirstOrDefault(x => x.TargetMethod.MethodName == renamableMethod.Name)
                .BlueprintMethod;

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

        module.Write(outputPath);
        Console.WriteLine("Done.");
        Console.ReadKey();
    }

    private static List<MethodAnonymizer> GetUniqueMethodDefinitions(string path)
    {
        var module = ModuleDefinition.ReadModule(path);
        var methods = module.Types.SelectMany(type => type.Methods);

        var anonymizedMethods = new List<MethodAnonymizer>();
        foreach (var method in methods)
        {
            anonymizedMethods.Add(new MethodAnonymizer(method));
        }

        return anonymizedMethods
            .GroupBy(o => o.AnonymousDefinition)
            .Where(g => g.Count() == 1)
            .SelectMany(g => g)
            .ToList();
    }

    private static List<(
        MethodAnonymizer TargetMethod,
        MethodAnonymizer BlueprintMethod
    )> GetMatchedMethods(
        List<MethodAnonymizer> targetMethods,
        List<MethodAnonymizer> blueprintMethods
    )
    {
        return targetMethods
            .Join(
                blueprintMethods,
                targetMethod => targetMethod.AnonymousDefinition,
                blueprintMethod => blueprintMethod.AnonymousDefinition,
                (targetMethod, blueprintMethod) => (targetMethod, blueprintMethod)
            )
            .Where(x => x.blueprintMethod.MethodName != x.targetMethod.MethodName)
            .ToList();
    }
}
