using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace cool_jojo_stands.Items
{
    class StrangeOre : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Strange ore");
            Tooltip.SetDefault("Smells like space");

            Main.RegisterItemAnimation(Item.type, new Terraria.DataStructures.DrawAnimationVertical(12, 4));
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 18;
            Item.maxStack = 999;
            Item.material = true;
            Item.rare = ItemRarityID.Expert;
            Item.value = Item.buyPrice(0, 0, 1, 0);
        }
    }
}
