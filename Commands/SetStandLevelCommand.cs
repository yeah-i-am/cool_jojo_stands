using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace cool_jojo_stands.Commands
{
    class SetStandLevelCommand : ModCommand
    {
        public override CommandType Type
            => CommandType.Chat;

        public override string Command
            => "setStandLevel";

        public override string Usage
            => "/setStandLevel level pass";

        public override string Description
            => "Set your stand level";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (args[1] != "239")
                throw new UsageException("Incorrect pass", Color.Red);

            StandoPlayer pl = caller.Player.GetModPlayer<StandoPlayer>();
            int Level = int.Parse(args[0]);

            if (Level < 1)
                Level = 1;
            else if (Level > 100)
                Level = 100;

            pl.StandXP = 0;
            pl.StandLevel = Level;
            pl.StandNeedToUpXP = (int)(Math.Pow(pl.StandLevel, 1.4) * 3000); // New xp to level up
            CombatText.NewText(new Rectangle((int)pl.player.position.X, (int)pl.player.position.Y, pl.player.width, pl.player.height), Color.Aquamarine, "New LVL!", true, false);
            int proj = Projectile.NewProjectile(Main.player[Main.myPlayer].Center, new Vector2(0, -8f), ProjectileID.RocketFireworksBoxYellow, 0, 0f);

            Main.projectile[proj].timeLeft = 30;
        }
    }
}
