using Terraria;
using Terraria.ModLoader;

namespace cool_jojo_stands.Projectiles.Minions
{
    public abstract class Minion : ModProjectile
    {
        public override void AI()
        {
            try
            {
                CheckActive();
                Behavior();
            }
            catch
            {

            }
        }

        public abstract void CheckActive();

        public abstract void Behavior();

        public abstract void Some();
    }
}