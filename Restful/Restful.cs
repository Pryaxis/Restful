using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using TerrariaApi.Server;
using Restful.Legacy;
using TShockAPI;

namespace Restful
{
	/// <summary>
	/// The plugin class for managing the RESTful API.
	/// </summary>
	[ApiVersion(2, 1)]
	public class Restful : TerrariaPlugin
	{
		/// <inheritdoc/>
		public override string Author => "Pryaxis";

		/// <inheritdoc/>
		public override string Description => "Provides a RESTful API to manage your TShock Server remotely.";

		/// <summary>
		/// A static proxy to the active Restful Plugin Instance.
		/// </summary>
		public static Restful Instance { get; private set; }

		/// <inheritdoc/>
		public override string Name => "Restful";

		/// <inheritdoc/>
		public override Version Version => Assembly.GetExecutingAssembly().GetName().Version;

		/// <summary>
		/// The RESTful API Authentication Manager instance.
		/// </summary>
		public SecureRest Api { get; private set; }

		/// <summary>
		/// A list of chat commands provided by this plugin.
		/// </summary>
		public ChatCommands ChatCommands { get; private set; }

		/// <summary>
		/// The RESTful API configuration file instance.
		/// </summary>
		public ConfigFile Config { get; private set; }

		/// <summary>
		/// The RESTful API Manager instance.
		/// </summary>
		public Legacy.RestManager Manager { get; private set; }

		/// <summary>
		/// Creates a new instance of this plugin bound to a <see cref="Main"/> instance.
		/// </summary>
		/// <param name="game">The <see cref="Main"/> instance.</param>
		public Restful(Main game) : base(game)
		{
			Instance = this;
		}

		/// <inheritdoc/>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				Api.Dispose();
			}
		}

		/// <inheritdoc/>
		public override void Initialize()
		{
			ServerApi.Hooks.GameInitialize.Register(this, OnInitialize);

			Config = ConfigFile.Read();
			Api = new SecureRest(Netplay.ServerIP, Config.Port);
			Manager = new Legacy.RestManager(Api);
			ChatCommands = new ChatCommands();
		}

		/// <summary>
		/// Occurs after the game has finished initializing.
		/// </summary>
		/// <param name="args">The EventArgs object.</param>
		private void OnInitialize(EventArgs args)
		{
			Manager.RegisterRestfulCommands();
			Api.Start();
			ChatCommands.RegisterAll();
		}
	}
}
