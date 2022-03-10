using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;

namespace cool_jojo_stands.Buffs
{
    public class StarPlatinumStand : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("You are the owner of Star Platinum");
            Description.SetDefault("This is melee stand, very fast\n"
                + "Special Ability: Time Stop\n"
                + "Ability opens after level 40");
            Main.buffNoSave[Type] = false;
            canBeCleared = false;
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            StandoPlayer StandPlayer = player.GetModPlayer<StandoPlayer>();

            if (StandPlayer.Stand != StandType.StarPlatinum)
            {
                StandPlayer.Stand = StandType.StarPlatinum;
                StandPlayer.HaveStand = true;
                StandPlayer.StandBuffName = "StarPlatinum";
            }

            player.buffTime[buffIndex] = 32767;
        }

        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
            rare = ItemRarityID.Expert;
        }
    }
}