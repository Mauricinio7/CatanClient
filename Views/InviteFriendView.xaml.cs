﻿using Autofac;
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
    /// Lógica de interacción para InviteFriendView.xaml
    /// </summary>
    public partial class InviteFriendView : UserControl
    {
        public InviteFriendView(string accesKey)
        {
            InitializeComponent();
            this.DataContext = App.Container.Resolve<InviteFriendViewModel>(
                new TypedParameter(typeof(string), accesKey));
        }
    }
}
