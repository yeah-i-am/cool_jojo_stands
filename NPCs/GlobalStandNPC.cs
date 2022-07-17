using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static cool_jojo_stands.Projectiles.Minions.Stand;

namespace cool_jojo_stands.NPCs
{
    public class GlobalStandNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public int[] damageFromPlayer, damageFromPlayerStand;

        public override void SetDefaults(NPC npc)
        {
            damageFromPlayer = new int[256];
            damageFromPlayerStand = new int[256];
        }

        public override void OnHitByProjectile(NPC npc, Projectile projectile, int damage, float knockback, bool crit)
        {
            if (CheckStando(projectile))
                damageFromPlayerStand[projectile.owner] += Math.Min(damage, npc.life + damage);
            else
                damageFromPlayer[projectile.owner] += Math.Min(damage, npc.life + damage);

            ///if (npc.life <= 0)
            ///    Main.player[Main.myPlayer].GetModPlayer<StandoPlayer>().NPCDeadGetXP(npc);
        }

        public override void OnHitByItem(NPC npc, Player player, Item item, int damage, float knockback, bool crit)
        {
            damageFromPlayer[player.whoAmI] += Math.Min(damage, npc.life + damage);

            ///if (npc.life <= 0)
            ///    Main.player[Main.myPlayer].GetModPlayer<StandoPlayer>().NPCDeadGetXP(npc);
        }
        public override bool CheckDead(NPC npc)
        {
            bool ded = base.CheckDead(npc);

            if (ded)
                Main.player[Main.myPlayer].GetModPlayer<StandoPlayer>().NPCDeadGetXP(npc);

            return ded;
        }

        public int DamageFromPlayer(int player) => damageFromPlayer[player];
        public int DamageFromStand(int player) => damageFromPlayerStand[player];
    }
}
