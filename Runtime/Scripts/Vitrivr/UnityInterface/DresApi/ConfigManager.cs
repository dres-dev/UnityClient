using System.IO;
using Newtonsoft.Json;
using Org.Vitrivr.DresApi.Client;
using UnityEngine;

namespace Vitrivr.UnityInterface.DresApi
{
  public class ConfigManager
  {
    public const string FileName = "dresapi";
    public const string CredentialsName = "credentials";
    public const string FileExtension = "json";
    public const string ConfigFile = FileName +"."+FileExtension;
    public const string CredentialsFile = CredentialsName + "." + FileExtension;

    private static ConfigManager _instance;

    public static ConfigManager Instance => _instance ?? (_instance = new ConfigManager());

    private ConfigManager()
    {
      apiConfig = new Configuration {BasePath = Config.Endpoint};
    }

    private DresConfig config;

    public DresConfig Config
    {
      get
      {
        if (config != null)
        {
          return config;
        }
        
        if (File.Exists(GetFilePath(ConfigFile)))
        {
          var streamReader = File.OpenText(GetFilePath(ConfigFile));
          var json = streamReader.ReadToEnd();
          streamReader.Close();
          config = DresConfig.GetDefault();
          JsonUtility.FromJsonOverwrite(json, config);
        }
        else
        {
          config = DresConfig.GetDefault();
        }

        if (!config.HasCredentials())
        {
          if (!File.Exists(GetFilePath(CredentialsFile)))
          {
            throw new FileNotFoundException("Expects a credentials file at " + GetFilePath(CredentialsFile));
          }
          var streamReader = File.OpenText(GetFilePath(CredentialsFile));
          var json = streamReader.ReadToEnd();
          streamReader.Close();
          var cred = JsonUtility.FromJson<Credentials>(json);
          config.user = cred.username;
          config.password = cred.password;
        }
        return config;
      }
      set => config = value;
    }

    public void StoreConfig()
    {
      var user = config.user;
      var password = config.password;
      config.user = null;
      config.password = null;
      var sw = File.CreateText(GetFilePath(ConfigFile));
      sw.Write(JsonConvert.SerializeObject(config));
      sw.WriteLine(""); // empty line at EOF
      sw.Flush();
      sw.Close();
      config.user = user;
      config.password = password;
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