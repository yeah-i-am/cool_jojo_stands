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
                Behaviour();
            }
            catch
            {

            }
        }

        public abstract void CheckActive();

        public abstract void Behaviour();

        public virtual void PostUpdate() { }
    }
}