using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using cool_jojo_stands.NPCs;

namespace cool_jojo_stands.Projectiles.Minions
{
    public abstract class Stand : Minion
    {
        /* tech variables */
        protected Player player;
        protected StandoPlayer pl;
        protected float TargetDist, targetDist, // 0o0 Phew! yeah, its different variables
            manualControlNearEnemyDist;
        protected int NewDirection;
        protected Vector2 NewVelocity, trgDir, standPos, Direction, targetPos;
        protected float ReloadTime{
            get => projectile.ai[0];
            set => projectile.ai[0] = value;
        }
        protected float AttackTime{
            get => projectile.ai[1];
            set => projectile.ai[1] = value;
        }

        /* Stand parameters */
        protected Vector2 stayPlayerDist = new Vector2(30, 30); // Relative position for player

        protected float viewEnemyDist;        // View enemy distance (from stand to enemy)
        protected float chasePlayerSpeed;     // Standart chase player speed (normal speed)
        protected float maxSpeed;             // why not?
        protected float maxPlayerDist;        // Max player distance to chase with normal speed
        protected float inertia;              // Physycal variable
        protected float maxDist;              // Max player distance to attack enemy
        protected bool StandHaveTarget;       // Stand have target
        protected bool attacking;             // Stand attack target
        protected float AttackSpeed;          // Stand attack speed
        protected int lastAttack = 0;         // Last attack time
        protected float maxAttackTime = 2f;   // Max attacking stand time
        protected float maxReloadTime = 1.5f; // Time needs to reload stand attack
        public int Shoot;                     // Stand shoot projectile id
        public float ShootVel;                // Stand shoot velocity

        protected float SpeedRedusePower = 0.833333333f; // Speed reduce

        /* Hurt function */
        public void Hurt(PlayerDeathReason damageSource, int Damage, int hitDirection, bool pvp = false, bool quiet = false, bool Crit = false, int cooldownCounter = -1)
        {
            Main.player[projectile.owner].Hurt(damageSource, Damage, hitDirection, pvp, quiet, Crit, cooldownCounter);
        } /* End of 'Hurt' function */

        /* Check projectile function */
        public static bool CheckStando( Projectile proj ) =>
            proj.type == ModContent.ProjectileType<Projectiles.Minions.StarPlatinum>() ||
            proj.type == ModContent.ProjectileType<Projectiles.Minions.HierophantGreen>() ||
            proj.type == ModContent.ProjectileType<Projectiles.Minions.MagicianRed>() ||
            proj.type == ModContent.ProjectileType<Projectiles.Minions.StarPlatinumRequiem>() ||
            proj.type == ModContent.ProjectileType<Projectiles.HermitPurple>() ||
            proj.type == ModContent.ProjectileType<Projectiles.Dolphin>() ||
            proj.type == ModContent.ProjectileType<Projectiles.FireBlast>() ||
            proj.type == ModContent.ProjectileType<Projectiles.EmeraldBlast>();

        /* Set tech variables to default value */
        public virtual void BehavourStart()
        {
            player = Main.player[projectile.owner];
            pl = player.GetModPlayer<StandoPlayer>();
            StandHaveTarget = false;
            attacking = false;
            targetPos = Vector2.Zero;   // Target position
            targetDist = viewEnemyDist; // Max target distance or target distance if Stand have target
            standPos = Vector2.Zero;    // Target position
            trgDir = Vector2.Zero;      // Direction to target
            NewVelocity = projectile.velocity; // New stand velocity
            manualControlNearEnemyDist = 239f; // Near enemy distance if manual control mode on
        } /* End of 'BehavourStart' function */

        /* Chase owner function */
        public void ChasePlayer()
        {
            Direction = player.Center - projectile.Center;
            Direction.X -= stayPlayerDist.X * player.direction;
            Direction.Y -= stayPlayerDist.Y;

            Direction.Normalize();

            TargetDist = Vector2.Distance(player.Center - new Vector2(stayPlayerDist.X * player.direction, stayPlayerDist.Y), projectile.Center); // Player distance
            NewDirection = Main.player[projectile.owner].direction;
        } /* End of 'ChasePlayer' function */

