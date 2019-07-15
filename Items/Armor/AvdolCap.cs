using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace cool_jojo_stands.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class AvdolCap : ModItem
    {
        bool Set = false;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Avdol head");
            Tooltip.SetDefault("Looks like cactus");
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
            return (Set = (body.type == mod.ItemType<AvdolCoat>() && legs.type == mod.ItemType<AvdolLegs>()));
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Magican Red became powerful";
            StandoPlayer pl = player.GetModPlayer<StandoPlayer>(mod);
            pl.HaveStandUpSet = true;
            pl.StandAvdolSetBonus = 1;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Silk, 20);
            recipe.AddIngredient(ItemID.GoldBar, 10);
            recipe.AddIngredient(ItemID.HellstoneBar, 8);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}