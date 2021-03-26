namespace Bhasha.Common.Arguments
{
    public interface IArgumentAssemblyProvider
    {
        IAssembleArguments GetAssembly(PageType key);
    }
}
