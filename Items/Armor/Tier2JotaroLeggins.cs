using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace cool_jojo_stands.Items.Armor
{
	[AutoloadEquip(EquipType.Legs)]
	public class Tier2JotaroLeggins : ModItem
	{
		public override void SetStaticDefaults()
		{
            DisplayName.SetDefault("Armored Jotaro pants");
			Tooltip.SetDefault("Cost twenty thousand armored yen");
		}

		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
            Item.value = Item.buyPrice(0, 19, 47, 0);
            Item.rare = ItemRarityID.Purple;
			Item.defense = 12;
		}

		public override void UpdateEquip(Player player)
		{
			player.moveSpeed *= 1.05f;
            player.GetDamage(DamageClass.Melee) *= 1.05f;
		}

        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.AdamantiteBar, 18);
			recipe.AddIngredient(ModContent.ItemType<JotaroLeggins>(), 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();

			recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.TitaniumBar, 18);
			recipe.AddIngredient(ModContent.ItemType<JotaroLeggins>(), 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}