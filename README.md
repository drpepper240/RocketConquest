# RocketConquest
A plugin for Rocket that provides Conquest gamemode (also known as Control Points, Capture Points or Tug of War):
- Points are captured sequentally
- A point can have a box-like or cylinder-like zone for capturing mechanic
- Experience bonuses for killing other players
- Configurable player classes (engineer, medic, sniper etc.)
- Configurable spawn points (separate for each team)
- Two teams based on Steam groups, which SteamIDs and URLs are to be provided in the config file
- Optional autokick for players not belonging to these groups
### Commands:
**/gc** - print current coordinates, useful for debug or setting things up on a new map
**/class** - change player class, may affect item and skill loadout on respawn (configurable)
**/spawn** - change player respawn place between the base of their team and the farthest control point captured by their team
### Notes:
- Does not use Translations file, all strings are hardcoded.
