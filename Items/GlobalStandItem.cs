using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace cool_jojo_stands.Items
{
    public class GlobalStandItem : GlobalItem
    {
        private const double StrangeOreDropChance = 0.02;

        public override void ExtractinatorUse(int extractType, ref int resultType, ref int resultStack)
        {
            if (extractType == ItemID.StoneBlock)
                if (Main.rand.NextDouble() < StrangeOreDropChance)
                {
                    resultType = ModContent.ItemType<StrangeOre>();
                    resultStack = 1;
                }
                else
                    resultType = 0;
        }
    }
}
