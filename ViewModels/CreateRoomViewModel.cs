using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatanClient.ViewModels
{
    internal class CreateRoomViewModel : ViewModelBase
    {
        private string _selectedOption;

        public ObservableCollection<string> OptionsList { get; set; }

        public string SelectedOption
        {
            get { return _selectedOption; }
            set
            {
                if (_selectedOption != value)
                {
                    _selectedOption = value;
                    OnPropertyChanged(nameof(SelectedOption));
                }
            }
        }

        public CreateRoomViewModel()
        {
            OptionsList = new ObservableCollection<string>
            {
                "2",
                "3",
                "4",
            };
        }
    }
}
