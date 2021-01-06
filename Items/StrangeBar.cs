using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace cool_jojo_stands.Items
{
    class StrangeBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Strange bar");
            Tooltip.SetDefault("Smells like space");

            Main.RegisterItemAnimation(item.type, new Terraria.DataStructures.DrawAnimationVertical(7, 8));
        }

        public override void SetDefaults()
        {
            item.width = 34;
            item.height = 30;
            item.maxStack = 999;
            item.material = true;
            item.rare = ItemRarityID.Expert;
            item.value = Item.buyPrice(0, 0, 4, 0);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<StrangeOre>(), 4);
            recipe.AddTile(TileID.Furnaces);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
