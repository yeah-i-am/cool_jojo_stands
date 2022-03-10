/***************************************************************
 * Copyright (C) 2019-2020       G@yLord239
 * No part of this file may be changed without agreement of
 * G@yLord239
 ***************************************************************/

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
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
using static cool_jojo_stands.cool_jojo_stands;

namespace cool_jojo_stands
{
    /**************
     * Mod Player *
     **************/
    public class StandoPlayer : ModPlayer
    {
        /* Have stand flags */
        public bool HaveStand = false;
        public StandType Stand;
        public bool DisabledAttackings;

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
        public const int MaxStandLevel = 500;

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
                NetMessage.BroadcastChatMessage(NetworkText.FromKey(message), messageColor);
            else
                Main.NewText(message, messageColor);
        }

        /* Send message to chat with color */
        public static void Talk(string message, Color messageColor)
        {
            if (Main.netMode == NetmodeID.Server)
                NetMessage.BroadcastChatMessage(NetworkText.FromKey(message), messageColor);
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

                CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), Color.Aquamarine, "New LVL!", true, false);

                int proj = Projectile.NewProjectile(Main.player[Main.myPlayer].Center, new Vector2(0, -8f), ProjectileID.RocketFireworksBoxYellow, 0, 0f);

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

            switch (Stand)
            {
                case StandType.HierophantGreen:
                    player.ClearBuff(ModContent.BuffType<Buffs.HierophantGreenStand>());
                    break;
                case StandType.StarPlatinum:
                    player.ClearBuff(ModContent.BuffType<Buffs.StarPlatinumStand>());
                    break;
                case StandType.HermitPurple:
                    player.ClearBuff(ModContent.BuffType<Buffs.HermitPurpleStand>());
                    break;
                case StandType.MagicianRed:
                    player.ClearBuff(ModContent.BuffType<Buffs.MagicianRedStand>());
                    break;
                case StandType.TheWorld:
                    player.ClearBuff(ModContent.BuffType<Buffs.TheWorldStand>());
                    break;
                case StandType.SilverChariot:
                    player.ClearBuff(ModContent.BuffType<Buffs.SilverChariotStand>());
                    break;
            }
        }

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

                if (Stand != StandType.None)
                {
                    StandId = Projectile.NewProjectile(player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2) + 15,
                            0f, 0f, mod.ProjectileType(Stand.ToString()), 1, 5f, player.whoAmI, 0f, 0f);
                }
            }
            else if (cool_jojo_stands.SwitchStandControlHT.JustPressed)
            {
                StandManualControl = !StandManualControl;
                cool_jojo_stands.ManualingPlayers[player.whoAmI] = StandManualControl;

                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    ModPacket packet = cool_jojo_stands.mod.GetPacket();

                    packet.Write((byte)StandMessageType.StandManual);
                    packet.Write(player.whoAmI);
                    packet.Write((bool)ManualingPlayers[player.whoAmI]);

                    packet.Send();
                }
            }
            else if (cool_jojo_stands.SpecialAbilityHT.JustPressed)
            {
                if (Stand != StandType.None)
                    cool_jojo_stands.StandAbilities[Stand](player, StandLevel);
            }
            if (StandSpawned && StandManualControl && cool_jojo_stands.Stands[Stand] is NearStand)
            {
                if (cool_jojo_stands.StandAttack.Old && !AttackingPlayers[player.whoAmI])
                {
                    AttackingPlayers[player.whoAmI] = true;
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        ModPacket packet = cool_jojo_stands.mod.GetPacket();

                        packet.Write((byte)StandMessageType.StandAttack);
                        packet.Write(player.whoAmI);
                        packet.Write((bool)AttackingPlayers[player.whoAmI]);

                        packet.Send();
                    }
                }
                else if (!cool_jojo_stands.StandAttack.Old && AttackingPlayers[player.whoAmI])
                {
                    AttackingPlayers[player.whoAmI] = false;
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        ModPacket packet = cool_jojo_stands.mod.GetPacket();

                        packet.Write((byte)StandMessageType.StandAttack);
                        packet.Write(player.whoAmI);
                        packet.Write((bool)AttackingPlayers[player.whoAmI]);

                        packet.Send();
                    }
                }
            }
            else if (cool_jojo_stands.Stands[Stand] is NearStand && AttackingPlayers[player.whoAmI])
            {
                AttackingPlayers[player.whoAmI] = false;
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    ModPacket packet = cool_jojo_stands.mod.GetPacket();

                    packet.Write((byte)StandMessageType.StandManual);
                    packet.Write((bool)AttackingPlayers[player.whoAmI]);

                    packet.Send();
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
            player.oldVelocity = player.velocity;
        }

        public override void PreUpdateMovement()
        {
            if (player.grapCount > 0
                && Main.projectile[player.grappling[0]].type == ModContent.ProjectileType<Projectiles.HermitPurple>())
            {
                (Main.projectile[player.grappling[0]].modProjectile as Projectiles.HermitPurple).SwingUpdate();
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
        public override TagCompound Save()
        {
            TagCompound Data = new TagCompound
            {
                { "StandXP", StandXP },
                { "StandLVL", StandLevel },
                { "StandLVLUP", StandNeedToUpXP },
                { "BuffName", StandBuffName }
            };

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
                    player.AddBuff(mod.BuffType(StandBuffName + "Stand"), 32767);
        }

        /*******************************
         * New Level System Processing *
         *******************************/
        public void NPCDeadGetXP( NPC target )
        {
            if (target.life <= 0 && !target.SpawnedFromStatue && StandLevel < MaxStandLevel)
            {
                int dmgPlayer = target.GetGlobalNPC<GlobalStandNPC>().DamageFromPlayer(player.whoAmI);
                int dmgStand = target.GetGlobalNPC<GlobalStandNPC>().DamageFromStand(player.whoAmI);

                float XP = dmgPlayer * 0.1f + dmgStand * 0.3f;

                StandXP += XP;

                if (XP > 0.8f)
                    CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y - 25, player.width, player.height),
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