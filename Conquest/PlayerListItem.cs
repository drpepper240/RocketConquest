﻿namespace Conquest
{
	public class PlayerListItem
	{
		//public UInt64 steamId;
		public int classId;
		public bool spawnAtBase;

		public PlayerListItem()
		{
			spawnAtBase = true;
			classId = Conquest.instance.Configuration.Instance.defaultPlayerClassIndex;
		}
	}
}
