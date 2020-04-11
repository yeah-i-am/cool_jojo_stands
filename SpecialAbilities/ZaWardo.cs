using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Microsoft.Xna.Framework.Graphics;
using cool_jojo_stands.Utils;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.Localization;
using Terraria.Graphics;
using Terraria.ID;
using System.IO;

namespace cool_jojo_stands.SpecialAbilities
{
    public class ZaWardo : SpecialAbility
    {
        public int whoAmI;
        float progress;

        /* Time stop variables */
        /* NPC */
        private static NPC[] NPCis;
        private static bool[] NPCActive;

        private static float[,] NPCAi;
        private static float[,] NPCLocalAi;

        /* Projectile */
        private static bool[] ProjActive;
        private static int[] ProjTimeLeft;

        private static Vector2[] ProjOldPos;
        private static float[] ProjOldRot;

        private static float[,] ProjAi;
        private static float[,] ProjLocalAi;

        /* Items */
        private static bool[] ItemActive;
        private static Vector2[] ItemPos;

        /* Gores */
        private static bool[] GoreActive;
        private static Vector2[] GorePos;
        private static Vector2[] GoreVel;
        private static float[] GoreRot;
        private static int[] GoreTimeLeft;

        /* Clouds */
        private static float WindSpeed;

        public ZaWardo()
        {
            AbilityCooldown = 120;
            AbilityTime = 10;
        }

        public void Init( int WhoAmI )
        {
            whoAmI = WhoAmI;
        }

        public override void Load()
        {
            NPCis = new NPC[200];
            NPCActive = new bool[200];

            NPCAi = new float[1000, 4];
            NPCLocalAi = new float[1000, 4];

            ProjActive = new bool[1000];
            ProjTimeLeft = new int[1000];

            ProjOldRot = new float[1000];
            ProjOldPos = new Vector2[1000];

            ProjAi = new float[1000, 2];
            ProjLocalAi = new float[1000, 2];

            ItemActive = new bool[400];
            ItemPos = new Vector2[400];

            GoreActive = new bool[500];
            GorePos = new Vector2[500];
            GoreVel = new Vector2[500];
            GoreRot = new float[500];
            GoreTimeLeft = new int[500];

            WindSpeed = Main.windSpeed;
        }

        public override void ShaderLoad()
        {
                Ref<Effect> screenRef = new Ref<Effect>(cool_jojo_stands.mod.GetEffect("Effects/ZaWardo"));

                Filters.Scene["ZaWardo"] = new Filter(new ScreenShaderData(screenRef, "ZaWardo"), EffectPriority.VeryHigh);
                Filters.Scene["ZaWardo"].Load();
        }

        public override void Unload()
        {
            NPCis = null;
            ProjActive = null;
            ProjTimeLeft = null;
            ProjAi = null;
            ProjLocalAi = null;
            NPCActive = null;
            NPCAi = null;
            NPCLocalAi = null;
            if (!Main.dedServ)
              Filters.Scene["ZaWardo"].Deactivate();
        }

        public override void Start()
        {
            StandoPlayer pl = Main.player[whoAmI].GetModPlayer<StandoPlayer>();
            ///StandoPlayer.Talk("TimeStop: On");

            pl.InvincibilityInStopTime = true;
            progress = 0;

            for (int k = 0; k < 200; k++)
            {
                NPC n = Main.npc[k];

                NPCis[k] = Main.npc[k].Clone() as NPC;
                NPCActive[k] = n.active;

                NPCAi[k, 0] = n.ai[0];
                NPCAi[k, 1] = n.ai[1];
                NPCAi[k, 2] = n.ai[2];
                NPCAi[k, 3] = n.ai[3];

                NPCLocalAi[k, 0] = n.localAI[0];
                NPCLocalAi[k, 1] = n.localAI[1];
                NPCLocalAi[k, 2] = n.localAI[2];
                NPCLocalAi[k, 3] = n.localAI[3];
            }

            for (int k = 0; k < 1000; k++)
            {
                Projectile p = Main.projectile[k];

                ProjOldRot[k] = p.rotation;
                ProjTimeLeft[k] = p.timeLeft;
                ProjOldPos[k] = p.position;
                ProjActive[k] = p.active;

                ProjLocalAi[k, 0] = p.localAI[0];
                ProjLocalAi[k, 1] = p.localAI[1];

                ProjAi[k, 0] = p.ai[0];
                ProjAi[k, 1] = p.ai[1];
            }

            /* Items */
            for (int k = 0; k < 400; k++)
            {
                Item i = Main.item[k];

                ItemPos[k] = i.position;
                ItemActive[k] = i.active;
                i.oldVelocity = i.velocity;
            }

            /* Gores */
            for (int k = 0; k < 500; k++)
            {
                Gore g = Main.gore[k];

                GoreActive[k] = g.active;
                GorePos[k] = g.position;
                GoreVel[k] = g.velocity;
                GoreRot[k] = g.rotation;
                GoreTimeLeft[k] = g.timeLeft;
            }

            if (!Main.dedServ)
            {
                if (!Filters.Scene["ZaWardo"].IsActive())
                    Filters.Scene.Activate("ZaWardo", pl.player.Center);

                Filters.Scene["ZaWardo"].GetShader().UseTargetPosition(pl.player.Center)
                        .UseOpacity(1.3f)               // Speed
                        .UseColor(AbilityTime, 1, 0.0f) // Stop time, back to normal time, End of 1/2 za wardo skip time
                        .UseSecondaryColor(1, 0, 239);  // End of 1/2 za wardo, negative offset
            }
        }

