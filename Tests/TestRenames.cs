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
            const string targetPath = @"\TestLibraries\TestTarget.dll";
            const string blueprintPath = @"\TestLibraries\TestBlueprint.dll";

            var tempPath =
                System.IO.Path.GetTempPath() + "output_" + DateTime.Now.Ticks + "_.dll";
            var processDirectory = Path.GetDirectoryName(Environment.ProcessPath);

            var methodRenamer = new MethodRenamer(
                processDirectory + targetPath,
                processDirectory + blueprintPath,
                tempPath
            );

            var module = ModuleDefinition.ReadModule(tempPath);
            var methods = module.Types.SelectMany(type => type.Methods).Select(x => x.FullName); ;


            Assert.AreEqual(string.Join(',', methods), string.Join(',', expectedNames));
        }
        [TestMethod]
        public void TestOther()
        {
            List<string> expectedNames = new List<string>() { "System.Void Test1.MyClass::.ctor()", "System.Void Test1.MyClass::void_class_int_string(System.Int32,System.String)", "System.Int32 Test1.MyClass::int_class_int_string(System.Int32,System.String)", "System.Char Test1.MyClass::45645(System.Int32,System.String)", "System.Char Test1.MyClass::gdfbv34v(System.Int32,System.String)", "Test1.MyClass Test1.MyClass:: (System.Int32,System.String)" };
            const string targetPath = @"\TestLibraries\TestTargetOther.dll";
            const string blueprintPath = @"\TestLibraries\TestBlueprint.dll";

            var tempPath =
                System.IO.Path.GetTempPath() + "output_" + DateTime.Now.Ticks + "_.dll";
            var processDirectory = Path.GetDirectoryName(Environment.ProcessPath);

            var methodRenamer = new MethodRenamer(
                processDirectory + targetPath,
                processDirectory + blueprintPath,
                tempPath
            );

            var module = ModuleDefinition.ReadModule(tempPath);
            var methods = module.Types.SelectMany(type => type.Methods).Select(x => x.FullName); ;

            Assert.AreEqual(string.Join(',', methods.OrderBy(a => a)), string.Join(',', expectedNames.OrderBy(a => a)));
        }

        [TestMethod]
        public void TestIKVM()
        {
            List<string> expectedNames = new List<string>() { "System.Void Test1.MyClass::.ctor()", "System.Void Test1.MyClass::void_class_int_string(System.Int32,System.String)", "System.Int32 Test1.MyClass::int_class_int_string(System.Int32,System.String)", "System.Char Test1.MyClass::45645(System.Int32,System.String)", "System.Char Test1.MyClass::gdfbv34v(System.Int32,System.String)", "Test1.MyClass Test1.MyClass:: (System.Int32,System.String)" };
            const string targetPath = @"\TestLibraries\TestIKVM.dll";
            const string blueprintPath = @"\TestLibraries\TestBlueprint.dll";

            var tempPath =
                System.IO.Path.GetTempPath() + "output_" + DateTime.Now.Ticks + "_.dll";
            var processDirectory = Path.GetDirectoryName(Environment.ProcessPath);

            var methodRenamer = new MethodRenamer(
                processDirectory + targetPath,
                processDirectory + blueprintPath,
                tempPath
            );

            var module = ModuleDefinition.ReadModule(tempPath);
            var methods = module.Types.SelectMany(type => type.Methods).Select(x => x.FullName); ;

            Assert.AreEqual(string.Join(',', methods.OrderBy(a => a)), string.Join(',', expectedNames.OrderBy(a => a)));
        }
    }
}
