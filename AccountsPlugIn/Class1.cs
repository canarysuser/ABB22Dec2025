using MEFInterfaceLibrary;
using System.Composition;
using System.Runtime.InteropServices.JavaScript;

namespace AccountsPlugIn
{
    [Export(typeof(IOperation))]
    [ExportMetadata("OperationType", OperationTypeEnum.Account)]
    public class AccountOperation : IOperation
    {
        public string GetData()
        {
            return "Account Operations is executed.";
        }
    }
}
