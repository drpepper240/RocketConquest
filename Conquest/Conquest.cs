﻿using System;
using System.Collections.Generic;
using UnityEngine;

using Rocket.API;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using Rocket.Unturned.Chat;
using System.Collections;
using SDG.Unturned;
using Steamworks;
using Rocket.Unturned.Skills;

namespace Conquest
{
	//main class
	public class Conquest : RocketPlugin<Configuration>
	{
		public static Conquest instance;
		public UInt32 lastUpdatedTicks;

		public Dictionary<ulong, PlayerListItem> playerList;

		protected override void Load()
		{
			instance = this;
			lastUpdatedTicks = 0;
			playerList = new Dictionary<ulong, PlayerListItem>();
			U.Events.OnPlayerConnected += UnturnedEvents_OnPlayerConnected;
			U.Events.OnPlayerDisconnected += UnturnedEvents_OnPlayerDisconnected;
			UnturnedPlayerEvents.OnPlayerRevive += UnturnedPlayerEvents_OnPlayerRevive;
			UnturnedPlayerEvents.OnPlayerDeath += UnturnedPlayerEvents_OnPlayerDeath;
		}


		protected override void Unload()
		{
			U.Events.OnPlayerConnected -= UnturnedEvents_OnPlayerConnected;
			U.Events.OnPlayerDisconnected -= UnturnedEvents_OnPlayerDisconnected;
			UnturnedPlayerEvents.OnPlayerRevive -= UnturnedPlayerEvents_OnPlayerRevive;
			UnturnedPlayerEvents.OnPlayerDeath -= UnturnedPlayerEvents_OnPlayerDeath;
			playerList = null;
			instance = null;
		}


		private void UnturnedEvents_OnPlayerConnected(UnturnedPlayer player)
		{
			if (player.SteamGroupID.m_SteamID == instance.Configuration.Instance.teamASteamId)
			{
				UnturnedChat.Say(player.CharacterName + " has joined team A", instance.Configuration.Instance.messageColor);
			}
			else if (player.SteamGroupID.m_SteamID == instance.Configuration.Instance.teamBSteamId)
			{
				UnturnedChat.Say(player.CharacterName + " has joined team B", instance.Configuration.Instance.messageColor);
			}
			else
			{
				UnturnedChat.Say(player.CharacterName + " haven't set their team setting correctly."
								+ (instance.Configuration.Instance.kickPlayerWithInvalidTeam ? (" Kicking in " + instance.Configuration.Instance.kickDelaySeconds.ToString()) + " seconds" : ""), instance.Configuration.Instance.messageColor);
				if (instance.Configuration.Instance.kickPlayerWithInvalidTeam)
				{
					StartCoroutine(KickPlayer(player, instance.Configuration.Instance.kickDelaySeconds, "   To play on this server please select one of these groups as primary in your Unturned settings (Survivors -> Group -> Group):\n"
						+ instance.Configuration.Instance.teamASteamUri + "    " + instance.Configuration.Instance.teamBSteamUri));
				}
			}
		}


		private void UnturnedEvents_OnPlayerDisconnected(UnturnedPlayer player)
		{
			UnturnedChat.Say(player.CharacterName + " left", instance.Configuration.Instance.messageColor);
		}


		private void UnturnedPlayerEvents_OnPlayerRevive(UnturnedPlayer player, Vector3 position, byte angle)
		{
			if (instance.Configuration.Instance.spawnZone.IsInside(position))
			{
				//let UpdateSpawnZone() do everything
			}
			else
			{
				//bedroll
				postSpawnPlayer(player, false, true, true);
			}
		}


		private void UnturnedPlayerEvents_OnPlayerDeath(UnturnedPlayer player, SDG.Unturned.EDeathCause cause, SDG.Unturned.ELimb limb, CSteamID murderer)
		{
			if (instance.playerList.ContainsKey(murderer.m_SteamID))
			{
				if (murderer.m_SteamID != player.CSteamID.m_SteamID)
					try
					{
						UnturnedPlayer.FromCSteamID(murderer).Experience += instance.Configuration.Instance.killPlayerExperience;
					}
					catch (Exception)
					{
						throw;
					}

			}
		}

		private void FixedUpdate()
		{
			if (lastUpdatedTicks < instance.Configuration.Instance.ticksUpdateZone)
			{
				lastUpdatedTicks++;
				return;
			}
			else
			{
				lastUpdatedTicks = 0;

				UpdatePoints();

				UpdateSpawnZone();
			}
		}


