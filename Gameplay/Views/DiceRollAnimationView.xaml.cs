using CatanClient.UIHelpers;
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
using WpfAnimatedGif;

namespace CatanClient.Gameplay.Views
{
    /// <summary>
    /// Lógica de interacción para DiceRollAnimationView.xaml
    /// </summary>
    public partial class DiceRollAnimationView : UserControl
    {
        public event EventHandler AnimationCompleted;
        public DiceRollAnimationView()
        {
            InitializeComponent();
            StartDiceAnimation();
        }
        private void StartDiceAnimation()
        {
            string diceGifPath = Utilities.DICE_ROLL_ANIMATION;
            var imageUri = new Uri(diceGifPath, UriKind.Absolute);
            var imageSource = new BitmapImage(imageUri);

            RenderOptions.SetBitmapScalingMode(DiceImage, BitmapScalingMode.NearestNeighbor);

            ImageBehavior.SetAnimatedSource(DiceImage, imageSource);
            ImageBehavior.SetRepeatBehavior(DiceImage, new RepeatBehavior(5)); 

            ImageBehavior.AddAnimationCompletedHandler(DiceImage, async (s, e) =>
            {
                AnimationCompleted?.Invoke(this, EventArgs.Empty);
                await Task.Delay(500);
                ShowDiceResult();
            });
        }

        private void ShowDiceResult()
        {
            Random random = new Random();
            int diceResult = random.Next(2, 13); 
            MessageBox.Show($"El resultado de los dados es: {diceResult}");
        }
    }
}
