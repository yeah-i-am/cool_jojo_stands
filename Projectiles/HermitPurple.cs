using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameInput;

namespace cool_jojo_stands.Projectiles
{
    public class HermitPurple : ModProjectile
    {
        double
            angle,
            length,
            ampl,
            swingSpeed = 3,
            time = 0;

        double angSpeed;

       float timer
        {
            get => projectile.localAI[1];
            set => projectile.localAI[1] = value;
        }

        Vector2[] coll = new Vector2[10];
        Vector2[] collPos = new Vector2[10];
        double[] collAngle = new double[10];
        double[] collLen = new double[10];
        double[] collAnglVel = new double[10];
        
        int collSize = 0;
        int lastColl => collSize - 1;
        int preLastColl => collSize - 2;

        int lastColOutTime = 0;
        const int minCollOutTime = 3;

        Vector2 oldVel;
        Vector2 controlPos;
        double controlAmpl;

        int goToControl;
        const int gtcSpeps = 10;

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.SlimeHook);
        }

        public void SwingUpdate()
        {
            if (projectile.ai[0] == 2)
            {
                timer += 1f / 60f;

                Vector2 Dir;

                if (timer > 0.2f)
                {
                    Player player = Main.player[projectile.owner];

                    if (time == 0)
                    {
                        if ((Dir = player.Center - projectile.Center).Y > 23.9)
                        {
                            collAngle[collSize] = 239;
                            collAnglVel[collSize] = -239;
                            coll[collSize++] = projectile.Center;

                            ampl = -Math.Atan2(Dir.X, Dir.Y);
                            length = Dir.Length();
                            swingSpeed = Math.Sqrt(1000.0 / length);

                            collLen[lastColl] = length;

                            controlPos = player.position;
                            controlAmpl = ampl;
                        }
                        else
                        {
                            timer = float.NegativeInfinity;
                            return;
                        }
                    } else if ((oldVel - player.oldVelocity).LengthSquared() > 0.1) // player colliding
                    {
                        if (ampl * angle < 0f)
                        {
                            ampl *= -Math.Cos(swingSpeed * (time - 1 / 60.0));

                            if (angSpeed * ampl > 0)
                                time = 0;
                            else
                                time = Math.PI / swingSpeed;
                        }
                        else
                        {
                            player.velocity = Vector2.Zero;
                            oldVel = new Vector2(239, 239);
                            return;
                        }
                    }

                    bool collided = false, callin = false;

                    if (lastColOutTime > minCollOutTime)
                    {
                        Vector2 v = coll[lastColl] - player.Center;
                        int vl = (int)v.Length() - 26;
                        v.Normalize();
                        Vector2 c;

                        for (int i = 1; i < vl; i++)
                        {
                            c = Collision.TileCollision(player.Center + v * i, v, 0, 0);

                            if (c != v && collSize < coll.Length)
                            {
                                Vector2 newColl = player.Center + v * i + c;

                               // newColl = newColl.Floor() * 16f;

                                if ((newColl - coll[lastColl]).LengthSquared() > 256f)
                                {
                                    collAngle[collSize] = angle;
                                    collAnglVel[collSize] = angSpeed;
                                    collPos[collSize] = player.position;
                                    coll[collSize++] = newColl;
                                    collided = true;
                                    callin = true;
                                }

                                break;
                            }
                        }
                    }

                    if (collAnglVel[lastColl] < 0 &&
                        (collAngle[lastColl] < 0 && angle > collAngle[lastColl] ||
                        collAngle[lastColl] > 0 && angle > collAngle[lastColl]) ||
                        collAnglVel[lastColl] > 0 &&
                        (collAngle[lastColl] < 0 && angle < collAngle[lastColl] ||
                        collAngle[lastColl] > 0 && angle < collAngle[lastColl]))
                    {
                        player.position = collPos[lastColl];
                        collSize--;
                        collided = true;
                        lastColOutTime = 0;
                    }

                    if (collided)
                    {
                        Dir = player.Center - coll[lastColl];

                        if (callin)
                        {
                            length = Dir.Length(); 
                            collLen[lastColl] = length;
                        }
                        else
                            length = collLen[lastColl];

                        double velLen = oldVel.LengthSquared() * 3600.0;
                        swingSpeed = Math.Sqrt(1000.0 / length);
                        ampl = Math.Sign(-angSpeed) * Math.Sqrt(velLen / (length * 1000.0) + angle * angle);  // sqrt(vel^2 / (len^2 * swingspd^2) + angle^2)

                        time = Math.Acos(angle / ampl) / swingSpeed;

                        if (callin)
                          collAngle[lastColl] = angle;

                        time += 1 / 10000000000.0;
                    }

                    angSpeed = ampl * swingSpeed * Math.Cos(swingSpeed * time + MathHelper.PiOver2);

                    // Swing up
                    /*if (Math.Abs(angle) < 0.05)
                        if (PlayerInput.Triggers.Current.KeyStatus["Left"])
                        {
                            if (dir > 0)
                                ampl *= 1.03f;
                            else if (ampl > 0)
                                ampl *= 0.97f;

                            dir = ampl * (float)Math.Cos(swingSpeed * projectile.localAI[1] + MathHelper.PiOver2);
                        }
                        else if (PlayerInput.Triggers.Current.KeyStatus["Right"])
                        {
                            if (dir < 0)
                                ampl *= 1.03f;
                            else if (ampl > 0)
                                ampl *= 0.97f;

                            dir = ampl * (float)Math.Cos(swingSpeed * projectile.localAI[1] + MathHelper.PiOver2);
                        }  */

                    angle = ampl * Math.Cos(swingSpeed * time);

                    oldVel = player.velocity = new Vector2(-1, 0).RotatedBy(angle) * (float)(length * angSpeed / 60.0);

                    time += 1.0 / 60.0;

                    if (goToControl > 0)
                    {
                        if (goToControl == 1)
                        {
                            player.position = controlPos;
                            time = 0.0000001;
                            ampl = controlAmpl;

                            angSpeed = ampl * swingSpeed * Math.Cos(swingSpeed * time + MathHelper.PiOver2);
                            angle = ampl * Math.Cos(swingSpeed * time);

                            oldVel = player.velocity = new Vector2(-1, 0).RotatedBy(angle) * (float)(length * angSpeed / 60.0);
                        }
                        else
                            oldVel = player.velocity = (controlPos -  player.position) / 2;

                        goToControl--;
                    }
                    else if (lastColl == 0 && player.oldVelocity.X * player.velocity.X < 0 && angle * controlAmpl > 0)
                    {
                        if (Math.Abs(player.position.X - controlPos.X) > 16)
                        {
                            controlPos = player.position;
                            controlAmpl = angle;
                        }
                        else
                        {
                            goToControl = gtcSpeps;
                        }
                    }

                    lastColOutTime++;
                }
            }
        }

        public override bool? CanUseGrapple(Player player)
        {
            int hooksOut = 0;

            for (int l = 0; l < 1000; l++)
                if (Main.projectile[l].active && Main.projectile[l].owner == Main.myPlayer && Main.projectile[l].type == projectile.type)
                    hooksOut++;

            if (hooksOut > 2)
                return false;

            return true;
        }

        public override float GrappleRange()
        {
            return 500f;
        }

        public override void NumGrappleHooks(Player player, ref int numHooks)
        {
            numHooks = 2;
        }

        public override void GrappleRetreatSpeed(Player player, ref float speed)
        {
            speed = 24f;
        }

        public override bool PreDrawExtras(SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            spriteBatch.Begin();

            Texture2D texture = ModContent.GetTexture("cool_jojo_stands/Projectiles/HermitPurple_Chain");

            Vector2 position = projectile.Center;
            Vector2 mountedCenter = Main.player[projectile.owner].MountedCenter;
            Microsoft.Xna.Framework.Rectangle? sourceRectangle = new Microsoft.Xna.Framework.Rectangle?();
            Vector2 origin = new Vector2((float)texture.Width * 0.5f, (float)texture.Height * 0.5f);
            float num1 = (float)texture.Height;
            Vector2 vector2_4 = mountedCenter - position;
            float rotation = (float)Math.Atan2((double)vector2_4.Y, (double)vector2_4.X) - 1.57f;
            bool flag = true;

            if (float.IsNaN(position.X) && float.IsNaN(position.Y))
                flag = false;

            if (float.IsNaN(vector2_4.X) && float.IsNaN(vector2_4.Y))
                flag = false;

            while (collSize == 0 && flag)
            {
                if (vector2_4.Length() < num1)
                {
                    flag = false;
                }
                else
                {
                    Vector2 vector2_1 = vector2_4;
                    vector2_1.Normalize();
                    position += vector2_1 * num1;
                    vector2_4 = mountedCenter - position;
                    Microsoft.Xna.Framework.Color color2 = Lighting.GetColor((int)position.X / 16, (int)((double)position.Y / 16.0));
                    color2 = projectile.GetAlpha(color2);
                    spriteBatch.Draw(texture, position - Main.screenPosition, sourceRectangle, color2, rotation, origin, 1f, SpriteEffects.None, 0.0f);
                }
            }

            for (int i = 0; i < collSize; i++)
            {
                position = coll[i];

                if (collSize - i > 1)
                    mountedCenter = coll[i + 1];
                else
                    mountedCenter = Main.player[projectile.owner].MountedCenter;

                sourceRectangle = new Microsoft.Xna.Framework.Rectangle?();
                origin = new Vector2((float)texture.Width * 0.5f, (float)texture.Height * 0.5f);
                num1 = (float)texture.Height;
                vector2_4 = mountedCenter - position;
                rotation = (float)Math.Atan2((double)vector2_4.Y, (double)vector2_4.X) - 1.57f;
                flag = true;

                if (float.IsNaN(position.X) && float.IsNaN(position.Y))
                    flag = false;

                if (float.IsNaN(vector2_4.X) && float.IsNaN(vector2_4.Y))
                    flag = false;

                while (flag)
                {
                    if (vector2_4.Length() < num1)
                    {
                        flag = false;
                    }
                    else
                    {
                        Vector2 vector2_1 = vector2_4;
                        vector2_1.Normalize();
                        position += vector2_1 * num1;
                        vector2_4 = mountedCenter - position;
                        Microsoft.Xna.Framework.Color color2 = Lighting.GetColor((int)position.X / 16, (int)((double)position.Y / 16.0));
                        color2 = projectile.GetAlpha(color2);
                        spriteBatch.Draw(texture, position - Main.screenPosition, sourceRectangle, color2, rotation, origin, 1f, SpriteEffects.None, 0.0f);
                    }
                }
            }

            return false;
        }
    }
}