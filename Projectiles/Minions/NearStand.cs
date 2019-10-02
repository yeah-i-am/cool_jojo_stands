using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace cool_jojo_stands.Projectiles.Minions
{
    public abstract class NearStand : Minion
    {
        protected float viewEnemyDist = 300f;  // View enemy distance (from stand to enemy)
        protected float chasePlayerSpeed = 6f; // Standart chase player speed (normal speed)
        protected float maxSpeed = 10000f;     // ... i don't use it
        protected float maxPlayerDist = 400f;  // Max player distance to chase with normal speed
        protected float inertia = 20f;         // Physycal variable
        protected float maxDist = 380f;        // Max player distance to attack enemy
        protected bool StandHaveTarget;        // Stand have target
        protected bool attacking;              // Stand attack target
        protected float AttackSpeed = 10;      // Stand attack speed
        private int lastAttack = 0;            // Last attack time

        public virtual void SelectFrame()
        {
        }

        public virtual void CreateDust()
        {
        }

        public override void Behavior()
        {
            Player player = Main.player[projectile.owner];
            StandoPlayer pl = player.GetModPlayer<StandoPlayer>(mod);
            lastAttack++;

            /****************
             * Chase Player *
             ****************/
            Vector2 Direction = player.Center - projectile.Center;
            Direction.X -= 30f * player.direction;
            Direction.Y -= 30f;

            Direction.Normalize();

            float TargetDist = Vector2.Distance(player.Center - new Vector2(30f * player.direction, 30f), projectile.Center); // Player distance
            int NewDirection = Main.player[projectile.owner].direction; // New stand direction
            Vector2 NewVelocity = projectile.velocity; // New stand velocity

            float manualControlNearEnemyDist = 239f; // Near enemy distance if manual control mode on
            float targetDist = viewEnemyDist; // Max target distance or target distance if Stand have target
            Vector2 standPos = new Vector2(0f, 0f); // Target position
            Vector2 trgDir = new Vector2(0, 0); // Direction to target

            StandHaveTarget = false;
            attacking = false;

            if (pl.StandManualControl)
            {
                /******************
                 * Manual control *
                 ******************/
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
            }
            else
            {
                /*************
                 * Chase NPC *
                 *************/
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
            }

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
                NewVelocity *= 0.833333333f;

            if (NewVelocity.HasNaNs())
                NewVelocity = projectile.velocity;  // This is bug fix ^_^

            if (projectile.velocity.Length() > maxSpeed) // why not?
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
            if (lastAttack >= 60 / AttackSpeed)
            {
                Damage();
                lastAttack = 0;
            }
        }

        public override void Some()
        {
        }

        /* Processing stand damage function */
        private void Damage()
        {
            StandoPlayer pl = Main.player[projectile.owner].GetModPlayer<StandoPlayer>(mod);
            Random rand = new Random((int)(Main.GlobalTime * 100));

            if (pl.StandJotaroSetBonus > 0)
                AttackSpeed = 20;
            else
                AttackSpeed = 10;

            for (int k = 0; k < 200; k++)
            {
                NPC npc = Main.npc[k];

                if (attacking && npc.CanBeChasedBy(this, false) && !npc.friendly || npc.type == NPCID.TargetDummy)
                    if (projectile.Colliding(projectile.Hitbox, npc.Hitbox))
                    {
                        Vector2 Dir = npc.Center - projectile.Center;
                        bool crit = rand.Next(10) > 7;
                        int damage = (pl.StandLevel * pl.StandLevel) + pl.StandLevel + 1;

                        npc.StrikeNPC(
                             damage,
                             projectile.knockBack,
                             Math.Sign(Dir.X),
                             crit);

                        //projectile.StatusNPC(k);

                        // Network mode
                        if (Main.netMode != 0)
                            NetMessage.SendData(28, -1, -1, Terraria.Localization.NetworkText.FromLiteral(""), k, (float)damage, projectile.knockBack, Math.Sign(Dir.X), crit ? 1 : 0);

                        pl.player.addDPS(damage);

                        if (npc.life <= 0 && !npc.SpawnedFromStatue && pl.StandLevel < StandoPlayer.MaxStandLevel)
                            pl.StandXP += npc.lifeMax * 0.3f;
                    }
            }
        }

        /* Stand activity function */
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
    } /* End of 'NearStand' class */
}