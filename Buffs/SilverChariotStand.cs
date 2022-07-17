using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace cool_jojo_stands.Buffs
{
    public class SilverChariotStand : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("You are the owner of Silver Chariot");
            Description.SetDefault("France is next door, boy");
            Main.buffNoSave[Type] = false;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            StandoPlayer StandPlayer = player.GetModPlayer<StandoPlayer>();

            if (!StandPlayer.HaveStarPlatinumRequiem)
            {
                StandPlayer.HaveSilverChariotStand = true;
                StandPlayer.HaveStand = true;
                StandPlayer.StandBuffName = "SilverChariot";
            }

            player.buffTime[buffIndex] = 2390;
        }
    }

}