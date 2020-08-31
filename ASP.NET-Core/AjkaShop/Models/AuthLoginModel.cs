using System.ComponentModel.DataAnnotations;

namespace AjkaShop.Models
{
    public class AuthLoginModel
    {
        /// <summary>
        /// Login of user.
        /// </summary>
        [Required]
        public string Username { get; set; }

        /// <summary>
        /// Password.
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
