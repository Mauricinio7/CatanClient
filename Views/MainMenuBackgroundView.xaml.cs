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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfAnimatedGif;

namespace CatanClient.Views
{
  
    public partial class MainMenuBackgroundView : UserControl
    {
        public MainMenuBackgroundView()
        {
            InitializeComponent();
            Load_Gif();
        }

        private void Load_Gif()
        {
            var imageUri = new Uri("pack://application:,,,/Resources/Gifs/SynthWaveAnimatedBackground2.gif", UriKind.Absolute);
            var imageSource = new BitmapImage(imageUri);
            ImageBehavior.SetAnimatedSource(SynthWaveAnimatedBackground2, imageSource);
        }
    }
}
