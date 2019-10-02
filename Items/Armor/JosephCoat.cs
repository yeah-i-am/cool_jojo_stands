using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;

namespace cool_jojo_stands.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    class JosephCoat : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Joseph Coat");
            Tooltip.SetDefault("...");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 14;
            item.value = Item.buyPrice(0, 3, 47, 0);
            item.rare = 6;
            item.defense = 7;
        }

        public override void DrawHands(ref bool drawHands, ref bool drawArms)
        {
            drawHands = true;
            drawArms = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Silk, 24);
            recipe.AddIngredient(ItemID.Hay, 19);
            recipe.AddIngredient(ItemID.HellstoneBar, 15);
            recipe.AddIngredient(ItemID.FallenStar, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
