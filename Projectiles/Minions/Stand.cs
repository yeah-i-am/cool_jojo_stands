using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using cool_jojo_stands.NPCs;
using static cool_jojo_stands.cool_jojo_stands;

namespace cool_jojo_stands.Projectiles.Minions
{
    public abstract class Stand : Minion
    {
        public virtual int StandDamage { get; protected set; }
        public virtual int StandSpeed { get; protected set; }


        /* tech variables */
        protected Player _player;
        protected StandoPlayer _standPlayer;
        protected float _targetDistance, _maxTargetDistance, // 0o0 Phew! yeah, its different variables
            _maxManualTargetDistance;
        protected int _newDirection;
        protected Vector2 _newStandDirection, _targetDirection, _standPosition, _standDirection, _targetPosition;
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
        protected float _maxSpeed;             // why not?
        protected float _maxPlayerDistance;        // Max player distance to chase with normal speed
        protected float _inertia;              // Physycal variable
        protected float _maxAgressiveDistance;              // Max player distance to attack enemy
        protected bool _havingTarget;       // Stand have target
        protected bool _attacking;             // Stand attack target
        protected float _attackSpeed;          // Stand attack speed
        protected int _lastAttack = 0;         // Last attack time
        protected float _maxAttackTime = 2f;   // Max attacking stand time
        protected float _maxReloadTime = 1.5f; // Time needs to reload stand attack
        protected float _speedReducePower = 0.833333333f; // Speed reduce

        public int shootId;                     // Stand shoot projectile id
        public float shootVelocity;                // Stand shoot velocity

        /* Hurt function */
        public void Hurt(PlayerDeathReason damageSource, int Damage, int hitDirection, bool pvp = false, bool quiet = false, bool Crit = false, int cooldownCounter = -1)
        {
            Main.player[projectile.owner].Hurt(damageSource, Damage, hitDirection, pvp, quiet, Crit, cooldownCounter);
        } /* End of 'Hurt' function */

        /* Check projectile function */
        public static bool CheckStando(Projectile proj) =>
            proj.modProjectile is Stand;

        /* Set tech variables to default value */
        public virtual void BehaviourStart()
        {
            _player = Main.player[projectile.owner];
            _standPlayer = _player.GetModPlayer<StandoPlayer>();
            _havingTarget = false;
            _attacking = false;
            _targetPosition = Vector2.Zero;   // Target position
            _maxTargetDistance = viewEnemyDist; // Max target distance or target distance if Stand have target
            _standPosition = Vector2.Zero;    // Target position
            _targetDirection = Vector2.Zero;      // Direction to target
            _newStandDirection = projectile.velocity; // New stand velocity
            _maxManualTargetDistance = 239f; // Near enemy distance if manual control mode on
        } /* End of 'BehavourStart' function */

        /* Chase owner function */
        public void ChasePlayer()
        {
            _standDirection = _player.Center - projectile.Center;
            _standDirection.X -= stayPlayerDist.X * _player.direction;
            _standDirection.Y -= stayPlayerDist.Y;

            _standDirection.Normalize();

            _targetDistance = Vector2.Distance(_player.Center - new Vector2(stayPlayerDist.X * _player.direction, stayPlayerDist.Y), projectile.Center); // Player distance
            _newDirection = Main.player[projectile.owner].direction;
        } /* End of 'ChasePlayer' function */

        /* Manual control for standart near stand */
        public virtual void ManualControlNear()
        {
            _havingTarget = true;

            float DistToPlayer = Vector2.Distance(Main.MouseWorld, _player.Center);
            _targetDirection = Main.MouseWorld - projectile.Center;

            if (DistToPlayer >= _maxAgressiveDistance)
            {
                Vector2 trgDirection = Main.MouseWorld - _player.Center;
                trgDirection.Normalize();
                _standPosition = _player.Center + trgDirection * _maxAgressiveDistance;
            }
            else
                _standPosition = Main.MouseWorld;

            _maxTargetDistance = Vector2.Distance(_standPosition, projectile.Center);

            for (int k = 0; k < 200; k++)
            {
                NPC npc = Main.npc[k];
                bool targetDummy = npc.type == NPCID.TargetDummy;

                if (npc.CanBeChasedBy(this, false) || targetDummy)
                {
                    float distance = Vector2.Distance(npc.Center, projectile.Center);

                    if (distance < _maxManualTargetDistance)
                        _maxManualTargetDistance = distance;
                }
            }
        } /* End of 'ManualControlNear' function */

        /* Manual control for standart far stand */
        public virtual void ManualControlFar()
        {
            _havingTarget = true;

            float DistToPlayer = Vector2.Distance(Main.MouseWorld, _player.Center);
            _targetDirection = Main.MouseWorld - projectile.Center;

            if (DistToPlayer >= _maxAgressiveDistance)
            {
                Vector2 trgDirection = Main.MouseWorld - _player.Center;
                trgDirection.Normalize();
                _standPosition = _player.Center + trgDirection * _maxAgressiveDistance;
            }
            else
                _standPosition = Main.MouseWorld;

            _maxTargetDistance = Vector2.Distance(_standPosition, projectile.Center);
        } /* End of 'ManualControlFar' function */

