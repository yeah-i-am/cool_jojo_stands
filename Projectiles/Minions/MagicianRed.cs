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
            Main.projFrames[Projectile.type] = 14;
            DisplayName.SetDefault("Magician Red");
            Main.projPet[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 92;
            Projectile.height = 92;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 18000;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ignoreWater = true;
            Projectile.alpha = 30;

            Shoot = ModContent.ProjectileType<FireBlast>();
            ShootVel = 12f;
        }

        public override void SelectFrame()
        {
            if (Projectile.alpha > 30)
                Projectile.alpha -= 8;

            Player player = Main.player[Projectile.owner];
            StandoPlayer pl = player.GetModPlayer<StandoPlayer>();

            if (pl.StandAvdolSetBonus == 1)
                TypeOfAttack = 1;
            else
                TypeOfAttack = 0;

            Projectile.frameCounter++;

            if (atacking)
            {
                if (pl.StandAvdolSetBonus > 0)
                {
                    if (Projectile.frameCounter >= 3)
                    {
                        Projectile.frame = 6 + (Projectile.frame + 1) % 8;
                        Projectile.frameCounter = 0;
                    }
                }
                else if (Projectile.ai[1] > 0.01f)
                {
                    Projectile.frameCounter = 0;
                    Projectile.frame = 5;
                }
                else
                {
                    Projectile.frameCounter = 0;
                    Projectile.frame = 4;
                }
            }
            else if (Projectile.frameCounter >= 10)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 4;
            }
        }

        public override void CreateDust()
        {
            Player player = Main.player[Projectile.owner];
            StandoPlayer pl = player.GetModPlayer<StandoPlayer>();

            Lighting.AddLight(Projectile.position + Projectile.Size / 2, 0.7f, 0f, 0f);

            int i;

              for (i = 0; i < 4; i++)
              {
                    int num309 = Dust.NewDust(Projectile.position + Projectile.Size / 2 +
                        new Vector2
                        (
                            3 + 2.5f * (i - 2) + (Projectile.direction - 1) * 2.5f + ((atacking && pl.StandAvdolSetBonus > 0) ? -4 : 0),
                            30 - 1.5f + ((atacking && pl.StandAvdolSetBonus > 0) ? -8 : 0)
                        ),
                        8, 8, DustID.Torch, Projectile.velocity.X, Projectile.velocity.Y, 0, default(Color), 2.25f);

                  Main.dust[num309].velocity += new Vector2(0, -0.1f);
                  Main.dust[num309].noGravity = true;
                  Main.dust[num309].position += Projectile.velocity;/// * 0.5f;
              }

            if (Projectile.ai[0] < 1.5f && Projectile.ai[0] > 0.2f)
            {
                Vector2 R = new Vector2(0, -Projectile.Size.Y * 0.6f);

                for (i = 0; i < 1 + 9 * (1.5f - Projectile.ai[0]) / 1.3f; i++)
                {
                    int num309 = Dust.NewDust(Projectile.position + Projectile.Size / 2 + R.RotatedBy(Math.PI / 5 * i),
                        8, 8, DustID.Torch, Projectile.velocity.X, Projectile.velocity.Y - 0.1f, 0, default(Color), 2.25f);

                    Main.dust[num309].noGravity = true;
                    Main.dust[num309].position += Projectile.velocity;
                }
            }

            if (!atacking)
            {


            }
        }

        public override bool? CanDamage()
        {
            return null;
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
            Player player = Main.player[Projectile.owner];
            StandoPlayer pl = player.GetModPlayer<StandoPlayer>();

            damage *= (pl.StandLevel * pl.StandLevel) + pl.StandLevel;

            if (pl.StandAvdolSetBonus > 0)
                target.StrikeNPCNoInteraction(damage, 0f, hitDirection);

            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override void Some()
        { }
    }
}
