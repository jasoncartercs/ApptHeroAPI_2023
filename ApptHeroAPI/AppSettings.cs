namespace ApptHeroAPI
{
    public class AppSettings
    {
        public string WebApiBaseUrl { get; set; }
        public LoggingSection Logging { get; set; }
        public ConnectionStringsSection ConnectionStrings { get; set; }
        public TokenAuthenticationSection TokenAuthentication { get; set; }
        public SmtpSection Smtp { get; set; }
    }

    public class LoggingSection
    {
        public LogLevelSection LogLevel { get; set; }
    }

    public class LogLevelSection
    {
        public string Default { get; set; }
        public string Microsoft { get; set; }
        public string MicrosoftHostingLifetime { get; set; }
    }

    public class ConnectionStringsSection
    {
        public string DefaultConnection { get; set; }
    }

    public class TokenAuthenticationSection
    {
        public string SecretKey { get; set; }
        public int ExpiryDurationMinutes { get; set; }
    }

    public class SmtpSection
    {
        public string Host { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public string From { get; set; }
    }

}
