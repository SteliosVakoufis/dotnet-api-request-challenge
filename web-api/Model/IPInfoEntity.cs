using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace web_api.Model
{
    [Table("ip_info")]
    public class IPInfoEntity
    {
        [Key, Required, Column("ip"), RegularExpression(@"^((25[0-5]|(2[0-4]|1\d|[1-9]|)\d)(\.(?!$)|$)){4}$")]
        public string? IP { get; set; }

        [Column("city")]
        public string? City { get; set; }

        [Column("country")]
        public string? Country { get; set; }

        [Column("continent")]
        public string? Continent { get; set; }

        [Column("latitude")]
        public double? Latitude { get; set; }

        [Column("longitude")]
        public double? Longitude { get; set; }
    }
}
