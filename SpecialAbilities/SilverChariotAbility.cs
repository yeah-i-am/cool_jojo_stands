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
    public class SilverChariotAbility : SpecialAbility
    {
        public int User { get; private set; }

        int _ghostNum;

        public SilverChariotAbility()
        {
            abilityCooldown = 1;
            abilityTime = 1;
        }

        public void Init( int WhoAmI )
        {
            User = WhoAmI;

            StandoPlayer pl = Main.player[User].GetModPlayer<StandoPlayer>();

            _ghostNum = (pl.StandLevel - 10) / 10;
        }

        public override void Start()
        {
            StandoPlayer pl = Main.player[User].GetModPlayer<StandoPlayer>();

            if (pl.StandId == -1)
            {
                _time = 0;
                _cooldown = 0;
                return;
            }

            Projectile projectile = Main.projectile[pl.StandId];

            // Smoke Dust spawn
            for (int i = 0; i < 50; i++)
            {
                int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 31, 0f, 0f, 100, default(Color), 2f);
                Main.dust[dustIndex].velocity *= 1.4f;
            }
            // Large Smoke Gore spawn
            for (int g = 0; g < 2; g++)
            {
                int goreIndex = Gore.NewGore(projectile.Center - new Vector2(24f, 0f), default(Vector2), Main.rand.Next(61, 64), 2f);
                Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X + 1.5f;
                Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 1.5f;
                goreIndex = Gore.NewGore(projectile.Center - new Vector2(24f, 0f), default(Vector2), Main.rand.Next(61, 64), 2f);
                Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X - 1.5f;
                Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 1.5f;
                goreIndex = Gore.NewGore(projectile.Center - new Vector2(24f, 0f), default(Vector2), Main.rand.Next(61, 64), 2f);
                Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X + 1.5f;
                Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y - 1.5f;
                goreIndex = Gore.NewGore(projectile.Center - new Vector2(24f, 0f), default(Vector2), Main.rand.Next(61, 64), 2f);
                Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X - 1.5f;
                Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y - 1.5f;
            }

            //Silver Chariot armor spawn
            Gore.NewGore(projectile.Center, projectile.velocity + Main.rand.NextVector2Square(-10f, 10f), cool_jojo_stands.mod.GetGoreSlot("Gores/SC/SilverChariot_Armor_Chestplate"), 1f);
            Gore.NewGore(projectile.Center, projectile.velocity + Main.rand.NextVector2Square(-10f, 10f), cool_jojo_stands.mod.GetGoreSlot("Gores/SC/SilverChariot_Armor_Forearm"), 1f);
            Gore.NewGore(projectile.Center, projectile.velocity + Main.rand.NextVector2Square(-10f, 10f), cool_jojo_stands.mod.GetGoreSlot("Gores/SC/SilverChariot_Armor_Forearm2"), 1f);
            Gore.NewGore(projectile.Center, projectile.velocity + Main.rand.NextVector2Square(-10f, 10f), cool_jojo_stands.mod.GetGoreSlot("Gores/SC/SilverChariot_Armor_Legs"), 1f);
            Gore.NewGore(projectile.Center, projectile.velocity + Main.rand.NextVector2Square(-10f, 10f), cool_jojo_stands.mod.GetGoreSlot("Gores/SC/SilverChariot_Armor_Neck"), 1f);
            Gore.NewGore(projectile.Center, projectile.velocity + Main.rand.NextVector2Square(-10f, 10f), cool_jojo_stands.mod.GetGoreSlot("Gores/SC/SilverChariot_Armor_Shoulder"), 1f);
            Gore.NewGore(projectile.Center, projectile.velocity + Main.rand.NextVector2Square(-10f, 10f), cool_jojo_stands.mod.GetGoreSlot("Gores/SC/SilverChariot_Armor_Shoulder2"), 1f);
            Gore.NewGore(projectile.Center, projectile.velocity + Main.rand.NextVector2Square(-10f, 10f), cool_jojo_stands.mod.GetGoreSlot("Gores/SC/SilverChariot_Armor_Stomach"), 1f);
            Gore.NewGore(projectile.Center, projectile.velocity + Main.rand.NextVector2Square(-10f, 10f), cool_jojo_stands.mod.GetGoreSlot("Gores/SC/SilverChariot_Armor_Wires"), 1f);
            Gore.NewGore(projectile.Center, projectile.velocity + Main.rand.NextVector2Square(-10f, 10f), cool_jojo_stands.mod.GetGoreSlot("Gores/SC/SilverChariot_Armor_Helmet"), 1f);

            projectile.alpha = 255;
        }
    }
}
