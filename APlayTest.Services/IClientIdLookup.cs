namespace APlayTest.Services
{
    public interface IClientIdLookup
    {
        bool IsUsed(int clientId);
        void Lock(int clientId);
        void Free(int clientId);
    }
}