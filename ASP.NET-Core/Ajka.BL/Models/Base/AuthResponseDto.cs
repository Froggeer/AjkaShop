namespace Ajka.BL.Models.Base
{
    public class AuthResponseDto
    {
        public int UserId { get; set; }

        /// <summary>
        /// Security token.
        /// </summary>
        public string AccessToken { get; set; }

        public string ErrorMessage { get; set; }
    }
}
