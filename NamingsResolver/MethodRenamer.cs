using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
namespace NamingsResolver;

public class MethodRenamer
{
    public MethodRenamer(string targetPath, string blueprintPath, string outputPath) {
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
            _methodsRenamed++;

            if (renamableMethod.DeclaringType.Name == blueprintMethod.DeclaringType)
                continue;

            Console.WriteLine(
                $"Renaming type from {renamableMethod.DeclaringType.Name} to {blueprintMethod.DeclaringType}"
            );
            renamableMethod.DeclaringType.Name = blueprintMethod.DeclaringType;
        }

        module.Write(outputPath);

    }

    private static IEnumerable<MethodAnonymizer> GetUniqueMethodDefinitions(string path)
    {
        var module = ModuleDefinition.ReadModule(path);
        var methods = module.Types.SelectMany(type => type.Methods);

        var anonymizedMethods = methods.Select(method => new MethodAnonymizer(method)).ToList();

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
        IEnumerable<MethodAnonymizer> targetMethods,
        IEnumerable<MethodAnonymizer> blueprintMethods
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
    private readonly int _methodsRenamed = 0;
    public int MethodsRenamed => _methodsRenamed;

}
