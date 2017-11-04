using System.Collections.Generic;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;

namespace Conquest
{
	public class CommandSpawn : IRocketCommand
	{
		public string Name
		{
			get { return "spawn"; }
		}

		public string Help
		{
			get { return ("Change spawn point"); }
		}

		public AllowedCaller AllowedCaller
		{
			get { return AllowedCaller.Player; }
		}

		public string Syntax
		{
			get { return "Usage: /spawn [base | point]"; }
		}

		public List<string> Aliases
		{
			get { return new List<string>() { }; }
		}

		public List<string> Permissions
		{
			get { return new List<string>() { "Conquest.spawn" }; }

		}

		public void Execute(IRocketPlayer caller, string[] command)
		{
			UnturnedPlayer player = (UnturnedPlayer)caller;
			if (Conquest.instance == null || player == null)
			{
				UnturnedChat.Say("Something's wrong with the plugin!", UnityEngine.Color.red);
				return;
			}

			if (!Conquest.instance.playerList.ContainsKey(player.CSteamID.m_SteamID))
			{
				Conquest.instance.playerList.Add(player.CSteamID.m_SteamID, new PlayerListItem());
			}

			if (command == null || command.Length != 1)
			{
				UnturnedChat.Say(caller, "Set spawn point for all your following non-bedroll respawns. Usage: /spawn [base | point]", Conquest.instance.Configuration.Instance.messageColor);
				return;
			}

			if (command[0] == "base")
			{
				Conquest.instance.playerList[player.CSteamID.m_SteamID].spawnAtBase = true;
				UnturnedChat.Say(caller, "Next time you'll respawn at the base", Conquest.instance.Configuration.Instance.messageColor);
				return;
			}

			if (command[0] == "point")
			{
				Conquest.instance.playerList[player.CSteamID.m_SteamID].spawnAtBase = false;
				UnturnedChat.Say(caller, "Next time you'll respawn at the farthest captured control point ", Conquest.instance.Configuration.Instance.messageColor);
				return;
			}

			UnturnedChat.Say(caller, "Set spawn point for next non-bedroll respawns. Usage: /spawn [base | point]", Conquest.instance.Configuration.Instance.messageColor);
		}
	}
}
