using System.IO;
using Newtonsoft.Json;
using Org.Vitrivr.DresApi.Client;
using UnityEngine;

namespace Vitrivr.UnityInterface.DresApi
{
  public class DresConfigManager
  {
    public const string FileName = "dresapi";
    public const string CredentialsName = "credentials";
    public const string FileExtension = "json";
    public const string ConfigFile = FileName + "." + FileExtension;
    public const string CredentialsFile = CredentialsName + "." + FileExtension;

    private static DresConfigManager _instance;

    public static DresConfigManager Instance => _instance ?? (_instance = new DresConfigManager());

    private DresConfigManager()
    {
      apiConfig = new Configuration {BasePath = Config.Endpoint};
    }

    private DresConfig _config;

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

    private Configuration apiConfig;

    public Configuration ApiConfiguration => apiConfig;

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