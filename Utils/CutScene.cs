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

namespace cool_jojo_stands.Utils
{
    public abstract class CutScene
    {
        public Vector2 camPos, camZoom, camVel;
        public string shaderName = "StdCutScene", shaderDir = "Effects/StdCutScene";
        public EffectPriority effectPriority = EffectPriority.VeryHigh;
        public string key;
        public float progress;

        public virtual void ShaderLoad()
        {
            if (!Main.dedServ)
            {
                Ref<Effect> screenRef = new Ref<Effect>(cool_jojo_stands.mod.GetEffect(shaderDir));

                Filters.Scene[shaderName] = new Filter(new ScreenShaderData(screenRef, shaderName), effectPriority);
                Filters.Scene[shaderName].Load();
            }
        }

        public virtual void Load()
        {}

        public virtual void Start()
        {}

        public virtual void Update()
        {}

        public virtual void End()
        {}

        public virtual void UnLoad()
        {}

        /* Activate cut scene function */
        public void Activate()
        {
            CutSceneManager.Activate(key);
        }

        /* Deactivate cut scene function */
        public void DeActivate()
        {
            CutSceneManager.DeActivate(key);
        }
    } /* End of 'CutScene' class */

    public class CutSceneManager
    {
        public static Dictionary<string, CutScene> Scenes { get; set; }
        private static Queue<CutScene> Activated;
        public static bool playing = false;

        /* Add scene function */
        public static void AddScene(string key, CutScene scene)
        {
            Scenes.Add(key, scene);
            scene.key = key;
            scene.Load();
            scene.ShaderLoad();
        }

        /* Activate cut scene function */
        public static void Activate( string key )
        {
            CutScene scene = Scenes[key];
            Activated.Enqueue(scene);
            scene.Start();
        }

        /* Dectivate cut scene function */
        public static void DeActivate(string key)
        {
            CutScene scene = Activated.Peek();

            if (scene.key == key)
            {
                Activated.Dequeue();
                scene.End();
            }
        }

        /* Update cut scenes function */
        public static void Update()
        {
            if (Activated.Count != 0)
            {
                Activated.Peek().Update();
                playing = true;
            }
            else
                playing = false;
        }

        /* Load scenes manager function */
        public static void Load()
        {
            Scenes = new Dictionary<string, CutScene>();
            Activated = new Queue<CutScene>();
        }

        /* Unload scenes function */
        public static void UnLoad()
        {
            foreach (CutScene scene in Scenes.Values)
            {
                scene.UnLoad();
            }

            Scenes = null;
            Activated = null;
        }

        public static Vector2 GetZoom() => Activated.Count > 0 ? Activated.Peek().camZoom : new Vector2(Main.GameZoomTarget);
        public static Vector2 GetPos() => Activated.Count > 0 ? Activated.Peek().camPos : Main.screenPosition;

    }
}
