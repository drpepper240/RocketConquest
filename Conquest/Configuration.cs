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

		public float controlPointExperienceMultiplier;

		public uint controlPointExperienceCapture;

		public UInt32 killPlayerExperience; 
				
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

			//autokicker for players not belonging to these teams
			kickPlayerWithInvalidTeam = true;
			kickDelaySeconds = 10;

			//all zones are updated inside FixedUpdate() every fixed amount of ticks
			ticksUpdateZone = 50;

			//seconds to capture control point
			secondsCapture = 180;

			//capturing control point gives this much experience each tick
			controlPointExperienceMultiplier = 0.02f;

			//experience bonus for capturing a control point
			controlPointExperienceCapture = 50;

			//experience bonus for killing another player
			killPlayerExperience = 75;

			//Capture points array, in order from team A to team B
			CpArray = new Zone[] { new ZoneBox(new Vector3(12.0f, 38.0f, 659.5f), new Vector3(16.0f, 46.0f, 677.0f), new Vector3(13.0f, 39.0f, 665.5f)),
									new ZoneBox(new Vector3(-141.0f, 52.0f, 399.0f), new Vector3(-119.0f, 63.0f, 421.0f), new Vector3(-130.0f, 56.0f, 409.0f)),
									new ZoneBox(new Vector3(-267.5f, 33.0f, -5.5f), new Vector3(-231.0f, 48.0f, 33.0f), new Vector3(-242.5f, 40.0f, 2.5f)),
									new ZoneBox(new Vector3(117.0f, 50.0f, -31.5f), new Vector3(139.0f, 61.0f, -9.5f), new Vector3(129.5f, 53.5f, -21.5f)),
									new ZoneBox(new Vector3(347.0f, -34.0f, -115.0f), new Vector3(359.0f, 42.0f, -98.5f), new Vector3(353.5f, 34.5f, -114.5f))};
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
			TeamBSpawn = new Vector3(551.5f, 38.5f, -187.4f);

			messageColor = new UnityEngine.Color(0.18f, 0.8f, 0.44f, 0);

			playerClasses = new PlayerClass[] {new PlayerClass("Engineer",
												new ushort[] {307, 209, 81, 394, 394, 394, 388, 253, 310, 76, 1481, 1483, 1483, 1483, 240, 1440, 490, 141, 67, 67, 67, 67, 67, 67, 67, 67, 67, 67}),
												
												new PlayerClass("Medic",
												new ushort[] {307, 308, 81, 394, 394, 394, 388,		1481, 1483, 1483, 1483,		431, 251, 15, 15, 15, 15, 15, 15, 389, 389, 394, 394, 394, 394, 388, 388, 310}),
												new PlayerClass("Grunt",
												new ushort[] {307, 308, 81, 394, 394, 394, 388,		309, 310,						363, 6, 6, 6, 6, 6, 254, 254, 263}),
												new PlayerClass("SpecOps",
												new ushort[] {1171, 1172, 81, 394, 394, 394, 388,	1389, 1169,	334,				116, 6, 6, 6, 1200,, 1346, 1346, 261, 261, 1100, 1100, 254, 254, 147, 151 }),
												new PlayerClass("Marksman",
												new ushort[] {307, 308, 81, 394, 394, 394, 388,     433, 238,                       1018, 1020, 1020, 1020, 148, 1021, 1022, 1022, 1022, 310 })
												};
			defaultPlayerClassIndex = 2;
			playerClasses[0].skills = new string[] { "Crafting", "Mechanic", "Engineer" };
			playerClasses[1].skills = new string[] { "Immunity", "Healing", "Exercise" };
			playerClasses[2].skills = new string[] { "Exercise", "Strength" };
			playerClasses[3].skills = new string[] { "Dexterity", "Vitality", "Survival" };
			playerClasses[4].skills = new string[] { "Sharpshooter", "Dexterity", "Survival" };
		}
	}
}