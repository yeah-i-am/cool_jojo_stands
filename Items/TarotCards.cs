using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace cool_jojo_stands.Items
{
    class TarotCards : ModItem
    {
        string[] stands =
        {
          "StarPlatinumStand", "MagicianRedStand", "HierophantGreenStand"///, "HermitPurpleStand"
        };

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tarot cards");
            Tooltip.SetDefault("Picks your stand");

            Main.RegisterItemAnimation(Item.type, new Terraria.DataStructures.DrawAnimationVertical(7, 12));
        }
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 40;
            Item.useAnimation = 40;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.value = Item.buyPrice(0, 0, 47, 0);
            Item.rare = ItemRarityID.Purple;
            Item.UseSound = SoundID.Item1;
            Item.maxStack = 99;
            Item.consumable = true;
            Item.autoReuse = false;
            Item.noUseGraphic = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.TarotCards>();
            Item.shootSpeed = 40f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.FallenStar, 1);
            recipe.AddIngredient(ItemID.Cactus, 10);
            recipe.AddIngredient(ItemID.GoldCoin, 10);
            recipe.AddTile(TileID.Tables);
            recipe.Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            StandoPlayer pl = player.GetModPlayer<StandoPlayer>();

            if (!pl.HaveStand)
                return false;

            Terraria.Utilities.UnifiedRandom rand = Main.rand;

            if (rand.NextBool(1000) && !player.HasBuff(ModContent.BuffType<Buffs.StarPlatinumRequiemStand>()))
            {
                pl.DeleteStand();
                player.AddBuff(ModContent.BuffType<Buffs.StarPlatinumRequiemStand>(), 239);

                return true;
            }
            else
            {
                int k = rand.Next(stands.Length);

                while (player.HasBuff(Mod.Find<ModBuff>(stands[k]).Type))
                    k = rand.Next(stands.Length);

                pl.DeleteStand();

                player.AddBuff(Mod.Find<ModBuff>(stands[k]).Type, 239);

                return true;
            }
        }
    }
}
