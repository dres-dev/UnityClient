using System.Threading.Tasks;
using Org.Vitrivr.DresApi.Api;
using Org.Vitrivr.DresApi.Client;
using Org.Vitrivr.DresApi.Model;
using UnityEngine;
using UnityEngine.Serialization;

namespace Vitrivr.UnityInterface.DresApi
{
  public class DresClient : MonoBehaviour
  {
    public DresConfig dresConfig;

    public UserApi UserService;
    public LogApi LoggingService;
    public SubmissionApi SubmissionService;
    
    public string session; // Could be private


    private async void Start()
    {
      dresConfig = ConfigManager.Instance.Config;
      UserService = new UserApi(ConfigManager.Instance.ApiConfiguration);
      LoggingService = new LogApi(ConfigManager.Instance.ApiConfiguration);
      SubmissionService = new SubmissionApi(ConfigManager.Instance.ApiConfiguration);
      await Login(dresConfig.user, dresConfig.password);
      Debug.Log($"Login gave Session: {session}");
    }

    public async Task Login(string user, string pw)
    {
      var loginRequest = new LoginRequest(user, pw);
      var userDetails = await UserService.PostApiLoginAsync(loginRequest);
      session = userDetails.SessionId;
    }

    public async Task SubmitResult(string item, int frame)
    {
      var submitStatus = await SubmissionService.GetSubmitAsync(collection: null, item: item,
        frame: frame, session: session);
      Debug.Log(submitStatus);
    }
  }
}