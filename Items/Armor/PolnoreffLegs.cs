using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace cool_jojo_stands.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public abstract class PolnareffLegs : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Polnareff pants");
            Tooltip.SetDefault("...");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = Item.buyPrice(0, 3, 47, 0);
            item.rare = ItemRarityID.LightPurple;
            item.defense = 6;
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed *= 1.1f;
            player.meleeDamage *= 1.05f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Silk, 22);
            recipe.AddRecipeGroup("CoolJoJoStands:DemoniteOrCrimtane", 11);
            recipe.AddIngredient(ItemID.Obsidian, 13);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}