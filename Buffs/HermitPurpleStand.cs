using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace cool_jojo_stands.Buffs
{
    public class HermitPurpleStand : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("You are the owner of Hermit Purple");
            Description.SetDefault("Oh...");
            Main.buffNoSave[Type] = false;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            StandoPlayer StandPlayer = player.GetModPlayer<StandoPlayer>();

            if (!StandPlayer.HaveHarmitPurpleStand)
            {
                StandPlayer.HaveHarmitPurpleStand = true;
                StandPlayer.HaveStand = true;
                StandPlayer.StandBuffName = "HermitPurple";
            }

            player.buffTime[buffIndex] = 2390;
        }
    }
}
