using System.Collections.Generic;
using System.Threading.Tasks;
using Dev.Dres.ClientApi.Api;
using Dev.Dres.ClientApi.Client;
using Dev.Dres.ClientApi.Model;

namespace Dres.Unityclient
{
  /// <summary>
  /// Class wrapping all of the DRES REST APIs functionality.
  /// Using this class ensures only a single instance of each endpoint is created.
  /// <br />
  /// This class is held deliberately stateless (no session ID etc.) to allow multi-user scenarios in the future.
  /// </summary>
  internal static class DresWrapper
  {
    
    /// <summary>
    /// The deliberately single Logging Api instance of DRES. Used to send logs to DRES
    /// </summary>
    internal static readonly LogApi LogApi = new LogApi(DresConfigManager.Instance.ApiConfiguration);
    /// <summary>
    /// The deliberately single Status Api instance of DRES. Used to get the status of DRES
    /// </summary>
    internal static readonly StatusApi StatusApi = new StatusApi(DresConfigManager.Instance.ApiConfiguration);
    /// <summary>
    /// The deliberately single Submission Api instance of DRES. Used to submit media items during competitions to DRES:
    /// </summary>
    internal static readonly SubmissionApi SubmissionApi = new SubmissionApi(DresConfigManager.Instance.ApiConfiguration);
    /// <summary>
    /// The deliberately single User Api instance of DRES. Used to log into DRES and retrieve the session id of the user.
    /// </summary>
    internal static readonly UserApi UserApi = new UserApi(DresConfigManager.Instance.ApiConfiguration);

    /// <summary>
    /// Login to DRES with given username and password.
    /// The login state (i.e. the <see cref="Dev.Dres.ClientApi.Model.UserDetails"/>) are not kept
    /// and have to be managed by the caller.
    /// </summary>
    /// <param name="user">The DRES username</param>
    /// <param name="password">The DRES password</param>
    /// <returns>The login state on success.</returns>
    /// <exception cref="ApiException">If the config has no credentials set and no credentials file exists</exception>
    internal static async Task<UserDetails> Login(string user, string password)
    {
      var loginRequest = new LoginRequest(user, password);
      return await UserApi.PostApiV1LoginAsync(loginRequest);
    }

    /// <summary>
    /// Submits an item to the DRES endpoint.
    /// Submissions are only allowed during active competitions (inferred from the given sesssion id)
    /// </summary>
    /// <param name="item">The name of the item (or identifier) to submit</param>
    /// <param name="session">The session id to which this submission belongs</param>
    /// <param name="frame">Optionally a frame number to specify which frame of the item.
    /// If no notion of frames exist for the item, this can likely be omitted.</param>
    /// <returns>The submission state on success / failure.</returns>
    /// <exception cref="ApiException">A 404 if there is no ongoing competition for this session, a 403 if there is no such user</exception>
    internal static async Task<SuccessfulSubmissionsStatus> Submit(string item, string session, int? frame = null)
    {
      return await SubmissionApi.GetApiV1SubmitAsync(item: item, frame: frame, session: session);
    }
    
    /// <summary>
    /// Submits given TEXT to the DRES endpoint.
    /// Submissions are only allowed during active competitions (inferred from the given sesssion id)
    /// </summary>
    /// <param name="text">The submission in textual form (might also be a number) submit</param>
    /// <param name="session">The session id to which this submission belongs</param>
    /// <returns>The submission state on success / failure.</returns>
    /// <exception cref="ApiException">A 404 if there is no ongoing competition for this session, a 403 if there is no such user</exception>
    internal static async Task<SuccessfulSubmissionsStatus> SubmitText(string text, string session)
    {
      return await SubmissionApi.GetApiV1SubmitAsync(text: text, session: session);
    }


    /// <summary>
    /// Send result logs to the DRES endpoint.
    /// For further information on the logging format, please consider visiting the
    /// <see href="https://www.overleaf.com/read/rppygxshvhrn">external document</see>
    /// </summary>
    /// <param name="timestamp">The client side timestamp of this log</param>
    /// <param name="sortType">The sort type which was used for these results</param>
    /// <param name="resultSetAvailability">The results et availability for these results</param>
    /// <param name="results">The actual results</param>
    /// <param name="events">The events that lead to the results</param>
    /// <param name="session">The session id to which this log belongs</param>
    /// <returns>The state of success / failure of the log sending.</returns>
    /// <exception cref="ApiException">A 404 if there is no ongoing competition for this session, a 403 if there is no such user</exception>
    internal static async Task<SuccessStatus> LogResults(long timestamp, string sortType, string resultSetAvailability,
      List<QueryResult> results, List<QueryEvent> events, string session)
    {
      var resultLog = new QueryResultLog(timestamp, sortType, resultSetAvailability, results, events);
      return await LogApi.PostApiV1LogResultAsync(session, resultLog);
    }

    /// <summary>
    /// Send interaction logs to the DRES endpoint.
    /// For further information on the logging format, please consider visiting the
    /// <see href="https://www.overleaf.com/read/rppygxshvhrn">external document</see>
    /// </summary>
    /// <param name="timestamp">The client side timestamp of this log</param>
    /// <param name="events">The events to log</param>
    /// <param name="session">The session id to which this log belongs</param>
    /// <returns>The state of success / failure of the log sending.</returns>
    /// <exception cref="ApiException">A 404 if there is no ongoing competition for this session, a 403 if there is no such user</exception>
    internal static async Task<SuccessStatus> LogQueryEvents(long timestamp, List<QueryEvent> events, string session)
    {
      var queryEventLog = new QueryEventLog(timestamp, events);
      return await LogApi.PostApiV1LogQueryAsync(session, queryEventLog);
    }
  }
}