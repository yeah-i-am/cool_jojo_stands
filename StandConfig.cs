using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.Config.UI;
using Terraria.UI;

namespace cool_jojo_stands
{
    public enum UIPos
    {
        Top, Bottom, Left, Right
    }

    [Label("JoJo Stand Configuration")]
    class StandConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        public override void OnLoaded()
        {
            StandModSystem.StandClientConfig = this;
        }

        [Header("UI Settings")]

        [Label("Level info position")]
        [SliderColor(0, 255, 100)]
        [DefaultValue(UIPos.Top)]
        [ReloadRequired]
        [DrawTicks]
        public UIPos LvlPos { get; set; }

        [Header("Sounds Settings")]

        [Label("Stand summon sound volume")]
        [DefaultValue(100)]
        [Range(0, 100)]
        [Increment(1)]
        [Slider]
        public int StandSummonSoundVolume { get; set; }


        [Label("Stand bullets sound volume")]
        [DefaultValue(100)]
        [Range(0, 100)]
        [Increment(1)]
        [Slider]
        public int StandBulletSoundVolume { get; set; }
    }
}
