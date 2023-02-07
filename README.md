# MethodRenamer

MethodRenamer is a .NET library for renaming methods in .NET assemblies, specifically .net assemblies that have been obfuscated. 

## Usage

To use MethodRenamer, create an instance of the `MethodRenamer` class and pass in the paths to the target assembly, blueprint assembly, and the output path for the renamed assembly. The blueprint assembly provides the names for the methods in the target assembly.

```csharp
const string targetPath = @"TestTarget.dll";
const string blueprintPath = @"TestBlueprint.dll";

string outputPath = targetPath.Replace(".dll", "-renamed.dll");

var renamer = new MethodRenamer(targetPath, blueprintPath, outputPath);
```

## How it works

1. The target assembly and blueprint assembly are read into memory as ModuleDefinition's.
2. The method definitions are than uniqued for the assembly and stored in a list of MethodAnonymizer's.
3. The MethodAnonymizer objects are compared to find the methods with matching anonymous definitions, when the anonymouse definition only exists once.
4. The methods in the target assembly are renamed to the names of the corresponding methods in the blueprint assembly. The declaring type name can also be changed if it differs.
5. The modified target assembly is written to the specified output path.

## Conclusion

The MethodRenamer library is a useful tool for anonymizing or standardizing the names of methods in a .NET assembly, based on a blueprint assembly. By using the MethodAnonymizer class, the library provides a simple and easy-to-use way to rename methods in a .NET assembly.
