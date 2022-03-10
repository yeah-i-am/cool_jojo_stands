using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace cool_jojo_stands.Items
{
    public class stand_arrow : ModItem
    {
        public string[] stands =
        {
            "HermitPurple", "HierophantGreen", "MagicianRed", "StarPlatinum" , "TheWorld"
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

            player.Hurt(Terraria.DataStructures.PlayerDeathReason.ByCustomReason(player.name + " died by stand arrow"), 99, 1);
            player.AddBuff(mod.BuffType(stands[new Random().Next(stands.Length)]), 239);

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
