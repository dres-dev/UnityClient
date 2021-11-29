using System;

namespace Dres.UnityClient
{
  [Serializable]
  public class Credentials
  {
    /// <summary>
    /// The DRES username, created and given by the DRES operator.
    /// </summary>
    public string username;
    /// <summary>
    /// The DRES password, created and given by the DRES operator.
    /// </summary>
    public string password;
  }
}