using cool_jojo_stands.Stands;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace cool_jojo_stands.Projectiles.Minions
{
    class StarPlatinum : NearStand
    {

        List<Fist> _fists = new List<Fist>();

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
            projectile.timeLeft = 32767;
            projectile.tileCollide = false;
            projectile.melee = true;
            projectile.ignoreWater = true;
            projectile.alpha = 255;
        }

        public override bool CanDamage() => false;
        public override bool? CanCutTiles() => false;
        public override bool MinionContactDamage() => false;

        /* Select animation frame function */
        public override void SelectFrame()
        {
            if (projectile.alpha > 30)
                projectile.alpha -= 8;

            StandoPlayer pl = Main.player[projectile.owner].GetModPlayer<StandoPlayer>();

            projectile.frameCounter++;

            if ((!cool_jojo_stands.ManualingPlayers[projectile.owner] && _attacking) || (cool_jojo_stands.ManualingPlayers[projectile.owner] && cool_jojo_stands.AttackingPlayers[projectile.owner]))
            {
                if (projectile.frameCounter >= 5 && projectile.frame % 2 == 1 || projectile.frameCounter >= 2 && projectile.frame % 2 == 0)
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

        /* Stand kill function */
        public override void Kill(int timeLeft)
        {
            Player player = Main.player[projectile.owner];
            StandoPlayer pl = player.GetModPlayer<StandoPlayer>();
            pl.StandSpawned = false;
        }

        /* Stand fists effect */
        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            projectile.Opacity = 0.7f;

            Texture2D fistT2d = ModContent.GetTexture("cool_jojo_stands/Projectiles/Minions/StarPlatinum_Fist");
            if (!Main.player[projectile.owner].GetModPlayer<StandoPlayer>().DisabledAttackings && ((cool_jojo_stands.ManualingPlayers[projectile.owner] && cool_jojo_stands.AttackingPlayers[projectile.owner]) || (!cool_jojo_stands.ManualingPlayers[projectile.owner] && _attacking)))
            {
                for (int i = 0; i < 3; i++)
                {
                    Fist newFist = new Fist(projectile.spriteDirection, 25, new Vector2(0f, new UnifiedRandom().Next(-15, 15)));
                    if (!_fists.Contains(newFist))
                        _fists.Add(newFist);
                }
            }

            for (int i = 0; i < _fists.Count; i++)
            {
                Fist fist = _fists[i];
                fist.time -= 2;
                fist.position.X += (fist.direction == -1 ? 4f : -4f);
                if (fist.time > 1)
                {
                    Main.spriteBatch.Draw(fistT2d, new Rectangle((int)(projectile.position.X - fist.position.X + (fist.direction == -1 ? -projectile.width / 2 + 107f : projectile.width / 2 - 14f)) - 13 - (int)Main.screenPosition.X, (int)fist.position.Y + (int)projectile.Center.Y - (int)Main.screenPosition.Y, 26, 14), null, lightColor * (fist.time * 4 * 0.01f), 0f, Vector2.Zero, (fist.direction == -1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
                }
                else
                {
                    _fists.Remove(fist);
                }
            }
        }
    } /* End of 'StarPlatinum' class */
}
