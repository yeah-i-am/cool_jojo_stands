using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace cool_jojo_stands.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class KakyoinCoat : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Kakyoin Coat");
            Tooltip.SetDefault("Perfect green");
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 3, 47, 0);
            Item.rare = ItemRarityID.LightPurple;
            Item.defense = 7;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Silk, 20);
            recipe.AddIngredient(ItemID.Cactus, 20);
            recipe.AddIngredient(ItemID.Ruby, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}