        public override void End()
        {
            Main.windSpeed = WindSpeed;

            StandoPlayer pl = Main.player[whoAmI].GetModPlayer<StandoPlayer>();

            pl.InvincibilityInStopTime = false;
        }

        public override void PreUpdate()
        {
            Main.windSpeed = 0;

            for (int k = 0; k < 200; k++)
            {
                NPC n = Main.npc[k];

                n.ai[0] = NPCAi[k, 0];
                n.ai[1] = NPCAi[k, 1];
                n.ai[2] = NPCAi[k, 2];
                n.ai[3] = NPCAi[k, 3];

                n.localAI[0] = NPCLocalAi[k, 0];
                n.localAI[1] = NPCLocalAi[k, 1];
                n.localAI[2] = NPCLocalAi[k, 2];
                n.localAI[3] = NPCLocalAi[k, 3];

                Main.npc[k].frameCounter--;
            }

            if (!Main.dedServ)
            {
                Filters.Scene["ZaWardo"].GetShader().UseProgress(progress);

                Matrix projection = Matrix.CreateOrthographicOffCenter(0,
                    Main.graphics.GraphicsDevice.Viewport.Width,
                    Main.graphics.GraphicsDevice.Viewport.Height,
                    0, 0, 1);

                Matrix halfPixelOffset = Matrix.CreateTranslation(-0.5f, -0.5f, 0);

                Filters.Scene["ZaWardo"].GetShader().Shader.Parameters["MatrixTransform"].SetValue(halfPixelOffset * projection);
            }

            progress += 1f / 60f;
        }

        public override void PostUpdate()
        {
            NPC n;

            /* NPC */
            for (int k = 0; k < 200; k++)
            {
                if (!Main.npc[k].active)
                {
                    NPCActive[k] = Main.npc[k].active;
                    continue;
                }

                // Fix King Slime bag

                n = Main.npc[k].Clone() as NPC;

                Main.npc[k] = NPCis[k].Clone() as NPC;

                Main.npc[k].active = NPCActive[k];
                Main.npc[k].life = n.life;
            }

            /* Projectiles */
            for (int k = 0; k < 1000; k++)
            {
                Projectile p = Main.projectile[k];

                if (p.owner == whoAmI && p.melee == true)
                    continue;

                if (!p.active || !ProjActive[k])
                {
                    ProjActive[k] = p.active;

                    ProjLocalAi[k, 0] = p.localAI[0];
                    ProjLocalAi[k, 1] = p.localAI[1];

                    ProjAi[k, 0] = p.ai[0];
                    ProjAi[k, 1] = p.ai[1];

                    ProjTimeLeft[k] = p.timeLeft;
                    ProjOldRot[k] = p.rotation;
                    ProjOldPos[k] = p.position;
                    continue;
                }

                p.position = ProjOldPos[k];
                p.rotation = ProjOldRot[k];
                p.velocity = p.oldVelocity;
                p.direction = p.oldDirection;
                p.spriteDirection = p.oldSpriteDirection[0];
                p.timeLeft = ProjTimeLeft[k];
                p.frameCounter--;

                p.localAI[0] = ProjLocalAi[k, 0];
                p.localAI[1] = ProjLocalAi[k, 1];

                p.ai[0] = ProjAi[k, 0];
                p.ai[1] = ProjAi[k, 1];

                ProjActive[k] = p.active;

                ProjLocalAi[k, 0] = p.localAI[0];
                ProjLocalAi[k, 1] = p.localAI[1];

                ProjAi[k, 0] = p.ai[0];
                ProjAi[k, 1] = p.ai[1];
            }

            /* Items */
            for (int k = 0; k < 400; k++)
            {
                Item i = Main.item[k];

                if (ItemActive[k] != i.active)
                {
                    ItemActive[k] = i.active;
                    ItemPos[k] = i.position;
                    i.oldVelocity = i.velocity;
                    continue;
                }

                i.position = ItemPos[k];
                i.velocity = i.oldVelocity;
            }

            /* Gores */
            for (int k = 0; k < 500; k++)
            {
                Gore g = Main.gore[k];

                if (GoreActive[k] != g.active)
                {
                    GoreActive[k] = g.active;
                    GorePos[k] = g.position;
                    GoreVel[k] = g.velocity;
                    GoreRot[k] = g.rotation;
                    GoreTimeLeft[k] = g.timeLeft;
                }

                g.position = GorePos[k];
                g.velocity = GoreVel[k];
                g.rotation = GoreRot[k];
                g.timeLeft = GoreTimeLeft[k];
            }

            Main.time--;
        }

    }
}
