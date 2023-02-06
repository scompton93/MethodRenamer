using Mono.Cecil;
using System.Text;

namespace NamingsResolver;

public class AnonymizedMethod
{
    public AnonymizedMethod(MethodDefinition md)
    {
        foreach (var parameter in md.Parameters)
        {
            parameters.Add(
                parameter.ParameterType.Namespace != ""
                    ? parameter.ParameterType.FullName
                    : "Object"
            );
        }

        ReturnType = md.ReturnType.Namespace != "" ? md.ReturnType.FullName : "Object";

        OriginalDefinition = md.ToString();
        DeclaringType = md.DeclaringType.Name;
        MethodName = md.Name;
        MethodFullName = md.FullName;
    }

    public string Definition =>
        $"{ReturnType} {DeclaringType}::{MethodName}({string.Join(',', parameters)})";
    public string AnonymousDefinition => $"{ReturnType} ({string.Join(',', parameters)})";

    public string DeclaringType { get; private set; }
    public string MethodName { get; private set; }
    public string MethodFullName { get; private set; }

    public string OriginalDefinition { get; private set; }

    public string ReturnType { get; private set; }
    public List<string> parameters { get; } = new List<string>();
}
