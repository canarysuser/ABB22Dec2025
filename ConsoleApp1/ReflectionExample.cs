using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection; 
using System.Reflection.Emit;

namespace ConsoleApp1
{
    public class TestClass
    {
        public int IntField = 10;
        public string Name { get; set; } = "TestName";
        public TestClass(int number, string name)
        {
            IntField = number;
            Name = name;
        }
        public void ShowInfo()
        {
            Console.WriteLine($"IntField: {IntField}, Name: {Name}");
        }
    }

    internal class ReflectionExample
    {
        internal static void Test()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Module module = assembly.GetModule("ConsoleApp1.dll")!;

            //Type type = typeof(TestClass);
            Type type = module.GetType("ConsoleApp1.TestClass")!;
            Console.WriteLine($"Type Name: {type.Name}");

            var constructor = type?.GetConstructor(new Type[] { typeof(int), typeof(string) });

            //var instance = (TestClass)constructor?.Invoke(new object[] { 42, "Reflection" })!;
            var instance = Activator.CreateInstance(type!, new object[] { 999, "ABB" })!;

            var field = type?.GetField("IntField");
            var property = type?.GetProperty("Name");

            Console.WriteLine($"Before Modification - IntField: {field?.GetValue(instance)}, Name: {property?.GetValue(instance)}");

            field?.SetValue(instance, 100);
            property?.SetValue(instance, "Modified via Reflection");

            Console.WriteLine($"After Modification - IntField: {field?.GetValue(instance)}, Name: {property?.GetValue(instance)}");

            var method = type?.GetMethod("ShowInfo");
            //method?.Invoke(instance, null);
            type?.InvokeMember(
                name: "ShowInfo",
                invokeAttr: BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance,
                binder: null,
                target: instance,
                args: null);

            Console.WriteLine("==============================================================");
            var methods = type?.GetMethods();
            foreach (var m in methods!)
            {
                Console.WriteLine($"Method Name: {m.Name}, {m.ReturnType.FullName}, {m.DeclaringType.FullName}");
            }
            Console.WriteLine("==============================================================");
            var fields = type?.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (var f in fields!)
            {
                Console.WriteLine($"Field Name: {f.Name}, {f.FieldType.FullName}, {f.DeclaringType.FullName}");
            }
        }
        internal static void TestDynamicAssembly()
        {
            AssemblyName assemblyName = new AssemblyName("DynamicAssemblyExample");
            AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");
            TypeBuilder typeBuilder = moduleBuilder.DefineType("DynamicClass", TypeAttributes.Public);
            FieldBuilder fieldBuilder = typeBuilder.DefineField("DynamicField", typeof(int), FieldAttributes.Public);
            MethodBuilder methodBuilder = typeBuilder.DefineMethod("ShowDynamicField", MethodAttributes.Public, null, null);
            ILGenerator ilGenerator = methodBuilder.GetILGenerator();
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ldfld, fieldBuilder);
            ilGenerator.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new Type[] { typeof(int) })!);
            ilGenerator.Emit(OpCodes.Ret);
            Type dynamicType = typeBuilder.CreateType()!;
            object dynamicInstance = Activator.CreateInstance(dynamicType)!;
            FieldInfo dynamicFieldInfo = dynamicType.GetField("DynamicField")!;
            dynamicFieldInfo.SetValue(dynamicInstance, 12345);
            MethodInfo showDynamicFieldMethod = dynamicType.GetMethod("ShowDynamicField")!;
            showDynamicFieldMethod.Invoke(dynamicInstance, null);
        }
    }
}
