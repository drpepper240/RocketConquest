# RocketConquest
A plugin for Rocket that provides Conquest gamemode (also known as Control Points, Capture Points or Tug of War):
- Points are captured sequentally
- A point can have a box-like or cylinder-like zone for capturing mechanic
- Experience bonuses for killing other players (configurable)
- Experience bonuses for capturing control points (configurable)
- Configurable player classes (engineer, medic, sniper etc.)
- Configurable spawn points (separate for each team)
- Two teams based on Steam groups, which SteamIDs and URLs are to be provided in the config file
- Optional autokick for players not belonging to these groups
### Commands:
**/gc** - print current coordinates, useful for debug or setting things up on a new map

**/class** - change player class, may affect item and skill loadout on respawn (configurable)

**/spawn** - change player respawn place between the base of their team and the farthest control point captured by their team

**/status** - print captured control points for both teams
### Notes:
- Does not use Translations file, all strings are hardcoded.
- Default settings are designed to work with this map: http://steamcommunity.com/sharedfiles/filedetails/?id=1180523226 
To make another map working with this plugin you have to manually edit Conquest.configuration.xml file with desired coordinates (which you can obtain using /gc command)
