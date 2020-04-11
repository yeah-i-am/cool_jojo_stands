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

namespace cool_jojo_stands
{
	class cool_jojo_stands : Mod
	{
        public static ModHotKey StandSummonHT, SpecialAbilityHT, SwitchStandControlHT; // Hotkeys
        public static Mod mod; // This mod class
        public static float summonVolume;
        public static float standBulletVolume;

        /* Interface variables */
        private UserInterface StandInterface;
        private StandUI StandoUI;
        private GameTime StandUILastUpdate;

        internal static StandConfig StandClientConfig; // Config

        public static bool usingSteam = false;
        public static ulong SteamId = 0;

        public cool_jojo_stands()
		{
            if (!Main.Support4K)
            {
                Main.Support4K = true;
                Main.Support8K = true;
                throw new System.Exception("\n!!! Jojo Stands:\nYour graphic device settings are incorrect. " +
                    "Please restart Terraria and try again(Do not forget to turn on the mod after rebooting)\n!!!\n");
            }
		}

        public override void Load()
        {
            mod = this;

            mod.Logger.Info("Starting loading...");

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
            StandSummonHT = RegisterHotKey("Summon Stand", "Insert");
            SpecialAbilityHT = RegisterHotKey("Special ability", "P");
            SwitchStandControlHT = RegisterHotKey("Switch stand control mode", "Home");

            SpecialAbilityManager.Load();
            SpecialAbilityManager.AddAbility("ZaWardo", new ZaWardo());
            SpecialAbilityManager.AddAbility("SCA", new SilverChariotAbility());

            /* Client only loading */
            if (!Main.dedServ)
            {
                /* Interface loading */
                StandInterface = new UserInterface();
                StandoUI = new StandUI();

                StandoUI.Activate();

                StandInterface.SetState(StandoUI);

                CutSceneManager.Load();
                CutSceneManager.AddScene("Test", new CutScenes.TestCSc());

                GameShaders.Misc["RedDolphin"] = new MiscShaderData(new Ref<Effect>(GetEffect("Effects/RedDolphin")), "RedDolphine");

                try
                {
                    BonusManager.Init();

                    if (BonusManager.UpdateBonuses())
                        Logger.Info("Bonuses loaded");

                    BonusManager.UnLoad();
                }
                catch (Exception e)
                {
                    mod.Logger.Warn("DB error: " + e.Message);
                }
            }

            mod.Logger.Info("Load successful");
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

        /* I don't know how it working, but it needs to UI */
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));

            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "cool_jojo_stands:StandInterface",
                    delegate
                    {
                        if (StandUILastUpdate != null && StandInterface.CurrentState != null)
                        {
                            StandInterface.Draw(Main.spriteBatch, StandUILastUpdate);
                        }
                        return true;
                    },
                    InterfaceScaleType.UI));
            }
        }

        /* Update function */
        private void Update()
        {
            summonVolume = StandClientConfig.StandSummonSoundVolume / 100f;
            standBulletVolume = StandClientConfig.StandBulletSoundVolume / 100f;
        }

        /* Update UI function */
        public override void UpdateUI(GameTime gameTime)
        {
            StandUILastUpdate = gameTime;

            if (StandInterface.CurrentState != null)
                StandInterface.Update(gameTime);
        }

        /* Mod unloading function */
        public override void Unload()
        {
            if (!Main.dedServ)
            {
                StandInterface = null;
                StandoUI = null;

                BonusManager.UnLoad();
                CutSceneManager.UnLoad();
            }

            mod = null;
            StandSummonHT = null;
            SpecialAbilityHT = null;
            StandClientConfig = null;
            SwitchStandControlHT = null;

            SpecialAbilityManager.UnLoad();
        }

        /* Cutscene zoom */
        public override void ModifyTransformMatrix(ref SpriteViewMatrix Transform)
        {
            if (!Main.gameMenu && Utils.CutSceneManager.playing)
            {
                Transform.Zoom = Utils.CutSceneManager.GetZoom();
            }
        }

        internal enum StandMessageType : byte
        {
            SyncNPCFromStatue, TimeStopActivate
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            StandMessageType msgType = (StandMessageType)reader.ReadByte();

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

        public override void MidUpdatePlayerNPC()
        {
            if (Main.netMode == NetmodeID.Server)
            {
                List<int> sfs = new List<int>();

                for (int i = 0; i < 200; i++)
                    if (Main.npc[i].SpawnedFromStatue && Main.npc[i].active)
                        sfs.Add(i);

                if (sfs.Count != 0)
                {
                    ModPacket packet = mod.GetPacket();

                    packet.Write((byte)StandMessageType.SyncNPCFromStatue);
                    packet.Write(sfs.Count);

                    foreach (int i in sfs)
                        packet.Write(i);

                    packet.Send();
                }
            }
        }

        public override void PreUpdateEntities()
        {
            Utils.SpecialAbilityManager.PreUpdate();
        }

        public override void PostUpdateEverything()
        {
            if (!Main.dedServ)
                Utils.CutSceneManager.Update();

            Utils.SpecialAbilityManager.Update();
            Utils.SpecialAbilityManager.PostUpdate();
            Update();
        }
    } /* End of 'cool_jojo_stands' class */
}
