namespace Inscribe.Email
{
    public class SmtpSettings
    {
        public string Host { get; set; }

        public bool UseDefaultCredentials { get; set; }

        public Credentials Credentials { get; set; }
    }
}
