/*using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace cool_jojo_stands.Commands
{
    class GetStandCommand : ModCommand
    {
        public override CommandType Type
           => CommandType.Chat;

        public override string Command
            => "getstand";

        public override string Usage
            => "/getstand";

        public override string Description
            => "Get stand";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (args.Length < 1 || args[0] == "")
                throw new UsageException("Incorrect key", Color.Red);

            StandoPlayer pl = Main.player[Main.myPlayer].GetModPlayer<StandoPlayer>();
            if (args[0] == "TW")
            {
                pl.DeleteStand();
                pl.HaveStand = true;
                pl.Stand = StandType.TheWorld;
                pl.player.AddBuff(ModContent.BuffType<Buffs.TheWorldStand>(), 239);
            }
            if (args[0] == "SP")
            {
                pl.DeleteStand();
                pl.HaveStand = true;
                pl.Stand = StandType.StarPlatinum;
                pl.player.AddBuff(ModContent.BuffType<Buffs.StarPlatinumStand>(), 239);
            }
        }
    }
}*/