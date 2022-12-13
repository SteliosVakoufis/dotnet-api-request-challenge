using ipstack_lib.dto;

namespace ipstack_lib.interfaces
{
    public interface IIPInfoProvider
    {
        public Task<IPDetails> GetDetails(string ip);
    }
}
