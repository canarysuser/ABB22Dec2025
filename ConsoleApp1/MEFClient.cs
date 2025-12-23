using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using MEFInterfaceLibrary;
using System.Composition.Hosting;
using System.Reflection;
using System.Composition;

namespace ConsoleApp1
{
    public class PluginHost
    {
        [System.Composition.ImportMany]
        public IEnumerable<IOperation> PlugIns { get; set; }
    }
    internal class MEFClient
    {

        public static CompositionHost LoadPlugins(string pluginPath)
        {
            var assemblies = Directory.GetFiles(pluginPath, "*.dll")
                .Select(Assembly.LoadFrom)
                .ToList();

            var configuration = new ContainerConfiguration()
                .WithAssembly(Assembly.GetExecutingAssembly())
                .WithAssemblies(assemblies: assemblies);
            return configuration.CreateContainer();
        }
        
        internal static void TestPlugIns()
        {
            var container = LoadPlugins(@"../../../Plugins");
            
            var host = container.GetExport<PluginHost>();
            foreach(var plugin in host.PlugIns)
            {
                Console.WriteLine($"Name: {plugin.GetData()}");
                plugin.GetData();
            }
        }

        [System.ComponentModel.Composition.Import(typeof(IDataRetriever))]
        public IDataRetriever DataRetriever { get; set; } = null!;

        private CompositionContainer _container;

        public MEFClient()
        {
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(MEFClient).Assembly));
            catalog.Catalogs.Add(new DirectoryCatalog(@"../../../Plugins"));
            _container = new CompositionContainer(catalog);
            _container.ComposeParts(this);
        }

        internal static void Test()
        {
            MEFClient client = new MEFClient();
            string productData = client.DataRetriever.GetData(OperationTypeEnum.Product);
            Console.WriteLine(productData);
            string accountData = client.DataRetriever.GetData(OperationTypeEnum.Account);
            Console.WriteLine(accountData);
        }
    }
}
