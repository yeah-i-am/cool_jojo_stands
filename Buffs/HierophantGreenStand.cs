using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.ID;

namespace cool_jojo_stands.Buffs
{
    public class HierophantGreenStand : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("You are the owner of Hierophant Green");
            Description.SetDefault("nO oNe CaN jUsT dEfLeCt ThE eMeRaLd SpLaSh\n"
                + "Special Ability: None");
            Main.buffNoSave[Type] = false; // false
            canBeCleared = false;
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            StandoPlayer StandPlayer = player.GetModPlayer<StandoPlayer>();

            if (StandPlayer.Stand != StandType.HierophantGreen)
            {
                StandPlayer.Stand = StandType.HierophantGreen;
                StandPlayer.HaveStand = true;
                StandPlayer.StandBuffName = "HierophantGreen";
            }

            player.buffTime[buffIndex] = 32767;
        }

        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
            rare = ItemRarityID.Expert;
        }
    }
}