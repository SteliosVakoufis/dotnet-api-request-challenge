namespace ipstack_lib.exceptions
{
    public class IPServiceNotAvailableException : Exception
    {
        public IPServiceNotAvailableException(string? message) : base(message) { }
    }
}
