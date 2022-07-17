using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace cool_jojo_stands.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class KakyoinCap : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Kakyoin Hair");
            Tooltip.SetDefault("Ebaniy hvost");
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 3, 47, 0);
            Item.rare = ItemRarityID.LightPurple;
            Item.defense = 6;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<KakyoinCoat>() && legs.type == ModContent.ItemType<KakyoinLeggins>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "nothing";
            StandoPlayer pl = player.GetModPlayer<StandoPlayer>();
            pl.HaveStandUpSet = true;
            pl.StandKakyoinSetBonus = 1;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Silk, 20);
            recipe.AddIngredient(ItemID.PinkGel, 4);
            recipe.AddIngredient(ItemID.Ruby, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}