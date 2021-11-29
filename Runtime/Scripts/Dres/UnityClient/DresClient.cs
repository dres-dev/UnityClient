using System.Collections.Generic;
using System.Threading.Tasks;
using Dev.Dres.ClientApi.Model;

namespace Dres.Unityclient
{
  /// <summary>
  /// Instantiable class to contain all client functionality for the DRES API.
  /// Holds all state information for a DRES user.<br />
  /// Stateful version of the <see cref="DresWrapper"/>
  /// </summary>
  public class DresClient
  {
    /// <summary>
    /// The user state, available after <see cref="Login"/>.
    /// </summary>
    public UserDetails UserDetails;

    /// <summary>
    /// Login to DRES with the currently loaded credentials.
    /// The user state, <see cref="UserDetails"/> is available after the operation.
    /// </summary>
    public async Task Login()
    {
      var config = DresConfigManager.Instance.Config;
      UserDetails = await DresWrapper.Login(config.user, config.password);
    }

    /// <summary>
    /// Submits the given item (and optionally frame informaiton) to the DRES instance as current user.
    /// </summary>
    /// <param name="item">The item name or identifier to submit</param>
    /// <param name="frame">Optional, the item's frame number. This can likely be omitted, if there is no such
    /// concept as frames for the given item (e.g. for videos, a frame is reasonable while for images it isn't.</param>
    /// <returns>The success / failure state of the operation</returns>
    public async Task<SuccessfulSubmissionsStatus> SubmitResult(string item, int? frame = null)
    {
      return await DresWrapper.Submit(item, UserDetails.SessionId, frame);
    }

    /// <summary>
    /// Sends the given results as result log to the DRES instance as current user.
    /// For further information on the logging format, please consider visiting the
    /// <see href="https://www.overleaf.com/read/rppygxshvhrn">external document</see>
    /// </summary>
    /// <param name="timestamp">The client side timestamp of this log</param>
    /// <param name="sortType">The sort type used</param>
    /// <param name="resultSetAvailability">The result set availability</param>
    /// <param name="results">The results to log</param>
    /// <param name="events">The events associated with these results</param>
    /// <returns>The success / failure state of the operation</returns>
    public async Task<SuccessStatus> LogResults(long timestamp, string sortType, string resultSetAvailability,
      List<QueryResult> results, List<QueryEvent> events)
    {
      return await DresWrapper.LogResults(timestamp, sortType, resultSetAvailability, results, events,
        UserDetails.SessionId);
    }

    /// <summary>
    /// Sends the given interaction evenets to log to the DRES instance as current user.
    /// For further information on the logging format, please consider visiting the
    /// <see href="https://www.overleaf.com/read/rppygxshvhrn">external document</see>
    /// </summary>
    /// <param name="timestamp">The client side timestamp of this log</param>
    /// <param name="events">The events to log</param>
    /// <returns>The success / failure state of the operation</returns>
    public async Task<SuccessStatus> LogQueryEvents(long timestamp, List<QueryEvent> events)
    {
      return await DresWrapper.LogQueryEvents(timestamp, events, UserDetails.SessionId);
    }
  }
}