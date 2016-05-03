using System.Collections.Generic;

namespace APlayTest.Services
{
    public class ClientIdLookup : IClientIdLookup
    {
       

        private readonly HashSet<int> _clientIds = new HashSet<int>();
        public bool IsUsed(int clientId)
        {
            return _clientIds.Contains(clientId);
        }

        public void Lock(int clientId)
        {
            _clientIds.Add(clientId);
        }

        public void Free(int clientId)
        {
            _clientIds.Remove(clientId);
        }
    }
}