using System;
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

		public Vector3 TeamASpawn;
		public Vector3 TeamBSpawn;

		//autokick for plauers not belonging to either Team A or Team B
		public bool kickPlayerWithInvalidTeam;
		public UInt32 kickDelaySeconds;

		public UInt32 ticksUpdateZone;

		public UInt32 secondsCapture;

		//players get experience for capturing control points. Base value is equal to ticks spent in the zone, this is the multiplier
		public float controlPointExperienceMultiplier;

		//experience for killing another player
		public UInt32 killPlayerExperience; 

		//Capture points array, in order from team A to team B
		public Zone[] CpArray;

		public Zone spawnZone;

		public Color messageColor;

		//classes
		public PlayerClass[] playerClasses;
		public int defaultPlayerClassIndex;

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

			controlPointExperienceMultiplier = 0.1f;

			killPlayerExperience = 75;

			CpArray = new Zone[5] { new ZoneBox(new Vector3(-438.0f, 30.0f, 479.0f), new Vector3(-423.0f, 40.0f, 494.0f), new Vector3(-431.5f, 40.0f, 486.5f)),
									new ZoneBox(new Vector3(-392.0f, 30.0f, 479.0f), new Vector3(-377.0f, 40.0f, 494.0f), new Vector3(-384.5f, 40.0f, 486.5f)),
									new ZoneBox(new Vector3(-348.0f, 30.0f, 479.0f), new Vector3(-333.0f, 40.0f, 494.0f), new Vector3(-440.5f, 40.0f, 486.5f)),
									new ZoneBox(new Vector3(-302.0f, 30.0f, 479.0f), new Vector3(-287.0f, 40.0f, 494.0f), new Vector3(-494.5f, 40.0f, 486.5f)),
									new ZoneBox(new Vector3(-262.0f, 30.0f, 479.0f), new Vector3(-247.0f, 40.0f, 494.0f), new Vector3(-464.5f, 40.0f, 486.5f))  };
			CpArray[0].name = "Alpha";
			CpArray[0].state = Zone.State.TEAMA;
			CpArray[1].name = "Bravo";
			CpArray[2].name = "Charlie";
			CpArray[3].name = "Delta";
			CpArray[4].name = "Echo";
			CpArray[4].state = Zone.State.TEAMB;

			spawnZone = new ZoneCylinder(new Vector3(197.0f, 30.0f, -804.0f), 20.0f, 20.0f, new Vector3(197.0f, 30.0f, -804.0f));
			spawnZone.name = "Spawn";

			TeamASpawn = new Vector3(200.0f, 58.0f, 800.4f);
			TeamBSpawn = new Vector3(624.0f, 36.0f, -195.0f);	//TODO update

			messageColor = new UnityEngine.Color(0.18f, 0.8f, 0.44f, 0);

			playerClasses = new PlayerClass[] {new PlayerClass("Engineer",
												new ushort[] {307, 308, 81, 394, 394, 394, 388,		1481, 1483, 1483, 1483,		240, 253, 1440, 76, 490, 141}),
												new PlayerClass("Medic",
												new ushort[] {307, 308, 81, 394, 394, 394, 388,		1481, 1483, 1483, 1483,		431, 251, 15, 15, 15, 15, 15, 15, 389, 389, 394, 394, 394, 394}),
												new PlayerClass("Grunt",
												new ushort[] {307, 308, 81, 394, 394, 394, 388,		309, 310,						363, 6, 6, 6, 6, 6, 254, 254, 263}),
												new PlayerClass("SpecOps",
												new ushort[] {1171, 1172, 81, 394, 394, 394, 388,	1389, 1169,	334,				116, 6, 6, 6, 1200, 1346, 261, 1100, 254 }),
												new PlayerClass("Marksman",
												new ushort[] {235, 236, 81, 394, 394, 394, 388,     433, 238,                       1018, 1020, 1020, 1020, 148, 1021, 1022, 1022, 1483 })
												};
			defaultPlayerClassIndex = 2;
		}
	}
}