using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
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

            Main.RegisterItemAnimation(item.type, new Terraria.DataStructures.DrawAnimationVertical(7, 12));
        }
        public override void SetDefaults()
        {
            item.width = 40;
            item.height = 40;
            item.useTime = 40;
            item.useAnimation = 40;
            item.useStyle = 4;
            item.value = Item.buyPrice(0, 0, 47, 0);
            item.rare = 11;
            item.UseSound = SoundID.Item1;
            item.maxStack = 99;
            item.consumable = true;
            item.autoReuse = false;
            item.noUseGraphic = true;
            item.shoot = mod.ProjectileType<Projectiles.TarotCards>();
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
            StandoPlayer pl = player.GetModPlayer<StandoPlayer>(mod);

            if (!pl.HaveStand)
                return false;

            Random rand = new Random(DateTime.Now.Second);

            int k = rand.Next(stands.Length);

            while (player.HasBuff(mod.BuffType(stands[k])))
                k = rand.Next(stands.Length);

            if (pl.HaveStarPlatinum)
            {
                player.ClearBuff(mod.BuffType<Buffs.StarPlatinumStand>());
                pl.HaveStarPlatinum = false;
            }
            else if (pl.HaveMagicianRedStand)
            {
                player.ClearBuff(mod.BuffType<Buffs.MagicianRedStand>());
                pl.HaveMagicianRedStand = false;
            }
            else if(pl.HaveHierophantGreenStand)
            {
                player.ClearBuff(mod.BuffType<Buffs.HierophantGreenStand>());
                pl.HaveHierophantGreenStand = false;
            }
            else if (pl.HaveHarmitPurpleStand)
            {
                player.ClearBuff(mod.BuffType<Buffs.HermitPurpleStand>());
                pl.HaveHarmitPurpleStand = false;
            }
            else if (pl.HaveSilverChariotStand)
            {
                ///
            }

            player.AddBuff(mod.BuffType(stands[k]), 239);

            return true;
        }
    }
}
