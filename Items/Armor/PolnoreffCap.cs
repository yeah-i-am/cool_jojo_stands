using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace cool_jojo_stands.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public abstract class PolnoreffCap : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Polnoreff Cap");
            Tooltip.SetDefault("So long");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = Item.buyPrice(0, 3, 47, 0);
            item.rare = ItemRarityID.LightPurple;
            item.defense = 5;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<PolnareffCoat>() && legs.type == ModContent.ItemType<PolnareffLegs>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Silver Chariont became faster";
            StandoPlayer pl = player.GetModPlayer<StandoPlayer>();
            pl.HaveStandUpSet = true;
            pl.StandPornoleffSetBonus = 1;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Silk, 20);
            recipe.AddIngredient(ItemID.SilkRope, 50);
            recipe.AddRecipeGroup("CoolJoJoStands:DemoniteOrCrimtane", 10);
            recipe.AddIngredient(ItemID.Obsidian, 12);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
