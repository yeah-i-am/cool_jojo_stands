using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace cool_jojo_stands
{
    /**************
     * Mod Player *
     **************/
    public class StandoPlayer : ModPlayer
    {
        /* Have stand flags */
        public bool HaveStand = false;
        public bool HaveStarPlatinum = false;
        public bool HaveMagicianRedStand = false;
        public bool HaveHarmitPurpleStand = false;
        public bool HaveHierophantGreenStand = false;
        public bool HaveSilverChariotStand = false;

        /* Time stop variable */
        public bool InvincibilityInStopTime = false;

        /* Stands users sets variables */
        public bool HaveStandUpSet = false;
        public int StandJotaroSetBonus = 0;
        public int StandAvdolSetBonus = 0;
        public int StandJosephSetBonus = 0;
        public int StandKakyoinSetBonus = 0;
        public int StandPornoleffSetBonus = 0;

        public string StandBuffName = "";

        /* Stand level variables */
        public int StandLevel = 1;
        public float StandXP = 0;
        public float StandNeedToUpXP = 3000;
        public const int MaxStandLevel = 100;

        /* Stand control variables */
        public bool StandManualControl = false;
        public bool StandSpawned = false;
        public bool StandJustSpawned = false;

        /*********************
         * Utility functions *
         *********************/

        /* Returns stand level */
        public string GetStandLevel() => StandLevel.ToString();

        /* Returns stand xp */
        public string GetStandXP() => ((int)StandXP).ToString() + "/" + StandNeedToUpXP.ToString();

        /* Returns xp progress */
        public float GetStandXPProgress() => StandXP / StandNeedToUpXP;

        /* Send message to chat */
        public static void Talk(string message)
        {
            if (Main.netMode != 2)
                Main.NewText(message, 150, 250, 150);
        }
        
        /* Level up processing */
        bool CheckLevelUp()
        {
            if (StandXP >= StandNeedToUpXP)
            {
                StandXP -= StandNeedToUpXP;
                StandLevel++;
                StandNeedToUpXP = (int)(Math.Pow(StandLevel, 1.4) * 3000); // New xp to level up
                return true;
            }

            return false;
        }

        /* Check projectile */
        private bool CheckStando(Projectile proj) =>
            proj.type == mod.ProjectileType<Projectiles.Minions.HierophantGreen>() ||
            proj.type == mod.ProjectileType<Projectiles.Minions.MagicianRed>() ||
            proj.type == mod.ProjectileType<Projectiles.Minions.StarPlatinum>() ||
            proj.type == mod.ProjectileType<Projectiles.FireBlast>() ||
            proj.type == mod.ProjectileType<Projectiles.EmeraldBlast>();


        /********************
         * Player functions *
         ********************/

        /* Give stand arrow */
        public override void SetupStartInventory(IList<Item> items, bool mediumcoreDeath)
        {
            Item item = new Item();
            item.SetDefaults(mod.ItemType("stand_arrow"));
            item.stack = 1;
            items.Add(item);
        }

        /* Processing Hotkeys */
        public override void ProcessTriggers( TriggersSet triggersSet )
        {
            if (!HaveStand)
                return;

            /* Stand summon processing */
            if (cool_jojo_stands.StandSummonHT.JustPressed && !StandSpawned)
            {
                StandSpawned = true;
                StandJustSpawned = true;

                if (HaveStarPlatinum)
                {
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Star_Platinum"), player.Center);

                    Projectile.NewProjectile(player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2) + 15,
                        0f, 0f, mod.ProjectileType("StarPlatinum"), StandLevel, 5f, player.whoAmI, 0f, 0f);
                }
                else if (HaveMagicianRedStand)
                {
                    Projectile.NewProjectile(player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2) + 15,
                        0f, 0f, mod.ProjectileType<Projectiles.Minions.MagicianRed>(), StandLevel, 2.0f, player.whoAmI, 2f, 0f);
                }
                else if (HaveHierophantGreenStand)
                {
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Hierophant_Green"), player.Center);

                    Projectile.NewProjectile(player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2),
                        0, 0, mod.ProjectileType<Projectiles.Minions.HierophantGreen>(), StandLevel, 2.0f, player.whoAmI, 1.5f, 0f);

                }
                else if (HaveHarmitPurpleStand)
                {
                    Vector2 Dir = Main.MouseWorld - player.Center;
                    Dir.Normalize();
                    Dir *= 24f;

                    int proj = Projectile.NewProjectile(player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2),
                        Dir.X, Dir.Y, mod.ProjectileType<Projectiles.HermitPurple>(), StandLevel, 2.0f, player.whoAmI, 0f, 0f);

                    StandSpawned = false;
                }
            }
            else if (cool_jojo_stands.SwitchStandControlHT.JustPressed)
            {
                StandManualControl = !StandManualControl;
            }
            else if (cool_jojo_stands.SpecialAbilityHT.JustPressed)
            {
                //Utils.CutSceneManager.Scenes["Test"].Activate();
            }
        }

        /* Reset effects after dead */
        public override void ResetEffects()
        {
            /* Save stand after dead */
            if (HaveStarPlatinum)
                player.AddBuff(mod.BuffType<Buffs.StarPlatinumStand>(), 300);
            else if (HaveMagicianRedStand)
                player.AddBuff(mod.BuffType<Buffs.MagicianRedStand>(), 300);
            else if (HaveHarmitPurpleStand)
                player.AddBuff(mod.BuffType<Buffs.HermitPurpleStand>(), 300);
            else if (HaveHierophantGreenStand)
                player.AddBuff(mod.BuffType<Buffs.HierophantGreenStand>(), 300);
        }

        /* Pre update function */
        public override void PreUpdate()
        {
            if (!HaveStandUpSet)
                StandJotaroSetBonus = StandAvdolSetBonus = StandJosephSetBonus = StandKakyoinSetBonus = StandPornoleffSetBonus = 0;

            if (CheckLevelUp())
                Talk("Congratulations! Your stand level increased: " + StandLevel.ToString());

            Utils.CutSceneManager.Update();
        }

        /* Post update function */
        public override void PostUpdate()
        {
            HaveStandUpSet = false;
        }

        /* Pre hurt function */
        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (InvincibilityInStopTime)
                return false;

            return base.PreHurt(pvp, quiet, ref damage, ref hitDirection, ref crit, ref customDamage, ref playSound, ref genGore, ref damageSource);
        }

        /* Clone client function
         * for future
         */
        public override void clientClone(ModPlayer clientClone)
        {
        }

        /* Initializing player data function
         */
        public override void Initialize()
        {
            /// TODO: check xp
        }

        /* Save player data function */
        public override TagCompound Save()
        {
            TagCompound Data = new TagCompound();

            Data.Add("StandXP", StandXP);
            Data.Add("StandLVL", StandLevel);
            Data.Add("StandLVLUP", StandNeedToUpXP);
            Data.Add("BuffName", StandBuffName);

            return Data;
        }

        /* Load player data function */
        public override void Load(TagCompound tag)
        {
            if (tag.ContainsKey("StandXP"))
            {
                /* In old version 'StandXP' and 'StandNeedToUpXP' was 'int'
                 * Needs to support old version saves */
                try
                {
                    StandXP = tag.GetFloat("StandXP");
                    StandNeedToUpXP = tag.GetFloat("StandLVLUP");
                }
                catch
                {
                    StandXP = tag.GetInt("StandXP");
                    StandNeedToUpXP = tag.GetInt("StandLVLUP");
                }
                finally
                {
                    StandLevel = tag.GetInt("StandLVL");

                    if (tag.ContainsKey("BuffName"))
                        StandBuffName = tag.GetString("BuffName");
                }

                /* Support old version levels */
                if (StandNeedToUpXP < 1000)
                {
                    StandXP /= StandNeedToUpXP;
                    StandNeedToUpXP = (int)(Math.Pow(StandLevel, 1.4) * 3000);
                    StandXP *= StandNeedToUpXP;
                }
            }

            base.Load(tag);
        }

        public override void PostUpdateBuffs()
        {
            if (StandBuffName != "")
                if (!player.HasBuff(mod.BuffType(StandBuffName + "Stand")))
                    player.AddBuff(mod.BuffType(StandBuffName + "Stand"), 2390);
        }

        /*******************************
         * New Level System Processing *
         *******************************/
        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
            if (target.life <= 0 && !target.SpawnedFromStatue && StandLevel <= MaxStandLevel)
                StandXP += target.lifeMax * 0.1f;
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if (target.life <= 0 && !target.SpawnedFromStatue && StandLevel < MaxStandLevel)
              if (CheckStando(proj))
                    StandXP += target.lifeMax * 0.3f;
                else
                    StandXP += target.lifeMax * 0.1f;
        }

        /* Cutscene screen position */
        public override void ModifyScreenPosition()
        {
            if (Utils.CutSceneManager.playing)
            {
                Main.screenPosition = Utils.CutSceneManager.GetPos();
            }
        }
    } /* End of 'StandoPlayer' class */
}