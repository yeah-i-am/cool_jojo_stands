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
        static int[] immune = new int[200];
        int lastDamage = 0;

        public NearStand()
        {
            viewEnemyDist = 300f;  // View enemy distance (from stand to enemy)
            chasePlayerSpeed = 9f; // Standart chase player speed (normal speed)
            maxSpeed = 10000f;     // ... i don't use it
            maxPlayerDist = 400f;  // Max player distance to chase with normal speed
            inertia = 13f;         // Physycal variable
            maxDist = 280f;        // Max player distance to attack enemy
            AttackSpeed = 10;      // Stand attack speed
            SpeedRedusePower = 0.7f;
        }

        public override void Behavior()
        {
            BehavourStart();

            lastAttack++;
            lastDamage++;

            ChasePlayer();

            if (pl.StandManualControl)
                ManualControlNear();
            else
                ChaseNPCNear();

            TargetProcessingNear();
            CheckPlayerDist();

            SpeedProcessing();
            BehavourEnd();

            if (pl.StandJotaroSetBonus > 0)
                AttackSpeed = 20;
            else
                AttackSpeed = 10;

            CheckDamage();
        }

        /*public override bool? CanHitNPC( NPC target )
        {
            if (lastAttack >= 60f / AttackSpeed && !target.friendly && attacking)
            {
                StandoPlayer.Talk("a: " + lastAttack);
                lastAttack = 0;
                //immune[target.whoAmI] = target.immune[projectile.owner];
                //target.immune[projectile.owner] = 0;
                return true;
            }

            return false;
        }

        public override void OnHitNPC( NPC target, int damage, float knockback, bool crit )
        {
            //target.immune[projectile.owner] = immune[target.whoAmI];
            StandoPlayer.Talk("d: " + lastDamage.ToString());
            lastDamage = 0;
        } */

        public override void Some()
        {
        }
    } /* End of 'NearStand' class */
}