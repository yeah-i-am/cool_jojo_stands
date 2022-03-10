using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace cool_jojo_stands.Projectiles.Minions
{
    public class MagicianRed : TwoTipeAttackStand
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 14;
            DisplayName.SetDefault("Magician Red");
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

            Shoot = ModContent.ProjectileType<FireBlast>();
            ShootVel = 12f;
        }

        public override void SelectFrame()
        {
            if (projectile.alpha > 30)
                projectile.alpha -= 8;

            Player player = Main.player[projectile.owner];
            StandoPlayer pl = player.GetModPlayer<StandoPlayer>();

            if (pl.StandAvdolSetBonus == 1)
                TypeOfAttack = 1;
            else
                TypeOfAttack = 0;

            projectile.frameCounter++;

            if (atacking)
            {
                if (pl.StandAvdolSetBonus > 0)
                {
                    if (projectile.frameCounter >= 3)
                    {
                        projectile.frame = 6 + (projectile.frame + 1) % 8;
                        projectile.frameCounter = 0;
                    }
                }
                else if (projectile.ai[1] > 0.01f)
                {
                    projectile.frameCounter = 0;
                    projectile.frame = 5;
                }
                else
                {
                    projectile.frameCounter = 0;
                    projectile.frame = 4;
                }
            }
            else if (projectile.frameCounter >= 10)
            {
                projectile.frameCounter = 0;
                projectile.frame = (projectile.frame + 1) % 4;
            }
        }

        public override void CreateDust()
        {
            Player player = Main.player[projectile.owner];
            StandoPlayer pl = player.GetModPlayer<StandoPlayer>();

            Lighting.AddLight(projectile.position + projectile.Size / 2, 0.7f, 0f, 0f);

            int i;

              for (i = 0; i < 4; i++)
              {
                    int num309 = Dust.NewDust(projectile.position + projectile.Size / 2 +
                        new Vector2
                        (
                            3 + 2.5f * (i - 2) + (projectile.direction - 1) * 2.5f + ((atacking && pl.StandAvdolSetBonus > 0) ? -4 : 0),
                            30 - 1.5f + ((atacking && pl.StandAvdolSetBonus > 0) ? -8 : 0)
                        ),
                        8, 8, DustID.Fire, projectile.velocity.X, projectile.velocity.Y, 0, default(Color), 2.25f);

                  Main.dust[num309].velocity += new Vector2(0, -0.1f);
                  Main.dust[num309].noGravity = true;
                  Main.dust[num309].position += projectile.velocity;/// * 0.5f;
              }

            if (projectile.ai[0] < 1.5f && projectile.ai[0] > 0.2f)
            {
                Vector2 R = new Vector2(0, -projectile.Size.Y * 0.6f);

                for (i = 0; i < 1 + 9 * (1.5f - projectile.ai[0]) / 1.3f; i++)
                {
                    int num309 = Dust.NewDust(projectile.position + projectile.Size / 2 + R.RotatedBy(Math.PI / 5 * i),
                        8, 8, DustID.Fire, projectile.velocity.X, projectile.velocity.Y - 0.1f, 0, default(Color), 2.25f);

                    Main.dust[num309].noGravity = true;
                    Main.dust[num309].position += projectile.velocity;
                }
            }

            if (!atacking)
            {


            }
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
            return !target.friendly && TypeOfAttack != 0;
        }

        public override bool MinionContactDamage()
        {
            return true;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Player player = Main.player[projectile.owner];
            StandoPlayer pl = player.GetModPlayer<StandoPlayer>();

            damage *= (pl.StandLevel * pl.StandLevel) + pl.StandLevel;

            if (pl.StandAvdolSetBonus > 0)
                target.StrikeNPCNoInteraction(damage, 0f, hitDirection);

            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override void PostUpdate()
        { }
    }
}
