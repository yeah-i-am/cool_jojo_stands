using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace cool_jojo_stands.Projectiles.Minions
{
    public class SilverChariotGhost : Stand
    {
        static int[] targets = new int[10];
        //int id;

        Vector2[] armShift = new Vector2[10];
        float[] armPhase = new float[10];
        float
            armSpeed = 35f,
            armLenght = 20f;

        public SilverChariotGhost()
        {
            viewEnemyDist = 300f;  // View enemy distance (from stand to enemy)
            chasePlayerSpeed = 6f; // Standart chase player speed (normal speed)
            maxSpeed = 10000f;     // ... i don't use it
            maxPlayerDist = 400f;  // Max player distance to chase with normal speed
            inertia = 20f;         // Physycal variable
            maxDist = 380f;        // Max player distance to attack enemy
            AttackSpeed = 10;      // Stand attack speed

            if (Main.rand != null)
                for (int i = 0; i < 10; i++)
                {
                    armPhase[i] = Main.rand.NextFloat(MathHelper.TwoPi);
                    armShift[i] = new Vector2(Main.rand.NextFloat(-15f, 5f), Main.rand.NextFloat(-15f, 15f));
                    targets[i] = -1;
                }
        }

        public override void Behavior()
        {
            BehavourStart();

            lastAttack++;

            ChasePlayer();

            if (pl.StandManualControl)
                ManualControlNear();
            else
                ChaseNPCNear();

            TargetProcessingNear();
            CheckPlayerDist();

            SpeedProcessing();
            BehavourEnd();

            if (pl.StandPornoleffSetBonus > 0)
                AttackSpeed = 20;
            else
                AttackSpeed = 10;

            CheckDamage();
        }

        /*****************
         * Some settings *
         *****************/
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 15;
            DisplayName.SetDefault("Silver Chariot");
            Main.projPet[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 92;
            projectile.height = 92;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 239;
            projectile.tileCollide = false;
            projectile.melee = true;
            projectile.ignoreWater = true;
            projectile.alpha = 30;
        }

        public override bool CanDamage() => false;
        public override bool? CanCutTiles() => false;
        public override bool MinionContactDamage() => false;

        /* Select animation frame function */
        public override void SelectFrame()
        {
            StandoPlayer pl = Main.player[projectile.owner].GetModPlayer<StandoPlayer>();

            projectile.frameCounter++;

            if (attacking)
            {
                if (pl.StandJotaroSetBonus > 0)
                {
                    if (projectile.frameCounter >= 3)
                    {
                        projectile.frame = 12 + (projectile.frame + 1) % 3;
                        projectile.frameCounter = 0;
                    }

                }
                else if (projectile.frameCounter >= 5 && projectile.frame % 2 == 1 || projectile.frameCounter >= 2 && projectile.frame % 2 == 0)
                {
                    projectile.frameCounter = 0;
                    projectile.frame = 8 + (projectile.frame + 1) % 4;
                }
            }
            else if (projectile.frameCounter >= 10)
            {
                projectile.frameCounter = 0;
                projectile.frame = (projectile.frame + 1) % 4;
            }
        }

        /* Predraw function */
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (pl.StandPornoleffSetBonus == 0 && !attacking && false)
                return true;

            Vector2 direction = targetPos - projectile.position;
            direction.Normalize();

            //////////////////
            direction = new Vector2(1, 0);

            Texture2D texture = ModContent.GetTexture("cool_jojo_stands/Projectiles/Minions/SilverChariot_Arm");
            Vector2 position;
            Microsoft.Xna.Framework.Rectangle? sourceRectangle = new Microsoft.Xna.Framework.Rectangle?();
            Vector2 origin = Vector2.Zero;
            float rotation = 0;
            lightColor = projectile.GetAlpha(lightColor);

            for (int i = 0; i < 10; i++)
            {
                float si = (float)Math.Sin(armPhase[i]);

                if (si < -0.9)
                {
                    armShift[i] = new Vector2(Main.rand.NextFloat(-15f, 5f), Main.rand.NextFloat(-15f, 15f));
                    armPhase[i] -= Main.rand.NextFloat(0f, 0.8f) * armSpeed / 60f;
                }

                float[] oldPhase = new float[3] {
                    armPhase[i] - armSpeed / 180f,
                    armPhase[i] - armSpeed / 120f,
                    armPhase[i] - armSpeed / 60f
                };

                Vector2[] oldPos = new Vector2[3];

                armPhase[i] += armSpeed / 60f;

                if (projectile.spriteDirection == -1)
                {
                    position = projectile.Center + armShift[i] - armLenght * direction * si;
                    for (int l = 0; l < 3; l++)
                        oldPos[l] = projectile.Center - armLenght * direction * (float)Math.Sin(oldPhase[l]);
                    ///rotation = attackAngle - (float)Math.PI * Math.Sign(attackAngle);
                    ///rotation = (rotation > invMaxAttackAngle) ? invMaxAttackAngle :
                    ///((rotation < invMinAttackAngle) ? invMinAttackAngle : rotation);
                }
                else
                {
                    position = projectile.Center + armShift[i] + armLenght * direction * si - new Vector2(armLenght * 0.8f, 0);

                    for (int l = 0; l < 3; l++)
                        oldPos[l] = projectile.Center + armShift[i] + armLenght * direction * (float)Math.Sin(oldPhase[l]) - new Vector2(armLenght * 0.8f, 0);
                    ///rotation = (attackAngle > maxAttackAngle) ? maxAttackAngle :
                    ///((attackAngle < minAttackAngle) ? minAttackAngle : attackAngle);

                }
                Color last = lightColor;

                if (si < -0.6f)
                    si = -1f;

                lightColor = lightColor * ((1f + si) * 0.1f);

                Main.spriteBatch.Draw(
                    texture,
                    position - Main.screenPosition,
                    sourceRectangle,
                    lightColor,
                    rotation,
                    origin,
                    1f,
                    (projectile.spriteDirection == -1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                    0.0f);

                for (int l = 0; l < 3; l++)
                    Main.spriteBatch.Draw(
                        texture,
                        oldPos[l] - Main.screenPosition,
                        sourceRectangle,
                        lightColor * 0.6f * (1 - l * 0.25f),
                        rotation,
                        origin,
                        1f,
                        (projectile.spriteDirection == -1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                        0.0f);

                lightColor = last;
            }

            return true;
        }

        /* Dust function */
        public override void CreateDust()
        {
            Lighting.AddLight(projectile.Center, 0.7f, 0.7f, 0.7f);
        }

        /* Stand kill function */
        public override void Kill(int timeLeft)
        {
            Player player = Main.player[projectile.owner];
            StandoPlayer pl = player.GetModPlayer<StandoPlayer>();
            pl.StandSpawned = false;
        }
    }
}
