﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rocket.Unturned.Player;
using UnityEngine;
using Rocket.Unturned.Chat;

namespace Conquest
{
	public class PlayerComponentConquest : UnturnedPlayerComponent
	{
		UInt32 lastUpdatedTicks; //TODO initial value


		private bool isInside(Vector3 pos, Vector3 min, Vector3 max)
		{
			return (pos.x > min.x && pos.x < max.x && pos.y > min.y && pos.y < max.y && pos.z > min.z && pos.z < max.z); //TODO make y axis configurable
		}
		private void FixedUpdate()
		{
			if (lastUpdatedTicks < Conquest.instance.Configuration.Instance.ticksUpdatePlayerPosition)
			{
				lastUpdatedTicks++;
				return;
			}
			else
			{
				lastUpdatedTicks = 0;

				foreach (var item in Conquest.instance.Configuration.Instance.CpArray)
				{
					if (item.IsInside(this.Player.Position))
					{
						//UnturnedChat.Say(DateTime.Now.ToString("s") + " " + "YARR");
						item.playerList.Add(this.Player.CSteamID.m_SteamID);
					}
					else
					{
						item.playerList.Remove(this.Player.CSteamID.m_SteamID);
					}
				}
			}
		}
	}
}
