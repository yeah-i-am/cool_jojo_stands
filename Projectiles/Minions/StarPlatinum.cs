using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;

namespace cool_jojo_stands.Projectiles.Minions
{
    public class StarPlatinum : NearStand
    {
        bool TimeStop = false;

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
            projectile.timeLeft = 18000;
            projectile.tileCollide = false;
            projectile.melee = true;
            projectile.ignoreWater = true;
            projectile.alpha = 30;
        }

        public override void SelectFrame()
        {
            Player player = Main.player[projectile.owner];
            StandoPlayer pl = player.GetModPlayer<StandoPlayer>(mod);

            projectile.frameCounter++;

            if (atacking)
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

        public override void CreateDust()
        {
            Lighting.AddLight(projectile.position + projectile.Size / 2, 0.7f, 0f, 0.7f);
        }

        public override bool CanDamage()
        {
            return true;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        public override bool? CanHitNPC(NPC target)
        {
            return !target.friendly && atacking;
        }

        public override bool MinionContactDamage()
        {
            return true;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Player player = Main.player[projectile.owner];
            StandoPlayer pl = player.GetModPlayer<StandoPlayer>(mod);

            damage *= (pl.StandLevel * pl.StandLevel) + pl.StandLevel;

            if (pl.StandJotaroSetBonus > 0)
              target.StrikeNPC(damage, 0f, hitDirection);

            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        private static NPC[] NPCis = new NPC[200];
        private static bool[] ProjActive  = new bool[1000];
        private static int[] ProjTimeLeft  = new int[1000];
        private static float[,] ProjAi  = new float[1000, 2];
        private static float[,] ProjLocalAi  = new float[1000, 2];
        private static bool[] NPCActive = new bool[200];

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
            if (cool_jojo_stands.SpecialAbilityHT.JustPressed && false /* in progress */)
            {
                TimeStop = !TimeStop;

                StandoPlayer pl = Main.player[projectile.owner].GetModPlayer<StandoPlayer>();

                pl.InvincibilityInStopTime = TimeStop;

                StandoPlayer.Talk("TimeStop: " + (TimeStop ? "On" : "Off"));

                for (int k = 0; k < 200; k++)
                {
                    NPCis[k] = Main.npc[k].Clone() as NPC;
                    NPCActive[k] = Main.npc[k].active;
                }

                for (int k = 0; k < 1000; k++)
                    ProjTimeLeft[k] = Main.projectile[k].timeLeft;
            }
            if (TimeStop)
            {
                for (int k = 0; k < 200; k++)
                    Main.npc[k].frameCounter--;
            }

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

        ///public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        ///{
        ///    Main.spriteBatch.End();
        ///    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
        ///
        ///    var ZWShader = GameShaders.Misc["cool_jojo_stands:ZaWardo"];
        ///
        ///    ZWShader.UseOpacity(1f);
        ///
        ///    ZWShader.Apply(null);
        ///    return true;
        ///}
    }
}