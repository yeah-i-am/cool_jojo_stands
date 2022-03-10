using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace cool_jojo_stands.Utils
{
    public abstract class SpecialAbility
    {
        /* Ability parameters */
        public float abilityCooldown = 0;
        public float abilityTime = 0;

        protected float _cooldown = 0;
        protected float _time = 0;
        public string key;

        public bool Activated => _time > 0;
        public bool CanBeUsed => _cooldown < 0;

        public virtual void Load() { }
        public virtual void ShaderLoad() { }
        public virtual void Start() { }
        public virtual void PreUpdate() { }
        public virtual void Update() { }
        public virtual void PostUpdate() { }
        public virtual void End() { }
        public virtual void Unload() { }

        /* Abilities time syncronization */
        public void Sync()
        {
            if (_time <= 0)
            {
                _cooldown -= 1f / 60f;

                if (SpecialAbilityManager.ActivedAbilities.Contains(this))
                    DeActivate();
            }

            _time -= 1f / 60f;
        } /* End of 'Sync' function */

        /* Set ability parameters to start values */
        public void SetUp()
        {
            _time = abilityTime;
            _cooldown = abilityCooldown;
        } /* Endof 'SetUp' function */

        /* Activate ability function */
        public bool Activate()
        {
            return SpecialAbilityManager.Activate(key);
        } /* End of 'Activate' function */

        /* Deactivate ability function */
        public bool DeActivate()
        {
            return SpecialAbilityManager.DeActivate(key);
        } /* End of 'DeActivate' function */

        /* Get your ability class */
        public AbilytyType GetAbilty<AbilytyType>() where AbilytyType : SpecialAbility
        {
            return this as AbilytyType;
        } /* End of 'GetAbilty' function */
    }

    public class SpecialAbilityManager
    {
        public static Dictionary<string, SpecialAbility> Abilities { get; set; }
        private static List<SpecialAbility> Active;
        public static List<SpecialAbility> ActivedAbilities => Active;

        public static void AddAbility( string key, SpecialAbility ability )
        {
            ability.key = key;
            Abilities.Add(key, ability);
            ability.Load();

            if (!Main.dedServ)
                ability.ShaderLoad();
        }

        public static void PreUpdate()
        {
            foreach (var ability in Abilities)
                ability.Value.Sync();

            foreach (var ability in Active)
                ability.PreUpdate();
        }

        public static void Update()
        {
            foreach (var ability in Active)
                ability.Update();
        }

        public static void PostUpdate()
        {
            foreach (var ability in Active)
                ability.PostUpdate();
        }

        /* Activate ability function */
        public static bool Activate( string key )
        {
            SpecialAbility ability = Abilities[key];

            if (ability.CanBeUsed)
            {
                ability.SetUp();
                ability.Start();
                Active.Add(ability);

                return true;
            }

            return false;
        } /* End of 'Activate' function */

        /* Deactivate ability function */
        public static bool DeActivate( string key )
        {
            int i = Active.FindIndex(x => x.key == key);

            if (i == -1)
                return false;

            Active[i].End();
            Active.RemoveAt(i);

            return true;
        } /* End of 'DeActivate' function */

        public static void Load()
        {
            Abilities = new Dictionary<string, SpecialAbility>();
            Active = new List<SpecialAbility>();
        }

        /* Unload abilities function */
        public static void UnLoad()
        {
            foreach (var ability in Active)
                ability.DeActivate();

            foreach (var ability in Abilities)
                ability.Value.Unload();

            Abilities = null;
            Active = null;
        } /* End of 'UnLoad' function */
    }
}
