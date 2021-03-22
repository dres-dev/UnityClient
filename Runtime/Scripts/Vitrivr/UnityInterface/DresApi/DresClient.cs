using System.Threading.Tasks;
using Org.Vitrivr.DresApi.Model;

namespace Vitrivr.UnityInterface.DresApi
{
  /// <summary>
  /// Instantiable class to contain all client functionality for the DRES API.
  /// Holds all state information for a DRES user.
  /// </summary>
  public class DresClient
  {
    public UserDetails UserDetails;

    public async Task Login()
    {
      var config = DresConfigManager.Instance.Config;
      UserDetails = await DresWrapper.Login(config.user, config.password);
    }

    public async Task<SuccessfulSubmissionsStatus> SubmitResult(string item, int frame)
    {
      return await DresWrapper.Submit(item, frame, UserDetails.SessionId);
    }
  }
}