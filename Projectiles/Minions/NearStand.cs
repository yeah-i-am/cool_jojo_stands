using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace cool_jojo_stands.Projectiles.Minions
{
    public abstract class NearStand : Stand
    {
        static int[] ImmuneStands = new int[200];

        public NearStand()
        {
            viewEnemyDist = 300f;  // View enemy distance (from stand to enemy)
            chasePlayerSpeed = 9f; // Standart chase player speed (normal speed)
            _maxSpeed = 10000f;     // ... i don't use it
            _maxPlayerDistance = 400f;  // Max player distance to chase with normal speed
            _inertia = 13f;         // Physycal variable
            _maxAgressiveDistance = 280f;        // Max player distance to attack enemy
            _attackSpeed = 10;      // Stand attack speed
            _speedReducePower = 0.7f;
        }

        public override void Behaviour()
        {
            BehaviourStart();

            _lastAttack++;

            ChasePlayer();

            if (_standPlayer.StandManualControl)
                ManualControlNear();
            else
                ChaseNPCNear();

            TargetProcessingNear();
            CheckPlayerDist();

            SpeedProcessing();
            BehaviourEnd();

            if (_standPlayer.StandJotaroSetBonus > 0)
                _attackSpeed = 20;
            else
                _attackSpeed = 10;

            CheckDamage();
        }

        /*public override bool? CanHitNPC( NPC target )
        {
            if ((lastAttack >= 60f / AttackSpeed || lastAttack == 0) && !target.friendly && attacking)
            {
                lastAttack = 0;
                immune[target.whoAmI] = target.immune[projectile.owner];
                return true;
            }

            return false;
        }

        public override void OnHitNPC( NPC target, int damage, float knockback, bool crit )
        {
            target.immune[projectile.owner] = immune[target.whoAmI];
        }*/

        public override void PostUpdate()
        {
        }
    } /* End of 'NearStand' class */
}