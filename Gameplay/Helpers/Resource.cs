using CatanClient.Commands;
using CatanClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatanClient.Gameplay.Helpers
{
    internal class Resource : ViewModelBase
    {
        private int quantity;
        public string Name { get; set; }
        public string ImageSource { get; set; }

        public int Quantity
        {
            get => quantity;
            set
            {
                if (quantity != value)
                {
                    quantity = value;
                    OnPropertyChanged(nameof(Quantity)); 
                    IncreaseCommand.RaiseCanExecuteChanged();
                    DecreaseCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public RelayCommand IncreaseCommand { get; }
        public RelayCommand DecreaseCommand { get; }

        public Resource()
        {
            IncreaseCommand = new RelayCommand(_ => OnIncrease(), _ => CanIncrease());
            DecreaseCommand = new RelayCommand(_ => OnDecrease(), _ => CanDecrease());
        }

        private void OnIncrease()
        {
            Quantity++;
        }

        private bool CanIncrease()
        {
            return Quantity < 5;
        }

        private void OnDecrease()
        {
            Quantity--;
        }

        private bool CanDecrease()
        {
            return Quantity > 0;
        }

    }
}
