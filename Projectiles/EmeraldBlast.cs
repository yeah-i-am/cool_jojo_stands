using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace cool_jojo_stands.Projectiles
{
    class EmeraldBlast : ModProjectile
    {
        /*****************
         * Some settings *
         *****************/
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Emerald Blast");
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 6;
            Projectile.alpha = 0;
            Projectile.timeLeft = 600;
            Projectile.penetrate = 1;
            Projectile.hostile = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.netUpdate = true;
        }

        public override bool? CanCutTiles() => true;
        public override bool? CanHitNPC(NPC target) => !target.friendly;
        public override bool CanHitPlayer(Player target) => false;

        /* Emerald blast AI function */
        public override void AI()
        {
            if (Projectile.localAI[0] == 0f)
            {
                //Main.PlaySound(SoundID.Item20, projectile.position);
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

        /* Emerald blast dust function */
        public void CreateDust()
        {
            int dust = Dust.NewDust(Projectile.position,
                6, 6,
                DustID.CursedTorch,
                Projectile.velocity.X, Projectile.velocity.Y,
                0, default(Color), 2.75f);

            Main.dust[dust].position += Projectile.velocity;
            Main.dust[dust].noGravity = true;
        }

        /* NPC hit function */
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Projectile.penetrate--;

            Player player = Main.player[Projectile.owner];
            StandoPlayer pl = player.GetModPlayer<StandoPlayer>();

            damage *= 3 * (pl.StandLevel - 1) + 1;

            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }
    } /* End of 'EmeraldBlast' class */
}
