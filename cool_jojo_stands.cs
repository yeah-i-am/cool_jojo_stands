using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.Localization;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.ObjectData;
using cool_jojo_stands.SpecialAbilities;
using cool_jojo_stands.Utils;
using System.IO;
using System;
using ReLogic.Content;

namespace cool_jojo_stands
{
    class cool_jojo_stands : Mod
    {
        public static ModKeybind StandSummonHT, SpecialAbilityHT, SwitchStandControlHT; // Hotkeys
        public static Mod mod; // This mod class

        public static bool usingSteam = false; // Is terraria starter via steam flag
        public static ulong SteamId = 0; // Steam profile id

        public static bool leveledModLoaded = false;
        public static Mod leveledMod = null;

        /* Mod constructor */
        public cool_jojo_stands()
        {
            /* It needs to support shader version highter than 2.0 */
            if (!Main.Support4K)
            {
                Main.Support4K = true;
                Main.Support8K = true;
                throw new System.Exception("\n!!! Jojo Stands:\nYour graphic device settings are incorrect. " +
                    "Please restart Terraria and try again(Do not forget to turn on the mod after rebooting)\n!!!\n");
            }
        }

        /* Mod loading */
        public override void Load()
        {
            mod = this; // mod

            mod.Logger.Info("Starting loading...");

            /* Try to get steam info */
            try
            {
                usingSteam = Steamworks.SteamAPI.IsSteamRunning();

                if (usingSteam)
                {
                    SteamId = Steamworks.SteamUser.GetSteamID().m_SteamID; // klass
                    mod.Logger.Info("Steam login = true");
                }
                else
                    mod.Logger.Warn("Steam login = false");
            }
            catch (Exception e)
            {
                usingSteam = false;
                SteamId = 0;
                mod.Logger.Warn("Steam login = false");
                mod.Logger.Error(e.Message);
            }

            /* Register hotkeys */
            StandSummonHT = KeybindLoader.RegisterKeybind(this, "Summon Stand", "Insert");
            SpecialAbilityHT = KeybindLoader.RegisterKeybind(this, "Special ability", "P");
            SwitchStandControlHT = KeybindLoader.RegisterKeybind(this, "Switch stand control mode", "Home");

            /* Loading special abilities */
            SpecialAbilityManager.Load();
            SpecialAbilityManager.AddAbility("ZaWardo", new ZaWardo());
            SpecialAbilityManager.AddAbility("SCA", new SilverChariotAbility());

            leveledModLoaded = ModLoader.TryGetMod("Leveled", out leveledMod);

            /* Client only loading */
            if (!Main.dedServ)
            {
                /* Cut scene */
                CutSceneManager.Load();
                CutSceneManager.AddScene("Test", new CutScenes.TestCSc());

                /* Red dolphin shader */
                GameShaders.Misc["RedDolphin"] = new MiscShaderData(
                    new Ref<Effect>(Assets.Request<Effect>("Effects/RedDolphin", AssetRequestMode.ImmediateLoad).Value), "RedDolphine");

                /* Bonus manager (not needed) */
                /*try
                {
                    BonusManager.Init();

                    if (BonusManager.UpdateBonuses())
                        Logger.Info("Bonuses loaded");

                    BonusManager.UnLoad();
                }
                catch (Exception e)
                {
                    mod.Logger.Warn("DB error: " + e.Message);
                }*/
            }

            mod.Logger.Info("Load successful");
        }

        /* Post setup content */
        public override void PostSetupContent()
        {
            ItemID.Sets.ExtractinatorMode[ItemID.StoneBlock] = ItemID.StoneBlock;
        }

        /* Add recipe groups */
        public override void AddRecipeGroups()
        {
            RecipeGroup group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " " +
            Language.GetTextValue("ItemName.GoldBar"), new int[]
            {
                ItemID.GoldBar,
                ItemID.PlatinumBar
            });

            RecipeGroup.RegisterGroup("CoolJoJoStands:GoldOrPlatinum", group);

            group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " " +
            Language.GetTextValue("ItemName.DemoniteBar"), new int[]
            {
                ItemID.DemoniteBar,
                ItemID.CrimtaneBar
            });

            RecipeGroup.RegisterGroup("CoolJoJoStands:DemoniteOrCrimtane", group);
        }

        /* Mod unloading function */
        public override void Unload()
        {
            if (!Main.dedServ)
            {
                //BonusManager.UnLoad();
                CutSceneManager.UnLoad();
            }

            mod = null;
            StandSummonHT = null;
            SpecialAbilityHT = null;
            SwitchStandControlHT = null;

            SpecialAbilityManager.UnLoad();
        }

        internal enum StandMessageType : byte
        {
            SyncNPCFromStatue, TimeStopActivate
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            StandMessageType msgType = (StandMessageType)reader.ReadByte();

            /// debug
            /*if (Main.netMode == NetmodeID.Server) // Server
            {
                NetMessage.BroadcastChatMessage(NetworkText.FromKey("Server get packet"), Color.Green);
            }
            if (Main.netMode == NetmodeID.MultiplayerClient) // Client
            {
                Main.NewText("Client get packet", Color.Red);
            }*/

            switch (msgType)
            {
                case StandMessageType.SyncNPCFromStatue:
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        int n = reader.ReadInt32();

                        if (n != 0)
                        {
                            int[] NetId = new int[n];

                            for (int i = 0; i < n; i++)
                                NetId[i] = reader.ReadInt32();

                            for (int i = 0; i < n; i++)
                                Main.npc[NetId[i]].SpawnedFromStatue = true;
                        }
                    }
                    break;

                case StandMessageType.TimeStopActivate:
                    int fromWho = reader.ReadInt32();

                    if (Main.netMode == NetmodeID.Server)
                    {
                        ModPacket packet = mod.GetPacket();

                        packet.Write((byte)StandMessageType.TimeStopActivate);
                        packet.Write(fromWho);

                        packet.Send(-1, fromWho);
                    }

                    SpecialAbilityManager.Abilities["ZaWardo"].GetAbilty<ZaWardo>().Init(fromWho);
                    SpecialAbilityManager.Activate("ZaWardo");
                    break;
            }
        }
    } /* End of 'cool_jojo_stands' class */
}
