namespace Ajka.Common.Helpers
{
    public class AppSettings
    {
        public string ClientSecret { get; set; }

        public string PasswordSalt { get; set; }

        public string MailAddress { get; set; }

        public string MailCredentialsName { get; set; }

        public string MailCredentialsPassword { get; set; }

        public string SmtpPort { get; set; }

        public string SmtpHost { get; set; }
    }
}
