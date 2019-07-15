using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace cool_jojo_stands.Projectiles.Minions
{
    public class StarPlatinum : NearStand
    {
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
            projectile.timeLeft = 18000;
            projectile.tileCollide = false;
            projectile.melee = true;
            projectile.ignoreWater = true;
            projectile.alpha = 30;
        }

        public override void SelectFrame()
        {
            Player player = Main.player[projectile.owner];
            StandoPlayer pl = player.GetModPlayer<StandoPlayer>(mod);

            projectile.frameCounter++;

            if (atacking)
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

        public override void CreateDust()
        {
            Lighting.AddLight(projectile.position + projectile.Size / 2, 0.7f, 0f, 0.7f);

            /*int i;

            for (i = 0; i < 10; i++)
            {
                int num309 = Dust.NewDust(projectile.position + projectile.Size / 2 + new Vector2(2 * (i - 5) - 6, 9), 8, 8, DustID.Fire, projectile.velocity.X, projectile.velocity.Y, 0, default(Color), 1.75f);
                //Main.dust[num309].velocity += new Vector2(0, 3);
                Main.dust[num309].noGravity = true;

                //Main.dust[num309].position -= projectile.velocity * 0.5f;
            }*/
        }

        public override bool CanDamage()
        {
            return true;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        public override bool? CanHitNPC(NPC target)
        {
            return !target.friendly;
        }

        public override bool MinionContactDamage()
        {
            return true;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Player player = Main.player[projectile.owner];
            StandoPlayer pl = player.GetModPlayer<StandoPlayer>(mod);

            damage *= (pl.StandLevel * pl.StandLevel) + pl.StandLevel;

            if (pl.StandJotaroSetBonus > 0)
              target.StrikeNPCNoInteraction(damage, 0f, hitDirection);

            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }
    }
}