﻿using System;
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
    class JotaroCoat : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Jotaro Coat");
            Tooltip.SetDefault("Looks like school uniform");
            ArmorIDs.Body.Sets.HidesHands[Item.bodySlot] = false;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 14;
            Item.value = Item.buyPrice(0, 3, 47, 0);
            Item.rare = ItemRarityID.LightPurple;
            Item.defense = 7;
        }

        public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor)
        {
            base.DrawArmorColor(drawPlayer, shadow, ref color, ref glowMask, ref glowMaskColor);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Silk, 24);
            recipe.AddRecipeGroup("CoolJoJoStands:GoldOrPlatinum", 13);
            recipe.AddIngredient(ItemID.Obsidian, 15);
            recipe.AddIngredient(ItemID.FallenStar, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
