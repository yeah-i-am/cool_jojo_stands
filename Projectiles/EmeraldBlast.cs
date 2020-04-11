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
            projectile.width = 8;
            projectile.height = 6;
            projectile.alpha = 0;
            projectile.timeLeft = 600;
            projectile.penetrate = 1;
            projectile.hostile = true;
            projectile.ranged = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.netUpdate = true;
        }

        public override bool? CanCutTiles() => true;
        public override bool? CanHitNPC(NPC target) => !target.friendly;
        public override bool CanHitPlayer(Player target) => false;

        /* Emerald blast AI function */
        public override void AI()
        {
            if (projectile.localAI[0] == 0f)
            {
                //Main.PlaySound(SoundID.Item20, projectile.position);
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

        /* Emerald blast dust function */
        public void CreateDust()
        {
            int dust = Dust.NewDust(projectile.position,
                6, 6,
                75,
                projectile.velocity.X, projectile.velocity.Y,
                0, default(Color), 2.75f);

            Main.dust[dust].position += projectile.velocity;
            Main.dust[dust].noGravity = true;
        }

        /* NPC hit function */
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            projectile.penetrate--;

            Player player = Main.player[projectile.owner];
            StandoPlayer pl = player.GetModPlayer<StandoPlayer>();

            damage *= 3 * (pl.StandLevel - 1) + 1;

            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }
    } /* End of 'EmeraldBlast' class */
}
