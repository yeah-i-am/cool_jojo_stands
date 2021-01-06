using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.ID;

namespace cool_jojo_stands.Buffs
{
    public class MagicianRedStand : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("You are the owner of Magician Red");
            Description.SetDefault("This is fire stand, very hot\n"
                + "Special Ability: None");
            Main.buffNoSave[Type] = false; // false
            canBeCleared = false;
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            StandoPlayer StandPlayer = player.GetModPlayer<StandoPlayer>();

            if (!StandPlayer.HaveMagicianRedStand)
            {
                StandPlayer.HaveMagicianRedStand = true;
                StandPlayer.HaveStand = true;
                StandPlayer.StandBuffName = "MagicianRed";
            }

            player.buffTime[buffIndex] = 2390;
        }

        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
            rare = ItemRarityID.Red;
        }
    }
}
