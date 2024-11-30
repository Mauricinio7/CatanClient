using Autofac;
using CatanClient.ChatService;
using CatanClient.Gameplay.ViewModels;
using CatanClient.UIHelpers;
using CatanClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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

        private void OnVertexOccupied(string tag, bool haveCity, bool isOwner)
        {
            Button button = FindButtonByTag(tag);
            if (button != null)
            {
                BitmapImage imageSource = haveCity
                    ? new BitmapImage(new Uri(Utilities.CITY_PATH))
                    : new BitmapImage(new Uri(Utilities.TOWN_PATH));

                Image image = new Image
                {
                    Source = imageSource,
                    Stretch = Stretch.Uniform
                };

                if (isOwner)
                {
                    button.Content = ApplyColorFilter(image, Colors.Gold); 
                }
                else
                {
                    button.Content = ApplyColorFilter(image, Colors.Red);
                }

                button.Background = new SolidColorBrush(Colors.Transparent);
            }
        }

        private void OnEdgeOccupied(string tag, bool isOwner)
        {
            Button button = FindButtonByTag(tag);
            if (button != null)
            {
                BitmapImage imageSource = new BitmapImage(new Uri(Utilities.ROAD_PATH));
                    
                Image image = new Image
                {
                    Source = imageSource,
                    Stretch = Stretch.Uniform
                };

                if (isOwner)
                {
                    button.Content = image;
                }
                else
                {
                    button.Content = ApplyColorFilter(image, Colors.Red);
                }

                button.Background = new SolidColorBrush(Colors.Transparent);
            }
        }

        private Button FindButtonByTag(string tag)
        {
            foreach (object child in MainGrid.Children)
            {
                if (child is Button button && button.Tag?.ToString() == tag)
                {
                    return button;
                }
            }
            return null; 
        }

        private UIElement ApplyColorFilter(Image image, Color color)
        {

            Grid container = new Grid();
            container.Children.Add(image);

            Border overlay = new Border
            {
                Background = new SolidColorBrush(color),
                Opacity = 0.5 
            };

            container.Children.Add(overlay);

            return container; 
        }

        private void OnListBoxLoaded(object sender, RoutedEventArgs e)
        {
            ListBox listBox = sender as ListBox;
            if (listBox?.ItemsSource is INotifyCollectionChanged collection)
            {
                collection.CollectionChanged += (s, args) =>
                {
                    if (args.Action == NotifyCollectionChangedAction.Add)
                    {
                        listBox.ScrollIntoView(listBox.Items[listBox.Items.Count - 1]);
                    }
                };
            }
        }


    }
}
