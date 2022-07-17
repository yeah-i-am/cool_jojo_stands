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

            Main.RegisterItemAnimation(Item.type, new Terraria.DataStructures.DrawAnimationVertical(7, 8));
        }

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 30;
            Item.maxStack = 999;
            Item.material = true;
            Item.rare = ItemRarityID.Expert;
            Item.value = Item.buyPrice(0, 0, 4, 0);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<StrangeOre>(), 4);
            recipe.AddTile(TileID.Furnaces);
            recipe.Register();
        }
    }
}
