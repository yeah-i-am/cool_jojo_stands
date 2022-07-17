using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace cool_jojo_stands.Items
{
    class ceasar : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ceasar");
            Tooltip.SetDefault("CEEEEEEEEEEASAAAAAAAAAAAAAR.");
        }
        public override void SetDefaults()
        {
            Item.noMelee = true;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 47;
            Item.useAnimation = 47;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.value = Item.buyPrice(0, 0, 0, 47);
            Item.rare = ItemRarityID.Purple;
            Item.expert = true;
            Item.UseSound = SoundID.NPCDeath1;
            Item.consumable = true;
        }

        public override void AddRecipes()
        {
            ///ModRecipe recipe = new ModRecipe(mod);
            ///recipe.AddIngredient(ItemID.StoneBlock, 10);
            ///recipe.AddTile(TileID.WorkBenches);
            ///recipe.SetResult(this);
            ///recipe.AddRecipe();
        }

        public override bool? UseItem(Player player)
        {
            StandoPlayer.Talk("Well. The mod is closed. There will be no new updates, but thanks for playing with it.");

            return true;
        }
    }
}
