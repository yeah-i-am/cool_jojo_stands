﻿using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace cool_jojo_stands.Projectiles.Minions
{
    public class HierophantGreen : Stand
    {
        protected int   TypeOfAttack = 0;

        public HierophantGreen()
        {
            viewEnemyDist = 900f;  // View enemy distance (from stand to enemy)
            chasePlayerSpeed = 6f; // Standart chase player speed (normal speed)
            maxSpeed = 10000f;     // ... i don't use it
            maxPlayerDist = 330f;  // Max player distance to chase with normal speed
            inertia = 20f;         // Physycal variable
            maxDist = 300f;        // Max player distance to attack enemy
            ShootVel = 14f;        // Stand shoot velocity
            stayPlayerDist = new Vector2(30, 20);
        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 16;
            DisplayName.SetDefault("Hierophant Green");
            Main.projPet[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 98;
            Projectile.height = 98;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 18000;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.alpha = 255;

            Shoot = ModContent.ProjectileType<EmeraldBlast>();
        }

        public override void SelectFrame()
        {
            if (Projectile.alpha > 30)
                Projectile.alpha -= 8;

            Player player = Main.player[Projectile.owner];
            StandoPlayer pl = player.GetModPlayer<StandoPlayer>();

            if (pl.StandKakyoinSetBonus == 1)
                TypeOfAttack = 1;
            else
                TypeOfAttack = 0;

            Projectile.frameCounter++;

            if (attacking)
            {
                if (AttackTime > 0.01f)
                {
                    Projectile.frameCounter = 0;
                    Projectile.frame = 11;
                }
                else if (ReloadTime >= 1.25f || (ReloadTime >= 0.5 && ReloadTime < 0.75))
                {
                    Projectile.frameCounter = 0;
                    Projectile.frame = 8;
                }
                else if (ReloadTime >= 1.0f || (ReloadTime >= 0.25 && ReloadTime < 0.5))
                {
                    Projectile.frameCounter = 0;
                    Projectile.frame = 9;
                }
                else 
                {
                    Projectile.frameCounter = 0;
                    Projectile.frame = 10;
                }
            }
            else if (Projectile.frameCounter >= 10)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 8;
            }
        }

        public override void CreateDust()
        {
            Lighting.AddLight(Projectile.Center, 0f, 0.7f, 0f);
        }

        public override void Behavior()
        {
            BehavourStart();

            ReloadTime -= 1 / 60f;
            AttackTime -= 1 / 60f;

            ChasePlayer();

            if (pl.StandManualControl)
                ManualControlFar();
            else
                ChaseNPCFar();

            TargetProcessingFar();
            CheckPlayerDist();

            if (attacking && AttackTime > 0 && AttackTime % (1f / 20f) < 1f / 60f)
            {
                Vector2 ShootPos = Projectile.Center + new Vector2(Main.rand.Next(-10, 10), Main.rand.Next(-10, 10)) / 10f;
                Vector2 D = targetPos - ShootPos;
                D.Normalize();

                Vector2 ShootV = D * ShootVel;

                Projectile.NewProjectile(Projectile.GetSource_FromThis(), ShootPos.X, ShootPos.Y, ShootV.X, ShootV.Y, Shoot, Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
            }

            SpeedProcessing();
            BehavourEnd();
        }

        public override void Kill(int timeLeft)
        {
            StandoPlayer pl = Main.player[Projectile.owner].GetModPlayer<StandoPlayer>();
            pl.StandSpawned = false;
        }
    } /* End of 'HierophantGreen' class */
}
