using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace cool_jojo_stands.Projectiles.Minions
{
    public class HierophantGreen : Minion
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

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 16;
            DisplayName.SetDefault("Hierophant Green");
            Main.projPet[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 98;
            projectile.height = 98;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 18000;
            projectile.tileCollide = false;
            projectile.melee = true;
            projectile.ignoreWater = true;
            projectile.alpha = 30;

            Shoot = mod.ProjectileType<EmeraldBlast>();
            ShootVel = 14f;
        }

        public void SelectFrame()
        {
            Player player = Main.player[projectile.owner];
            StandoPlayer pl = player.GetModPlayer<StandoPlayer>(mod);

            if (pl.StandKakyoinSetBonus == 1)
                TypeOfAttack = 1;
            else
                TypeOfAttack = 0;

            projectile.frameCounter++;

            if (atacking)
            {
                if (projectile.ai[1] > 0.01f)
                {
                    projectile.frameCounter = 0;
                    projectile.frame = 11;
                }
                else if (projectile.ai[0] > 1f / 3f)
                {
                    projectile.frameCounter = 0;
                    projectile.frame = 8;
                }
                else if (projectile.ai[0] > 0.5f / 3f)
                {
                    projectile.frameCounter = 0;
                    projectile.frame = 9;
                }
                else
                {
                    projectile.frameCounter = 0;
                    projectile.frame = 10;
                }
            }
            else if (projectile.frameCounter >= 10)
            {
                projectile.frameCounter = 0;
                projectile.frame = (projectile.frame + 1) % 8;
            }
        }

        public void CreateDust()
        {
            Player player = Main.player[projectile.owner];
            StandoPlayer pl = player.GetModPlayer<StandoPlayer>(mod);

            Lighting.AddLight(projectile.position + projectile.Size / 2, 0f, 0.7f, 0f);
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
            return !target.friendly && false;
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

            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override void Behavior()
        {
            Player player = Main.player[projectile.owner];
            const float PlayerStayDist = 30f;

            projectile.ai[0] -= 1 / 60f;
            projectile.ai[1] -= 1 / 60f;

            /****************
             * Chase Player *
             ****************/
            Vector2 Direction = player.Center - projectile.Center;
            Direction.X -= PlayerStayDist * player.direction;
            Direction.Y -= 20f;

            Direction.Normalize();

            float TargetDist = Vector2.Distance(player.Center - new Vector2(PlayerStayDist * player.direction, 20f), projectile.Center);
            int NewDirection = Main.player[projectile.owner].direction;
            Vector2 NewVelocity = projectile.velocity;

            if (TypeOfAttack != 0 || projectile.ai[0] < 0.01f)
            {
                AUA = false;
                atacking = false;
            }

            if (TypeOfAttack == 0 || true)
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

                        if ((distance < targetDist) && Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height))
                        {
                            targetDist = Vector2.Distance(npc.Center, projectile.Center);
                            trgdir = npc.Center - projectile.Center;
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
                        projectile.ai[1] = 0f;
                    }

                    NewDirection = Math.Sign(trgdir.X);

                    if (projectile.ai[0] < 0.01f || projectile.ai[1] > 0.01f)
                    {
                        if (projectile.ai[1] < 0.01f && projectile.ai[0] < 0.01f)
                        {
                            projectile.ai[1] = 2f;
                        }
                        else if (projectile.ai[1] % (1f / 20f) < 1f / 60f)
                        {
                            Random rand = new Random();

                            Vector2 ShootPos = projectile.Center + new Vector2(rand.Next(-10, 10), rand.Next(-10, 10)) / 10f;
                            Vector2 D = targetPos - ShootPos;
                            D.Normalize();

                            Vector2 ShootV = D * ShootVel;

                            int proj = Projectile.NewProjectile(ShootPos.X, ShootPos.Y, ShootV.X, ShootV.Y, Shoot, projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
                            Main.projectile[proj].timeLeft = 300;
                            Main.projectile[proj].netUpdate = true;
                        }

                        projectile.ai[0] = 0.5f;
                    }

                    atacking = true;
                }
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
            projectile.netUpdate = true;
        }

        public override void CheckActive()
        {
            Player player = Main.player[projectile.owner];
            StandoPlayer pl = player.GetModPlayer<StandoPlayer>(mod);

            if (pl.HaveStand)
                projectile.timeLeft = 239;

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

        public override void Some()
        { }
    }
}
