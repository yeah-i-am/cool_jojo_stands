using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace cool_jojo_stands.Buffs
{
    public class MagicianRedStand : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("You are the owner of Magician Red");
            Description.SetDefault("This is fire stand, very hot");
            Main.buffNoSave[Type] = false; // false
            canBeCleared = false;
            Main.buffNoTimeDisplay[Type] = true;
            ///Main.pvpBuff[Type] = true;
            //Main.persistentBuff[Type] = true;
            Main.debuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            ///StandoPlayer.Talk(player.name);
            StandoPlayer StandPlayer = player.GetModPlayer<StandoPlayer>(mod);

            if (!StandPlayer.HaveMagicianRedStand)
            {
                StandPlayer.HaveMagicianRedStand = true;
                StandPlayer.HaveStand = true;
            }

            player.buffTime[buffIndex] = 300;

            if (!StandPlayer.StandSpawned && cool_jojo_stands.StandSummonHT.JustPressed)
            {
                StandPlayer.StandSpawned = true;
                StandPlayer.StandJustSpawned = true;
                ///Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Star_Platinum"));

                Projectile.NewProjectile(player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2) + 15,
                    0f, 0f, mod.ProjectileType<Projectiles.Minions.MagicianRed>(), 1 * StandPlayer.StandLevel, 2.0f, player.whoAmI, 0f, 0f);
            }
        }
    }
}
