using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace cool_jojo_stands.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class JosephCap : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Joseph Head");
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

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<JotaroCoat>() && legs.type == ModContent.ItemType<JotaroLeggins>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Hermit Purple became powerful";
            StandoPlayer pl = player.GetModPlayer<StandoPlayer>();
            pl.HaveStandUpSet = true;
            pl.StandJotaroSetBonus = 1;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Silk, 20);
            recipe.AddIngredient(ItemID.Hay, 17);
            recipe.AddIngredient(ItemID.HellstoneBar, 13);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}