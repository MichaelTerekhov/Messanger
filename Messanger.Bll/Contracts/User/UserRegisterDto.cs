using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Messanger.Bll.Contracts.User
{
    public class UserRegisterDto
    {
        [JsonPropertyName("login")]
        [Required(AllowEmptyStrings = false)]
        public string Login { get; set; }
        
        [JsonPropertyName("password")]
        [Required(AllowEmptyStrings = false)]
        [MinLength(6)]
        public string Password { get; set; }

        [JsonPropertyName("mobile")]
        [Required(AllowEmptyStrings = false)]
        public string MobilePhone { get; set; }
    }
}
