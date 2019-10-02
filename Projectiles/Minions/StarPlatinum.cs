using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace cool_jojo_stands.Projectiles.Minions
{
    public class StarPlatinum : NearStand
    {
        /* Time stop variables */
        bool TimeStop = false;
        private static NPC[] NPCis = new NPC[200];
        private static bool[] NPCActive = new bool[200];
        private static bool[] ProjActive  = new bool[1000];
        private static int[] ProjTimeLeft  = new int[1000];
        private static float[,] ProjAi  = new float[1000, 2];
        private static float[,] ProjLocalAi  = new float[1000, 2];

        /*****************
         * Some settings *
         *****************/
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 15;
            DisplayName.SetDefault("Star Platinum");
            Main.projPet[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 92;
            projectile.height = 92;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 239;
            projectile.tileCollide = false;
            projectile.melee = true;
            projectile.ignoreWater = true;
            projectile.alpha = 30;
        }

        public override bool CanDamage() => false;
        public override bool? CanCutTiles() => false;
        public override bool? CanHitNPC(NPC target) => !target.friendly && attacking;
        public override bool MinionContactDamage() => false;

        /* Select animation frame function */
        public override void SelectFrame()
        {
            StandoPlayer pl = Main.player[projectile.owner].GetModPlayer<StandoPlayer>(mod);

            projectile.frameCounter++;

            if (attacking)
            {
                if (pl.StandJotaroSetBonus > 0)
                {
                    if (projectile.frameCounter >= 3)
                    {
                        projectile.frame = 12 + (projectile.frame + 1) % 3;
                        projectile.frameCounter = 0;
                    }

                }
                else if (projectile.frameCounter >= 5 &&  projectile.frame % 2 == 1 || projectile.frameCounter >= 2 && projectile.frame % 2 == 0)
                {
                    projectile.frameCounter = 0;
                    projectile.frame = 8 + (projectile.frame + 1) % 4;
                }
            }
            else
              if (projectile.frameCounter >= 10)
              {
                  projectile.frameCounter = 0;
                  projectile.frame = (projectile.frame + 1) % 8;
              }
        }

        /* Star Platinum dust function */
        public override void CreateDust()
        {
            Lighting.AddLight(projectile.Center, 0.7f, 0f, 0.7f);
        }

        /***********************
         * Time stop functions *
         ***********************/
        public static void Unload()
        {
            NPCis = null;
            ProjActive = null;
            ProjTimeLeft = null;
            ProjAi = null;
            ProjLocalAi = null;
            NPCActive = null;
        }

        private void ZaWardo()
        {
            NPC n = new NPC();

            for (int k = 0; k < 200; k++)
            {
                if (!Main.npc[k].active)
                    NPCActive[k] = Main.npc[k].active;

                if (!Main.npc[k].active)
                    continue;

                n = Main.npc[k].Clone() as NPC;
                Main.npc[k] = NPCis[k].Clone() as NPC;
                Main.npc[k].life = n.life;
                Main.npc[k].active = NPCActive[k];
            }

            for (int k = 0; k < 1000; k++)
            {
                Projectile p = Main.projectile[k];


                if (p.whoAmI == projectile.whoAmI || !p.active || !ProjActive[k])
                {
                    ProjActive[k] = p.active;

                    ProjLocalAi[k, 0] = p.localAI[0];
                    ProjLocalAi[k, 1] = p.localAI[1];

                    ProjAi[k, 0] = p.ai[0];
                    ProjAi[k, 1] = p.ai[1];

                    ProjTimeLeft[k] = p.timeLeft;
                    continue;
                }

                p.position = p.oldPosition;
                p.rotation = p.oldRot[0];
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
            Main.time--;
        }

        public override bool PreAI()
        {
            if (cool_jojo_stands.SpecialAbilityHT.JustPressed && false/* in progress */)
            {
                TimeStop = !TimeStop;

                StandoPlayer pl = Main.player[projectile.owner].GetModPlayer<StandoPlayer>();

                pl.InvincibilityInStopTime = TimeStop;

                StandoPlayer.Talk("TimeStop: " + (TimeStop ? "On" : "Off"));

                if (TimeStop)
                {
                    for (int k = 0; k < 200; k++)
                    {
                        NPCis[k] = Main.npc[k].Clone() as NPC;
                        NPCActive[k] = Main.npc[k].active;
                    }

                    for (int k = 0; k < 1000; k++)
                        ProjTimeLeft[k] = Main.projectile[k].timeLeft;

                    if (!Filters.Scene["ZaWardo"].IsActive())
                    {
                        Filters.Scene.Activate("ZaWardo", pl.player.Center).GetShader().UseTargetPosition(pl.player.Center)
                            .UseOpacity(1f)                 // Speed
                            .UseColor(6, 1, 0.0f)           // Stop time, back to normal time, End of 1/2 za wardo skip time
                            .UseSecondaryColor(1, 0, 239);  // End of 1/2 za wardo, negative offset
                    }

                    projectile.ai[0] = 0f;
                }
            }
            if (TimeStop)
            {
                for (int k = 0; k < 200; k++)
                    Main.npc[k].frameCounter--;

                /* Za Wardo shader */
                float progress = projectile.ai[0];
                Filters.Scene["ZaWardo"].GetShader().UseProgress(progress);

                Matrix projection = Matrix.CreateOrthographicOffCenter(0,
                    Main.graphics.GraphicsDevice.Viewport.Width,
                    Main.graphics.GraphicsDevice.Viewport.Height,
                    0, 0, 1);
                Matrix halfPixelOffset = Matrix.CreateTranslation(-0.5f, -0.5f, 0);
                Filters.Scene["ZaWardo"].GetShader().Shader.Parameters["MatrixTransform"].SetValue(halfPixelOffset * projection);
            }
            else
            {
                //if ()
            }

            projectile.ai[0] += 1 / 60f;

            return base.PreAI();
        }

        public override bool PreKill(int timeLeft)
        {
            StandoPlayer pl = Main.player[projectile.owner].GetModPlayer<StandoPlayer>();

            pl.InvincibilityInStopTime = false;

            return base.PreKill(timeLeft);
        }

        public override void PostAI()
        {
            if (TimeStop)
                ZaWardo();

            base.PostAI();
        }

        /* Stand kill function */
        public override void Kill(int timeLeft)
        {
            Player player = Main.player[projectile.owner];
            StandoPlayer pl = player.GetModPlayer<StandoPlayer>(mod);
            Filters.Scene["ZaWardo"].Deactivate();
            pl.StandSpawned = false;
        }
    } /* End of 'StarPlatinum' class */
}