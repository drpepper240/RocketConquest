using System;
using System.Collections.Generic;
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

		public UInt32 ticksUpdatePlayerPosition;

		public UInt32 ticksUpdateZone;

		public Vector3 zoneMin;
		public Vector3 zoneMax;

		public void LoadDefaults()
		{
			logFileName = @"Logs\Conquest.log";
			ticksUpdatePlayerPosition = 10;
			ticksUpdateZone = 101;
			zoneMin = new Vector3(0.5f, 0.5f, 0.5f);
			zoneMax= new Vector3(0.51f, 0.51f, 0.51f);
		}
	}
}