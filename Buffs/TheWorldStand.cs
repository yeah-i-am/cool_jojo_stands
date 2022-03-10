using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.ID;

namespace cool_jojo_stands.Buffs
{
    public class TheWorldStand : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("You are the owner of The World");
            Description.SetDefault("fr za wardo!!-\n"
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

            if (StandPlayer.Stand != StandType.TheWorld)
            {
                StandPlayer.Stand = StandType.TheWorld;
                StandPlayer.HaveStand = true;
                StandPlayer.StandBuffName = "TheWorld";
            }

            player.buffTime[buffIndex] = 32767;
        }
        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
            rare = ItemRarityID.Expert;
        }
    }

}