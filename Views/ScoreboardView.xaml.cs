using Autofac;
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

namespace CatanClient.Views
{
    /// <summary>
    /// Lógica de interacción para ScoreboardView.xaml
    /// </summary>
    public partial class ScoreboardView : UserControl
    {
        public ScoreboardView()
        {
            InitializeComponent();
            DataContext = App.Container.Resolve<ScoreboardViewModel>();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ScoreboardViewModel viewModel = (ScoreboardViewModel)DataContext;

            TabControl tabControl = sender as TabControl;
            TabItem selectedTab = tabControl.SelectedItem as TabItem;

            if (selectedTab != null)
            {
                switch (selectedTab.Tag.ToString())
                {
                    case "Friends":
                        viewModel.LoadFriendScores();
                        break;
                    case "World":
                        viewModel.LoadWorldScores();
                        break;
                    case "Week":
                        viewModel.LoadWeeklyScores();
                        break;
                }
            }
        }
    }
}
