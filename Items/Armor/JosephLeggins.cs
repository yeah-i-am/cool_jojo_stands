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
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 3, 47, 0);
            Item.rare = ItemRarityID.LightPurple;
            Item.defense = 6;
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed *= 1.05f;
            player.GetDamage(DamageClass.Melee) *= 1.1f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Silk, 22);
            recipe.AddIngredient(ItemID.Hay, 18);
            recipe.AddIngredient(ItemID.HellstoneBar, 13);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}