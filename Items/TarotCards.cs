﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace cool_jojo_stands.Items
{
    class TarotCards : ModItem
    {
        public string[] stands =
        {
            "HermitPurple", "HierophantGreen", "MagicianRed", "StarPlatinum" , "TheWorld"
        };

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tarot cards");
            Tooltip.SetDefault("Picks your stand");

            Main.RegisterItemAnimation(item.type, new Terraria.DataStructures.DrawAnimationVertical(7, 12));
        }
        public override void SetDefaults()
        {
            item.width = 40;
            item.height = 40;
            item.useTime = 40;
            item.useAnimation = 40;
            item.useStyle = ItemUseStyleID.HoldingUp;
            item.value = Item.buyPrice(0, 0, 47, 0);
            item.rare = ItemRarityID.Purple;
            item.UseSound = SoundID.Item1;
            item.maxStack = 99;
            item.consumable = true;
            item.autoReuse = false;
            item.noUseGraphic = true;
            item.shoot = ModContent.ProjectileType<Projectiles.TarotCards>();
            item.shootSpeed = 40f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.FallenStar, 1);
            recipe.AddIngredient(ItemID.Cactus, 10);
            recipe.AddIngredient(ItemID.GoldCoin, 10);
            recipe.AddTile(TileID.Tables);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            StandoPlayer pl = player.GetModPlayer<StandoPlayer>();

            if (!pl.HaveStand)
                return false;

            Terraria.Utilities.UnifiedRandom rand = Main.rand;

            int k = rand.Next(stands.Length);

            while (player.HasBuff(mod.BuffType(stands[k] + "Stand")))
                k = rand.Next(stands.Length);

            pl.DeleteStand();
            pl.StandBuffName = stands[k];
            pl.Stand = (StandType)Enum.Parse(typeof(StandType), stands[k]);
            player.AddBuff(mod.BuffType(stands[k] + "Stand"), 32767);

            return true;
        }
    }
}
