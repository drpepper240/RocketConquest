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

		public UnityEngine.Color messageColor;

		public Vector3 TeamASpawn;
		public Vector3 TeamBSpawn;

		public void LoadDefaults()
		{
			logFileName = @"Logs\Conquest.log";

			teamASteamUri = @"http://steamcommunity.com/groups/YARR-240";
			teamASteamId = 103582791457241638;
			teamBSteamUri = @"http://steamcommunity.com/groups/YARR-240-B";
			teamBSteamId = 103582791457591564;

			kickPlayerWithInvalidTeam = true;
			kickDelaySeconds = 10;

			ticksUpdateZone = 50;

			secondsCapture = 30;

			CpArray = new Zone[5] { new ZoneBox(new Vector3(-438.0f, 30.0f, 479.0f), new Vector3(-423.0f, 40.0f, 494.0f)),
									new ZoneBox(new Vector3(-392.0f, 30.0f, 479.0f), new Vector3(-377.0f, 40.0f, 494.0f)),
									new ZoneBox(new Vector3(-348.0f, 30.0f, 479.0f), new Vector3(-333.0f, 40.0f, 494.0f)),
									new ZoneBox(new Vector3(-302.0f, 30.0f, 479.0f), new Vector3(-287.0f, 40.0f, 494.0f)),
									new ZoneBox(new Vector3(-262.0f, 30.0f, 479.0f), new Vector3(-247.0f, 40.0f, 494.0f))	};
			CpArray[0].name = "Alpha";
			CpArray[1].name = "Bravo";
			CpArray[2].name = "Charlie";
			CpArray[3].name = "Delta";
			CpArray[4].name = "Echo";

			spawnZone = new ZoneCylinder(new Vector3(197.0f, 30.0f, -804.0f), 20.0f, 20.0f);
			spawnZone.name = "Spawn";

			TeamASpawn = new Vector3(200.0f, 58.0f, 800.4f);
			TeamBSpawn = new Vector3(624.0f, 36.0f, -195.0f);	//TODO update

			messageColor = new UnityEngine.Color(0.18f, 0.8f, 0.44f, 0);
		}
	}
}