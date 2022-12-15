using ipstack_lib.interfaces;
using Microsoft.AspNetCore.Mvc;
using web_api.Model;

namespace web_api.Services
{
    public interface IIPService
    {
        Task<List<IPInfoEntity>> GetAllIpDetails();
        public Task<IPInfoEntity> GetIpDetails(string ip);
    }
}
