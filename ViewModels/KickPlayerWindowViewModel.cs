using Autofac;
using CatanClient.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatanClient.ViewModels
{
    internal class KickPlayerWindowViewModel : ViewModelBase
    {
        public ObservableCollection<KickPlayerCardViewModel> Friends { get; }

        public KickPlayerWindowViewModel()
        {
            Friends = new ObservableCollection<KickPlayerCardViewModel>
            {
                    App.Container.Resolve<KickPlayerCardViewModel>(
                        new NamedParameter("playerName", "TonyGamer54"), new NamedParameter("isOnline", true)),
                App.Container.Resolve<KickPlayerCardViewModel>(
                    new NamedParameter("playerName", "GaboGamer81"), new NamedParameter("isOnline", false)),
                App.Container.Resolve<KickPlayerCardViewModel>(
                    new NamedParameter("playerName", "YaelGamer91"), new NamedParameter("isOnline", true)),
                App.Container.Resolve<KickPlayerCardViewModel>(
                    new NamedParameter("playerName", "NoelGamer761"), new NamedParameter("isOnline", false))
            };

        }
    }
}
