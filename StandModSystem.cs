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
    internal class StandModSystem : ModSystem
    {
        public static float summonVolume; // Stand summon sound volume
        public static float standBulletVolume; // Stand bullets summon volume

        /* Interface variables */
        private UserInterface StandInterface;
        private StandUI StandoUI;
        private GameTime StandUILastUpdate;

        internal static StandConfig StandClientConfig; // Config

        public override void Load()
        {
            if (!Main.dedServ)
            {
                /* Interface loading */
                StandInterface = new UserInterface();
                StandoUI = new StandUI();

                StandoUI.Activate();

                StandInterface.SetState(StandoUI);
            }
        }

        public override void Unload()
        {
            if (!Main.dedServ)
            {
                StandInterface = null;
                StandoUI = null;
            }

            StandClientConfig = null;
        }

        /* I don't know how it working, but it needs to UI go brrrrr... */
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

        /* Update UI function */
        public override void UpdateUI(GameTime gameTime)
        {
            StandUILastUpdate = gameTime;

            if (StandInterface.CurrentState != null)
                StandInterface.Update(gameTime);
        }

        /* Cutscene zoom */
        public override void ModifyTransformMatrix(ref SpriteViewMatrix Transform)
        {
            if (!Main.gameMenu && Utils.CutSceneManager.playing)
            {
                Transform.Zoom = Utils.CutSceneManager.GetZoom();
            }
        }

        /* Mid update Player -> NPC */
        public override void PostUpdatePlayers()
        {
            /* sync npcs from statues to xp manger */
            if (Main.netMode == NetmodeID.Server)
            {
                List<int> sfs = new List<int>();

                for (int i = 0; i < 200; i++)
                    if (Main.npc[i].SpawnedFromStatue && Main.npc[i].active)
                        sfs.Add(i);

                if (sfs.Count != 0)
                {
                    ModPacket packet = cool_jojo_stands.mod.GetPacket();

                    packet.Write((byte)cool_jojo_stands.StandMessageType.SyncNPCFromStatue);
                    packet.Write(sfs.Count);

                    foreach (int i in sfs)
                        packet.Write(i);

                    packet.Send();
                }
            }
        }

        /* Pre update */
        public override void PreUpdateEntities()
        {
            Utils.SpecialAbilityManager.PreUpdate();
        }

        /* Post update */
        public override void PostUpdateEverything()
        {
            if (!Main.dedServ)
                Utils.CutSceneManager.Update();

            Utils.SpecialAbilityManager.Update();
            Utils.SpecialAbilityManager.PostUpdate();
            Update();
        }

        /* Update function */
        private void Update()
        {
            summonVolume = StandClientConfig.StandSummonSoundVolume / 100f;
            standBulletVolume = StandClientConfig.StandBulletSoundVolume / 100f;
        }
    }
}
