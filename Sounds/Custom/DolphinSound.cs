using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Terraria.Audio;

namespace cool_jojo_stands.Sounds.Custom
{
    internal static class DolphinSound
    {
        public static SoundStyle GetInstance(string path, float volume)
        {
            return new SoundStyle(path) with
            {
                Volume = volume * 0.8f,
            };
        }
    }
}
