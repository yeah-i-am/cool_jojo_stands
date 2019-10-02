using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace cool_jojo_stands.Buffs
{
    public class StarPlatinumStand : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("You are the owner of Star Platinum");
            Description.SetDefault("This is melee stand, very fast");
            Main.buffNoSave[Type] = false;
            canBeCleared = false;
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            StandoPlayer StandPlayer = player.GetModPlayer<StandoPlayer>(mod);

            if (!StandPlayer.HaveStarPlatinum)
            {
                StandPlayer.HaveStarPlatinum = true;
                StandPlayer.HaveStand = true;
                StandPlayer.StandBuffName = "StarPlatinum";
            }

            player.buffTime[buffIndex] = 2390;
        }
    }

}