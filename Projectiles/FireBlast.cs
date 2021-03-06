﻿using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace cool_jojo_stands.Projectiles
{
    public class FireBlast : ModProjectile
    {
        /*****************
         * Some settings *
         *****************/
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fire Blast");
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.alpha = 0;
            projectile.timeLeft = 600;
            projectile.penetrate = 1;
            projectile.hostile = true;
            projectile.ranged = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = false;
        }

        public override bool? CanCutTiles() => true;
        public override bool? CanHitNPC(NPC target) => !target.friendly;
        public override bool CanHitPlayer(Player target) => false;

        /* Fire blast AI function */
        public override void AI()
        {
            if (projectile.localAI[0] == 0f)
            {
                Main.PlaySound(SoundID.Item20, projectile.position);
                projectile.localAI[0] = 1f;
            }

            if (projectile.ai[0] != 1)
            {
                CreateDust();
                projectile.ai[0] = 1;
            }
            else
                projectile.ai[0] = 0;
        }

        /* Fire blast dust function */
        public void CreateDust()
        {
            int i, j;
            float x, y;

            for (i = 0; i < projectile.Size.Y; i += 4)
                for (j = 0; j < projectile.Size.X; j += 4)
                {
                    x = j - projectile.Size.X / 2;
                    y = i - projectile.Size.Y / 2;

                    if (x * x + y * y > 16)
                        continue;

                    int dust = Dust.NewDust(projectile.position + new Vector2(j, i), 
                        6, 6,
                        DustID.Fire,
                        projectile.velocity.X, projectile.velocity.Y,
                        0, default(Color), 2.75f);

                    Main.dust[dust].position += projectile.velocity;
                    Main.dust[dust].noGravity = true;
                }
        }

        /* NPC hit function */
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            projectile.penetrate--;

            Player player = Main.player[projectile.owner];
            StandoPlayer pl = player.GetModPlayer<StandoPlayer>();

            damage *= 2 * pl.StandLevel;

            target.AddBuff(BuffID.OnFire, 239);

            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }
    } /* End of 'FireBlast' function */
}
