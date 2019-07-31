using Terraria;
using Terraria.ModLoader;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Microsoft.Xna.Framework.Graphics;

namespace cool_jojo_stands
{
	class cool_jojo_stands : Mod
	{
        public static ModHotKey StandSummonHT, SpecialAbilityHT;

        public cool_jojo_stands()
		{
		}

        public override void Load()
        {
            StandSummonHT = RegisterHotKey("Summon Stand", "Insert");
            SpecialAbilityHT = RegisterHotKey("Special ability", "P");

            GameShaders.Misc["cool_jojo_stands:ZaWardo"] = new MiscShaderData(new Ref<Effect>(GetEffect("Effects/ZaWardo0")), "ZaWardo");
        }

        public override void Unload()
        {
            if (!Main.dedServ)
              GameShaders.Misc.Remove("cool_jojo_stands:ZaWardo");

            StandSummonHT = null;
            SpecialAbilityHT = null;
        }
    }
}
