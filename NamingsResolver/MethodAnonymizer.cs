using Mono.Cecil;
using System.Text;

namespace NamingsResolver;

public class MethodAnonymizer
{
    private readonly List<string> _parameters = new List<string>();
    private readonly string _declaringType;
    private readonly string _methodName;
    private readonly string _returnType;

    public MethodAnonymizer(MethodDefinition md)
    {
        foreach (var parameter in md.Parameters)
        {
            _parameters.Add(
                parameter.ParameterType.Namespace != ""
                    ? parameter.ParameterType.FullName
                    : "Object"
            );
        }

        _returnType = md.ReturnType.Namespace != "" ? md.ReturnType.FullName : "Object";

        _declaringType = md.DeclaringType.Name;
        _methodName = md.Name;
    }

    public string AnonymousDefinition => $"{_returnType} ({string.Join(',', _parameters)})";
    public string DeclaringType => _declaringType;
    public string MethodName => _methodName;
    public string ReturnType => _returnType;
}
