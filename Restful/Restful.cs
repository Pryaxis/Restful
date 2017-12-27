using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using TerrariaApi.Server;

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

		/// <inheritdoc/>
		public override string Name => "Restful";

		/// <inheritdoc/>
		public override Version Version => Assembly.GetExecutingAssembly().GetName().Version;

		/// <summary>
		/// Creates a new instance of this plugin bound to a <see cref="Main"/> instance.
		/// </summary>
		/// <param name="game">The <see cref="Main"/> instance.</param>
		public Restful(Main game) : base(game)
		{

		}

		/// <inheritdoc/>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{

			}
		}

		/// <inheritdoc/>
		public override void Initialize()
		{
			
		}
	}
}
