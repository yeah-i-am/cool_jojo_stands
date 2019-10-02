using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace cool_jojo_stands.Projectiles.Minions
{
    public class HierophantGreen : Minion
    {
        protected float viewEnemyDist = 900f;  // View enemy distance (from stand to enemy)
        protected float chasePlayerSpeed = 6f; // Standart chase player speed (normal speed)
        protected float maxSpeed = 10000f;     // ... i don't use it
        protected float maxPlayerDist = 330f;  // Max player distance to chase with normal speed
        protected float inertia = 20f;         // Physycal variable
        protected float maxDist = 300f;        // Max player distance to attack enemy
        protected bool StandHaveTarget;        // Stand have target
        protected bool attacking;              // Stand attack target
        public int Shoot;                      // Stand shoot projectile id
        public float ShootVel = 14f;           // Stand shoot velocity
        protected int TypeOfAttack = 0;

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 16;
            DisplayName.SetDefault("Hierophant Green");
            Main.projPet[projectile.type] = true;
            ///ProjectileID.Sets.Homing[projectile.type] = true;
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

            if (attacking)
            {
                if (projectile.ai[1] > 0.01f)
                {
                    projectile.frameCounter = 0;
                    projectile.frame = 11;
                }
                else if (projectile.ai[0] >= 1.25f || (projectile.ai[0] >= 0.5 && projectile.ai[0] < 0.75))
                {
                    projectile.frameCounter = 0;
                    projectile.frame = 8;
                }
                else if (projectile.ai[0] >= 1.0f || (projectile.ai[0] >= 0.25 && projectile.ai[0] < 0.5))
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
            Lighting.AddLight(projectile.Center, 0f, 0.7f, 0f);
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
            StandoPlayer pl = player.GetModPlayer<StandoPlayer>(mod);

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

            StandHaveTarget = false;
            attacking = false;

            float targetDist = viewEnemyDist; // Max target distance or target distance if Stand have target
            Vector2 targetPos = new Vector2(0f, 0f); // Target position
            Vector2 standPos = new Vector2(0f, 0f); // Target Stand position
            Vector2 trgDir = new Vector2(0, 0); // Direction to target

            if (pl.StandManualControl)
            {
                /******************
                 * Manual control *
                 ******************/
                StandHaveTarget = true;

                float DistToPlayer = Vector2.Distance(Main.MouseWorld, player.Center);
                trgDir = Main.MouseWorld - projectile.Center;

                if (Main.mouseRight)
                {
                    targetPos = Main.MouseWorld;
                    standPos = projectile.position;
                    attacking = true;
                }
                else if (DistToPlayer >= maxDist)
                {
                    Vector2 trgDirection = Main.MouseWorld - player.Center;
                    trgDirection.Normalize();
                    standPos = player.Center + trgDirection * maxDist;
                }
                else
                    standPos = Main.MouseWorld;

                targetDist = Vector2.Distance(standPos, projectile.Center);
            }
            else
            {
                /****************
                 * Shoot to NPC *
                 ****************/
                if (player.HasMinionAttackTargetNPC)
                {
                    NPC npc = Main.npc[player.MinionAttackTargetNPC];

                    targetDist = Vector2.Distance(npc.Center, projectile.Center);
                    trgDir = npc.Center - projectile.Center;
                    trgDir.Y = 0;
                    trgDir.X = Math.Sign(trgDir.X);
                    targetPos = npc.Center;
                    StandHaveTarget = true;
                }

                for (int k = 0; k < 200; k++)
                {
                    NPC npc = Main.npc[k];

                    if (npc.CanBeChasedBy(this, false))
                    {
                        float distance = Vector2.Distance(npc.Center, player.Center);

                        if (Main.mouseRight && distance < viewEnemyDist && npc.getRect().Contains((int)Main.MouseWorld.X, (int)Main.MouseWorld.Y))
                            player.MinionAttackTargetNPC = npc.whoAmI;

                        if (!player.HasMinionAttackTargetNPC && (distance < targetDist) &&
                            Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height))
                        {
                            targetDist = Vector2.Distance(npc.Center, projectile.Center);
                            trgDir = npc.Center - projectile.Center;

                            if (trgDir == null)
                                StandoPlayer.Talk("First exception");

                            if (npc.Center == null)
                                StandoPlayer.Talk("Second exception");

                            trgDir.X = Math.Sign(trgDir.X);
                            targetPos = npc.Center;
                            StandHaveTarget = true;
                        }
                    }
                }
            }

            if (StandHaveTarget)
            {
                if (targetPos == new Vector2(0f, 0f))
                {
                    projectile.ai[1] = 0f;

                    if (!pl.StandManualControl)
                        StandHaveTarget = false;
                }

                NewDirection = Math.Sign(trgDir.X);

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

                    projectile.ai[0] = 1.5f;
                }

                if (pl.StandManualControl)
                {
                    if (Main.mouseRight)
                        Direction = new Vector2(0, 0);
                    else
                    {
                        Direction = standPos - projectile.Center;
                        Direction.Normalize();
                    }

                    NewDirection = Math.Sign(trgDir.X);
                }
                else
                    attacking = true;

            }

            /*************************
             * Check Player distance *
             *************************/
            if (Vector2.Distance(player.Center, projectile.Center) > maxDist)
            {
                Direction = player.Center - projectile.Center;
                Direction.X -= 30f * player.direction;
                Direction.Y -= 30f;

                Direction.Normalize();

                StandHaveTarget = attacking = false;
            }
            if (Vector2.Distance(player.Center, projectile.Center) > maxPlayerDist + 239)
            {
                projectile.position = player.Center - new Vector2(30f * player.direction, 30f);
                TargetDist = 0;
                NewVelocity = new Vector2(0, 0);
            }

            /***************************
             * Processing speed(wagon) *
             ***************************/
            if (TargetDist > 1f || StandHaveTarget)
            {
                if (Vector2.Distance(player.Center, projectile.Center) > maxPlayerDist)
                {
                    NewVelocity = Direction * Math.Max(player.velocity.Length(), 5f);
                }
                else if (pl.StandManualControl && targetDist < 23.9f)
                {
                    NewVelocity *= (float)Math.Pow(targetDist / 23.9f, 0.39f);
                }
                else
                {
                    float temp = inertia * 0.5f;
                    NewVelocity = (projectile.velocity * temp + Direction * chasePlayerSpeed) / (temp + 1);
                }
            }
            else
                NewVelocity *= 0.8333333f;

            if (NewVelocity.HasNaNs())
                NewVelocity = projectile.velocity;  // This is bug fix ^_^

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
                projectile.Kill();
        }

        public override void Kill(int timeLeft)
        {
            StandoPlayer pl = Main.player[projectile.owner].GetModPlayer<StandoPlayer>(mod);
            pl.StandSpawned = false;
        }

        public override void Some()
        { }
    }
}
