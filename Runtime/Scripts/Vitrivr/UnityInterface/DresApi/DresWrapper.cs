using System.Collections.Generic;
using System.Threading.Tasks;
using Org.Vitrivr.DresApi.Api;
using Org.Vitrivr.DresApi.Model;

namespace Vitrivr.UnityInterface.DresApi
{
  /// <summary>
  /// Class wrapping all of the DRES REST APIs functionality.
  /// Using this class ensures only a single instance of each endpoint is created.
  ///
  /// This class is held deliberately stateless (no session ID etc.) to allow multi-user scenarios in the future.
  /// </summary>
  public static class DresWrapper
  {
    public static readonly LogApi LogApi = new LogApi(DresConfigManager.Instance.ApiConfiguration);
    public static readonly StatusApi StatusApi = new StatusApi(DresConfigManager.Instance.ApiConfiguration);
    public static readonly SubmissionApi SubmissionApi = new SubmissionApi(DresConfigManager.Instance.ApiConfiguration);
    public static readonly UserApi UserApi = new UserApi(DresConfigManager.Instance.ApiConfiguration);

    public static async Task<UserDetails> Login(string user, string password)
    {
      var loginRequest = new LoginRequest(user, password);
      return await UserApi.PostApiLoginAsync(loginRequest);
    }

    public static async Task<SuccessfulSubmissionsStatus> Submit(string item, int frame, string session)
    {
      return await SubmissionApi.GetSubmitAsync(item: item, frame: frame, session: session);
    }

    
    public static async Task<SuccessStatus> LogResults(long timestamp, string sortType, string resultSetAvailability,
      List<QueryResult> results, List<QueryEvent> events, string session)
    {
      var resultLog = new QueryResultLog(timestamp, sortType, resultSetAvailability, results, events);
      return await LogApi.PostLogResultAsync(session, resultLog);
    }
  }
}