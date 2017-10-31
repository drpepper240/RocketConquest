using System.Collections.Generic;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;

namespace Conquest
{
	public class CommandGetCoordinates : IRocketCommand
	{
		public string Name
		{
			get { return "gc"; }
		}

		public string Help
		{
			get { return ("Get coordinates"); }
		}

		public AllowedCaller AllowedCaller
		{
			get { return AllowedCaller.Player; }
		}

		public string Syntax
		{
			get { return "Usage: /gc"; }
		}

		public List<string> Aliases
		{
			get { return new List<string>() { }; }
		}

		public List<string> Permissions
		{
			get { return new List<string>() { "Conquest.permission" }; }

		}

		public void Execute(IRocketPlayer caller, string[] command)
		{
			UnturnedPlayer up = (UnturnedPlayer)caller;
			if (Conquest.instance == null || up == null)
			{
				UnturnedChat.Say("Something's wrong with the plugin!", UnityEngine.Color.red);
				return;
			}

			UnturnedChat.Say(caller, "X = " + up.Position.x.ToString());
			UnturnedChat.Say(caller, "Y = " + up.Position.y.ToString());
			UnturnedChat.Say(caller, "Z = " + up.Position.z.ToString());
		}
	}
}
