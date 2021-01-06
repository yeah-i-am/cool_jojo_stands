using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.ID;

namespace cool_jojo_stands.Buffs
{
    public class StarPlatinumRequiemStand : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("You are the owner of Star Platinum Requiem");
            Description.SetDefault("This is Dolphin\n"
                + "Special Ability: Time Stop\n"
                + "Ability opens after level 10");
            Main.buffNoSave[Type] = false;
            canBeCleared = false;
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            StandoPlayer StandPlayer = player.GetModPlayer<StandoPlayer>();

            if (!StandPlayer.HaveStarPlatinumRequiem)
            {
                StandPlayer.HaveStarPlatinumRequiem = true;
                StandPlayer.HaveStand = true;
                StandPlayer.StandBuffName = "StarPlatinumRequiem";
            }

            player.buffTime[buffIndex] = 2390;
        }
        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
            rare = ItemRarityID.Purple;
        }
    }

}