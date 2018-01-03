using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Restful.Legacy;
using System.IO;
using TShockAPI;

namespace Restful
{
	/// <summary>
	/// Stores the configuration for the RESTful API that is serialized as JSON.
	/// </summary>
	public class ConfigFile
	{
		/// <summary>
		/// A dictionary of REST tokens that external applications may use to make queries to your server.
		/// </summary>
		public Dictionary<string, SecureRest.TokenData> ApplicationTokens { get; set; } = new Dictionary<string, SecureRest.TokenData>();

		/// <summary>
		/// Whether or not to require token authentication for the public API endpoints.
		/// </summary>
		public bool EnableTokenEndpointAuthentication { get; set; } = false;

		/// <summary>
		/// Whether or not to log connections.
		/// </summary>
		public bool LogConnections { get; set; } = false;

		/// <summary>
		/// The port used by the RESTful API.
		/// </summary>
		public int Port { get; set; } = 7878;

		/// <summary>
		/// The maximum REST requests in the bucket before denying requests. Minimum value is 5.
		/// </summary>
		public int MaximumRequestsPerInterval { get; set; } = 5;

		/// <summary>
		/// How often in minutes the REST requests bucket is decreased by one. Minimum value is 1 minute.
		/// </summary>
		public int RequestBucketDecreaseIntervalMinutes { get; set; } = 1;

		/// <summary>
		/// Whether or not to limit only the max failed login requests, or all login requests.
		/// </summary>
		public bool LimitOnlyFailedLoginRequests { get; set; } = true;

		/// <summary>
		/// Reads a JSON text file from the default save path and loads a new <see cref="ConfigFile"/>
		/// instance by deserializing its contents.
		/// </summary>
		/// <returns>
		/// A new ConfigFile instance obtained by deserializing the contents of a JSON text file.
		/// If something goes wrong, an instance with the default values set will be returned instead.
		/// </returns>
		public static ConfigFile Read() => Read(Path.Combine(TShock.SavePath, "Restful.json"));

		/// <summary>
		/// Reads a JSON text file and loads a new <see cref="ConfigFile"/> instance by deserializing its contents.
		/// </summary>
		/// <param name="path">The path to the JSON-serialized text file to read.</param>
		/// <returns>
		/// A new ConfigFile instance obtained by deserializing the contents of a JSON text file.
		/// If something goes wrong, an instance with the default values set will be returned instead.
		/// </returns>
		public static ConfigFile Read(string path)
		{
			try
			{
				Directory.CreateDirectory(path);
				ConfigFile config;

				if (File.Exists(path))
				{
					config = JsonConvert.DeserializeObject<ConfigFile>(File.ReadAllText(path));
				}
				else
				{
					config = new ConfigFile();
					File.WriteAllText(path, JsonConvert.SerializeObject(config, Formatting.Indented));
					TShock.Log.ConsoleInfo($"[Restful] Generated a new Configuration at '{path}'.");
				}
				return config;
			}
			catch (Exception ex)
			{
				TShock.Log.ConsoleError($"[Restful] Failed to load Configuration from '{path}'." +
										$"[Restful] Message: {ex.Message}." +
										 "[Restful] The full exception has been logged.");
				TShock.Log.Error(ex.ToString());
				TShock.Log.ConsoleInfo("[Restful] The default Configuration will be used to start the REST Service.");
				return new ConfigFile();
			}
		}
	}
}
