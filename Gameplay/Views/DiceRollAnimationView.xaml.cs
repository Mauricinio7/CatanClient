using CatanClient.UIHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        public DiceRollAnimationView(int result)
        {
            InitializeComponent();
            StartDiceAnimation(result);
        }
        private void StartDiceAnimation(int result)
        {
            string diceGifPath = Utilities.DICE_ROLL_ANIMATION;
            Uri imageUri = new Uri(diceGifPath, UriKind.Absolute);
            BitmapImage imageSource = new BitmapImage(imageUri);

            RenderOptions.SetBitmapScalingMode(DiceImage, BitmapScalingMode.NearestNeighbor);

            ImageBehavior.SetAnimatedSource(DiceImage, imageSource);

            ImageBehavior.SetRepeatBehavior(DiceImage, new RepeatBehavior(5));

            ImageBehavior.AddAnimationCompletedHandler(DiceImage, async (s, e) =>
            {
                AnimationCompleted?.Invoke(this, EventArgs.Empty);
                await Task.Delay(500);
                ShowDiceResult(result);
            });
        }

        private static void ShowDiceResult(int result)
        {
            App.Current.Dispatcher.InvokeAsync(async () =>
            {
                Mediator.Notify(Utilities.SHOW_DICE_RESULT_ANIMATION, result);
                await Task.Delay(5000);
                Mediator.Notify(Utilities.HIDE_LOADING_SCREEN, null);
            });
        }
    }
}
