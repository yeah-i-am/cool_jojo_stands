﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace cool_jojo_stands.Projectiles
{
    public class HermitPurple : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.GemHookAmethyst);
        }

        // Use this hook for hooks that can have multiple hooks midflight: Dual Hook, Web Slinger, Fish Hook, Static Hook, Lunar Hook
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

        // Return true if it is like: Hook, CandyCaneHook, BatHook, GemHooks
        //public override bool? SingleGrappleHook(Player player)
        //{
        //	return true;
        //}

        // Use this to kill oldest hook. For hooks that kill the oldest when shot, not when the newest latches on: Like SkeletronHand
        // You can also change the projectile likr: Dual Hook, Lunar Hook
        //public override void UseGrapple(Player player, ref int type)
        //{
        //	int hooksOut = 0;
        //	int oldestHookIndex = -1;
        //	int oldestHookTimeLeft = 100000;
        //	for (int i = 0; i < 1000; i++)
        //	{
        //		if (Main.projectile[i].active && Main.projectile[i].owner == projectile.whoAmI && Main.projectile[i].type == projectile.type)
        //		{
        //			hooksOut++;
        //			if (Main.projectile[i].timeLeft < oldestHookTimeLeft)
        //			{
        //				oldestHookIndex = i;
        //				oldestHookTimeLeft = Main.projectile[i].timeLeft;
        //			}
        //		}
        //	}
        //	if (hooksOut > 1)
        //	{
        //		Main.projectile[oldestHookIndex].Kill();
        //	}
        //}

        // Amethyst Hook is 300, Static Hook is 600
        public override float GrappleRange()
        {
            return 500f;       //this is the grappling hook range
        }

        public override void NumGrappleHooks(Player player, ref int numHooks)
        {
            numHooks = 2;
        }

        // default is 11, Lunar is 24
        public override void GrappleRetreatSpeed(Player player, ref float speed)
        {
            speed = 24f;   //this is the grappling hook retire speed
        }

        public override void PostDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color lightColor)
        {
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
            while (flag)
            {
                if ((double)vector2_4.Length() < (double)num1 + 1.0)
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
                    Main.spriteBatch.Draw(texture, position - Main.screenPosition, sourceRectangle, color2, rotation, origin, 1f, SpriteEffects.None, 0.0f);
                }
            }
        }
    }
}