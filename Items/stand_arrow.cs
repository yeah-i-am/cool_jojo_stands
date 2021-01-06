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
          "StarPlatinumStand", "MagicianRedStand", "HierophantGreenStand"///, "SilverChariotStand"///, "HermitPurpleStand"
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
            item.useStyle = ItemUseStyleID.EatingUsing;
            item.knockBack = 0;
            item.value = Item.buyPrice(0, 0, 40, 0);
            item.rare = ItemRarityID.Expert;
            item.UseSound = SoundID.Item1;
            item.autoReuse = false;
        }

        public override bool UseItem(Player player)
        {
            if (player.GetModPlayer<StandoPlayer>().HaveStand)
                return true;

            switch (player.name)
            {
                case "Avdol":
                case "avdol":
                    player.AddBuff(ModContent.BuffType<Buffs.MagicianRedStand>(), 239);
                    return true;

                case "Jotaro":
                case "jotaro":
                    player.AddBuff(ModContent.BuffType<Buffs.StarPlatinumStand>(), 239);
                    return true;

                case "Joseph":
                case "joseph":
                    player.AddBuff(ModContent.BuffType<Buffs.HermitPurpleStand>(), 239);
                    return true;

                case "Kakyoin":
                case "kakyoin":
                    player.AddBuff(ModContent.BuffType<Buffs.HierophantGreenStand>(), 239);
                    return true;

                ///case "Froloh":
                ///case "froloh":
                ///    player.AddBuff(ModContent.BuffType<Buffs.StarPlatinumRequiemStand>(), 239);
                ///    return true;

                case "Pornoleff":
                case "Polnoreff":
                case "Polnoref":
                case "polnoreff":
                case "polnoref":
                    player.AddBuff(ModContent.BuffType<Buffs.SilverChariotStand>(), 239);
                    return true;
            }

            Terraria.Utilities.UnifiedRandom rand = Main.rand;

            if (rand.Next(1000) == 239)
                player.AddBuff(ModContent.BuffType<Buffs.StarPlatinumRequiemStand>(), 239);
            else
            {
                player.Hurt(Terraria.DataStructures.PlayerDeathReason.ByCustomReason(player.name + " died because was stupid"), 99, 1);
                player.AddBuff(mod.BuffType(stands[rand.Next(stands.Length)]), 239);
            }
            /// add cut scene

            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<StrangeBar>(), 7);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
