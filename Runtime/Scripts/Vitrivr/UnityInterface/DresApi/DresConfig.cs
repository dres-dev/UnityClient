using System;

namespace Vitrivr.UnityInterface.DresApi
{
  
  [Serializable]
  public class DresConfig
  {
    public string host;
    public int port;

    public string user;
    public string password;

    public bool tls;


    public bool HasCredentials()
    {
      return !string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(password);
    }
    public string Endpoint => (tls ? "https://" : "http://") + host + ":" + port + "/";

    public DresConfig(string host, int port, string user, string password, bool tls)
    {
      this.host = host;
      this.port = port;
      this.user = user;
      this.password = password;
      this.tls = tls;
    }

    public DresConfig(string host, int port, bool tls)
    {
      this.host = host;
      this.port = port;
      this.tls = tls;
    }

    public static DresConfig GetDefault()
    {
      return new DresConfig("localhost", 8080, false);
    }
  }
}