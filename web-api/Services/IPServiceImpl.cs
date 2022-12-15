using ipstack_lib.exceptions;
using ipstack_lib.interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web_api.Model;
using web_api.utils;

namespace web_api.Services
{
    public class IPServiceImpl : IIPService
    {
        private readonly DataContext _context;
        private readonly IIPInfoProvider _ipProdiver;
        private readonly WebApiUtils _utils;

        public IPServiceImpl(DataContext context, IIPInfoProvider ipProdiver, WebApiUtils utils)
        {
            _context = context;
            _ipProdiver = ipProdiver;
            _utils = utils;
        }

        public Task<List<IPInfoEntity>> GetAllIpDetails()
        {
            try
            {
                return _context.IPInfo.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IPInfoEntity> GetIpDetails(string ip)
        {
            if (!_utils.IsIPValid(ip)) {
                throw new IPServiceNotAvailableException("IP service not available, Please try again.");
            }

            try
            {
                var entity = await _context.IPInfo.FindAsync(ip);

                if (entity is null)
                {
                    var ipDetails = await _ipProdiver.GetDetails(ip);
                    entity = _utils.IPDetailsToIPInfoEntity(ipDetails);
                    await _context.IPInfo.AddAsync(entity);
                    await _context.SaveChangesAsync();
                }

                return entity;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
