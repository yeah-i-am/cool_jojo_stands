using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace cool_jojo_stands
{
    class StandUI : UIState
    {
        public UIText 
            lvlNumText = new UIText("239", 0.9f), // Level text field
            xpNumText = new UIText("239", 0.6f);  // XP text field

        /* Separate elements */
        public UIPanel panel = new UIPanel();
        public UIProgressBar xpProgressBar = new UIProgressBar();
        public UIImageButton hideUiButton;

        Texture2D buttonTexture;    // Hide UI button texture
        const float SlideSpeed = 8; // Hide UI speed
        bool IsHideUI = true;       // UI hided flaq

        /* Initialize UI function */
        public override void OnInitialize()
        {
            panel.Width.Set(120, 0);
            panel.Height.Set(75, 0);
            panel.BackgroundColor = new Color(0.2f, 0.4f, 0.6f, 0.7f);
            Append(panel);

            UIText lvlText = new UIText("LVL");
            lvlText.VAlign = 0f;
            lvlText.HAlign = 0f;
            panel.Append(lvlText);

            UIText xpText = new UIText("XP");
            xpText.VAlign = 1f;
            xpText.HAlign = 0f;
            panel.Append(xpText);

            UIPanel lvlPanel = new UIPanel();
            lvlPanel.Width.Set(55, 0);
            lvlPanel.Height.Set(24, 0);
            lvlPanel.HAlign = 1f;
            lvlPanel.Top.Set(-4, 0);
            panel.Append(lvlPanel);

            UIPanel xpPanel = new UIPanel();
            xpPanel.Width.Set(73, 0);
            xpPanel.Height.Set(24, 0);
            xpPanel.HAlign = 1f;
            xpPanel.VAlign = 1f;
            xpPanel.Top.Set(4, 0);
            panel.Append(xpPanel);

            lvlNumText.HAlign = 0.5f;
            lvlNumText.VAlign = 0.5f;
            lvlPanel.Append(lvlNumText);

            xpNumText.HAlign = 0.5f;
            xpNumText.VAlign = 0.5f;
            xpPanel.Append(xpNumText);

            xpProgressBar.Width.Set(103, 0);
            xpProgressBar.Height.Set(2, 0);
            Append(xpProgressBar);

            /* Set color of button texture */
            CreateHideUIButtonTexture();

            /* Create button */
            hideUiButton = new UIImageButton(buttonTexture);

            hideUiButton.SetVisibility(1f, 0.7f);
            hideUiButton.OnClick += OnButtonClick;
            Append(hideUiButton);

            SetUIPos();
        }

        /* Update UI function */
        public override void Update(GameTime gameTime)
        {
            /* Set lvl and xp info */
            lvlNumText.SetText(Main.LocalPlayer.GetModPlayer<StandoPlayer>().GetStandLevel());
            xpNumText.SetText(Main.LocalPlayer.GetModPlayer<StandoPlayer>().GetStandXP());
            xpProgressBar.SetProgress(Main.LocalPlayer.GetModPlayer<StandoPlayer>().GetStandXPProgress());

            /* Block left click if mouse on button */
            if (hideUiButton.IsMouseHovering)
                Main.blockMouse = true;

            /* Hide animation */
            HideAnimation();

            base.Update(gameTime);
        }

        /* Hide animation function */
        void HideAnimation()
        {
            switch (cool_jojo_stands.StandClientConfig.LvlPos)
            {
                case UIPos.Right:
                    if (IsHideUI && panel.Left.Pixels < 120)
                    {
                        float Speed = Math.Min(120f - panel.Left.Pixels, SlideSpeed);

                        panel.Left.Pixels += Speed;
                        xpProgressBar.Left.Pixels += Speed;
                        hideUiButton.Left.Pixels += Speed;
                    }
                    else if (!IsHideUI && panel.Left.Pixels > 0)
                    {
                        float Speed = Math.Min(panel.Left.Pixels, SlideSpeed);

                        panel.Left.Pixels -= Speed;
                        xpProgressBar.Left.Pixels -= Speed;
                        hideUiButton.Left.Pixels -= Speed;
                    }
                    break;

                case UIPos.Left:
                    if (IsHideUI && panel.Left.Pixels > -120)
                    {
                        float Speed = Math.Min(120f + panel.Left.Pixels, SlideSpeed);

                        panel.Left.Pixels -= Speed;
                        xpProgressBar.Left.Pixels -= Speed;
                        hideUiButton.Left.Pixels -= Speed;
                    }
                    else if (!IsHideUI && panel.Left.Pixels < 0)
                    {
                        float Speed = Math.Min(-panel.Left.Pixels, SlideSpeed);

                        panel.Left.Pixels += Speed;
                        xpProgressBar.Left.Pixels += Speed;
                        hideUiButton.Left.Pixels += Speed;
                    }
                    break;

                case UIPos.Top:
                    if (IsHideUI && panel.Top.Pixels > -75)
                    {
                        float Speed = Math.Min(75f + panel.Top.Pixels, SlideSpeed);

                        panel.Top.Pixels -= Speed;
                        xpProgressBar.Top.Pixels -= Speed;
                        hideUiButton.Top.Pixels -= Speed;
                    }
                    else if (!IsHideUI && panel.Top.Pixels < 0)
                    {
                        float Speed = Math.Min(-panel.Top.Pixels, SlideSpeed);

                        panel.Top.Pixels += Speed;
                        xpProgressBar.Top.Pixels += Speed;
                        hideUiButton.Top.Pixels += Speed;
                    }
                    break;

                case UIPos.Bottom:
                    if (IsHideUI && panel.Top.Pixels < 75)
                    {
                        float Speed = Math.Min(75f - panel.Top.Pixels, SlideSpeed);

                        panel.Top.Pixels += Speed;
                        xpProgressBar.Top.Pixels += Speed;
                        hideUiButton.Top.Pixels += Speed;
                    }
                    else if (!IsHideUI && panel.Top.Pixels > 0)
                    {
                        float Speed = Math.Min(panel.Top.Pixels, SlideSpeed);

                        panel.Top.Pixels -= Speed;
                        xpProgressBar.Top.Pixels -= Speed;
                        hideUiButton.Top.Pixels -= Speed;
                    }
                    break;
            }

        }

        /* Processing hide UI button click function */
        private void OnButtonClick(UIMouseEvent evt, UIElement listeningElement)
        {
            IsHideUI = !IsHideUI;
        }

        /* Create a texture for hide UI button function */
        private void CreateHideUIButtonTexture()
        {
            buttonTexture = ModContent.GetTexture("cool_jojo_stands/Textures/button");
            Color[] OldTextureData = new Color[buttonTexture.Height * buttonTexture.Width];
            Color[] NewTextureData = new Color[buttonTexture.Height * buttonTexture.Width];

            buttonTexture.GetData<Color>(OldTextureData);

            for (int i = 0; i < OldTextureData.Length; i++)
            {
                OldTextureData[i].R = (byte)(OldTextureData[i].R * 0.2f);
                OldTextureData[i].G = (byte)(OldTextureData[i].G * 0.4f);
                OldTextureData[i].B = (byte)(OldTextureData[i].B * 0.6f);
            }

            switch (cool_jojo_stands.StandClientConfig.LvlPos)
            {
                case UIPos.Right:
                    buttonTexture.SetData<Color>(OldTextureData);
                    return;

                case UIPos.Left:
                    for (int y = 0; y < buttonTexture.Height; y++)
                      for (int x = 0; x < buttonTexture.Width; x++)
                        {
                            int OldIndex = y * buttonTexture.Width + x;
                            int NewIndex = y * buttonTexture.Width + buttonTexture.Width - 1 - x;

                            NewTextureData[NewIndex] = OldTextureData[OldIndex];
                        }
                    break;

                case UIPos.Bottom:
                    buttonTexture = new Texture2D(Main.graphics.GraphicsDevice, buttonTexture.Height, buttonTexture.Width);

                    for (int y = 0; y < buttonTexture.Height; y++)
                        for (int x = 0; x < buttonTexture.Width; x++)
                        {
                            int OldIndex = x * buttonTexture.Height + y;
                            int NewIndex = y * buttonTexture.Width + x;

                            NewTextureData[NewIndex] = OldTextureData[OldIndex];
                        }
                    break;

                case UIPos.Top:
                    buttonTexture = new Texture2D(Main.graphics.GraphicsDevice, buttonTexture.Height, buttonTexture.Width);

                    for (int y = 0; y < buttonTexture.Height; y++)
                        for (int x = 0; x < buttonTexture.Width; x++)
                        {
                            int OldIndex = x * buttonTexture.Height + y;
                            int NewIndex = (buttonTexture.Height - 1 - y) * buttonTexture.Width + x;

                            NewTextureData[NewIndex] = OldTextureData[OldIndex];
                        }
                    break;
            }

            buttonTexture.SetData<Color>(NewTextureData);
        }

        /* Set UI position function */
        private void SetUIPos()
        {
            switch (cool_jojo_stands.StandClientConfig.LvlPos)
            {
                case UIPos.Right:
                    panel.VAlign = 0.5f;
                    panel.HAlign = 1f;

                    /* Hide UI button position */
                    hideUiButton.Left.Set(-118, 0);

                    /* XP progress bar position */
                    xpProgressBar.Left.Set(-9, 0);
                    xpProgressBar.Top.Set(34, 0);
                    break;

                case UIPos.Left:
                    panel.VAlign = 0.5f;
                    panel.HAlign = 0f;

                    /* Hide UI button position */
                    hideUiButton.Left.Set(118, 0);

                    /* XP progress bar position */
                    xpProgressBar.Left.Set(9, 0);
                    xpProgressBar.Top.Set(34, 0);
                    break;

                case UIPos.Top:
                    panel.VAlign = 0f;
                    panel.HAlign = 0.5f;

                    /* Hide UI button position */
                    hideUiButton.Top.Set(73, 0);

                    /* XP progress bar position */
                    xpProgressBar.Left.Set(0, 0);
                    xpProgressBar.Top.Set(71, 0);
                    break;

                case UIPos.Bottom:
                    panel.VAlign = 1f;
                    panel.HAlign = 0.5f;

                    /* Hide UI button position */
                    hideUiButton.Top.Set(-73, 0);

                    /* XP progress bar position */
                    xpProgressBar.Left.Set(0, 0);
                    xpProgressBar.Top.Set(-2, 0);
                    break;

            }

            /* Hide UI button position */
            hideUiButton.VAlign = panel.VAlign;
            hideUiButton.HAlign = panel.HAlign;

            /* XP progress bar position */
            xpProgressBar.VAlign = panel.VAlign;
            xpProgressBar.HAlign = panel.HAlign;
        } /* End of 'SetUIPos' function */
    } /* End of 'StandUI' class */
}
