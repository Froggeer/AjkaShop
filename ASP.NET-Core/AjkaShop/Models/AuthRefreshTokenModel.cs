using System.ComponentModel.DataAnnotations;

namespace AjkaShop.Models
{
    public class AuthRefreshTokenModel
    {
        /// <summary>
        /// Access token.
        /// </summary>
        [Required]
        public string RefreshToken { get; set; }
    }
}
