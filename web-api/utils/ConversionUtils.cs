using ipstack_lib.dto;
using web_api.Model;

namespace web_api.utils
{
    public class ConversionUtils
    {
        public static IPInfoEntity IPDetailsToIPInfoEntity(IPDetails iPDetails)
        {
            return new IPInfoEntity()
            {
                IP = iPDetails.IP,
                City = iPDetails.City,
                Country = iPDetails.Country,
                Continent = iPDetails.Continent,
                Latitude = iPDetails.Latitude,
                Longitude = iPDetails.Longitude,
            };
        }
    }
}
