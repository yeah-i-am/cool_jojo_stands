using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace cool_jojo_stands
{
    public class DefeatedBosses
    {
        public bool Boss1 = false;
        public bool Boss2 = false;
        public bool Boss3 = false;
        public bool SlimeKing = false;
        public bool QueenBee = false;

        public bool WallOfFresh = false;

        public bool MechBoss1 = false;
        public bool MechBoss2 = false;
        public bool MechBoss3 = false;

        public bool PlantBoss = false;
        public bool GolemBoss = false;
        public bool Fishron = false;

        public bool MoonLord = false;

        public void Save( ref TagCompound tag )
        {
            tag.Add("DefBoss1", Boss1);
            tag.Add("DefBoss2", Boss2);
            tag.Add("DefBoss3", Boss3);
            tag.Add("DefSlimeKing", SlimeKing);
            tag.Add("DefQueenBee", QueenBee);
            tag.Add("DefWallOfFresh", WallOfFresh);
            tag.Add("DefMechBoss1", MechBoss1);
            tag.Add("DefMechBoss2", MechBoss2);
            tag.Add("DefMechBoss3", MechBoss3);
            tag.Add("DefPlantBoss", PlantBoss);
            tag.Add("DefGolemBoss", GolemBoss);
            tag.Add("DefFishron", Fishron);
            tag.Add("DefMoonlord", MoonLord);
        }

        public void Load(ref TagCompound tag)
        {
          Boss1 =     tag.GetBool("DefBoss1");
          Boss2 =     tag.GetBool("DefBoss2");
          Boss3 =     tag.GetBool("DefBoss3");
          SlimeKing = tag.GetBool("DefSlimeKing");
          QueenBee =  tag.GetBool("DefQueenBee");
          WallOfFresh =  tag.GetBool("DefWallOfFresh");
          MechBoss1 = tag.GetBool("DefMechBoss1");
          MechBoss2 = tag.GetBool("DefMechBoss2");
          MechBoss3 = tag.GetBool("DefMechBoss3");
          PlantBoss = tag.GetBool("DefPlantBoss");
          GolemBoss = tag.GetBool("DefGolemBoss");
          Fishron =   tag.GetBool("DefFishron");
          MoonLord =  tag.GetBool("DefMoonlord");
        }
    }

    public class BossChecker
    {
        public DefeatedBosses Def = new DefeatedBosses();

        private const int Boss1XP = 2;
        private const int Boss2XP = 2;
        private const int Boss3XP = 3;
        private const int SlimeKingXP = 1;
        private const int QueenBeeXP = 2;

        private const int WallOfFreshXP = 5;

        private const int MechBoss1XP = 10;
        private const int MechBoss2XP = 10;
        private const int MechBoss3XP = 10;

        private const int PlantBossXP = 20;
        private const int GolemBossXP = 30;
        private const int FishronXP = 20;

        private const int MoonLordXP = 50;

        /* Return stand XP for killing boss */
        public int CheckKilled()
        {
            if (!Def.Boss1 && NPC.downedBoss1)
            {
                Def.Boss1 = true;
                StandoPlayer.Talk("You killed Boss1!");
                return Boss1XP;
            }
            if (!Def.Boss2 && NPC.downedBoss2)
            {
                Def.Boss2 = true;
                StandoPlayer.Talk("You killed Boss2!");
                return Boss2XP;
            }
            if (!Def.Boss3 && NPC.downedBoss3)
            {
                Def.Boss3 = true;
                StandoPlayer.Talk("You killed Boss3!");
                return Boss3XP;
            }
            if (!Def.SlimeKing && NPC.downedSlimeKing)
            {
                Def.SlimeKing = true;
                StandoPlayer.Talk("You killed SlimeKing!");
                return SlimeKingXP;
            }
            if (!Def.QueenBee && NPC.downedQueenBee)
            {
                Def.QueenBee = true;
                StandoPlayer.Talk("You killed QueenBee!");
                return QueenBeeXP;
            }
            if (!Def.WallOfFresh && Main.hardMode)
            {
                Def.WallOfFresh = true;
                StandoPlayer.Talk("You killed WallOfFresh!");
                return WallOfFreshXP;
            }
            if (!Def.MechBoss1 && NPC.downedMechBoss1)
            {
                Def.MechBoss1 = true;
                StandoPlayer.Talk("You killed MechBoss1!");
                return MechBoss1XP;
            }
            if (!Def.MechBoss2 && NPC.downedMechBoss2)
            {
                Def.MechBoss2 = true;
                StandoPlayer.Talk("You killed MechBoss2!");
                return MechBoss2XP;
            }
            if (!Def.MechBoss3 && NPC.downedMechBoss3)
            {
                Def.MechBoss3 = true;
                StandoPlayer.Talk("You killed MechBoss3!");
                return MechBoss3XP;
            }
            if (!Def.PlantBoss && NPC.downedPlantBoss)
            {
                Def.PlantBoss = true;
                StandoPlayer.Talk("You killed PlantBoss!");
                return PlantBossXP;
            }
            if (!Def.GolemBoss && NPC.downedGolemBoss)
            {
                Def.GolemBoss = true;
                StandoPlayer.Talk("You killed GolemBoss!");
                return GolemBossXP;
            }
            if (!Def.Fishron && NPC.downedFishron)
            {
                Def.Fishron = true;
                StandoPlayer.Talk("You killed Fishron!");
                return FishronXP;
            }
            if (!Def.MoonLord && NPC.downedMoonlord)
            {
                Def.MoonLord = true;
                StandoPlayer.Talk("You killed Moonlord!");
                return MoonLordXP;
            }

            return 0;
        }
    };

    public class StandoPlayer : ModPlayer
    {
        public bool HaveStand = false;

        public bool HaveStarPlatinum = false;
        public bool HaveMagicianRedStand = false;

        public bool StandSpawned = false;
        public bool StandJustSpawned = false;

        public bool HaveStandUpSet = false;
        public int StandJotaroSetBonus = 0;
        public int StandAvdolSetBonus = 0;

        public int StandLevel = 1;
        public int StandXP = 0;
        public int StandNeedToUpXP = 3;

        public BossChecker KilledBosses = new BossChecker();

        public override void SetupStartInventory(IList<Item> items)
        {
            Item item = new Item();
            item.SetDefaults(mod.ItemType("stand_arrow"));
            item.stack = 1;
            items.Add(item);
        }

        public static void Talk(string message)
        {
            if (Main.netMode != 2)
            {
                Main.NewText(message, 150, 250, 150);
            }
        }

        public override void ResetEffects()
        {
            if (HaveStarPlatinum)
                player.AddBuff(mod.BuffType<Buffs.StarPlatinumStand>(), 300);
            else if (HaveMagicianRedStand)
                player.AddBuff(mod.BuffType<Buffs.MagicianRedStand>(), 300);
        }

        void CheckNewLevelXP()
        {
            StandNeedToUpXP = (int)(Math.Pow(StandLevel, 1.3) * 3);
        }

        bool CheckLevelUp()
        {
            if (StandXP >= StandNeedToUpXP)
            {
                StandXP -= StandNeedToUpXP;
                StandLevel++;
                CheckNewLevelXP();
                return true;
            }

            return false;
        }

        public override void PreUpdate()
        {
            if (!HaveStandUpSet)
                StandJotaroSetBonus = StandAvdolSetBonus = 0;

            StandXP += KilledBosses.CheckKilled();

            if (CheckLevelUp())
                Talk("Congratulations! Your stand level increased: " + StandLevel.ToString());

            base.PreUpdate();
        }

        public override void PostUpdate()
        {
            HaveStandUpSet = false;
            base.PostUpdate();
        }

        public override TagCompound Save()
        {
            TagCompound Data = new TagCompound();

            Data.Add("StandXP", StandXP);
            Data.Add("StandLVL", StandLevel);
            Data.Add("StandLVLUP", StandNeedToUpXP);
            KilledBosses.Def.Save(ref Data);

            return Data;
        }

        public override void Load(TagCompound tag)
        {
            if (tag.ContainsKey("StandXP"))
            {
                StandXP = tag.GetInt("StandXP");
                StandLevel = tag.GetInt("StandLVL");
                StandNeedToUpXP = tag.GetInt("StandLVLUP");
                KilledBosses.Def.Load(ref tag);
            }

            base.Load(tag);
        }
    }
}