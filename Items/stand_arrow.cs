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
            Item.damage = 99;
            Item.noMelee = true;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 20;
            Item.useAnimation = 21;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.knockBack = 0;
            Item.value = Item.buyPrice(0, 0, 40, 0);
            Item.rare = ItemRarityID.Expert;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = false;
        }

        public override bool? UseItem(Player player)
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
                player.AddBuff(Mod.Find<ModBuff>(stands[rand.Next(stands.Length)]).Type, 239);
            }
            /// add cut scene

            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<StrangeBar>(), 7);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
