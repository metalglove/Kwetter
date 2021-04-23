namespace Kwetter.Services.Common.Application.Configurations
{
    /// <summary>
    /// Represents the <see cref="MessagingConfiguration"/> class.
    /// </summary>
    public sealed class MessagingConfiguration
    {
        /// <summary>
        /// Gets and sets the host.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Gets and sets the username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets and sets the password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets and sets the virtual host.
        /// </summary>
        public string VirtualHost { get; set; }

        /// <summary>
        /// Gets and sets the port.
        /// </summary>
        public int Port { get; set; }
    }
}
