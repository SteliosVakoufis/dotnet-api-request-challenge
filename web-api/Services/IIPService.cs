using ipstack_lib.interfaces;
using Microsoft.AspNetCore.Mvc;
using web_api.Model;

namespace web_api.Services
{
    public interface IIPService
    {
        public Task<IPInfoEntity> GetIpDetails(string ip);
        public Task<List<IPInfoEntity>> UpdateIpDetails(List<IPInfoEntity> entities);
    }
}
