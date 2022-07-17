using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace cool_jojo_stands.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class Tier2JotaroCap : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Armored Jotaro Cap");
            Tooltip.SetDefault("Looks much cooler");
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 19, 47, 0);
            Item.rare = ItemRarityID.Purple;
            Item.defense = 20;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<Tier2JotaroCoat>() && legs.type == ModContent.ItemType<Tier2JotaroLeggins>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Star Platinum became faster";
            StandoPlayer pl = player.GetModPlayer<StandoPlayer>();
            pl.HaveStandUpSet = true;
            pl.StandJotaroSetBonus = 1;
        }

        public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor)
        {
            drawPlayer.yoraiz0rDarkness = true;
            base.DrawArmorColor(drawPlayer, shadow, ref color, ref glowMask, ref glowMaskColor);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.AdamantiteBar, 12);
            recipe.AddIngredient(ModContent.ItemType<JotaroCap>(), 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();

            recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.TitaniumBar, 12);
            recipe.AddIngredient(ModContent.ItemType<JotaroCap>(), 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}