        /* Manual control for standart near stand */
        public virtual void ManualControlNear()
        {
            StandHaveTarget = true;

            float DistToPlayer = Vector2.Distance(Main.MouseWorld, player.Center);
            trgDir = Main.MouseWorld - projectile.Center;

            if (DistToPlayer >= maxDist)
            {
                Vector2 trgDirection = Main.MouseWorld - player.Center;
                trgDirection.Normalize();
                standPos = player.Center + trgDirection * maxDist;
            }
            else
                standPos = Main.MouseWorld;

            targetDist = Vector2.Distance(standPos, projectile.Center);

            for (int k = 0; k < 200; k++)
            {
                NPC npc = Main.npc[k];
                bool targetDummy = npc.type == NPCID.TargetDummy;

                if (npc.CanBeChasedBy(this, false) || targetDummy)
                {
                    float distance = Vector2.Distance(npc.Center, projectile.Center);

                    if (distance < manualControlNearEnemyDist)
                        manualControlNearEnemyDist = distance;
                }
            }
        } /* End of 'ManualControlNear' function */

        /* Manual control for standart far stand */
        public virtual void ManualControlFar()
        {
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
        } /* End of 'ManualControlFar' function */

        /* Chase NPC for standart near stand */
        public virtual void ChaseNPCNear()
        {
            if (player.HasMinionAttackTargetNPC)
            {
                NPC npc = Main.npc[player.MinionAttackTargetNPC];

                targetDist = Vector2.Distance(npc.Center, projectile.Center);
                trgDir = npc.Center - projectile.Center;
                trgDir.Y = 0;
                trgDir.X = Math.Sign(trgDir.X);
                standPos = npc.Center - trgDir * (npc.Hitbox.Width + projectile.width) * 0.37f;
                StandHaveTarget = true;
            }

            for (int k = 0; k < 200; k++)
            {
                NPC npc = Main.npc[k];
                bool targetDummy = npc.type == NPCID.TargetDummy;

                if (npc.CanBeChasedBy(this, false) || targetDummy)
                {
                    float distance = Vector2.Distance(npc.Center, player.Center);

                    if (Main.mouseRight && distance < maxPlayerDist && npc.getRect().Contains((int)Main.MouseWorld.X, (int)Main.MouseWorld.Y))
                        player.MinionAttackTargetNPC = npc.whoAmI;

                    if (targetDummy && Vector2.Distance(projectile.Center, npc.Center) < projectile.width * 0.7)
                    {
                        attacking = true;
                        continue;
                    }

                    if (distance < targetDist && !player.HasMinionAttackTargetNPC && !targetDummy)
                    {
                        targetDist = Vector2.Distance(npc.Center, projectile.Center);
                        trgDir = npc.Center - projectile.Center;
                        trgDir.Y = 0;
                        trgDir.X = Math.Sign(trgDir.X);
                        standPos = npc.Center - trgDir * (npc.Hitbox.Width + projectile.width) * 0.37f;
                        StandHaveTarget = true;
                    }
                }
            }
        } /* End of 'ChaseNPCNear' function */