        /* Chase NPC for standart near stand */
        public virtual void ChaseNPCNear()
        {
            if (_player.HasMinionAttackTargetNPC)
            {
                NPC npc = Main.npc[_player.MinionAttackTargetNPC];

                _maxTargetDistance = Vector2.Distance(npc.Center, projectile.Center);
                _targetDirection = npc.Center - projectile.Center;
                _targetDirection.Y = 0;
                _targetDirection.X = Math.Sign(_targetDirection.X);
                _standPosition = npc.Center - _targetDirection * (npc.Hitbox.Width + projectile.width) * 0.37f;
                _havingTarget = true;
            }

            for (int k = 0; k < 200; k++)
            {
                NPC npc = Main.npc[k];
                bool targetDummy = npc.type == NPCID.TargetDummy;

                if (npc.CanBeChasedBy(this, false) || targetDummy)
                {
                    float distance = Vector2.Distance(npc.Center, _player.Center);

                    if (Main.mouseRight && distance < _maxPlayerDistance && npc.getRect().Contains((int)Main.MouseWorld.X, (int)Main.MouseWorld.Y))
                        _player.MinionAttackTargetNPC = npc.whoAmI;

                    if (targetDummy && Vector2.Distance(projectile.Center, npc.Center) < projectile.width * 0.7)
                    {
                        _attacking = true;
                        continue;
                    }

                    if (distance < _maxTargetDistance && !_player.HasMinionAttackTargetNPC && !targetDummy)
                    {
                        _maxTargetDistance = Vector2.Distance(npc.Center, projectile.Center);
                        _targetDirection = npc.Center - projectile.Center;
                        _targetDirection.Y = 0;
                        _targetDirection.X = Math.Sign(_targetDirection.X);
                        _standPosition = npc.Center - _targetDirection * (npc.Hitbox.Width + projectile.width) * 0.37f;
                        _havingTarget = true;
                    }
                }
            }
        } /* End of 'ChaseNPCNear' function */

        /* Chase NPC for standart far stand */
        public virtual void ChaseNPCFar()
        {
            if (_player.HasMinionAttackTargetNPC)
            {
                NPC npc = Main.npc[_player.MinionAttackTargetNPC];

                _maxTargetDistance = Vector2.Distance(npc.Center, projectile.Center);
                _targetDirection = npc.Center - projectile.Center;
                _targetDirection.Y = 0;
                _targetDirection.X = Math.Sign(_targetDirection.X);
                _targetPosition = npc.Center;
                _havingTarget = true;
            }

            for (int k = 0; k < 200; k++)
            {
                NPC npc = Main.npc[k];

                if (npc.CanBeChasedBy(this, false))
                {
                    float distance = Vector2.Distance(npc.Center, _player.Center);

                    if (Main.mouseRight && distance < viewEnemyDist && npc.getRect().Contains((int)Main.MouseWorld.X, (int)Main.MouseWorld.Y))
                        _player.MinionAttackTargetNPC = npc.whoAmI;

                    if (!_player.HasMinionAttackTargetNPC && (distance < _maxTargetDistance) &&
                        Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height))
                    {
                        _maxTargetDistance = Vector2.Distance(npc.Center, projectile.Center);
                        _targetDirection = npc.Center - projectile.Center;

                        _targetDirection.X = Math.Sign(_targetDirection.X);
                        _targetPosition = npc.Center;
                        _havingTarget = true;
                    }
                }
            }
        } /* End of 'ChaseNPCFar' function */

        /* Processing stand speed */
        public void SpeedProcessing()
        {
            if (_targetDistance > 1f || _havingTarget)
            {
                if (Vector2.Distance(_player.Center, projectile.Center) > _maxPlayerDistance)
                {
                    _newStandDirection = _standDirection * Math.Max(_player.velocity.Length(), 5f);
                }
                else if (_standPlayer.StandManualControl && _maxTargetDistance < 23.9f)
                {
                    _newStandDirection *= (float)Math.Pow(_maxTargetDistance / 23.9f, 0.39f);
                }
                else
                {
                    float temp = _inertia * 0.5f;
                    _newStandDirection = (projectile.velocity * temp + _standDirection * chasePlayerSpeed) / (temp + 1);

                    if (!_havingTarget && _newStandDirection.LengthSquared() > _targetDistance * _targetDistance)
                    {
                        _newStandDirection.Normalize();
                        _newStandDirection *= _targetDistance;
                    }
                }
            }
            else
                _newStandDirection *= _speedReducePower;

            if (_newStandDirection.HasNaNs())
                _newStandDirection = projectile.velocity;  // This is bug fix ^_^

            if (projectile.velocity.Length() > _maxSpeed) // why not?
            {
                projectile.velocity.Normalize();
                projectile.velocity *= _maxSpeed;
            }

        } /* End of 'SpeedProcessing' function */

