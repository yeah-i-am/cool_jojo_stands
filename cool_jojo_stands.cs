using Terraria.ModLoader;

namespace cool_jojo_stands
{
	class cool_jojo_stands : Mod
	{
        public static ModHotKey StandSummonHT;

		public cool_jojo_stands()
		{
		}

        public override void Load()
        {
            StandSummonHT = RegisterHotKey("Призвать станда", "Insert");

            AddEquipTexture(null, EquipType.Legs, "JotaroCoat_Legs", "cool_jojo_stands/Items/Armor/JotaroCoat_Legs");
            AddEquipTexture(null, EquipType.Legs, "JotaroCoat_LegsAndBoots", "cool_jojo_stands/Items/Armor/JotaroCoat_Legs2");
        }
    }
}
