using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace cool_jojo_stands.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class JotaroCap : ModItem
    {
        bool Set = false;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Jotaro Cap");
            Tooltip.SetDefault("Looks cool");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = Item.buyPrice(0, 3, 47, 0);
            item.rare = 6;
            item.defense = 6;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return (Set = (body.type == mod.ItemType<JotaroCoat>() && legs.type == mod.ItemType<JotaroLeggins>()));
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Star Platinum became faster";
            StandoPlayer pl = player.GetModPlayer<StandoPlayer>(mod);
            pl.HaveStandUpSet = true;
            pl.StandJotaroSetBonus = 1;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Silk, 20);
            recipe.AddIngredient(ItemID.GoldBar, 10);
            recipe.AddIngredient(ItemID.Obsidian, 12);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}