using Autofac;
using CatanClient.ChatService;
using CatanClient.Gameplay.ViewModels;
using CatanClient.ViewModels;
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

namespace CatanClient.Gameplay.Views
{
    /// <summary>
    /// Lógica de interacción para GameFrameView.xaml
    /// </summary>
    public partial class GameFrameView : UserControl
    {
        private GameFrameViewModel viewModel;
        public GameFrameView(GameDto game)
        {
            InitializeComponent();
            viewModel = App.Container.Resolve<GameFrameViewModel>(
               new TypedParameter(typeof(ChatService.GameDto), game)
            );
            this.DataContext = viewModel;

            viewModel.VertexOccupied += OnVertexOccupied;
            viewModel.EdgeOccupied += OnEdgeOccupied;
        }

        private void OnVertexOccupied(string tag, bool haveCity)
        {
            var button = FindButtonByTag(tag);
            if (button != null)
            {
                if (haveCity)
                {
                    button.Content = new Image
                    {
                        Source = new BitmapImage(new Uri("pack://application:,,,/Gameplay/Resources/Images/GameResources/City.png")),
                        Stretch = Stretch.Uniform
                    };
                }
                else
                {
                    button.Content = new Image
                    {
                        Source = new BitmapImage(new Uri("pack://application:,,,/Gameplay/Resources/Images/GameResources/Town.png")),
                        Stretch = Stretch.Uniform
                    };
                }

                button.Background = new SolidColorBrush(Colors.Transparent);
            }
        }

        private void OnEdgeOccupied(string tag)
        {
            var button = FindButtonByTag(tag);
            if (button != null)
            {
                button.Content = new Image
                {
                    Source = new BitmapImage(new Uri("pack://application:,,,/Gameplay/Resources/Images/GameResources/Road.png")),
                    Stretch = Stretch.Uniform
                };

                button.Background = new SolidColorBrush(Colors.Transparent);
            }
        }

        private Button FindButtonByTag(string tag)
        {
            foreach (var child in MainGrid.Children)
            {
                if (child is Button button && button.Tag?.ToString() == tag)
                {
                    return button;
                }
            }
            return null; 
        }


    }
}