        /* Chase NPC for standart far stand */
        public virtual void ChaseNPCFar()
        {
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

                        trgDir.X = Math.Sign(trgDir.X);
                        targetPos = npc.Center;
                        StandHaveTarget = true;
                    }
                }
            }
        } /* End of 'ChaseNPCFar' function */

        /* Processing stand speed */
        public void SpeedProcessing()
        {
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

                    if (!StandHaveTarget && NewVelocity.LengthSquared() > TargetDist * TargetDist)
                    {
                        NewVelocity.Normalize();
                        NewVelocity *= TargetDist;
                    }
                }
            }
            else
                NewVelocity *= SpeedRedusePower;

            if (NewVelocity.HasNaNs())
                NewVelocity = projectile.velocity;  // This is bug fix ^_^

            if (projectile.velocity.Length() > maxSpeed) // why not?
            {
                projectile.velocity.Normalize();
                projectile.velocity *= maxSpeed;
            }

        } /* End of 'SpeedProcessing' function */

        /* Check distance to player */
        public void CheckPlayerDist()
        {
            float dist = Vector2.Distance(player.Center, projectile.Center);

            if (dist > maxDist)
            {
                Direction = player.Center - projectile.Center;
                Direction.X -= stayPlayerDist.X * player.direction;
                Direction.Y -= stayPlayerDist.Y;

                Direction.Normalize();

                StandHaveTarget = attacking = false;
            }
            if (dist > maxPlayerDist + 239)
            {
                projectile.position = player.Center - new Vector2(stayPlayerDist.X * player.direction, stayPlayerDist.Y);
                TargetDist = 0;
                NewVelocity = new Vector2(0, 0);
            }
        } /* End of 'CheckPlayerDist' function */

        /* Processing target for standart near stand */
        public virtual void TargetProcessingNear()
        {
            if (StandHaveTarget)
            {
                Direction = standPos - projectile.Center;
                Direction.Normalize();
                NewDirection = Math.Sign(trgDir.X);

                if (targetDist < projectile.width && !pl.StandManualControl)
                    attacking = true;
                else if (manualControlNearEnemyDist < projectile.width)
                    attacking = true;
            }
        } /* End of 'TargetProcessingNear' function */

        /* Processing target for standart far stand */
        public virtual void TargetProcessingFar()
        {
            if (StandHaveTarget)
            {
                if (targetPos == Vector2.Zero)
                {
                    AttackTime = 0f;

                    if (!pl.StandManualControl)
                        StandHaveTarget = false;
                }

                NewDirection = Math.Sign(trgDir.X);

                // TODO: redo
                if (ReloadTime < 0.01f || AttackTime > 0.01f)
                {
                    if (AttackTime < 0.01f && ReloadTime < 0.01f)
                        AttackTime = maxAttackTime;

                    ReloadTime = maxReloadTime;
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
        } /* End of 'TargetProcessingFar' function */

        /* Final of AI, set stand params */
        public void BehavourEnd()
        {
            projectile.direction = NewDirection;
            projectile.spriteDirection = projectile.direction;
            projectile.velocity = NewVelocity;
            SelectFrame();
            CreateDust();
            Some();
            projectile.netUpdate = true;
        } /* End of 'BehavourEnd' function */

        /* Damage npc function */
        public void CheckDamage()
        {
            if (lastAttack >= 60f / AttackSpeed)
            {
                Damage();
                lastAttack = 0;
            }
        } /* End of 'CheckDamage' function */

        public virtual void SelectFrame()
        {
        }

        public virtual void CreateDust()
        {
        }

        /* Processing stand damage function
         * Only for near stands */
        protected void Damage()
        {
            StandoPlayer pl = Main.player[projectile.owner].GetModPlayer<StandoPlayer>();
            Random rand = new Random((int)(Main.GlobalTime * 100));

            for (int k = 0; k < 200; k++)
            {
                NPC npc = Main.npc[k];

                if (attacking && npc.CanBeChasedBy(this, false) && !npc.friendly || npc.type == NPCID.TargetDummy)
                    if (projectile.Colliding(projectile.Hitbox, npc.Hitbox))
                    {
                        Vector2 Dir = npc.Center - projectile.Center;
                        bool crit = rand.Next(10) > 7;
                        /// TODO: Balance SP damage
                        int damage = (pl.StandLevel * pl.StandLevel) + pl.StandLevel;

                        int hp = npc.life;
                        int dmg = (int)npc.StrikeNPC(
                             damage,
                             projectile.knockBack,
                             Math.Sign(Dir.X),
                             crit);

                        npc.GetGlobalNPC<GlobalStandNPC>().damageFromPlayerStand[projectile.owner] += Math.Min(dmg, hp);

                        //projectile.StatusNPC(k);

                        // Network mode
                        if (Main.netMode != 0)
                            NetMessage.SendData(28, -1, -1, Terraria.Localization.NetworkText.FromLiteral(""), k, (float)damage, projectile.knockBack, Math.Sign(Dir.X), crit ? 1 : 0);

                        pl.player.addDPS(dmg);

                        if (npc.life <= 0 && !npc.SpawnedFromStatue && pl.StandLevel < StandoPlayer.MaxStandLevel)
                            pl.StandXP += npc.lifeMax * 0.3f;
                    }
            }
        } /* End of 'Damage' function */

        /* Stand activity function */
        public override void CheckActive()
        {
            Player player = Main.player[projectile.owner];
            StandoPlayer pl = player.GetModPlayer<StandoPlayer>();

            if (pl.HaveStand)
                projectile.timeLeft = 239;

            if (pl.StandJustSpawned)
            {
                pl.StandJustSpawned = false;
                return;
            }

            if (pl.StandSpawned && cool_jojo_stands.StandSummonHT.JustPressed || player.dead)
                projectile.Kill();
        } /* End of 'CheckActive' function */
    } /* End of 'Stand' class */
}
