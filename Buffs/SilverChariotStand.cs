using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace cool_jojo_stands.Buffs
{
    public class SilverChariotStand : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("You are the owner of Silver Chariot");
            Description.SetDefault("France is next door, boy");
            Main.buffNoSave[Type] = false;
            canBeCleared = false;
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            StandoPlayer StandPlayer = player.GetModPlayer<StandoPlayer>();

            if (StandPlayer.Stand != StandType.SilverChariot)
            {
                StandPlayer.Stand = StandType.SilverChariot;
                StandPlayer.HaveStand = true;
                StandPlayer.StandBuffName = "SilverChariot";
            }

            player.buffTime[buffIndex] = 32767;
        }
        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
            rare = ItemRarityID.Expert;
        }
    }

}