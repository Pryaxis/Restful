using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TShockAPI;
using Restful.Legacy;
using Microsoft.Xna.Framework;

namespace Restful
{
	/// <summary>
	/// Manages all chat commands provided by this plugin.
	/// </summary>
	public class ChatCommands
	{
		/// <summary>
		/// A read-only list of commands provided by this class.
		/// </summary>
		public IReadOnlyList<Command> Commands;

		/// <summary>
		/// Initializes a new instance of this class and populates its command list.
		/// </summary>
		/// <param name="main">The main <see cref="Restful"/> instance.</param>
		public ChatCommands()
		{
			Commands = new List<Command>
			{
				new Command(RestPermissions.restmanage, DoRest, "rest", "restful")
				{
					HelpText = "Manages the REST API."
				}
			};
		}

		/// <summary>
		/// Registers all commands provided by this class to the list of TShock chat commands.
		/// </summary>
		public void RegisterAll()
		{
			foreach (Command cmd in Commands)
			{
				TShockAPI.Commands.ChatCommands.RemoveAll(c => c.Names.Intersect(cmd.Names).Any());
				TShockAPI.Commands.ChatCommands.Add(cmd);
			}
		}

		private void DoRest(CommandArgs args)
		{
			string subCommand = "help";
			if (args.Parameters.Count > 0)
				subCommand = args.Parameters[0];

			switch (subCommand.ToLower())
			{
				case "listusers":
					if (!PaginationTools.TryParsePageNumber(args.Parameters, 1, args.Player, out int pageNumber))
						return;

					var restUsersTokens = new Dictionary<string, int>();
					foreach (SecureRest.TokenData tokenData in Restful.Instance.Api.Tokens.Values)
					{
						if (restUsersTokens.ContainsKey(tokenData.Username))
							restUsersTokens[tokenData.Username]++;
						else
							restUsersTokens.Add(tokenData.Username, 1);
					}

					var restUsers = new List<string>(restUsersTokens.Select(ut => $"{ut.Key} ({ut.Value} tokens)"));

					PaginationTools.SendPage(args.Player, pageNumber, PaginationTools.BuildLinesFromTerms(restUsers),
						new PaginationTools.Settings
						{
							NothingToDisplayString = "There are currently no active REST users.",
							HeaderFormat = "Active REST Users ({0}/{1}):",
							FooterFormat = $"Type {TShockAPI.Commands.Specifier}rest listusers {{0}} for more."
						}
					);
					break;

				case "destroytokens":
					Restful.Instance.Api.Tokens.Clear();
					args.Player.SendSuccessMessage("All REST tokens have been destroyed.");
					break;

				default:
					args.Player.SendInfoMessage("Available REST Sub-Commands:");
					args.Player.SendMessage("listusers - Lists all REST users and their current active tokens.", Color.White);
					args.Player.SendMessage("destroytokens - Destroys all current REST tokens.", Color.White);
					break;
			}
		}
	}
}
