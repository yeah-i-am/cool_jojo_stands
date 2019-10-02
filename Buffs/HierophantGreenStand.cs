using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace cool_jojo_stands.Buffs
{
    public class HierophantGreenStand : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("You are the owner of Hierophant Green");
            Description.SetDefault("nO oNe CaN jUsT dEfLeCt ThE eMeRaLd SpLaSh");
            Main.buffNoSave[Type] = false; // false
            canBeCleared = false;
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            StandoPlayer StandPlayer = player.GetModPlayer<StandoPlayer>(mod);

            if (!StandPlayer.HaveHierophantGreenStand)
            {
                StandPlayer.HaveHierophantGreenStand = true;
                StandPlayer.HaveStand = true;
                StandPlayer.StandBuffName = "HierophantGreen";
            }

            player.buffTime[buffIndex] = 2390;
        }
    }
}