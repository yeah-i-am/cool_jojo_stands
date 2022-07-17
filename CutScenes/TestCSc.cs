using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.Graphics;
using Terraria.ID;
using cool_jojo_stands.Utils;


namespace cool_jojo_stands.CutScenes
{
    public class TestCSc : CutScene
    {
        float OldTime;

        public override void Load()
        {
            effectPriority = EffectPriority.VeryHigh;
        }

        public override void Start()
        {
            camVel = new Vector2(0f, 0);
            camPos = Main.screenPosition + new Vector2(-3, -4);
            camZoom = new Vector2(5, 5);
            progress = 0;

            Main.hideUI = true;

            if (!Filters.Scene[shaderName].IsActive())
            {
                Filters.Scene.Activate(shaderName).GetShader()
                    .UseColor(0.2f, 0f, 1f); // HAlign, black, need draw flag
            }

            OldTime = Main.GlobalTimeWrappedHourly;
        }

        public override void Update()
        {
            float dt = Main.GlobalTimeWrappedHourly - OldTime;
            OldTime = Main.GlobalTimeWrappedHourly;

            progress += 1 / 60f;

            if (progress > 6)
                DeActivate();

            if (progress > 3)
            {
                Filters.Scene[shaderName].GetShader().UseColor(0.2f, Math.Min((progress - 3) * 0.5f, 1f), 1f);
                return;
            }

            camPos += camVel * dt;

            Filters.Scene[shaderName].GetShader().UseColor(0.2f, Math.Max(1f - progress * 0.5f, 0.01f), 1f);
        }

        public override void End()
        {
            Main.hideUI = false;
            Filters.Scene[shaderName].Deactivate();
        }
    }
}
