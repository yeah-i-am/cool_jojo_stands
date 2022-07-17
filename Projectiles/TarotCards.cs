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
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 40;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            Projectile.velocity = new Vector2(0f, -1f);

            Projectile.alpha += 255 / 40;
        }
    }
}