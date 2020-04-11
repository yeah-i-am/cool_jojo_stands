using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace cool_jojo_stands.Projectiles
{
    public class Dolphin : ModProjectile
    {
        NPC target = null;
        public int type = 0;
        public float Speed = 12f;

        /*****************
         * Some settings *
         *****************/
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dolphin");
            Main.projFrames[projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            projectile.width = 40;
            projectile.height = 30;
            projectile.alpha = 0;
            projectile.timeLeft = 600;
            projectile.penetrate = 1;
            projectile.hostile = true;
            projectile.ranged = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.netUpdate = true;
        }

        public override bool? CanCutTiles() => true;
        public override bool? CanHitNPC(NPC target) => !target.friendly;
        public override bool MinionContactDamage() => true;
        public override bool CanHitPlayer(Player target) => false;

        /* AI function */
        public override void AI()
        {
            type = (int)projectile.ai[1];

            if (projectile.localAI[1] == 0)
            {
                Main.PlaySound(
                    mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/DolphinSound").WithVolume(cool_jojo_stands.standBulletVolume),
                    projectile.Center);
                projectile.localAI[1] = 239;
            }

            if (projectile.localAI[0] == 0f && type == 0)
            {
                projectile.oldVelocity = projectile.velocity;

                float distance = float.MaxValue, newDist;

                for (int k = 0; k < 200; k++)
                {
                    NPC npc = Main.npc[k];

                    if (npc.CanBeChasedBy(this, false) &&
                        (newDist = Vector2.DistanceSquared(projectile.Center, npc.Center)) < distance)
                    {
                        distance = newDist;
                        target = npc;
                    }
                }

                if (target != null)
                    projectile.localAI[0] = 1f;

            }

            if (projectile.ai[0] != 1)
            {
                CreateDust();
                projectile.ai[0] = 1;
            }
            else
                projectile.ai[0] = 0;

            if (projectile.localAI[0] == 1f && type == 0)
            {
                  if (!target.active)
                  {
                      target = null;
                      projectile.localAI[0] = 0f;
                      return;
                  }

                  Vector2 dir = target.Center - projectile.Center;
                  Vector2 vel = projectile.oldVelocity;

                vel.Normalize();

                dir /= dir.LengthSquared();
                dir *= 30;

                  Vector2 newVel = vel + dir;
                  newVel.Normalize();

                if (newVel.X < 0)
                    projectile.spriteDirection = -1;
                else
                    projectile.spriteDirection = 1;

                  projectile.rotation = MathHelper.PiOver2 * -(projectile.direction - 1) +
                    MathHelper.PiOver2 - (float)Math.Atan2(newVel.X, newVel.Y);

                  projectile.velocity = newVel * Speed;
            }
            else if (type == 1)
            {
                if (projectile.velocity.X < 0)
                    projectile.spriteDirection = -1;
                else
                    projectile.spriteDirection = 1;

                projectile.rotation = MathHelper.PiOver2 * -(projectile.direction - 1) +
                    MathHelper.PiOver2 - (float)Math.Atan2(projectile.velocity.X, projectile.velocity.Y);
            }

            SelectFrame();
        }

        /* Dust function */
        public void CreateDust()
        {
            /*int dust = Dust.NewDust(projectile.position,
                6, 6,
                75,
                projectile.velocity.X, projectile.velocity.Y,
                0, default(Color), 2.75f);

            Main.dust[dust].position += projectile.velocity;
            Main.dust[dust].noGravity = true;*/
        }

        public void SelectFrame()
        {
            projectile.frameCounter++;

            if (projectile.frameCounter > 5)
            {
                projectile.frame = (projectile.frame + 1) % 4;
                projectile.frameCounter = 0;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (type == 1)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

                var shader = GameShaders.Misc["RedDolphin"];

                shader.UseOpacity(1f);
                shader.Apply(null);
            }

            return true;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (type == 1)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
            }
        }

        /* NPC hit function */
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (projectile.penetrate < 1)
            {
                projectile.Kill();
                return;
            }

            if (type == 1 && projectile.alpha != 255)
            {
                projectile.alpha = 255;
                projectile.position = projectile.Center;
                projectile.width = 150;
                projectile.height = 150;
                projectile.Center = projectile.position;
                projectile.damage *= 2;
                projectile.knockBack *= 2f;
                projectile.timeLeft = 4;
                projectile.velocity = Vector2.Zero;
                projectile.penetrate = 100;

                damage *= 2;
                knockback *= 2f;
            }
            else
                projectile.penetrate--;

            Player player = Main.player[projectile.owner];
            StandoPlayer pl = player.GetModPlayer<StandoPlayer>();

            damage *= 13 * pl.StandLevel;
        }

        public override void Kill(int timeLeft)
        {
            if (type == 1)
            {
                // Play explosion sound
                Main.PlaySound(SoundID.Item14, projectile.position);

                // Smoke Dust spawn
                for (int i = 0; i < 50; i++)
                {
                    int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 31, 0f, 0f, 100, default(Color), 2f);
                    Main.dust[dustIndex].velocity *= 1.4f;
                }
                // Fire Dust spawn
                for (int i = 0; i < 80; i++)
                {
                    int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 3f);
                    Main.dust[dustIndex].noGravity = true;
                    Main.dust[dustIndex].velocity *= 5f;
                    dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 2f);
                    Main.dust[dustIndex].velocity *= 3f;
                }
                // Large Smoke Gore spawn
                for (int g = 0; g < 2; g++)
                {
                    int goreIndex = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                    Main.gore[goreIndex].scale = 1.5f;
                    Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X + 1.5f;
                    Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 1.5f;
                    goreIndex = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                    Main.gore[goreIndex].scale = 1.5f;
                    Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X - 1.5f;
                    Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 1.5f;
                    goreIndex = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                    Main.gore[goreIndex].scale = 1.5f;
                    Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X + 1.5f;
                    Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y - 1.5f;
                    goreIndex = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                    Main.gore[goreIndex].scale = 1.5f;
                    Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X - 1.5f;
                    Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y - 1.5f;
                }
            }
        }
    }
}