        /* Check distance to player */
        public void CheckPlayerDist()
        {
            float dist = Vector2.Distance(_player.Center, projectile.Center);

            if (dist > _maxAgressiveDistance)
            {
                _standDirection = _player.Center - projectile.Center;
                _standDirection.X -= stayPlayerDist.X * _player.direction;
                _standDirection.Y -= stayPlayerDist.Y;

                _standDirection.Normalize();

                _havingTarget = _attacking = false;
            }
            if (dist > _maxPlayerDistance + 239)
            {
                projectile.position = _player.Center - new Vector2(stayPlayerDist.X * _player.direction, stayPlayerDist.Y);
                _targetDistance = 0;
                _newStandDirection = new Vector2(0, 0);
            }
        } /* End of 'CheckPlayerDist' function */

        /* Processing target for standart near stand */
        public virtual void TargetProcessingNear()
        {
            if (_havingTarget)
            {
                _standDirection = _standPosition - projectile.Center;
                _standDirection.Normalize();
                _newDirection = Math.Sign(_targetDirection.X);

                if (_maxTargetDistance < projectile.width && !_standPlayer.StandManualControl)
                    _attacking = true;
                else if (_maxManualTargetDistance < projectile.width)
                    _attacking = true;
            }
        } /* End of 'TargetProcessingNear' function */

        /* Processing target for standart far stand */
        public virtual void TargetProcessingFar()
        {
            if (_havingTarget)
            {
                if (_targetPosition == Vector2.Zero)
                {
                    AttackTime = 0f;

                    if (!_standPlayer.StandManualControl)
                        _havingTarget = false;
                }

                _newDirection = Math.Sign(_targetDirection.X);

                // TODO: redo
                if (ReloadTime < 0.01f || AttackTime > 0.01f)
                {
                    if (AttackTime < 0.01f && ReloadTime < 0.01f)
                        AttackTime = _maxAttackTime;

                    ReloadTime = _maxReloadTime;
                }

                if (_standPlayer.StandManualControl)
                {
                    if (Main.mouseRight)
                        _standDirection = new Vector2(0, 0);
                    else
                    {
                        _standDirection = _standPosition - projectile.Center;
                        _standDirection.Normalize();
                    }

                    _newDirection = Math.Sign(_targetDirection.X);
                }
                else
                    _attacking = true;
            }
        } /* End of 'TargetProcessingFar' function */

        /* Final of AI, set stand params */
        public void BehaviourEnd()
        {
            projectile.direction = _newDirection;
            projectile.spriteDirection = projectile.direction;
            projectile.velocity = _newStandDirection;
            SelectFrame();
            CreateDust();
            PostUpdate();

            if (projectile.owner == Main.myPlayer)
            {
                if (projectile.direction != Main.LocalPlayer.direction)
                {
                    Main.LocalPlayer.direction = projectile.direction;
                    NetMessage.SendData(13, -1, -1, null, Main.myPlayer);
                }
                if (cool_jojo_stands.ManualingPlayers[Main.myPlayer])
                {
                    cool_jojo_stands.StandsPositions[projectile.owner] = projectile.position;
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        ModPacket packet = cool_jojo_stands.mod.GetPacket();

                        packet.Write((byte)StandMessageType.StandPosition);
                        packet.Write(_player.whoAmI);
                        packet.Write(projectile.position.X);
                        packet.Write(projectile.position.Y);

                        packet.Send();
                    }
                }
            }
            else if (projectile.owner != Main.myPlayer && cool_jojo_stands.ManualingPlayers[projectile.owner])
            {
                projectile.position = cool_jojo_stands.StandsPositions[projectile.owner];
            }
            projectile.netUpdate = true;
        } /* End of 'BehaviourEnd' function */

        /* Damage npc function */
        public void CheckDamage()
        {
            if (_lastAttack >= 60f / _attackSpeed)
            {
                if (!Main.player[projectile.owner].GetModPlayer<StandoPlayer>().DisabledAttackings && ((cool_jojo_stands.ManualingPlayers[projectile.owner] && cool_jojo_stands.AttackingPlayers[projectile.owner]) || !cool_jojo_stands.ManualingPlayers[projectile.owner]))
                Damage();
                _lastAttack = 0;
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

                if (_attacking && npc.CanBeChasedBy(this, false) && !npc.friendly || npc.type == NPCID.TargetDummy)
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
                        if (Main.netMode != NetmodeID.SinglePlayer)
                            NetMessage.SendData(MessageID.StrikeNPC, -1, -1, Terraria.Localization.NetworkText.FromLiteral(""), k, (float)damage, projectile.knockBack, Math.Sign(Dir.X), crit ? 1 : 0);

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
