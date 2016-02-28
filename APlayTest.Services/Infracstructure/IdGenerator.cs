using System.Threading;

namespace APlayTest.Services.Infracstructure
{
    public static class IdGenerator
    {
        const int StartId = 10 * 1000;

        private static int _currentId = StartId;

        public static int GetNextId()
        {
            Interlocked.Increment(ref _currentId);
            return _currentId;
        }
    }
}
