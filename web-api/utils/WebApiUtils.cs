using ipstack_lib.dto;
using System.Text.RegularExpressions;
using web_api.Model;

namespace web_api.utils
{
    public class WebApiUtils
    {
        public IPInfoEntity IPDetailsToIPInfoEntity(IPDetails iPDetails)
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

        public bool IsIPValid(string ip)
        {
            string strRegex = @"^((25[0-5]|(2[0-4]|1\d|[1-9]|)\d)(\.(?!$)|$)){4}$";
            Regex re = new Regex(strRegex);

            return re.IsMatch(ip);
        }
    }
}
