using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
      /*
namespace cool_jojo_stands.Items
{
    [AutoloadEquip(EquipType.Wings)]
    class MegaCarpet : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Удачи на цг практике");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 20;
            item.value = 47473;
            item.rare = 9;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.wingTimeMax = 7000;
        }

        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
            ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            StandoPlayer pl = player.GetModPlayer<StandoPlayer>(mod);

            if (++pl.UsedMegaCarpet > 15)
            {
                if (player.ConsumeItem(ItemID.Wood))
                    pl.UsedMegaCarpet = 0;
                else
                {
                    ascentWhenFalling = 10f;
                    ascentWhenRising = 10f;
                    maxCanAscendMultiplier = -1f;
                    maxAscentMultiplier = -1f;
                    constantAscend = -1f;
                    return;
                }
            }

            if (player.wingTime < 7000 - 8)
            {
                ascentWhenFalling = 1f;
                ascentWhenRising = 1f;
                maxCanAscendMultiplier = 0.0005f;
                maxAscentMultiplier = 0.0005f;
                constantAscend = 0.0005f;
                return;
            }

            ascentWhenFalling = 5f;
            ascentWhenRising = -5f;
            maxCanAscendMultiplier = 0.3f;
            maxAscentMultiplier = 1f;
            constantAscend = 0.5f;
        }

        public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
        {
            StandoPlayer pl = player.GetModPlayer<StandoPlayer>(mod);

            if (pl.UsedMegaCarpet > 15)
                return;

            speed = 10f;
            acceleration *= 7.5f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.StoneBlock, 47);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
        */