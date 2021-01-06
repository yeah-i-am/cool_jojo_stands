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

            Main.RegisterItemAnimation(item.type, new Terraria.DataStructures.DrawAnimationVertical(12, 4));
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 18;
            item.maxStack = 999;
            item.material = true;
            item.rare = ItemRarityID.Expert;
            item.value = Item.buyPrice(0, 0, 1, 0);
        }
    }
}
