using CatanClient.UIHelpers;
using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using WpfAnimatedGif;

namespace CatanClient.Gameplay.Views
{
    /// <summary>
    /// Lógica de interacción para WinnerAnimationView.xaml
    /// </summary>
    public partial class WinnerAnimationView : UserControl
    {
        public WinnerAnimationView(string winnerName)
        {
            InitializeComponent();

            WinnerText.Text = string.Empty;

            _ = StartAnimationsAsync(winnerName);
        }

        private async Task StartAnimationsAsync(string winnerName)
        {
            AnimateGradient();

            await Task.Delay(1800);

            AnimateText(winnerName);

            PlayWinnerGif();
        }

        private void PlayWinnerGif()
        {
            try
            {
                Uri gifUri = new Uri(Utilities.WIN_ANIMATION, UriKind.Absolute);
                BitmapImage gifImage = new BitmapImage(gifUri);

                RenderOptions.SetBitmapScalingMode(WinnerGif, BitmapScalingMode.NearestNeighbor);
                ImageBehavior.SetAnimatedSource(WinnerGif, gifImage);
                ImageBehavior.SetRepeatBehavior(WinnerGif, RepeatBehavior.Forever);

                DoubleAnimation fadeInAnimation = new DoubleAnimation
                {
                    From = 0, 
                    To = 1,   
                    Duration = TimeSpan.FromSeconds(1) 
                };

                WinnerGif.BeginAnimation(UIElement.OpacityProperty, fadeInAnimation);
            }
            catch (Exception ex)
            {
                Utilities.ShowMessageDataBaseUnableToLoad();
            }
        }

        private void AnimateGradient()
        {
            DoubleAnimation gradientAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(2),
                AutoReverse = false
            };
            GradientStart.BeginAnimation(GradientStop.OffsetProperty, gradientAnimation);
            GradientEnd.BeginAnimation(GradientStop.OffsetProperty, gradientAnimation);
        }

        private void AnimateText(string winnerName)
        {
            WinnerText.Text = $"{Utilities.WinnerText(CultureInfo.CurrentCulture.Name)}: {winnerName}";
            DoubleAnimation fadeInAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(1)
            };

            DoubleAnimation scaleAnimation = new DoubleAnimation
            {
                From = 0.5,
                To = 1.2,
                Duration = TimeSpan.FromSeconds(1)
            };

            WinnerText.BeginAnimation(UIElement.OpacityProperty, fadeInAnimation);
            WinnerText.RenderTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation);
            WinnerText.RenderTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation);
        }
    }
}