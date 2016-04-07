namespace APlayTest.Client.Modules.Inspector.Inspectors
{
    public interface IInspector
    {
        string Name { get; }
        bool IsReadOnly { get; }
    }
}