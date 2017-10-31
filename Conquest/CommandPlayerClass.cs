using System.Collections.Generic;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;

namespace Conquest
{
	public class CommandPlayerClass : IRocketCommand
	{
		public string Name
		{
			get { return "class"; }
		}

		public string Help
		{
			get { return ("Change player class"); }
		}

		public AllowedCaller AllowedCaller
		{
			get { return AllowedCaller.Player; }
		}

		public string Syntax
		{
			get { return "Usage: /class [class1 | class2 | class3]"; }
		}

		public List<string> Aliases
		{
			get { return new List<string>() { }; }
		}

		public List<string> Permissions
		{
			get { return new List<string>() { "Conquest.class" }; }

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
				UnturnedChat.Say(caller, "Usage: /class [class1 | class2 | class3]", Conquest.instance.Configuration.Instance.messageColor); //TODO class list
				return;
			}

			for (int i = 0; i < Conquest.instance.Configuration.Instance.playerClasses.Length; i++)
			{

				if (Conquest.instance.Configuration.Instance.playerClasses[i].name.ToLower() == command[0].ToLower())
				{
					Conquest.instance.playerList[player.CSteamID.m_SteamID].classId = i;
					UnturnedChat.Say(caller, "Your new class - " + Conquest.instance.Configuration.Instance.playerClasses[i].name + ", effective from next respawn.", Conquest.instance.Configuration.Instance.messageColor);
					return;
				}
			}

			UnturnedChat.Say(caller, "No such class!", Conquest.instance.Configuration.Instance.messageColor);
		}
	}
}
