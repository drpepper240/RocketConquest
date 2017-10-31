namespace Conquest
{
	public class PlayerClass
	{
		public string name;
		public ushort[] items;

		public PlayerClass()
		{
			name = "Unnamed class";
			items = new ushort[0] { };
		}

		public PlayerClass(string name, ushort[] items)
		{
			this.name = name;
			this.items = items;
		}
	}
}
