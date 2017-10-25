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

				UpdatePoints();
			}
		}


		private void UpdatePoints()
		{
			foreach (var zone in Configuration.Instance.CpArray)
			{
				string str = "Zone " + zone.name + ": ";
				foreach (var player in zone.playerList)
				{
					str = str + UnturnedPlayer.FromCSteamID(new CSteamID(player)).CharacterName.ToString() + ", ";
				}
				if (zone.playerList.Count > 0)
					UnturnedChat.Say(DateTime.Now.ToString("s") + " " + str);
			}

			for (int i = 0; i < Configuration.Instance.CpArray.Length; i++)
			{
					Configuration.Instance.CpArray[i].CountPlayersTeams(out int a, out int b);

					switch (Configuration.Instance.CpArray[i].state)
					{
						case Zone.State.NONE:
							{
								if (IsPointCapturableByA(i) && a > 0 && b == 0)
								{
									UnturnedChat.Say("Team A started capturing point " + Configuration.Instance.CpArray[i].name);
									Configuration.Instance.CpArray[i].state = Zone.State.TEAMA_C;
									Configuration.Instance.CpArray[i].captureStarted = DateTime.Now;
									Configuration.Instance.CpArray[i].prevState = Zone.State.NONE;
									break;
								}
								if (IsPointCapturableByB(i) && a == 0 && b > 0)
								{
									UnturnedChat.Say("Team B started capturing point " + Configuration.Instance.CpArray[i].name);
									Configuration.Instance.CpArray[i].state = Zone.State.TEAMB_C;
									Configuration.Instance.CpArray[i].captureStarted = DateTime.Now;
									Configuration.Instance.CpArray[i].prevState = Zone.State.NONE;
									break;
								}
								break;
							}

						case Zone.State.TEAMA_C:
							{
								if (!IsPointCapturableByA(i) || a == 0 || b > 0)
								{
									UnturnedChat.Say("Team A aborted capturing point " + Configuration.Instance.CpArray[i].name);
									Configuration.Instance.CpArray[i].state = Configuration.Instance.CpArray[i].prevState;
									break;
								}

								if ((DateTime.Now - Configuration.Instance.CpArray[i].captureStarted).TotalSeconds > Configuration.Instance.secondsCapture)
								{
									UnturnedChat.Say("Team A captured point " + Configuration.Instance.CpArray[i].name);
									Configuration.Instance.CpArray[i].state = Zone.State.TEAMA;
									break;
								}

								break;
							}

						case Zone.State.TEAMB_C:
							{
								if (!IsPointCapturableByB(i) || a > 0 || b == 0)
								{
									UnturnedChat.Say("Team B aborted capturing point " + Configuration.Instance.CpArray[i].name);
									Configuration.Instance.CpArray[i].state = Configuration.Instance.CpArray[i].prevState;
									break;
								}

								if ((DateTime.Now - Configuration.Instance.CpArray[i].captureStarted).TotalSeconds > Configuration.Instance.secondsCapture)
								{
									UnturnedChat.Say("Team B captured point " + Configuration.Instance.CpArray[i].name);
									Configuration.Instance.CpArray[i].state = Zone.State.TEAMB;
									break;
								}
								break;
							}

						case Zone.State.TEAMA:
							{
								if (IsPointCapturableByB(i) && (a == 0 || b > 0))
								{
									UnturnedChat.Say("Team B started capturing point " + Configuration.Instance.CpArray[i].name);
									Configuration.Instance.CpArray[i].state = Zone.State.TEAMB_C;
									Configuration.Instance.CpArray[i].captureStarted = DateTime.Now;
									Configuration.Instance.CpArray[i].prevState = Zone.State.TEAMA;
								}
								break;
							}

						case Zone.State.TEAMB:
							{
								if (IsPointCapturableByA(i) && (a > 0 || b == 0))
								{
									UnturnedChat.Say("Team A started capturing point " + Configuration.Instance.CpArray[i].name);
									Configuration.Instance.CpArray[i].state = Zone.State.TEAMA_C;
									Configuration.Instance.CpArray[i].captureStarted = DateTime.Now;
									Configuration.Instance.CpArray[i].prevState = Zone.State.TEAMB;
								}
								break;
							}

						default:
							break;
					}

			}
		}


		//private bool IsPointCapturable(int CpArrayIndex)
		//{
		//	if (Configuration.Instance.CpArray.Length < 2)
		//		return false;

		//	//first point from team A
		//	if (CpArrayIndex == 0 && Configuration.Instance.CpArray[1].state != Zone.State.TEAMA)
		//		return true;
		//	//last point from team A
		//	if ((CpArrayIndex == Configuration.Instance.CpArray.Length - 1) && Configuration.Instance.CpArray[Configuration.Instance.CpArray.Length - 2].state != Zone.State.TEAMB)
		//		return true;

		//	if (Configuration.Instance.CpArray.Length < 3)
		//		return false;

		//	//every other point
		//	Zone.State prev = Configuration.Instance.CpArray[CpArrayIndex - 1].state;
		//	Zone.State next = Configuration.Instance.CpArray[CpArrayIndex + 1].state;
		//	if ((prev == Zone.State.TEAMA && next == Zone.State.TEAMA) || (prev == Zone.State.TEAMB && next == Zone.State.TEAMB))
		//		return false;

		//	return true;
		//}


		private bool IsPointCapturableByA(int CpArrayIndex)
		{
			if (CpArrayIndex == 0)
				return true;
			if (Configuration.Instance.CpArray[CpArrayIndex - 1].state == Zone.State.TEAMA)
				return true;
			return false;
		}


		private bool IsPointCapturableByB(int CpArrayIndex)
		{
			if (CpArrayIndex == Configuration.Instance.CpArray.Length - 1)
				return true;
			if (Configuration.Instance.CpArray.Length > CpArrayIndex && Configuration.Instance.CpArray[CpArrayIndex + 1].state == Zone.State.TEAMB)
				return true;
			return false;
		}


		private void UpdateSpawnZone()
		{
			//TODO move
		}
	}
}
