using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CatanClient.Gameplay.Views
{
    /// <summary>
    /// Lógica de interacción para DiceResultAnimationView.xaml
    /// </summary>
    public partial class DiceResultAnimationView : UserControl
    {
        public DiceResultAnimationView(int number)
        {
            InitializeComponent();
            ShowNumber(number);
        }

        private void ShowNumber(int number)
        {
            NumberTextBlock.Text = number.ToString();

            var scaleAnimation = new DoubleAnimation
            {
                From = 0.1,     
                To = 1.0,       
                Duration = TimeSpan.FromSeconds(2),
                EasingFunction = new BounceEase 
                {
                    Bounces = 2,
                    Bounciness = 2
                }
            };

            var transform = (ScaleTransform)NumberTextBlock.RenderTransform;
            transform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation);
            transform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation);
        }
    }
}
