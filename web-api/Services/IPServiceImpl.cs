using ipstack_lib.exceptions;
using ipstack_lib.interfaces;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<IPInfoEntity> GetIpDetails(string ip)
        {
            if (!_utils.IsIPValid(ip))
            {
                throw new IPServiceNotAvailableException("IP provided is not valid, Please try again.");
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
                throw new IPServiceNotAvailableException("IP service is not available, Please try again.");
            }
        }

        public async Task<List<IPInfoEntity>> UpdateIpDetails(List<IPInfoEntity> entities)
        {
            try
            {
                var result = new List<IPInfoEntity>();

                foreach (var entity in entities)
                {
                    if (!_utils.IsIPValid(entity.IP))
                    {
                        throw new IPServiceNotAvailableException("One or more IPs are not valid, Please try again.");
                    }

                    var iPInfoEntity = await _context.IPInfo.FindAsync(entity.IP);

                    if (iPInfoEntity is null)
                    {
                        throw new IPServiceNotAvailableException("One or more IPs provided are not matching with the database, Please try again.");
                    }

                    iPInfoEntity.City = entity.City ?? iPInfoEntity.City;
                    iPInfoEntity.Country = entity.Country ?? iPInfoEntity.Country;
                    iPInfoEntity.Continent = entity.Continent ?? iPInfoEntity.Continent;
                    iPInfoEntity.Latitude = entity.Latitude ?? iPInfoEntity.Latitude;
                    iPInfoEntity.Longitude = entity.Longitude ?? iPInfoEntity.Longitude;

                    result.Add(iPInfoEntity);
                    await _context.IPInfo.AddAsync(iPInfoEntity);
                }

                return result;
            }
            catch (Exception e)
            {
                throw new IPServiceNotAvailableException(e.Message);
            }
        }
    }
}
