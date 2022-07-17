using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
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
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.alpha = 0;
            Projectile.timeLeft = 600;
            Projectile.penetrate = 1;
            Projectile.hostile = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
        }

        public override bool? CanCutTiles() => true;
        public override bool? CanHitNPC(NPC target) => !target.friendly;
        public override bool CanHitPlayer(Player target) => false;

        /* Fire blast AI function */
        public override void AI()
        {
            if (Projectile.localAI[0] == 0f)
            {
                SoundEngine.PlaySound(SoundID.Item20, Projectile.position);
                Projectile.localAI[0] = 1f;
            }

            if (Projectile.ai[0] != 1)
            {
                CreateDust();
                Projectile.ai[0] = 1;
            }
            else
                Projectile.ai[0] = 0;
        }

        /* Fire blast dust function */
        public void CreateDust()
        {
            int i, j;
            float x, y;

            for (i = 0; i < Projectile.Size.Y; i += 4)
                for (j = 0; j < Projectile.Size.X; j += 4)
                {
                    x = j - Projectile.Size.X / 2;
                    y = i - Projectile.Size.Y / 2;

                    if (x * x + y * y > 16)
                        continue;

                    int dust = Dust.NewDust(Projectile.position + new Vector2(j, i), 
                        6, 6,
                        DustID.Torch,
                        Projectile.velocity.X, Projectile.velocity.Y,
                        0, default(Color), 2.75f);

                    Main.dust[dust].position += Projectile.velocity;
                    Main.dust[dust].noGravity = true;
                }
        }

        /* NPC hit function */
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Projectile.penetrate--;

            Player player = Main.player[Projectile.owner];
            StandoPlayer pl = player.GetModPlayer<StandoPlayer>();

            damage *= 2 * pl.StandLevel;

            target.AddBuff(BuffID.OnFire, 239);

            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }
    } /* End of 'FireBlast' function */
}
