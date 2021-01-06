using System;
/*using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace cool_jojo_stands.Commands
{
    class ActivateBonusCommand : ModCommand
    {
        public override CommandType Type
           => CommandType.Chat;

        public override string Command
            => "stand";

        public override string Usage
            => "/stand key";

        public override string Description
            => "Activate bonus stands";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (args.Length < 1 || args[0] == "")
                throw new UsageException("Incorrect key", Color.Red);

            Utils.BonusManager.Init();

            if (!Utils.BonusManager.Connected)
            {
                for (int i = 0; i < Main.chatLine.Length; i++)
                {
                    Main.chatLine[i].text = "";

                    if (Main.chatLine[i].parsedText.Length > 0)
                    {
                        Main.chatLine[i].parsedText[0].Text = "";
                        Main.chatLine[i].parsedText[0].TextOriginal = "";
                    }
                }

                StandoPlayer.Talk("Can't connect to data base, please try again", Color.OrangeRed);
                return;
            }

            if (Utils.BonusManager.ActivateBonus(args[0]))
                StandoPlayer.Talk("Bonus activated.", Color.Green);
            else
                StandoPlayer.Talk("An error has occurred.", Color.Red);

            Utils.BonusManager.UnLoad();
        }
    }
}
*/