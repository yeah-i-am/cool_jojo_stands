using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace cool_jojo_stands.Projectiles.Minions
{
    public class StarPlatinum : NearStand
    {
        /*****************
         * Some settings *
         *****************/
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 15;
            DisplayName.SetDefault("Star Platinum");
            Main.projPet[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 92;
            Projectile.height = 92;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 239;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ignoreWater = true;
            Projectile.alpha = 255;
        }

        public override bool? CanDamage() => false;
        public override bool? CanCutTiles() => false;
        public override bool MinionContactDamage() => false;

        /* Select animation frame function */
        public override void SelectFrame()
        {
            if (Projectile.alpha > 30)
                Projectile.alpha -= 8;

            StandoPlayer pl = Main.player[Projectile.owner].GetModPlayer<StandoPlayer>();

            Projectile.frameCounter++;

            if (attacking)
            {
                if (pl.StandJotaroSetBonus > 0)
                {
                    if (Projectile.frameCounter >= 3)
                    {
                        Projectile.frame = 12 + (Projectile.frame + 1) % 3;
                        Projectile.frameCounter = 0;
                    }

                }
                else if (Projectile.frameCounter >= 5 &&  Projectile.frame % 2 == 1 || Projectile.frameCounter >= 2 && Projectile.frame % 2 == 0)
                {
                    Projectile.frameCounter = 0;
                    Projectile.frame = 8 + (Projectile.frame + 1) % 4;
                }
            }
            else
              if (Projectile.frameCounter >= 10)
              {
                  Projectile.frameCounter = 0;
                  Projectile.frame = (Projectile.frame + 1) % 8;
              }
        }

        /* Star Platinum dust function */
        public override void CreateDust()
        {
            Lighting.AddLight(Projectile.Center, 0.7f, 0f, 0.7f);
        }

        /* Stand kill function */
        public override void Kill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
            StandoPlayer pl = player.GetModPlayer<StandoPlayer>();
            pl.StandSpawned = false;
        }
    } /* End of 'StarPlatinum' class */
}