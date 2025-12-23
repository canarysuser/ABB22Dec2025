using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using MEFInterfaceLibrary;

namespace ConsoleApp1
{
    internal class MEFClient
    {
        [Import(typeof(IDataRetriever))]
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
