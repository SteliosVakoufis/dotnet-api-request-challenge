using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace web_api.Model
{
    [Table("ip_info")]
    public class IPInfoEntity
    {
        [Key, Required, Column("ip"), RegularExpression(@"^((25[0-5]|(2[0-4]|1\d|[1-9]|)\d)(\.(?!$)|$)){4}$")]
        public string IP { get; set; } = string.Empty;

        [Required, Column("city")]
        public string City { get; set; } = string.Empty;

        [Required, Column("country")]
        public string Country { get; set; } = string.Empty;

        [Required, Column("continent")]
        public string Continent { get; set; } = string.Empty;

        [Required, Column("latitude")]
        public double Latitude { get; set; }

        [Required, Column("longitude")]
        public double Longitude { get; set; }
    }
}
