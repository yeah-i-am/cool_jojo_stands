using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.Graphics;

namespace cool_jojo_stands
{
	class cool_jojo_stands : Mod
	{
        public static ModHotKey StandSummonHT, SpecialAbilityHT, SwitchStandControlHT; // Hotkeys
        public static Mod mod; // This mod class

        /* Interface variables */
        private UserInterface StandInterface;
        private StandUI StandoUI;
        private GameTime StandUILastUpdate;

        internal static StandConfig StandClientConfig; // Config

        public cool_jojo_stands()
		{
		}

        public override void Load()
        {
            mod = this;

            /* Register hotkeys */
            StandSummonHT = RegisterHotKey("Summon Stand", "Insert");
            SpecialAbilityHT = RegisterHotKey("Special ability", "P");
            SwitchStandControlHT = RegisterHotKey("Switch stand control mode", "Home");

            /* Client only loading */
            if (!Main.dedServ)
            {
                /* Za Wardo shader loading */
                Ref<Effect> screenRef = new Ref<Effect>(GetEffect("Effects/ZaWardo"));

                Filters.Scene["ZaWardo"] = new Filter(new ScreenShaderData(screenRef, "ZaWardo"), EffectPriority.VeryHigh);
                Filters.Scene["ZaWardo"].Load();

                /* Interface loading */
                StandInterface = new UserInterface();
                StandoUI = new StandUI();

                StandoUI.Activate();

                StandInterface.SetState(StandoUI);

                Utils.CutSceneManager.Load();

                Utils.CutSceneManager.AddScene("Test", new CutScenes.TestCSc());
            }
        }

        /* Add recipe groups */
        public override void AddRecipeGroups()
        {
            ///
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
            }

            mod = null;
            StandSummonHT = null;
            SpecialAbilityHT = null;
            StandClientConfig = null;
            SwitchStandControlHT = null;

            Projectiles.Minions.StarPlatinum.Unload();
            Utils.CutSceneManager.UnLoad();
        }

        /* Cutscene zoom */
        public override void ModifyTransformMatrix(ref SpriteViewMatrix Transform)
        {
            if (!Main.gameMenu && Utils.CutSceneManager.playing)
        	{
        		Transform.Zoom = Utils.CutSceneManager.GetZoom();
        	}
        }
    } /* End of 'cool_jojo_stands' class */
}
