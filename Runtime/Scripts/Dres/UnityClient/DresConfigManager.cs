using System.IO;
using Dev.Dres.ClientApi.Client;
using Dres.UnityClient;
using Newtonsoft.Json;
using UnityEngine;

namespace Dres.Unityclient
{
  /// <summary>
  /// The DRES config manager which handles loading (and storing) of configuration file(s).
  /// Since there ever has to exist one and only one config manager, this is implemented as singleton.
  /// </summary>
  public class DresConfigManager
  {
    
    /// <summary>
    /// The filename of the config file.
    /// </summary>
    public const string FileName = "dresapi";
    /// <summary>
    /// The filname of the credentials file
    /// </summary>
    public const string CredentialsName = "credentials";
    /// <summary>
    /// The config file extension.
    /// </summary>
    public const string FileExtension = "json";
    /// <summary>
    /// The full config file name (name + extension)
    /// </summary>
    public const string ConfigFile = FileName + "." + FileExtension;
    /// <summary>
    /// The full credentials file name (name + extension)
    /// </summary>
    public const string CredentialsFile = CredentialsName + "." + FileExtension;

    /// <summary>
    /// The instance of this singleton
    /// </summary>
    private static DresConfigManager _instance;

    /// <summary>
    /// The public available property for the single config manager instance.
    /// If none exists, a new one will be created.
    /// </summary>
    public static DresConfigManager Instance => _instance ?? (_instance = new DresConfigManager());

    /// <summary>
    /// Private constructor, to ensure no other instances are created.
    /// </summary>
    private DresConfigManager()
    {
      apiConfig = new Configuration {BasePath = Config.Endpoint};
    }

    /// <summary>
    /// The internal configuration property
    /// </summary>
    private DresConfig _config;

    /// <summary>
    /// The public available property to get or set the config.
    /// Loads the config from file if none was loaded previously
    /// </summary>
    /// <exception cref="FileNotFoundException">If the config has no credentials set and no credentials file exists</exception>
    public DresConfig Config
    {
      get
      {
        if (_config != null)
        {
          return _config;
        }

        if (File.Exists(GetFilePath(ConfigFile)))
        {
          var streamReader = File.OpenText(GetFilePath(ConfigFile));
          var json = streamReader.ReadToEnd();
          streamReader.Close();
          _config = DresConfig.GetDefault();
          JsonUtility.FromJsonOverwrite(json, _config);
        }
        else
        {
          Debug.Log($"No DRES config file found at {GetFilePath(ConfigFile)}, using defaults.");
          _config = DresConfig.GetDefault();
        }

        if (!_config.HasCredentials())
        {
          if (!File.Exists(GetFilePath(CredentialsFile)))
          {
            throw new FileNotFoundException("Expects a credentials file at " + GetFilePath(CredentialsFile));
          }

          var streamReader = File.OpenText(GetFilePath(CredentialsFile));
          var json = streamReader.ReadToEnd();
          streamReader.Close();
          var cred = JsonUtility.FromJson<Credentials>(json);
          _config.user = cred.username;
          _config.password = cred.password;
        }

        return _config;
      }
      set => _config = value;
    }

    /// <summary>
    /// Stores the currently active configuration to disk (overwrites any previously existing config)
    /// </summary>
    public void StoreConfig()
    {
      var user = _config.user;
      var password = _config.password;
      _config.user = null;
      _config.password = null;
      var sw = File.CreateText(GetFilePath(ConfigFile));
      sw.Write(JsonConvert.SerializeObject(_config));
      sw.WriteLine(""); // empty line at EOF
      sw.Flush();
      sw.Close();
      _config.user = user;
      _config.password = password;
    }

    /// <summary>
    /// The internal configuration for the openapi generated apis.
    /// </summary>
    private Configuration apiConfig;

    /// <summary>
    /// The public available property to pass OpenApi generated client APIs.
    /// </summary>
    public Configuration ApiConfiguration => apiConfig;

    /// <summary>
    /// Resolves unity-platform agnostic the given filename
    /// </summary>
    /// <param name="file">The filename to resolve relative to the unity platform's data folder.</param>
    /// <returns>A path representing the given filename as a child of the unity platform's data folder.</returns>
    public static string GetFilePath(string file)
    {
#if UNITY_EDITOR
      var folder = Application.dataPath;
#else
      var folder = Application.persistentDataPath;
#endif
      return Path.Combine(folder, file);
    }
  }
}