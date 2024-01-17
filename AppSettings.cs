using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using dotenv.net;
namespace WebAPIProduct
{
    public class AppSettings
    {
    private const string DEFAULT_DB_SERVER = "(LOCALDB)\\SERVERDB";
    private const string DEFAULT_DB_NAME = "WebsiteForms";
    private const string DEFAULT_DB_USER = "";
    private const string DEFAULT_DB_PASSWORD = "";
    private const string DEFAULT_DB_INTEGRATED_SECURITY = "true";
    public string RootPath { get; private set; }
    public string DbServer { get; set; }
    public string DbName { get; set; }
    public string DbUser { get; set; }
    public string DbPassword { get; set; }
    public string DbIntegratedSecuryty { get; set; }
    public AppSettings()
    {
      string projectDirectory = Path.GetFullPath(".\\");

      var envFilePaths = new string[]
      {
          Path.Combine(projectDirectory, ".env"),
          Path.Combine(Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..")), ".env")
      };

      DotEnv.Load(options: new DotEnvOptions(envFilePaths: envFilePaths)) ;

      RootPath = projectDirectory;
      
      DbServer = Environment.GetEnvironmentVariable("DB_HOST") ?? DEFAULT_DB_SERVER;
      DbName = Environment.GetEnvironmentVariable("DB_NAME") ?? DEFAULT_DB_NAME;
      DbUser = Environment.GetEnvironmentVariable("DB_USER") ?? DEFAULT_DB_USER;
      DbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? DEFAULT_DB_PASSWORD;
      DbIntegratedSecuryty = bool.Parse(Environment.GetEnvironmentVariable("DB_INTEGRATED_SECURITY") ?? DEFAULT_DB_INTEGRATED_SECURITY).ToString();
  }
    }
}