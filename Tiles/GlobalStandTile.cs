using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using cool_jojo_stands.Items;
using Terraria.DataStructures;

namespace cool_jojo_stands.Tiles
{
    public class GlobalStandTile : GlobalTile
    {
        private const double StrangeOreDropChance = 0.001;

        public override bool Drop(int i, int j, int type)
        {
            if (type == TileID.Stone && Main.rand.NextDouble() < StrangeOreDropChance)
            {
                Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ModContent.ItemType<StrangeOre>());
            }

            return base.Drop(i, j, type);
        }

        

    }
}
