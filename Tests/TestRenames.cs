using NamingsResolver;
using System.IO;
using Mono.Cecil;

namespace Tests
{
    [TestClass]
    public class TestRenames
    {
        [TestMethod]
        public void TestMonoLibrary()
        {
            List<string> expectedNames = new List<string>() { "System.Void Test1.MyClass::.ctor()", "System.Void Test1.MyClass::void_class_int_string(System.Int32,System.String)", "System.Int32 Test1.MyClass::int_class_int_string(System.Int32,System.String)", "System.Char Test1.MyClass::45645(System.Int32,System.String)", "System.Char Test1.MyClass::gdfbv34v(System.Int32,System.String)", "Test1.MyClass Test1.MyClass:: (System.Int32,System.String)" };
            const string targetPath = @"\TestLibraries\testTarget.dll";
            const string blueprintPath = @"\TestLibraries\testBlueprint.dll";

            string tempPath =
                System.IO.Path.GetTempPath() + "output_" + DateTime.Now.Ticks + "_.dll";
            string processDirectory = Path.GetDirectoryName(Environment.ProcessPath);

            var renamer = new MethodRenamer(
                processDirectory + targetPath,
                processDirectory + blueprintPath,
                tempPath
            );

            var module = ModuleDefinition.ReadModule(tempPath);
            var methods = module.Types.SelectMany(type => type.Methods).Select(x => x.FullName); ;


            Assert.AreEqual(string.Join(',', methods), string.Join(',', expectedNames));
        }
    }
}
