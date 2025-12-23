using MEFInterfaceLibrary;
using System.ComponentModel.Composition;

namespace ProductPlugin
{
    [Export(typeof(IOperation))]
    [ExportMetadata("OperationType", OperationTypeEnum.Product)]
    public class ProductOperation: IOperation
    {
        public string GetData()
        {
            return "Product Operations is executed.";
        }
    }
}
