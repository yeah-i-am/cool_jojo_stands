/***************************************************************
 * Copyright (C) 2019-2020       G@yLord239
 * No part of this file may be changed without agreement of
 * G@yLord239
 ***************************************************************/

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Localization;
using cool_jojo_stands.NPCs;
using cool_jojo_stands.SpecialAbilities;
using cool_jojo_stands.Projectiles;
using cool_jojo_stands.Projectiles.Minions;

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
        public bool HaveStarPlatinumRequiem = false;

        /* Time stop variable */
        public bool InvincibilityInStopTime = false;

        /* Stands users sets variables */
        public bool HaveStandUpSet = false;
        public int StandJotaroSetBonus = 0;
        public int StandAvdolSetBonus = 0;
        public int StandJosephSetBonus = 0;
        public int StandKakyoinSetBonus = 0;
        public int StandPornoleffSetBonus = 0;
        public int StandFrolohSetBonus = 0;

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

        /* Stand */
        public int StandId = -1;

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
            Color messageColor = new Color(150, 250, 150);

            if (Main.netMode == NetmodeID.Server)
                ChatHelper.BroadcastChatMessage(NetworkText.FromKey(message), messageColor);
            else
                Main.NewText(message, messageColor);
        }

        /* Send message to chat with color */
        public static void Talk(string message, Color messageColor)
        {
            if (Main.netMode == NetmodeID.Server)
                ChatHelper.BroadcastChatMessage(NetworkText.FromKey(message), messageColor);
            else
                Main.NewText(message, messageColor);
        }

        /* Level up processing */
        bool CheckLevelUp()
        {
            if (StandLevel > MaxStandLevel)
            {
                StandLevel = MaxStandLevel;
                StandXP = 0f;
                StandNeedToUpXP = 1f;
                return false;
            }

            if (StandXP >= StandNeedToUpXP)
            {
                StandXP -= StandNeedToUpXP;
                StandLevel++;

                if (StandLevel == MaxStandLevel)
                {
                    StandXP = 0f;
                    StandNeedToUpXP = 1f;
                }
                else
                    StandNeedToUpXP = (int)(Math.Pow(StandLevel, 1.4) * 3000); // New xp to level up

                CombatText.NewText(new Rectangle((int)Player.position.X, (int)Player.position.Y, Player.width, Player.height), Color.Aquamarine, "New LVL!", true, false);

                int proj = Projectile.NewProjectile(null, Main.player[Main.myPlayer].Center, new Vector2(0, -8f), ProjectileID.RocketFireworksBoxYellow, 0, 0f);

                Main.projectile[proj].timeLeft = 30;

                return true;
            }

            return false;
        }

        /* Delete player stand */
        public void DeleteStand()
        {
            if (StandSpawned)
            {
                Main.projectile[StandId].Kill();
                StandSpawned = false;
            }

            if (HaveStarPlatinum)
            {
                Player.ClearBuff(ModContent.BuffType<Buffs.StarPlatinumStand>());
                HaveStarPlatinum = false;
            }
            else if (HaveMagicianRedStand)
            {
                Player.ClearBuff(ModContent.BuffType<Buffs.MagicianRedStand>());
                HaveMagicianRedStand = false;
            }
            else if (HaveHierophantGreenStand)
            {
                Player.ClearBuff(ModContent.BuffType<Buffs.HierophantGreenStand>());
                HaveHierophantGreenStand = false;
            }
            else if (HaveHarmitPurpleStand)
            {
                Player.ClearBuff(ModContent.BuffType<Buffs.HermitPurpleStand>());
                HaveHarmitPurpleStand = false;
            }
            else if (HaveStarPlatinumRequiem)
            {
                Player.ClearBuff(ModContent.BuffType<Buffs.StarPlatinumRequiemStand>());
                HaveStarPlatinumRequiem = false;
            }
            else if (HaveSilverChariotStand)
            {
                Player.ClearBuff(ModContent.BuffType<Buffs.SilverChariotStand>());
                HaveSilverChariotStand = false;
            }
        }

        /********************
         * Player functions *
         ********************/

        /* Give stand arrow */
        public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
        {
            Item item = new Item();
            item.SetDefaults(Mod.Find<ModItem>("stand_arrow").Type);
            item.stack = 1;

            return new Item[] { item };
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
                    SoundEngine.PlaySound(new SoundStyle("cool_jojo_stands/Sounds/Custom/Star_Platinum") with { Volume = StandModSystem.summonVolume },
                        Player.Center);

                    StandId = Projectile.NewProjectile(null, Player.position.X + (float)(Player.width / 2), Player.position.Y + (float)(Player.height / 2) + 15,
                        0f, 0f, Mod.Find<ModProjectile>("StarPlatinum").Type, 1, 5f, Player.whoAmI, 0f, 0f);
                }
                else if (HaveMagicianRedStand)
                {
                    StandId = Projectile.NewProjectile(null, Player.position.X + (float)(Player.width / 2), Player.position.Y + (float)(Player.height / 2) + 15,
                        0f, 0f, ModContent.ProjectileType<MagicianRed>(), StandLevel, 2.0f, Player.whoAmI, 2f, 0f);
                }
                else if (HaveHierophantGreenStand)
                {
                    SoundEngine.PlaySound(new SoundStyle("cool_jojo_stands/Sounds/Custom/Hierophant_Green") with { Volume = StandModSystem.summonVolume },
                        Player.Center);

                    StandId = Projectile.NewProjectile(null, Player.position.X + (float)(Player.width / 2), Player.position.Y + (float)(Player.height / 2),
                        0, 0, ModContent.ProjectileType<HierophantGreen>(), 1, 2.0f, Player.whoAmI, 1.5f, 0f);
                }
                else if (HaveHarmitPurpleStand)
                {
                    Vector2 Dir = Main.MouseWorld - Player.Center;
                    Dir.Normalize();
                    Dir *= 24f;

                    StandId = Projectile.NewProjectile(null, Player.position.X + (float)(Player.width / 2), Player.position.Y + (float)(Player.height / 2),
                        Dir.X, Dir.Y, ModContent.ProjectileType<HermitPurple>(), StandLevel, 2.0f, Player.whoAmI, 0f, 0f);

                    StandSpawned = false;
                }
                else if (HaveStarPlatinumRequiem)
                {
                    SoundEngine.PlaySound(new SoundStyle("cool_jojo_stands/Sounds/Custom/Star_Platinum") with { Volume = StandModSystem.summonVolume },
                        Player.Center);

                    StandId = Projectile.NewProjectile(null, Player.position.X + (float)(Player.width / 2), Player.position.Y + (float)(Player.height / 2) + 15,
                        0f, 0f, Mod.Find<ModProjectile>("StarPlatinumRequiem").Type, 1, 5f, Player.whoAmI, 0f, 0f);
                }
                else if (HaveSilverChariotStand)
                {
                    SoundEngine.PlaySound(new SoundStyle("cool_jojo_stands/Sounds/Custom/Silver_Chariot") with { Volume = StandModSystem.summonVolume },
                        Player.Center);

                    StandId = Projectile.NewProjectile(null, Player.position.X + (float)(Player.width / 2), Player.position.Y + (float)(Player.height / 2) + 15,
                        0f, 0f, ModContent.ProjectileType<SilverChariot>(), 1, 5f, Player.whoAmI, 0f, 0f);
                }
            }
            else if (cool_jojo_stands.SwitchStandControlHT.JustPressed)
            {
                StandManualControl = !StandManualControl;
            }
            else if (cool_jojo_stands.SpecialAbilityHT.JustPressed)
            {
                //Utils.CutSceneManager.Scenes["Test"].Activate();
                if ((HaveStarPlatinum && StandLevel >= 40) || (HaveStarPlatinumRequiem && StandLevel >= 10))
                {
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        ModPacket packet = cool_jojo_stands.mod.GetPacket();

                        packet.Write((byte)cool_jojo_stands.StandMessageType.TimeStopActivate);
                        packet.Write(Player.whoAmI);

                        packet.Send();
                    }

                    Utils.SpecialAbilityManager.Abilities["ZaWardo"].GetAbilty<ZaWardo>().Init(Player.whoAmI);
                    Utils.SpecialAbilityManager.Activate("ZaWardo");
                }
                else if (HaveSilverChariotStand)
                {
                    Utils.SpecialAbilityManager.Abilities["SCA"].GetAbilty<SilverChariotAbility>().Init(Player.whoAmI);
                    Utils.SpecialAbilityManager.Activate("SCA");
                }
            }
        }

        /* Reset effects after dead */
        public override void ResetEffects()
        {
            if (!HaveStandUpSet)
                StandJotaroSetBonus = StandAvdolSetBonus =
                    StandJosephSetBonus = StandKakyoinSetBonus = StandPornoleffSetBonus = 0;
        }

        /* Pre update function */
        public override void PreUpdate()
        {
            if (CheckLevelUp())
                if (StandLevel == MaxStandLevel)
                    Talk("Congratulations! Your got the MAXIMUM stand level!");
                else
                    Talk("Congratulations! Your stand level increased: " + StandLevel.ToString());
        }

        /* Post update function */
        public override void PostUpdate()
        {
            HaveStandUpSet = false;
            Player.oldVelocity = Player.velocity;
        }

        public override void PreUpdateMovement()
        {
            if (Player.grapCount > 0
                && Main.projectile[Player.grappling[0]].type == ModContent.ProjectileType<Projectiles.HermitPurple>())
            {
                (Main.projectile[Player.grappling[0]].ModProjectile as Projectiles.HermitPurple).SwingUpdate();
            }
        }

        /* Pre hurt function */
        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (InvincibilityInStopTime)
                return false;

            return base.PreHurt(pvp, quiet, ref damage, ref hitDirection, ref crit, ref customDamage, ref playSound, ref genGore, ref damageSource);
        }

        /* Save player data function */
        public override void SaveData(TagCompound tag)
        {
            tag.Set("StandXP", StandXP, true);
            tag.Set("StandLVL", StandLevel, true);
            tag.Set("StandLVLUP", StandNeedToUpXP, true);
            tag.Set("BuffName", StandBuffName, true);
        }

        /* Load player data function */
        public override void LoadData(TagCompound tag)
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

            base.LoadData(tag);
        }

        public override void PostUpdateBuffs()
        {
            if (StandBuffName != "")
                if (!Player.HasBuff(Mod.Find<ModBuff>(StandBuffName + "Stand").Type))
                    Player.AddBuff(Mod.Find<ModBuff>(StandBuffName + "Stand").Type, 2390);
        }

        /*******************************
         * New Level System Processing *
         *******************************/
        public void NPCDeadGetXP( NPC target )
        {
            if (target.life <= 0 && !target.SpawnedFromStatue && StandLevel < MaxStandLevel)
            {
                int dmgPlayer = target.GetGlobalNPC<GlobalStandNPC>().DamageFromPlayer(Player.whoAmI);
                int dmgStand = target.GetGlobalNPC<GlobalStandNPC>().DamageFromStand(Player.whoAmI);

                float XP = dmgPlayer * 0.1f + dmgStand * 0.239f;

                if (XP > 0f)
                    StandXP += XP;

                if (XP > 1f)
                    CombatText.NewText(new Rectangle((int)Player.position.X, (int)Player.position.Y - 25, Player.width, Player.height),
                        Color.HotPink, Convert.ToInt32(XP).ToString() + "xp", true, false);
            }
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