		private void UpdatePoints()
		{

			//foreach (var zone in Configuration.Instance.CpArray)
			//{
			//	//TODO remove
			//	string str = "Zone " + zone.name + ": ";
			//	bool show = false;
			//	foreach (var client in Provider.clients)
			//	{
			//		if (client.player == null)
			//			continue;
			//		UnturnedPlayer uPlayer = UnturnedPlayer.FromPlayer(client.player);
			//		if (uPlayer == null)
			//			continue;

			//		if (!zone.IsInside(uPlayer.Position))
			//			continue;

			//		str = str + uPlayer.CharacterName.ToString() + ", ";
			//		show = true;
			//	}

			//	if (show)
			//		UnturnedChat.Say(DateTime.Now.ToString("s") + " " + str);
			//}

			//checking for win condition
			bool teamAwon = true, teamBwon = true;
			for (int i = 0; i < Configuration.Instance.CpArray.Length; i++)
			{
				teamAwon = teamAwon && (Configuration.Instance.CpArray[i].state == Zone.State.TEAMA);
				teamBwon = teamBwon && (Configuration.Instance.CpArray[i].state == Zone.State.TEAMB);
			}

			if (teamAwon)
			{
				UnturnedChat.Say("Team A won!", instance.Configuration.Instance.messageColor);
				return;
			}

			if (teamBwon)
			{
				UnturnedChat.Say("Team B won!", instance.Configuration.Instance.messageColor);
				return;
			}

			for (int i = 0; i < Configuration.Instance.CpArray.Length; i++)
			{
				//Configuration.Instance.CpArray[i].CountPlayersTeamsNow(out int a, out int b);
				Configuration.Instance.CpArray[i].CountPlayersTeamsNow(out HashSet<UnturnedPlayer> teamA, out HashSet<UnturnedPlayer> teamB);
				int a = teamA.Count;
				int b = teamB.Count;

				switch (Configuration.Instance.CpArray[i].state)
				{
					case Zone.State.NONE:
						{
							if (IsPointCapturableByA(i) && a > 0 && b == 0)
							{
								UnturnedChat.Say("Team A started capturing point " + Configuration.Instance.CpArray[i].name, instance.Configuration.Instance.messageColor);
								Configuration.Instance.CpArray[i].state = Zone.State.TEAMA_C;
								Configuration.Instance.CpArray[i].captureStarted = DateTime.Now;
								Configuration.Instance.CpArray[i].prevState = Zone.State.NONE;
								break;
							}
							if (IsPointCapturableByB(i) && a == 0 && b > 0)
							{
								UnturnedChat.Say("Team B started capturing point " + Configuration.Instance.CpArray[i].name, instance.Configuration.Instance.messageColor);
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
								UnturnedChat.Say("Team A aborted capturing point " + Configuration.Instance.CpArray[i].name, instance.Configuration.Instance.messageColor);
								Configuration.Instance.CpArray[i].state = Configuration.Instance.CpArray[i].prevState;
								break;
							}

							if ((DateTime.Now - Configuration.Instance.CpArray[i].captureStarted).TotalSeconds > Configuration.Instance.secondsCapture)
							{
								UnturnedChat.Say("Team A captured point " + Configuration.Instance.CpArray[i].name, instance.Configuration.Instance.messageColor);
								Configuration.Instance.CpArray[i].state = Zone.State.TEAMA;
								GiveExperienceToPlayers(teamA, instance.Configuration.Instance.controlPointExperienceCapture);
								break;
							}
							else
							{
								GiveExperienceToPlayers(teamA, Convert.ToUInt32(instance.Configuration.Instance.ticksUpdateZone * instance.Configuration.Instance.controlPointExperienceMultiplier));
							}

							break;
						}

					case Zone.State.TEAMB_C:
						{
							if (!IsPointCapturableByB(i) || a > 0 || b == 0)
							{
								UnturnedChat.Say("Team B aborted capturing point " + Configuration.Instance.CpArray[i].name, instance.Configuration.Instance.messageColor);
								Configuration.Instance.CpArray[i].state = Configuration.Instance.CpArray[i].prevState;
								break;
							}

							if ((DateTime.Now - Configuration.Instance.CpArray[i].captureStarted).TotalSeconds > Configuration.Instance.secondsCapture)
							{
								UnturnedChat.Say("Team B captured point " + Configuration.Instance.CpArray[i].name, instance.Configuration.Instance.messageColor);
								Configuration.Instance.CpArray[i].state = Zone.State.TEAMB;
								GiveExperienceToPlayers(teamB, instance.Configuration.Instance.controlPointExperienceCapture);
								break;
							}
							else
							{
								GiveExperienceToPlayers(teamB, Convert.ToUInt32(instance.Configuration.Instance.ticksUpdateZone * instance.Configuration.Instance.controlPointExperienceMultiplier));
							}
							break;
						}

					case Zone.State.TEAMA:
						{
							if (IsPointCapturableByB(i) && a == 0 && b > 0)
							{
								UnturnedChat.Say("Team B started capturing point " + Configuration.Instance.CpArray[i].name, instance.Configuration.Instance.messageColor);
								Configuration.Instance.CpArray[i].state = Zone.State.TEAMB_C;
								Configuration.Instance.CpArray[i].captureStarted = DateTime.Now;
								Configuration.Instance.CpArray[i].prevState = Zone.State.TEAMA;
							}
							break;
						}

					case Zone.State.TEAMB:
						{
							if (IsPointCapturableByA(i) && a > 0 && b == 0)
							{
								UnturnedChat.Say("Team A started capturing point " + Configuration.Instance.CpArray[i].name, instance.Configuration.Instance.messageColor);
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


		public void GiveExperienceToPlayers(HashSet<UnturnedPlayer> players, uint expAmountToEach)
		{
			foreach (UnturnedPlayer p in players)
			{
				p.Experience += expAmountToEach;
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
			foreach (var client in Provider.clients)
			{
				if (client.player == null)
					continue;
				UnturnedPlayer uPlayer = UnturnedPlayer.FromPlayer(client.player);
				if (uPlayer == null)
					continue;

				if (!instance.Configuration.Instance.spawnZone.IsInside(uPlayer.Position))
					continue;

				postSpawnPlayer(uPlayer, true, true, true);
			}
		}


		private void postSpawnPlayer(UnturnedPlayer player, bool teleport, bool fillInventory, bool giveSkills)
		{
			if (!playerList.ContainsKey(player.CSteamID.m_SteamID))
			{
				playerList.Add(player.CSteamID.m_SteamID, new PlayerListItem());
			}

			FillPlayerInventoryAndSkill(player);

			StartCoroutine(TeleportPlayerToSpawn(player));
		}

		
		//player should be in a playerList
		public IEnumerator TeleportPlayerToSpawn(UnturnedPlayer player)
		{
			ulong teamId = player.SteamGroupID.m_SteamID;

			if (teamId == instance.Configuration.Instance.teamASteamId)
			{
				int farthestPoint = -1;
				for (int i = 0; i < Configuration.Instance.CpArray.Length - 1; i++)
				{
					if (Configuration.Instance.CpArray[i].state == Zone.State.TEAMA)
						farthestPoint = i;
				}
				if (farthestPoint != -1 && !instance.playerList[player.CSteamID.m_SteamID].spawnAtBase)
				{
					player.Teleport(instance.Configuration.Instance.CpArray[farthestPoint].m_spawn, 0);
				}
				else
				{
					player.Teleport(instance.Configuration.Instance.TeamASpawn, 0);
				}
			}
			else if (teamId == instance.Configuration.Instance.teamBSteamId)
			{
				int farthestPoint = -1;
				for (int i = Configuration.Instance.CpArray.Length - 1; i >= 0; i--)
				{
					if (Configuration.Instance.CpArray[i].state == Zone.State.TEAMB)
						farthestPoint = i;
				}
				if (farthestPoint != -1 && !instance.playerList[player.CSteamID.m_SteamID].spawnAtBase)
				{
					player.Teleport(instance.Configuration.Instance.CpArray[farthestPoint].m_spawn, 0);
				}
				else
				{
					player.Teleport(instance.Configuration.Instance.TeamBSpawn, 0);
				}
			}
			else
			{
				yield break;
			}
		}


		//player should be in a playerList
		public void FillPlayerInventoryAndSkill(UnturnedPlayer player)
		{
			//player.GiveItem(UnturnedItems.AssembleItem(1018,
			//										   30, // clipsize
			//										   new Attachment(1201, 100), // sight
			//										   new Attachment(1008, 100), // tactical
			//										   new Attachment(8, 100), // grip
			//										   new Attachment(7, 100), // barrel
			//										   new Attachment(1020, 100), // magazine
			//										   EFiremode.AUTO, // firemode
			//										   1, 100 // amount, durability
			//  ));

			int classId = playerList[player.CSteamID.m_SteamID].classId;

			if (classId != -1)
			{
				foreach (var item in instance.Configuration.Instance.playerClasses[classId].items)
				{
					player.GiveItem(item, 1);
				}
			}

			foreach (var item in instance.Configuration.Instance.playerClasses[classId].skills)
			{
				UnturnedSkill skill = PlayerClass.GetUnturnedSkillByName(item);

				if (skill != null)
				{
					player.SetSkillLevel(skill, player.GetSkill(skill).max);
				}
			}

			return;
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
