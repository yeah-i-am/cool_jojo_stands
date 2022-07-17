using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using cool_jojo_stands.Sounds.Custom;

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
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 30;
            Projectile.alpha = 0;
            Projectile.timeLeft = 600;
            Projectile.penetrate = 1;
            Projectile.hostile = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.netUpdate = true;
        }

        public override bool? CanCutTiles() => true;
        public override bool? CanHitNPC(NPC target) => !target.friendly;
        public override bool MinionContactDamage() => true;
        public override bool CanHitPlayer(Player target) => false;

        /* AI function */
        public override void AI()
        {
            type = (int)Projectile.ai[1];

            if (Projectile.localAI[1] == 0)
            {
                SoundEngine.PlaySound(
                    DolphinSound.GetInstance("cool_jojo_stands/Sounds/Custom/DolphinSound", StandModSystem.standBulletVolume),
                    Projectile.Center);
                Projectile.localAI[1] = 239;
            }

            if (Projectile.localAI[0] == 0f && type == 0)
            {
                Projectile.oldVelocity = Projectile.velocity;

                float distance = float.MaxValue, newDist;

                for (int k = 0; k < 200; k++)
                {
                    NPC npc = Main.npc[k];

                    if (npc.CanBeChasedBy(this, false) &&
                        (newDist = Vector2.DistanceSquared(Projectile.Center, npc.Center)) < distance)
                    {
                        distance = newDist;
                        target = npc;
                    }
                }

                if (target != null)
                    Projectile.localAI[0] = 1f;

            }

            if (Projectile.ai[0] != 1)
            {
                CreateDust();
                Projectile.ai[0] = 1;
            }
            else
                Projectile.ai[0] = 0;

            if (Projectile.localAI[0] == 1f && type == 0)
            {
                  if (!target.active)
                  {
                      target = null;
                      Projectile.localAI[0] = 0f;
                      return;
                  }

                  Vector2 dir = target.Center - Projectile.Center;
                  Vector2 vel = Projectile.oldVelocity;

                vel.Normalize();

                dir /= dir.LengthSquared();
                dir *= 30;

                  Vector2 newVel = vel + dir;
                  newVel.Normalize();

                if (newVel.X < 0)
                    Projectile.spriteDirection = -1;
                else
                    Projectile.spriteDirection = 1;

                  Projectile.rotation = MathHelper.PiOver2 * -(Projectile.direction - 1) +
                    MathHelper.PiOver2 - (float)Math.Atan2(newVel.X, newVel.Y);

                  Projectile.velocity = newVel * Speed;
            }
            else if (type == 1)
            {
                if (Projectile.velocity.X < 0)
                    Projectile.spriteDirection = -1;
                else
                    Projectile.spriteDirection = 1;

                Projectile.rotation = MathHelper.PiOver2 * -(Projectile.direction - 1) +
                    MathHelper.PiOver2 - (float)Math.Atan2(Projectile.velocity.X, Projectile.velocity.Y);
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
            Projectile.frameCounter++;

            if (Projectile.frameCounter > 5)
            {
                Projectile.frame = (Projectile.frame + 1) % 4;
                Projectile.frameCounter = 0;
            }
        }

        public override bool PreDraw(ref Color lightColor)
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

        public override void PostDraw(Color lightColor)
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
            if (Projectile.penetrate < 1)
            {
                Projectile.Kill();
                return;
            }

            if (type == 1 && Projectile.alpha != 255)
            {
                Projectile.alpha = 255;
                Projectile.position = Projectile.Center;
                Projectile.width = 150;
                Projectile.height = 150;
                Projectile.Center = Projectile.position;
                Projectile.damage *= 2;
                Projectile.knockBack *= 2f;
                Projectile.timeLeft = 4;
                Projectile.velocity = Vector2.Zero;
                Projectile.penetrate = 100;

                damage *= 2;
                knockback *= 2f;
            }
            else
                Projectile.penetrate--;

            Player player = Main.player[Projectile.owner];
            StandoPlayer pl = player.GetModPlayer<StandoPlayer>();

            damage *= 13 * pl.StandLevel;
        }

        public override void Kill(int timeLeft)
        {
            if (type == 1)
            {
                // Play explosion sound
                SoundEngine.PlaySound(SoundID.Item14, Projectile.position);

                // Smoke Dust spawn
                for (int i = 0; i < 50; i++)
                {
                    int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default(Color), 2f);
                    Main.dust[dustIndex].velocity *= 1.4f;
                }
                // Fire Dust spawn
                for (int i = 0; i < 80; i++)
                {
                    int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default(Color), 3f);
                    Main.dust[dustIndex].noGravity = true;
                    Main.dust[dustIndex].velocity *= 5f;
                    dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default(Color), 2f);
                    Main.dust[dustIndex].velocity *= 3f;
                }
                // Large Smoke Gore spawn
                for (int g = 0; g < 2; g++)
                {
                    int goreIndex = Gore.NewGore(Projectile.GetSource_FromThis(), new Vector2(Projectile.position.X + (float)(Projectile.width / 2) - 24f, Projectile.position.Y + (float)(Projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                    Main.gore[goreIndex].scale = 1.5f;
                    Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X + 1.5f;
                    Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 1.5f;
                    goreIndex = Gore.NewGore(Projectile.GetSource_FromThis(), new Vector2(Projectile.position.X + (float)(Projectile.width / 2) - 24f, Projectile.position.Y + (float)(Projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                    Main.gore[goreIndex].scale = 1.5f;
                    Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X - 1.5f;
                    Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 1.5f;
                    goreIndex = Gore.NewGore(Projectile.GetSource_FromThis(), new Vector2(Projectile.position.X + (float)(Projectile.width / 2) - 24f, Projectile.position.Y + (float)(Projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                    Main.gore[goreIndex].scale = 1.5f;
                    Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X + 1.5f;
                    Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y - 1.5f;
                    goreIndex = Gore.NewGore(Projectile.GetSource_FromThis(), new Vector2(Projectile.position.X + (float)(Projectile.width / 2) - 24f, Projectile.position.Y + (float)(Projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                    Main.gore[goreIndex].scale = 1.5f;
                    Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X - 1.5f;
                    Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y - 1.5f;
                }
            }
        }
    }
}
