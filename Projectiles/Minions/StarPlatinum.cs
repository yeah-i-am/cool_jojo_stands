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
            Main.projFrames[projectile.type] = 15;
            DisplayName.SetDefault("Star Platinum");
            Main.projPet[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 92;
            projectile.height = 92;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 239;
            projectile.tileCollide = false;
            projectile.melee = true;
            projectile.ignoreWater = true;
            projectile.alpha = 255;
        }

        public override bool CanDamage() => false;
        public override bool? CanCutTiles() => false;
        public override bool MinionContactDamage() => false;

        /* Select animation frame function */
        public override void SelectFrame()
        {
            if (projectile.alpha > 30)
                projectile.alpha -= 8;

            StandoPlayer pl = Main.player[projectile.owner].GetModPlayer<StandoPlayer>();

            projectile.frameCounter++;

            if (attacking)
            {
                if (pl.StandJotaroSetBonus > 0)
                {
                    if (projectile.frameCounter >= 3)
                    {
                        projectile.frame = 12 + (projectile.frame + 1) % 3;
                        projectile.frameCounter = 0;
                    }

                }
                else if (projectile.frameCounter >= 5 &&  projectile.frame % 2 == 1 || projectile.frameCounter >= 2 && projectile.frame % 2 == 0)
                {
                    projectile.frameCounter = 0;
                    projectile.frame = 8 + (projectile.frame + 1) % 4;
                }
            }
            else
              if (projectile.frameCounter >= 10)
              {
                  projectile.frameCounter = 0;
                  projectile.frame = (projectile.frame + 1) % 8;
              }
        }

        /* Star Platinum dust function */
        public override void CreateDust()
        {
            Lighting.AddLight(projectile.Center, 0.7f, 0f, 0.7f);
        }

        /* Stand kill function */
        public override void Kill(int timeLeft)
        {
            Player player = Main.player[projectile.owner];
            StandoPlayer pl = player.GetModPlayer<StandoPlayer>();
            pl.StandSpawned = false;
        }
    } /* End of 'StarPlatinum' class */
}