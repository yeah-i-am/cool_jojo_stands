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
    class AvdolCoat : ModItem
    {
        bool HaveAvdolBoots = false;
        int a = 0;

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Avdol Coat");
            Tooltip.SetDefault("Oh, are you from Egypt?");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 14;
            item.value = Item.buyPrice(0, 3, 47, 0);
            item.rare = 6;
            item.defense = 7;
        }

        public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor)
        {
            /*if (drawPlayer.armor[2].type == mod.ItemType<AvdolLegs>())
                HaveJotaroBoots = true;
            else
                HaveJotaroBoots = false;*/

            base.DrawArmorColor(drawPlayer, shadow, ref color, ref glowMask, ref glowMaskColor);
        }

        public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
        {
            //robes = true;

            // The equipSlot is added in cool_jojo_stands.cs --> Load hook
            /*if (HaveJotaroBoots)
                equipSlot = mod.GetEquipSlot("JotaroCoat_LegsAndBoots", EquipType.Legs);
            else
                equipSlot = mod.GetEquipSlot("JotaroCoat_Legs", EquipType.Legs);
            */
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
            recipe.AddIngredient(ItemID.GoldBar, 13);
            recipe.AddIngredient(ItemID.HellstoneBar, 7);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
