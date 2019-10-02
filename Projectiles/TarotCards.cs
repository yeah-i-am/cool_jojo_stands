using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace cool_jojo_stands.Projectiles
{
    public class TarotCards : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 40;
            projectile.height = 40;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.timeLeft = 40;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];

            projectile.velocity = new Vector2(0f, -1f);

            projectile.alpha += 255 / 40;
        }
    }
}