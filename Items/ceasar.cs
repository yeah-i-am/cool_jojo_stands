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
            item.noMelee = true;
            item.width = 40;
            item.height = 40;
            item.useTime = 47;
            item.useAnimation = 47;
            item.useStyle = 4;
            item.consumable = true;
            item.value = Item.buyPrice(0, 0, 0, 47);
            item.rare = 11;
            item.expert = true;
            item.UseSound = SoundID.NPCDeath1;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.StoneBlock, 10);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool UseItem(Player player)
        {
            this.ConsumeItem(player);
            StandoPlayer.Talk("Shit ceasar");
            return base.UseItem(player);
        }
    }
}
