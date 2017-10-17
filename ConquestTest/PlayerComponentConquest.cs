using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rocket.Unturned.Player;
using UnityEngine;

namespace Conquest
{
	public class PlayerComponentConquest : UnturnedPlayerComponent
	{
		UInt32 lastUpdatedTicks; //TODO initial value


		private bool isInside(Vector3 pos, Vector3 min, Vector3 max)
		{
			return (pos.x > min.x && pos.x < max.x && pos.y > min.y && pos.y < max.y); //TODO z axis setting
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
				//TODO update player position
				if (isInside(this.Player.Position, Conquest.instance.Configuration.Instance.zoneMin, Conquest.instance.Configuration.Instance.zoneMax))
				{
					Conquest.instance.zoneList[this.Player.CSteamID] = this.Player.SteamGroupID.m_SteamID;
				}
				else
				{
					Conquest.instance.zoneList.Remove(this.Player.CSteamID);
				}
			}
		}
	}
}

