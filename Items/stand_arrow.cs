using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace cool_jojo_stands.Items
{
	public class stand_arrow : ModItem
	{
        string[] stands =
        {
          "StarPlatinumStand", "MagicianRedStand", "HierophantGreenStand"///, "HermitPurpleStand"
        };

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stand arrow");
			Tooltip.SetDefault("For outdoor use");
		}
		public override void SetDefaults()
		{
			item.damage = 99;
			item.noMelee = true;
			item.width = 40;
			item.height = 40;
			item.useTime = 20;
			item.useAnimation = 21;
			item.useStyle = 2;
			item.knockBack = 0;
			item.value = Item.buyPrice(0, 0, 70, 0);
			item.rare = 11;
            item.expert = true;
			item.UseSound = SoundID.Item1;
			item.autoReuse = false;
	}

		/*public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LunarBar, 47);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}*/

        public override bool UseItem( Player player )
        {
            if (player.GetModPlayer<StandoPlayer>(mod).HaveStand)
              return base.UseItem(player);

            switch(player.name)
            {
                case "Avdol":
                case "avdol":
                    player.AddBuff(mod.BuffType<Buffs.MagicianRedStand>(), 239);
                    return base.UseItem(player);

                case "Jotaro":
                case "jotaro":
                    player.AddBuff(mod.BuffType<Buffs.StarPlatinumStand>(), 239);
                    return base.UseItem(player);

                case "Joseph":
                case "joseph":
                    player.AddBuff(mod.BuffType<Buffs.HermitPurpleStand>(), 239);
                    return base.UseItem(player);

                case "Kakyoin":
                case "kakyoin":
                    player.AddBuff(mod.BuffType<Buffs.HierophantGreenStand>(), 239);
                    return base.UseItem(player);
            }

            Random rand = new Random(DateTime.Now.Second);

            ///StandoPlayer.Talk(DateTime.Now.Second.ToString());

            player.Hurt(Terraria.DataStructures.PlayerDeathReason.ByCustomReason(player.name + " died because was stupid"), 99, 1);

            player.AddBuff(mod.BuffType(stands[rand.Next(stands.Length)]), 101);
            return base.UseItem(player);
        }
    }
}
