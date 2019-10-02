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
            Main.debuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            StandoPlayer StandPlayer = player.GetModPlayer<StandoPlayer>(mod);

            if (!StandPlayer.HaveMagicianRedStand)
            {
                StandPlayer.HaveMagicianRedStand = true;
                StandPlayer.HaveStand = true;
                StandPlayer.StandBuffName = "MagicianRed";
            }

            player.buffTime[buffIndex] = 2390;
        }
    }
}
