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

		private string GetUsageString()
		{
			string usage = "Usage: /class [class1 | class2 | class3]";
			if (Conquest.instance == null || Conquest.instance.Configuration.Instance == null || Conquest.instance.Configuration.Instance.playerClasses == null || Conquest.instance.Configuration.Instance.playerClasses.Length == 0)
				return usage;
			usage = "";
			for (int i = 0; i < Conquest.instance.Configuration.Instance.playerClasses.Length; i++)
			{
				usage = usage + Conquest.instance.Configuration.Instance.playerClasses[i].name;
				if (i != Conquest.instance.Configuration.Instance.playerClasses.Length - 1)
					usage = usage + " | ";
			}
			return "Usage: /class [" + usage + "]";
		}

		public string Syntax
		{
			get { return GetUsageString(); }
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
				UnturnedChat.Say(caller, GetUsageString(), Conquest.instance.Configuration.Instance.messageColor);
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
