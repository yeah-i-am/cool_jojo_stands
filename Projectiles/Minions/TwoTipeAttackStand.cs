using System;
using Microsoft.Xna.Framework;
using Terraria;

namespace cool_jojo_stands.Projectiles.Minions
{
    public abstract class TwoTipeAttackStand : Minion
    {
        protected float viewEnemyDist = 150f;
        protected float chasePlayerSpeed = 6f;
        protected float maxSpeed = 10000f;
        protected float maxPlayerDist = 330f;
        protected float inertia = 20f;
        protected float MaxDist = 300f;
        protected bool AUA;
        protected bool atacking;
        protected int TypeOfAttack = 0;
        public int Shoot;
        public float ShootVel = 1f;

        public virtual void SelectFrame()
        {
        }

        public virtual void CreateDust()
        {
        }

        public override void Behavior()
        {
            Player player = Main.player[Projectile.owner];
            const float PlayerStayDist = 40f;

            Projectile.ai[0] -= 1 / 60f;
            Projectile.ai[1] -= 1 / 60f;

            /****************
             * Chase Player *
             ****************/
            Vector2 Direction = player.Center - Projectile.Center;
            Direction.X -= PlayerStayDist * player.direction;
            Direction.Y -= 30f;

            Direction.Normalize();

            float TargetDist = Vector2.Distance(player.Center - new Vector2(PlayerStayDist * player.direction, 30f), Projectile.Center);
            int NewDirection = Main.player[Projectile.owner].direction;
            Vector2 NewVelocity = Projectile.velocity;

            if (TypeOfAttack != 0 || Projectile.ai[0] < 0.01f)
            {
                AUA = false;
                atacking = false;
            }

            if (TypeOfAttack == 0)
            {
                /****************
                 * Shoot to NPC *
                 ****************/

                float targetDist = viewEnemyDist * 6;
                Vector2 targetPos = new Vector2(0f, 0f);

                Vector2 trgdir = new Vector2(NewDirection, 0);

                for (int k = 0; k < 200; k++)
                {
                    NPC npc = Main.npc[k];

                    if (npc.CanBeChasedBy(this, false))
                    {
                        float distance = Vector2.Distance(npc.Center, player.Center);

                        if ((distance < targetDist) && Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height))
                        {
                            targetDist = Vector2.Distance(npc.Center, Projectile.Center);
                            trgdir = npc.Center - Projectile.Center;
                            trgdir.X = Math.Sign(trgdir.X);
                            targetPos = npc.Center;
                            AUA = true;
                        }
                    }
                }

                if (AUA)
                {
                    if (targetPos == new Vector2(0f, 0f))
                    {
                        AUA = false;
                        Projectile.ai[1] = 0f;
                    }
                    ///Direction = targetPos - projectile.Center;
                    ///Direction.Normalize;
                    NewDirection = Math.Sign(trgdir.X);

                    if (Projectile.ai[0] < 0.01f || Projectile.ai[1] > 0.01f)
                    {
                        if (Projectile.ai[1] < 0.01f && Projectile.ai[0] < 0.01f)
                        {
                            Projectile.ai[1] = 1.09f;
                        }
                        else if (Projectile.ai[1] % 0.1f < 1f / 60f)
                        {
                            ///StandoPlayer.Talk((projectile.ai[1] % 0.1f).ToString());
                            Vector2 D = targetPos - Projectile.Center;
                            D.Normalize();

                            Vector2 ShootV = D * ShootVel;

                            ShootV = ShootV.RotatedByRandom(Math.PI / 12);

                            int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X + NewDirection * Projectile.Size.X * 0.2f, Projectile.Center.Y - Projectile.Size.Y * 0.2f, ShootV.X, ShootV.Y, Shoot, Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
                            Main.projectile[proj].timeLeft = 300;
                            Main.projectile[proj].netUpdate = true;
                        }

                        Projectile.ai[0] = 2f;
                    }

                    atacking = true;
                }
            }
            else if (TypeOfAttack != 0)
            {
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
                            targetDist = Vector2.Distance(npc.Center, Projectile.Center);
                            trgdir = npc.Center - Projectile.Center;
                            trgdir.Y = 0f;
                            trgdir.X = Math.Sign(trgdir.X);
                            targetPos = npc.Center - trgdir * (npc.width + Projectile.width) * 0.45f;
                            AUA = true;
                        }
                    }
                }

                if (AUA)
                {
                    Direction = targetPos - Projectile.Center;
                    Direction.Normalize();
                    NewDirection = Math.Sign(trgdir.X);

                    if (targetDist < Projectile.width)
                        atacking = true;
                }
            }

            /*************************
             * Check Player distance *
             *************************/
            if (Vector2.Distance(player.Center, Projectile.Center) > MaxDist)
            {
                Direction = player.Center - Projectile.Center;
                Direction.X -= 30f * player.direction;
                Direction.Y -= 30f;

                Direction.Normalize();

                AUA = atacking = false;
            }
            if (Vector2.Distance(player.Center, Projectile.Center) > maxPlayerDist + 239)
            {
                Projectile.position = player.Center - new Vector2(30f * player.direction, 30f);
                TargetDist = 0;
                NewVelocity = new Vector2(0, 0);
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
                    float temp = inertia * 0.5f;
                    NewVelocity = (Projectile.velocity * temp + Direction * chasePlayerSpeed) / (temp + 1);
                }
            }
            else
                NewVelocity *= 0.833333333f;

            if (NewVelocity.HasNaNs())
                NewVelocity = Projectile.velocity;  // This is bug fix ^_^

            if (Projectile.velocity.Length() > maxSpeed)
            {
                Projectile.velocity.Normalize();
                Projectile.velocity *= 9;
            }

            Projectile.direction = NewDirection;
            Projectile.spriteDirection = Projectile.direction;
            Projectile.velocity = NewVelocity;
            SelectFrame();
            CreateDust();
            Projectile.netUpdate = true;
        }

        public override void CheckActive()
        {
            Player player = Main.player[Projectile.owner];
            StandoPlayer pl = player.GetModPlayer<StandoPlayer>();

            if (pl.HaveStand)
                Projectile.timeLeft = 2;

            if (pl.StandJustSpawned)
            {
                pl.StandJustSpawned = false;
                return;
            }

            if (pl.StandSpawned && cool_jojo_stands.StandSummonHT.JustPressed || player.dead)
            {
                pl.StandSpawned = false;
                this.Projectile.Kill();
            }
        }

    }
}
