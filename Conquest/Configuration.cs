using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Rocket.API;
using UnityEngine;

namespace Conquest
{
	public class Configuration : IRocketPluginConfiguration
	{
		//Date-time logfile for this plugin
		public string logFileName;

		//Teams
		public string teamASteamUri;
		public UInt64 teamASteamId;

		public string teamBSteamUri;
		public UInt64 teamBSteamId;

		//autokick for plauers not belonging to either Team A or Team B
		public bool kickPlayerWithInvalidTeam;
		public UInt32 kickDelaySeconds;

		public UInt32 ticksUpdatePlayerPosition;

		public UInt32 ticksUpdateZone;

		public UInt32 secondsCapture;

		//Capture points array, in order from team A to team B
		public Zone[] CpArray;

		public Zone spawnZone;

		public void LoadDefaults()
		{
			logFileName = @"Logs\TDM.log";

			teamASteamUri = @"http://steamcommunity.com/groups/YARR-240";
			teamASteamId = 103582791457241638;
			teamBSteamUri = @"http://steamcommunity.com/groups/YARR-240-B";
			teamBSteamId = 103582791457591564;

			kickPlayerWithInvalidTeam = true;
			kickDelaySeconds = 10;

			ticksUpdatePlayerPosition = 10;
			ticksUpdateZone = 47;

			secondsCapture = 30;

			CpArray = new Zone[2] { new ZoneBox(new Vector3(-468.0f, 30.0f, 558.5f), new Vector3(-453.0f, 70.0f, 606.5f)), 
									new ZoneCylinder(new Vector3(-300.0f, 25.0f, 600.0f), 10.0f, 100.0f)  };
			CpArray[0].name = "Box Zone";
			CpArray[1].name = "Cylinder Zone";

			spawnZone = new ZoneCylinder(new Vector3(-468.0f, 30.0f, 558.5f), 50.0f, 20.0f);
			spawnZone.name = "Spawn";
		}
	}
}