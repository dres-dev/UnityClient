using System;

namespace Dres.Unityclient
{
  
  [Serializable]
  public class DresConfig
  {
    
    /// <summary>
    /// The DRES host address.
    /// Could be an IP or a name.
    /// Defaults to LOCALHOST
    /// </summary>
    public string host;
    
    /// <summary>
    /// The DRES port.
    /// Defaults to 8080
    /// </summary>
    public int port;

    /// <summary>
    /// A flag whether to use TLS or not.
    /// Defaults to FALSE
    /// </summary>
    public bool tls;
    
    
    
    /// <summary>
    /// The DRES credentials - username.
    /// </summary>
    public string user;
    /// <summary>
    /// The DRES credentials - password.
    /// </summary>
    public string password;



    /// <summary>
    /// Whether this configuration has credentials provided or not.
    /// </summary>
    /// <returns>TRUE if and only if both, user and password are set. FALSE otherwise</returns>
    public bool HasCredentials()
    {
      return !string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(password);
    }
    /// <summary>
    /// The DRES endpoint respecting TLS, host and 
    /// </summary>
    public string Endpoint => (tls ? "https://" : "http://") + host + ":" + port + "/";

    /// <summary>
    /// Constructor for a new DRES config instance. This is for convenience.
    /// </summary>
    /// <param name="host">The DRES host address</param>
    /// <param name="port">The DRES port</param>
    /// <param name="user">The DRES username</param>
    /// <param name="password">The DRES password</param>
    /// <param name="tls">Whether or not TLS is enabled</param>
    public DresConfig(string host, int port, string user, string password, bool tls)
    {
      this.host = host;
      this.port = port;
      this.user = user;
      this.password = password;
      this.tls = tls;
    }
    
    /// <summary>
    /// Constructor for a new DRES config instance. This is for convenience.
    /// </summary>
    /// <param name="host">The DRES host address</param>
    /// <param name="port">The DRES port</param>
    /// <param name="tls">Whether or not TLS is enabled</param>
    public DresConfig(string host, int port, bool tls)
    {
      this.host = host;
      this.port = port;
      this.tls = tls;
    }

    /// <summary>
    /// Creates the default DRES config.
    /// The host being localhost, the port 8080 and tls turned off.
    /// </summary>
    /// <returns></returns>
    public static DresConfig GetDefault()
    {
      return new DresConfig("localhost", 8080, false);
    }
  }
}