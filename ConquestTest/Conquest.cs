using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Steamworks;
using UnityEngine;

using Rocket.API;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using Rocket.Unturned.Chat;

namespace Conquest
{
	public class Conquest : RocketPlugin<Configuration>
	{
		public static Conquest instance;
		public UInt32 lastUpdatedTicks;

		public Dictionary<CSteamID, ulong> zoneList; //PlayerID, SteamGroupID

		protected override void Load()
		{
			instance = this;
			zoneList = new Dictionary<CSteamID, ulong>();
			lastUpdatedTicks = 0;
			U.Events.OnPlayerConnected += UnturnedEvents_OnPlayerConnected;
			U.Events.OnPlayerDisconnected += UnturnedEvents_OnPlayerDisconnected;
		}


		protected override void Unload()
		{
			U.Events.OnPlayerConnected -= UnturnedEvents_OnPlayerConnected;
			U.Events.OnPlayerDisconnected -= UnturnedEvents_OnPlayerDisconnected;
			zoneList = null;
			instance = null;
		}


		private void UnturnedEvents_OnPlayerConnected(UnturnedPlayer player)
		{
		}


		private void UnturnedEvents_OnPlayerDisconnected(UnturnedPlayer player)
		{
		}


		private void FixedUpdate()
		{
			if (lastUpdatedTicks < Conquest.instance.Configuration.Instance.ticksUpdateZone)
			{
				lastUpdatedTicks++;
				return;
			}
			else
			{
				lastUpdatedTicks = 0;
				if (zoneList.Count > 0)
				{
					string str = "In the zone: ";
					foreach (var player in zoneList)
					{
						str = str + UnturnedPlayer.FromCSteamID(player.Key).CharacterName.ToString() + ", ";
					}
					UnturnedChat.Say(DateTime.Now.ToString("s") + " " + str);
				}
			}
		}
	}
}
