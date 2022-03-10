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
        public override void SetDefaults()
        {
            DisplayName.SetDefault("You are the owner of Hermit Purple");
            Description.SetDefault("Oh...");
            Main.buffNoSave[Type] = false;
            canBeCleared = false;
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            StandoPlayer StandPlayer = player.GetModPlayer<StandoPlayer>();

            if (StandPlayer.Stand != StandType.HermitPurple)
            {
                StandPlayer.Stand = StandType.HermitPurple;
                StandPlayer.HaveStand = true;
                StandPlayer.StandBuffName = "HermitPurple";
            }

            player.buffTime[buffIndex] = 32767;
        }
        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
            rare = ItemRarityID.Expert;
        }
    }
}
