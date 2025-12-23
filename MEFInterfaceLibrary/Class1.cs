using System.ComponentModel.Composition;

namespace MEFInterfaceLibrary
{
    public enum OperationTypeEnum { None=0, Account=1, Product=2}

    public interface IDataRetriever
    {
        string GetData(OperationTypeEnum operationType);
    }
    public interface IOperation
    {
        string GetData();
    }
    public interface IOperationData
    {
        OperationTypeEnum OperationType { get; }
    }

    [Export(typeof(IDataRetriever))]
    public class DataRetriever : IDataRetriever
    {
        [ImportMany]
        public IEnumerable<Lazy<IOperation, IOperationData>> Operations { get; set; } = null!;
        public string GetData(OperationTypeEnum operationType)
        {
            var operation = Operations.FirstOrDefault(op => op.Metadata.OperationType == operationType);
            if (operation != null)
            {
                return operation.Value.GetData();
            }
            return $"No operation found for type {operationType}";
        }
    }


}
