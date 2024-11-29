using CatanClient.UIHelpers;
using System;
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

            WinnerText.Text = $"¡Ganador: {winnerName}!";

            _ = StartAnimationsAsync();
        }

        private async Task StartAnimationsAsync()
        {
            // 1. Animar el gradiente
            AnimateGradient();

            // 2. Pausa para que el gradiente termine
            await Task.Delay(1000);

            // 3. Animar el texto
            AnimateText();

            // 4. Pausa antes de mostrar el GIF
            await Task.Delay(1000);

            // 5. Mostrar el GIF
            PlayWinnerGif();
        }

        private void PlayWinnerGif()
        {
            try
            {
                // Usar el Pack URI para cargar el GIF
                var gifUri = new Uri(Utilities.WIN_ANIMATION, UriKind.Absolute);
                var gifImage = new BitmapImage(gifUri);

                // Configurar la animación del GIF
                RenderOptions.SetBitmapScalingMode(WinnerGif, BitmapScalingMode.NearestNeighbor);
                ImageBehavior.SetAnimatedSource(WinnerGif, gifImage);
                ImageBehavior.SetRepeatBehavior(WinnerGif, RepeatBehavior.Forever); // Repetir infinitamente

                // Aplicar fade-in al GIF
                var fadeInAnimation = new DoubleAnimation
                {
                    From = 0, // Transparente
                    To = 1,   // Opaco
                    Duration = TimeSpan.FromSeconds(1) // Duración de 1 segundo
                };

                WinnerGif.BeginAnimation(UIElement.OpacityProperty, fadeInAnimation);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el GIF: {ex.Message}", "Error");
            }
        }

        private void AnimateGradient()
        {
            var gradientAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(2),
                AutoReverse = false
            };
            GradientStart.BeginAnimation(GradientStop.OffsetProperty, gradientAnimation);
            GradientEnd.BeginAnimation(GradientStop.OffsetProperty, gradientAnimation);
        }

        private void AnimateText()
        {
            var fadeInAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(1)
            };

            var scaleAnimation = new DoubleAnimation
            {
                From = 0.5,
                To = 1.2,
                Duration = TimeSpan.FromSeconds(1)
            };

            WinnerText.BeginAnimation(UIElement.OpacityProperty, fadeInAnimation);
            WinnerText.RenderTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation);
            WinnerText.RenderTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation);
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}