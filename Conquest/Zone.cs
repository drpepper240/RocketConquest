﻿using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

namespace Conquest
{
	[XmlInclude(typeof(ZoneBox))]
	[XmlInclude(typeof(ZoneCylinder))]
	public abstract class Zone
	{
		//capture state
		public enum State {NONE, TEAMA_C, TEAMA, TEAMB_C, TEAMB};
		public State state;

		//for failed capture attempts
		[XmlIgnore]
		public State prevState;

		public abstract bool IsInside(Vector3 pos);

		//Capture timer
		[XmlIgnore]
		public DateTime captureStarted;

		public string name;

		//spawn point
		public Vector3 m_spawn;

		//default constructor
		public Zone()
		{
			state = State.NONE;
			prevState = State.NONE;
			captureStarted = DateTime.MaxValue;
			name = "Unnamed Point";
		}


		public void CountPlayersTeamsNow(out int playersTeamA, out int playersTeamB)
		{
			playersTeamA = playersTeamB = 0;
			if (Conquest.instance == null || Conquest.instance.Configuration.Instance == null)
				return;

			foreach (var client in Provider.clients)
			{
				if (client.player == null)
					continue;
				UnturnedPlayer uPlayer = UnturnedPlayer.FromPlayer(client.player);
				if (uPlayer == null)
					continue;

				if (!IsInside(uPlayer.Position))
					continue;

				ulong teamId = uPlayer.SteamGroupID.m_SteamID;
				if (teamId == Conquest.instance.Configuration.Instance.teamASteamId)
					playersTeamA += 1;
				if (teamId == Conquest.instance.Configuration.Instance.teamBSteamId)
					playersTeamB += 1;
			}
		}


		public void CountPlayersTeamsNow(out HashSet<UnturnedPlayer> playersTeamA, out HashSet<UnturnedPlayer> playersTeamB)
		{
			playersTeamA = new HashSet<UnturnedPlayer>();
			playersTeamB = new HashSet<UnturnedPlayer>();
			if (Conquest.instance == null || Conquest.instance.Configuration.Instance == null)
				return;

			foreach (var client in Provider.clients)
			{
				if (client.player == null)
					continue;
				UnturnedPlayer uPlayer = UnturnedPlayer.FromPlayer(client.player);
				if (uPlayer == null)
					continue;

				if (!IsInside(uPlayer.Position))
					continue;

				ulong teamId = uPlayer.SteamGroupID.m_SteamID;
				if (teamId == Conquest.instance.Configuration.Instance.teamASteamId)
					playersTeamA.Add(uPlayer);
				if (teamId == Conquest.instance.Configuration.Instance.teamBSteamId)
					playersTeamB.Add(uPlayer);
			}
		}
	}

	public class ZoneBox : Zone
	{
		public Vector3 m_min;
		public Vector3 m_max;

		public ZoneBox() { }

		public ZoneBox(Vector3 min, Vector3 max, Vector3 spawn)
		{
			m_min = min;
			m_max = max;
			m_spawn = spawn;
		}

		public override bool IsInside(Vector3 pos)
		{
			return (pos.x > m_min.x && pos.x < m_max.x && pos.y > m_min.y && pos.y < m_max.y && pos.z > m_min.z && pos.z < m_max.z); //TODO make y axis configurable
		}
	}

	public class ZoneCylinder : Zone
	{
		public Vector3 m_center;
		public float m_r;
		public float m_h;

		public ZoneCylinder() { }

		public ZoneCylinder(Vector3 center, float r, float h, Vector3 spawn)
		{
			m_center = center;
			m_r = r;
			m_h = h;
			m_spawn = spawn;
		}

		public override bool IsInside(Vector3 pos)
		{
			return ( pos.y - m_center.y < m_h) && (new Vector2(pos.x - m_center.x, pos.z - m_center.z).sqrMagnitude < m_r*m_r); //TODO make y axis configurable
		}
	}
}
