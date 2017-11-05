using Rocket.Unturned.Player;
using Rocket.Unturned.Skills;

namespace Conquest
{
	public class PlayerClass
	{
		public string name;
		public ushort[] items;

		public string[] skills;

		public PlayerClass()
		{
			name = "Unnamed class";
			items = new ushort[0] { };
			skills = new string[0] { };
		}

		public PlayerClass(string name, ushort[] items)
		{
			this.name = name;
			this.items = items;
		}

		public static UnturnedSkill GetUnturnedSkillByName(string name)
		{
			switch (name)
			{
				case "Overkill": return UnturnedSkill.Overkill;
				case "Sharpshooter": return UnturnedSkill.Sharpshooter;
				case "Dexerity": return UnturnedSkill.Dexerity;
				case "Dexterity": return UnturnedSkill.Dexerity;
				case "Cardio": return UnturnedSkill.Cardio;
				case "Exercise": return UnturnedSkill.Exercise;
				case "Diving": return UnturnedSkill.Diving;
				case "Parkour": return UnturnedSkill.Parkour;
				case "Sneakybeaky": return UnturnedSkill.Sneakybeaky;
				case "Vitality": return UnturnedSkill.Vitality;
				case "Immunity": return UnturnedSkill.Immunity;
				case "Toughness": return UnturnedSkill.Toughness;
				case "Strength": return UnturnedSkill.Strength;
				case "Warmblooded": return UnturnedSkill.Warmblooded;
				case "Survival": return UnturnedSkill.Survival;
				case "Healing": return UnturnedSkill.Healing;
				case "Crafting": return UnturnedSkill.Crafting;
				case "Outdoors": return UnturnedSkill.Outdoors;
				case "Cooking": return UnturnedSkill.Cooking;
				case "Fishing": return UnturnedSkill.Fishing;
				case "Agriculture": return UnturnedSkill.Agriculture;
				case "Mechanic": return UnturnedSkill.Mechanic;
				case "Engineer": return UnturnedSkill.Engineer;
				default: return null;
			}
		}
	}
}
