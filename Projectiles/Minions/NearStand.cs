using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace cool_jojo_stands.Projectiles.Minions
{
    public abstract class NearStand : Minion
    {
        protected float viewEnemyDist = 150f;
        protected float chasePlayerSpeed = 6f;
        protected float maxSpeed = 10000f;
        protected float maxPlayerDist = 330f;
        protected float inertia = 20f;
        protected float MaxDist = 300f;
        protected bool AUA;
        protected bool atacking;

        public virtual void SelectFrame()
        {
        }

        public virtual void CreateDust()
        {
        }

        public override void Behavior()
        {
            Player player = Main.player[projectile.owner];

            /****************
             * Chase Player *
             ****************/
            Vector2 Direction = player.Center - projectile.Center;
            Direction.X -= 30f * player.direction;
            Direction.Y -= 30f;

            Direction.Normalize();

            float TargetDist = Vector2.Distance(player.Center - new Vector2(30f * player.direction, 30f), projectile.Center);
            int NewDirection = Main.player[projectile.owner].direction;
            Vector2 NewVelocity = projectile.velocity;

            AUA = false;
            atacking = false;

            /*************
             * Chase NPC *
             *************/
            float targetDist = viewEnemyDist;
            Vector2 targetPos = new Vector2(0f, 0f);

            Vector2 trgdir = new Vector2(NewDirection, 0);

            for (int k = 0; k < 200; k++)
            {
                NPC npc = Main.npc[k];

                if (npc.CanBeChasedBy(this, false))
                {
                    float distance = Vector2.Distance(npc.Center, player.Center);

                    if ((distance < targetDist)/* && Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height)*/)
                    {
                        targetDist = Vector2.Distance(npc.Center, projectile.Center);
                        trgdir = npc.Center - projectile.Center;
                        trgdir.Y = 0f;
                        trgdir.X = Math.Sign(trgdir.X);
                        targetPos = npc.Center - trgdir * (npc.Hitbox.Width + projectile.width) * 0.37f;
                        AUA = true;
                    }
                }
            }

            if (AUA)
            {
                Direction = targetPos - projectile.Center;
                Direction.Normalize();
                NewDirection = Math.Sign(trgdir.X);

                if (targetDist < projectile.width)
                    atacking = true;
            }

            /*************************
             * Check Player distance *
             *************************/
            if (Vector2.Distance(player.Center, projectile.Center) > MaxDist)
            {
                Direction = player.Center - projectile.Center;
                Direction.X -= 30f * player.direction;
                Direction.Y -= 30f;

                Direction.Normalize();

                AUA = atacking = false;
            }

            /***************************
             * Processing speed(wagon) *
             ***************************/
            if (TargetDist > 1f)
            {
                if (TargetDist > maxPlayerDist)
                {
                    NewVelocity = Direction * Math.Max(player.velocity.Length(), 5f);
                }
                else
                {
                    float temp = inertia / 2f;
                    NewVelocity = (projectile.velocity * temp + Direction * chasePlayerSpeed) / (temp + 1);
                }
            }
            else
                NewVelocity /= 1.2f;

            if (projectile.velocity.Length() > maxSpeed)
            {
                projectile.velocity.Normalize();
                projectile.velocity *= 9;
            }

            projectile.direction = NewDirection;
            projectile.spriteDirection = projectile.direction;
            projectile.velocity = NewVelocity;
            SelectFrame();
            CreateDust();
            Some();
            projectile.netUpdate = true;
        }

        public override void Some()
        {
        }

        public override void CheckActive()
        {
            Player player = Main.player[projectile.owner];
            StandoPlayer pl = player.GetModPlayer<StandoPlayer>(mod);

            if (pl.HaveStand)
                projectile.timeLeft = 2;

            if (pl.StandJustSpawned)
            {
                pl.StandJustSpawned = false;
                return;
            }

            if (pl.StandSpawned && cool_jojo_stands.StandSummonHT.JustPressed || player.dead)
            {
                pl.StandSpawned = false;
                this.projectile.Kill();
            }
        }
    }
}