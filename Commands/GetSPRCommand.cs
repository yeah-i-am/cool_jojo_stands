/*using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace cool_jojo_stands.Commands
{
    class GetSPRCommand : ModCommand
    {
        public override CommandType Type
           => CommandType.Chat;

        public override string Command
            => "getbonus";

        public override string Usage
            => "/getbonus SPR";

        public override string Description
            => "Get your bonus stand";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (args.Length < 1 || args[0] == "")
                throw new UsageException("Incorrect key", Color.Red);

            StandoPlayer pl = Main.player[Main.myPlayer].GetModPlayer<StandoPlayer>();

            if (args[0] == "SPR" && Utils.BonusManager.StarPlatinumReguiemBonus)
            {
                pl.DeleteStand();

                pl.player.AddBuff(ModContent.BuffType<Buffs.StarPlatinumRequiemStand>(), 239);
            }
        }
    }
}
*/