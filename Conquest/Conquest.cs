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
using System.Collections;

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
			if (player.SteamGroupID.m_SteamID == instance.Configuration.Instance.teamASteamId)
			{
				UnturnedChat.Say(player.CharacterName + " has joined team A", Conquest.instance.Configuration.Instance.messageColor);
			}
			else if (player.SteamGroupID.m_SteamID == instance.Configuration.Instance.teamBSteamId)
			{
				UnturnedChat.Say(player.CharacterName + " has joined team B", Conquest.instance.Configuration.Instance.messageColor);
			}
			else
			{
				UnturnedChat.Say(player.CharacterName + " haven't set their team setting correctly."
								+ (instance.Configuration.Instance.kickPlayerWithInvalidTeam ? (" Kicking in " + instance.Configuration.Instance.kickDelaySeconds.ToString()) + " seconds" : ""), Conquest.instance.Configuration.Instance.messageColor);
				if (instance.Configuration.Instance.kickPlayerWithInvalidTeam)
				{
					StartCoroutine(KickPlayer(player, instance.Configuration.Instance.kickDelaySeconds, "   To play on this server please select one of these groups as primary in your Unturned settings (Survivors -> Group -> Group):\n"
						+ instance.Configuration.Instance.teamASteamUri + "    " + instance.Configuration.Instance.teamBSteamUri));
				}
			}
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
				//removing records about players not present
				zone.playerList.RemoveWhere(p => UnturnedPlayer.FromCSteamID(new CSteamID(p)) == null);

				//TODO remove
				string str = "Zone " + zone.name + ": ";
				foreach (var player in zone.playerList)
				{
					UnturnedPlayer uPlayer = UnturnedPlayer.FromCSteamID(new CSteamID(player));
					if (uPlayer == null)
						continue;
					str = str + uPlayer.CharacterName.ToString() + ", ";
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


		public IEnumerator KickPlayer(UnturnedPlayer player, uint delaySeconds, string reason)
		{
			if (player.HasPermission("kick.ignore"))
			{
				yield break;
			}
			else if (delaySeconds <= 0f)
			{
				yield return new WaitForSeconds(1f);
				player.Kick(reason);
			}
			else
			{
				yield return new WaitForSeconds(delaySeconds);
				player.Kick(reason);
			}
		}
	}
}
