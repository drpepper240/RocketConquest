using System.Collections.Generic;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;

namespace Conquest
{
	public class CommandStatus : IRocketCommand
	{
		public string Name
		{
			get { return "status"; }
		}

		public string Help
		{
			get { return ("Conquest status"); }
		}

		public AllowedCaller AllowedCaller
		{
			get { return AllowedCaller.Player; }
		}

		public string Syntax
		{
			get { return "Usage: /status"; }
		}

		public List<string> Aliases
		{
			get { return new List<string>() { }; }
		}

		public List<string> Permissions
		{
			get { return new List<string>() { "Conquest.status" }; }

		}

		public void Execute(IRocketPlayer caller, string[] command)
		{
			UnturnedPlayer player = (UnturnedPlayer)caller;
			if (Conquest.instance == null || player == null || Conquest.instance.Configuration.Instance == null)
			{
				UnturnedChat.Say("Something's wrong with the plugin!", UnityEngine.Color.red);
				return;
			}

			string teamA = "Team A captured: ";
			string teamB = "Team B captured: ";

			for (int i = 0; i < Conquest.instance.Configuration.Instance.CpArray.Length; i++)
			{
				if (Conquest.instance.Configuration.Instance.CpArray[i].state == Zone.State.TEAMA)
					teamA += Conquest.instance.Configuration.Instance.CpArray[i].name + ", ";
				if (Conquest.instance.Configuration.Instance.CpArray[i].state == Zone.State.TEAMB)
					teamB += Conquest.instance.Configuration.Instance.CpArray[i].name + ", ";
			}
			UnturnedChat.Say(caller, teamA, Conquest.instance.Configuration.Instance.messageColor);
			UnturnedChat.Say(caller, teamB, Conquest.instance.Configuration.Instance.messageColor);
		}
	}
}
