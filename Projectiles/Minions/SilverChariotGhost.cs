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
        static int[] _targets = new int[10];
        //int id;

        Vector2[] _armShift = new Vector2[10];
        float[] _armPhase = new float[10];
        float
            _armSpeed = 35f,
            _armLength = 20f;

        public SilverChariotGhost()
        {
            viewEnemyDist = 300f;  // View enemy distance (from stand to enemy)
            chasePlayerSpeed = 6f; // Standart chase player speed (normal speed)
            _maxSpeed = 10000f;     // ... i don't use it
            _maxPlayerDistance = 400f;  // Max player distance to chase with normal speed
            _inertia = 20f;         // Physycal variable
            _maxAgressiveDistance = 380f;        // Max player distance to attack enemy
            _attackSpeed = 10;      // Stand attack speed

            if (Main.rand != null)
                for (int i = 0; i < 10; i++)
                {
                    _armPhase[i] = Main.rand.NextFloat(MathHelper.TwoPi);
                    _armShift[i] = new Vector2(Main.rand.NextFloat(-15f, 5f), Main.rand.NextFloat(-15f, 15f));
                    _targets[i] = -1;
                }
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

            if (_standPlayer.StandPornoleffSetBonus > 0)
                _attackSpeed = 20;
            else
                _attackSpeed = 10;

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

            if (_attacking)
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
            if (_standPlayer.StandPornoleffSetBonus == 0 && !_attacking && false)
                return true;

            Vector2 direction = _targetPosition - projectile.position;
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
                float si = (float)Math.Sin(_armPhase[i]);

                if (si < -0.9)
                {
                    _armShift[i] = new Vector2(Main.rand.NextFloat(-15f, 5f), Main.rand.NextFloat(-15f, 15f));
                    _armPhase[i] -= Main.rand.NextFloat(0f, 0.8f) * _armSpeed / 60f;
                }

                float[] oldPhase = new float[3] {
                    _armPhase[i] - _armSpeed / 180f,
                    _armPhase[i] - _armSpeed / 120f,
                    _armPhase[i] - _armSpeed / 60f
                };

                Vector2[] oldPos = new Vector2[3];

                _armPhase[i] += _armSpeed / 60f;

                if (projectile.spriteDirection == -1)
                {
                    position = projectile.Center + _armShift[i] - _armLength * direction * si;
                    for (int l = 0; l < 3; l++)
                        oldPos[l] = projectile.Center - _armLength * direction * (float)Math.Sin(oldPhase[l]);
                    ///rotation = attackAngle - (float)Math.PI * Math.Sign(attackAngle);
                    ///rotation = (rotation > invMaxAttackAngle) ? invMaxAttackAngle :
                    ///((rotation < invMinAttackAngle) ? invMinAttackAngle : rotation);
                }
                else
                {
                    position = projectile.Center + _armShift[i] + _armLength * direction * si - new Vector2(_armLength * 0.8f, 0);

                    for (int l = 0; l < 3; l++)
                        oldPos[l] = projectile.Center + _armShift[i] + _armLength * direction * (float)Math.Sin(oldPhase[l]) - new Vector2(_armLength * 0.8f, 0);
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
