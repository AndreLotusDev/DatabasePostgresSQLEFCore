namespace DatabasePostgresSQLEFCore.Helper
{
    public class SortedIdGenerator
    {
        private long _lastId;

        public SortedIdGenerator()
        {
            _lastId = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }

        public long GenerateId()
        {
            return Interlocked.Increment(ref _lastId);
        }
    }
}
