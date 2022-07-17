using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace cool_jojo_stands.Projectiles.Minions
{
    class StarPlatinumRequiem : Stand
    {
        protected float attackAngle = 0f;      // Stand attack angle
        const float maxAttackAngle = (float)(Math.PI / 6);
        const float minAttackAngle = -(float)(Math.PI / 15);
        const float invMinAttackAngle = -(float)(Math.PI / 6);
        const float invMaxAttackAngle = (float)(Math.PI / 15);
        private Vector2 GlistTargetPos;
        private bool GlistHaveTarget;
        private int GlistFrame = 0;
        private int glistFrameCounter = 0;
        private bool GlistAttacked = false;

        public StarPlatinumRequiem()
        {
            viewEnemyDist = 1000f; // View enemy distance (from stand to enemy)
            chasePlayerSpeed = 6f; // Standart chase player speed (normal speed)
            maxSpeed = 10000f;     // ... i don't use it
            maxPlayerDist = 400f;  // Max player distance to chase with normal speed
            inertia = 20f;         // Physycal variable
            maxDist = 380f;        // Max player distance to attack enemy
            ShootVel = 12f;        // Stand shoot velocity
        }

        /*****************
         * Some settings *
         *****************/
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 12;
            DisplayName.SetDefault("Star Platinum Requiem");
            Main.projPet[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 92;
            Projectile.height = 92;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 239;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ignoreWater = true;
            Projectile.alpha = 255;

            Shoot = ModContent.ProjectileType<Dolphin>();
        }

        public override bool? CanDamage() => false;
        public override bool? CanCutTiles() => false;
        public override bool? CanHitNPC(NPC target) => false;
        public override bool MinionContactDamage() => false;

        /* Select animation frame function */
        public override void SelectFrame()
        {
            if (Projectile.alpha > 30)
                Projectile.alpha -= 8;

            StandoPlayer pl = Main.player[Projectile.owner].GetModPlayer<StandoPlayer>();

            Projectile.frameCounter++;
            glistFrameCounter++;

            if (StandHaveTarget && Projectile.frameCounter >= 5)
            {
                if (Projectile.frame < 8)
                    Projectile.frame++;
                else
                    Projectile.frame = 8 + (Projectile.frame + 1) % 4;

                Projectile.frameCounter = 0;
            }
            else if (Projectile.frame > 3 && Projectile.frameCounter >= 5)
            {
                if (Projectile.frame <= 8)
                    Projectile.frame--;
                else
                    Projectile.frame = 8 + (Projectile.frame + 1) % 4;

                Projectile.frameCounter = 0;

            }
            else if (Projectile.frameCounter >= 10)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 4;
            }

            if (GlistHaveTarget && glistFrameCounter > 5)
            {
                glistFrameCounter = 0;
                GlistFrame = 4 + (GlistFrame + 1) % 4;
            }
            else if (!GlistHaveTarget && glistFrameCounter >= 10)
            {
                glistFrameCounter = 0;
                GlistFrame = (GlistFrame + 1) % 4;
            }
        }

        private void DrawGlist( string type, Color lightColor )
        {
            Texture2D texture = ModContent.Request<Texture2D>("cool_jojo_stands/Projectiles/Minions/StarPlatinumRequiem_Glist" + type).Value;

            Vector2 position = Projectile.Center;
            Microsoft.Xna.Framework.Rectangle? sourceRectangle =
                new Microsoft.Xna.Framework.Rectangle?(new Rectangle(0, 92 * GlistFrame, 92, 92));
            Vector2 origin = new Vector2(46f, 46f);
            Vector2 mountedCenter = Main.player[Projectile.owner].MountedCenter;

            Main.spriteBatch.Draw(texture, position - Main.screenPosition, sourceRectangle, lightColor, 0f, origin, 1f, (Projectile.spriteDirection == -1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0.0f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            DrawGlist("Left", lightColor);

            if (Projectile.frame < 8)
                return true;

            /* Left arm */
            Texture2D texture = ModContent.Request<Texture2D>("cool_jojo_stands/Projectiles/Minions/StarPlatinumRequiem_ArmLeft").Value;

            Vector2 position;
            Microsoft.Xna.Framework.Rectangle? sourceRectangle = new Microsoft.Xna.Framework.Rectangle?();
            Vector2 origin;
            float rotation;

            if (Projectile.spriteDirection == -1)
            {
                position = Projectile.Center - new Vector2(-16f, -3f);
                origin = new Vector2((float)texture.Width * 0.70f, (float)texture.Height * 0.55f);
                rotation = attackAngle - (float)Math.PI * Math.Sign(attackAngle);
                rotation = (rotation > invMaxAttackAngle) ? invMaxAttackAngle :
                ((rotation < invMinAttackAngle) ? invMinAttackAngle : rotation);
            }
            else
            {
                position = Projectile.Center - new Vector2(16f, -3f);
                origin = new Vector2((float)texture.Width * 0.30f, (float)texture.Height * 0.55f);
                rotation = (attackAngle > maxAttackAngle) ? maxAttackAngle :
                ((attackAngle < minAttackAngle) ? minAttackAngle : attackAngle);
            }

            lightColor = Projectile.GetAlpha(lightColor);

            Main.spriteBatch.Draw(texture, position - Main.screenPosition, sourceRectangle, lightColor, rotation, origin, 1f, (Projectile.spriteDirection == -1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0.0f);

            return true;
        }

        public override void PostDraw(Color lightColor)
        {
            DrawGlist("Right", lightColor);

            if (Projectile.frame < 8)
                return;

            /* Right arm */
            Texture2D texture = ModContent.Request<Texture2D>("cool_jojo_stands/Projectiles/Minions/StarPlatinumRequiem_ArmRight").Value;

            Vector2 position;
            Vector2 mountedCenter = Main.player[Projectile.owner].MountedCenter;
            Microsoft.Xna.Framework.Rectangle? sourceRectangle = new Microsoft.Xna.Framework.Rectangle?();
            Vector2 origin;
            float rotation;

            if (Projectile.spriteDirection == -1)
            {
                position = Projectile.Center - new Vector2(-15f, 2f);
                origin = new Vector2((float)texture.Width * 0.70f, (float)texture.Height * 0.45f);
                rotation = attackAngle - (float)Math.PI * Math.Sign(attackAngle);
                rotation = (rotation > invMaxAttackAngle) ? invMaxAttackAngle :
                ((rotation < invMinAttackAngle) ? invMinAttackAngle : rotation);
            }
            else
            {
                position = Projectile.Center - new Vector2(15f, 2f);
                origin = new Vector2((float)texture.Width * 0.30f, (float)texture.Height * 0.45f);
                rotation = (attackAngle > maxAttackAngle) ? maxAttackAngle :
                ((attackAngle < minAttackAngle) ? minAttackAngle : attackAngle);
            }

            lightColor = Projectile.GetAlpha(lightColor);

            Main.spriteBatch.Draw(texture, position - Main.screenPosition, sourceRectangle, lightColor, rotation, origin, 1f, (Projectile.spriteDirection == -1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0.0f);

            /* Scapular */
            texture = ModContent.Request<Texture2D>("cool_jojo_stands/Projectiles/Minions/StarPlatinumRequiem_Scapular").Value;

            position = Projectile.Center;
            mountedCenter = Main.player[Projectile.owner].MountedCenter;
            sourceRectangle = new Microsoft.Xna.Framework.Rectangle?();
            origin = new Vector2((float)texture.Width * 0.5f, (float)texture.Height * 0.5f);

            lightColor = Projectile.GetAlpha(lightColor);

            Main.spriteBatch.Draw(texture, position - Main.screenPosition, sourceRectangle, lightColor, 0f, origin, 1f, (Projectile.spriteDirection == -1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0.0f);

            return;
        }

        /* Star Platinum Requiem dust function */
        public override void CreateDust()
        {
            Lighting.AddLight(Projectile.Center, 0.7f, 0f, 0.7f);
        }

        public override void BehavourStart()
        {
            base.BehavourStart();
            GlistTargetPos = Vector2.Zero;
            GlistHaveTarget = false;
        }

        public override void ManualControlFar()
        {
            StandHaveTarget = true;

            float DistToPlayer = Vector2.Distance(Main.MouseWorld, player.Center);
            trgDir = Main.MouseWorld - Projectile.Center;

            if (Main.mouseRight)
            {
                targetPos = Main.MouseWorld;
                standPos = Projectile.position;
                trgDir = targetPos - Projectile.Center;
                attackAngle = (float)Math.Atan2(trgDir.Y, trgDir.X);
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

            targetDist = Vector2.Distance(standPos, Projectile.Center);
        }

        public override void ChaseNPCFar()
        {
            if (player.HasMinionAttackTargetNPC)
            {
                NPC npc = Main.npc[player.MinionAttackTargetNPC];

                targetDist = Vector2.Distance(npc.Center, Projectile.Center);
                trgDir = npc.Center - Projectile.Center;
                attackAngle = (float)Math.Atan2(trgDir.Y, trgDir.X);
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

                    if (!player.HasMinionAttackTargetNPC && distance < targetDist)
                    {
                        targetDist = Vector2.Distance(npc.Center, Projectile.Center);
                        trgDir = npc.Center - Projectile.Center;
                        attackAngle = (float)Math.Atan2(trgDir.Y, trgDir.X);
                        trgDir.X = Math.Sign(trgDir.X);
                        targetPos = npc.Center;
                        StandHaveTarget = true;
                    }
                }
            }
        }

        private void FindGlistTarget()
        {
            float glistDist = viewEnemyDist * viewEnemyDist;

            for (int k = 0; k < 200; k++)
            {
                NPC npc = Main.npc[k];

                if (npc.CanBeChasedBy(this, false))
                {
                    float distance = Vector2.DistanceSquared(npc.Center, player.Center);

                    if (distance < glistDist)
                    {
                        glistDist = distance;
                        GlistTargetPos = npc.Center;
                        GlistHaveTarget = true;
                    }
                }
            }
        }

        public override void TargetProcessingFar()
        {
            if (StandHaveTarget)
            {
                if (targetPos == Vector2.Zero && !pl.StandManualControl)
                        StandHaveTarget = false;

                NewDirection = Math.Sign(trgDir.X);

                if (pl.StandManualControl)
                {
                    if (Main.mouseRight)
                        Direction = Vector2.Zero;
                    else
                    {
                        Direction = standPos - Projectile.Center;
                        Direction.Normalize();
                    }

                    NewDirection = Math.Sign(trgDir.X);
                }
                else
                    attacking = true;
            }
        }

        public override void Behavior()
        {
            BehavourStart();

            AttackTime += 1 / 60f;

            ChasePlayer();

            if (pl.StandManualControl)
                ManualControlFar();
            else
                ChaseNPCFar();

            FindGlistTarget();

            TargetProcessingFar();

            CheckPlayerDist();

            if (attacking && AttackTime % (1f / 3f) < 1f / 60f &&
                ((attackAngle <= maxAttackAngle && attackAngle >= minAttackAngle) ||
                (attackAngle >= Math.PI - maxAttackAngle || attackAngle <= -Math.PI - minAttackAngle)))
            {
                Vector2 l = new Vector2(1, 0);
                l = l.RotatedBy(attackAngle);

                Vector2 ShootPos = Projectile.Center + l * 50;

                Vector2 ShootV = l * ShootVel;

                Projectile.NewProjectile(Projectile.GetSource_FromThis(), ShootPos.X, ShootPos.Y, ShootV.X, ShootV.Y, Shoot, Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 1f);
            }

            if (GlistHaveTarget)
            {
                /* Left attack */
                if (GlistFrame == 4 && GlistAttacked)
                {
                    GlistAttacked = false;

                    Vector2 ShootPos = Projectile.Center + new Vector2(25f * NewDirection, -15f);
                    Vector2 D = new Vector2(NewDirection, 0);

                    Vector2 ShootV = D * ShootVel;

                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), ShootPos.X, ShootPos.Y, ShootV.X, ShootV.Y, Shoot, Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
                }
                /* Right attack */
                else if (GlistFrame == 6 && !GlistAttacked)
                {
                    GlistAttacked = true;

                    Vector2 ShootPos = Projectile.Center + new Vector2(0f, -17f);
                    Vector2 D = new Vector2(NewDirection, 0);

                    Vector2 ShootV = D * ShootVel;

                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), ShootPos.X, ShootPos.Y, ShootV.X, ShootV.Y, Shoot, Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
                }
            }

            SpeedProcessing();
            BehavourEnd();
        }

        /* Stand kill function */
        public override void Kill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
            StandoPlayer pl = player.GetModPlayer<StandoPlayer>();
            pl.StandSpawned = false;
        }
    } /* End of 'StarPlatinum' class */
}
