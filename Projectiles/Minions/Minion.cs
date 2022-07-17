using System;
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
            catch (Exception e)
            {
                StandoPlayer.Talk("JoJo Stands Mod Error:");
                StandoPlayer.Talk(e.Message);
                StandoPlayer.Talk("Please send this error to developer");
            }
        }

        public abstract void CheckActive();

        public abstract void Behavior();

        public virtual void Some() { }
    }
}