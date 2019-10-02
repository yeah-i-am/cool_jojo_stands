using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace cool_jojo_stands.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class JosephLeggins : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Joseph pants");
            Tooltip.SetDefault("Perfect for retreat");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = Item.buyPrice(0, 3, 47, 0);
            item.rare = 6;
            item.defense = 6;
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed *= 1.05f;
            player.meleeDamage *= 1.1f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Silk, 22);
            recipe.AddIngredient(ItemID.Hay, 18);
            recipe.AddIngredient(ItemID.LightningBoots, 13);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}