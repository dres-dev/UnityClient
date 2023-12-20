using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dev.Dres.ClientApi.Model;
using JetBrains.Annotations;

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
    public ApiUser UserDetails { get; private set; }

    /// <summary>
    /// List of the available evaluations for the current user.
    /// Must be updated manually with <see cref="UpdateEvaluations"/> before use.
    /// </summary>
    public List<ApiClientEvaluationInfo> EvaluationInfo { get; private set; }

    /// <summary>
    /// The currently selected evaluation. Used for submissions.
    /// </summary>
    public ApiClientEvaluationInfo CurrentEvaluation { get; private set; }

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
    /// Updates the list of available evaluations for the current user.
    /// </summary>
    /// <returns>List of available evaluations</returns>
    public async Task<List<ApiClientEvaluationInfo>> UpdateEvaluations()
    {
      EvaluationInfo = await DresWrapper.ListClientEvaluations(UserDetails.SessionId);
      return EvaluationInfo;
    }

    /// <summary>
    /// Sets the current evaluation to the one with the given id.
    /// </summary>
    /// <param name="evaluationId">The id of the evaluation to set as current</param>
    /// <returns>True if the evaluation was found and set, false otherwise</returns>
    public bool SetCurrentEvaluation(string evaluationId)
    {
      CurrentEvaluation = EvaluationInfo.Find(evaluation => evaluation.Id == evaluationId);
      return CurrentEvaluation != null;
    }

    /// <summary>
    /// Submits the given item (and optionally start & end information) to the DRES instance as current user.
    /// </summary>
    /// <param name="item">The item name or identifier to submit</param>
    /// <param name="start">Optional, the item's start time</param>
    /// <param name="end">Optional, the item's end time</param>
    /// <param name="evaluationId">Manual override of the evaluation ID of the currently set evaluation.</param>
    /// <returns>The success / failure state of the operation</returns>
    public Task<SuccessfulSubmissionsStatus> SubmitResultV2(string item, long? start = null, long? end = null,
      [CanBeNull] string evaluationId = null)
    {
      evaluationId ??= CurrentEvaluation.Id;
      return DresWrapper.SubmitV2(UserDetails.SessionId, evaluationId, item, start, end);
    }

    /// <summary>
    /// Submits the given text to the DRES instance as current user
    /// </summary>
    /// <param name="text">The text to submit (this can be anything).</param>
    /// <param name="evaluationId">Manual override of the evaluation ID of the currently set evaluation.</param>
    /// <returns>The success / failure state of the operation</returns>
    public Task<SuccessfulSubmissionsStatus> SubmitTextualResultV2(string text, [CanBeNull] string evaluationId = null)
    {
      evaluationId ??= CurrentEvaluation.Id;
      return DresWrapper.SubmitTextV2(UserDetails.SessionId, evaluationId, text);
    }


    /// <summary>
    /// Submits the given item (and optionally frame informaiton) to the DRES instance as current user.
    /// </summary>
    /// <param name="item">The item name or identifier to submit</param>
    /// <param name="frame">Optional, the item's frame number. This can likely be omitted, if there is no such
    /// concept as frames for the given item (e.g. for videos, a frame is reasonable while for images it isn't.</param>
    /// <returns>The success / failure state of the operation</returns>
    [Obsolete("Obsolete")]
    public Task<SuccessfulSubmissionsStatus> SubmitResult(string item, int? frame = null)
    {
      return DresWrapper.Submit(item, UserDetails.SessionId, frame);
    }

    /// <summary>
    /// Submits the given text to the DRES instance as current user
    /// </summary>
    /// <param name="text">The text to submit (this can be anything).</param>
    /// <returns>The success / failure state of the operation</returns>
    [Obsolete("Obsolete")]
    public Task<SuccessfulSubmissionsStatus> SubmitTextualResult(string text)
    {
      return DresWrapper.SubmitText(text, UserDetails.SessionId);
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
    public Task<SuccessStatus> LogResults(long timestamp, string sortType, string resultSetAvailability,
      List<QueryResult> results, List<QueryEvent> events)
    {
      return DresWrapper.LogResults(timestamp, sortType, resultSetAvailability, results, events,
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
    public Task<SuccessStatus> LogQueryEvents(long timestamp, List<QueryEvent> events)
    {
      return DresWrapper.LogQueryEvents(timestamp, events, UserDetails.SessionId);
    }
  }
}