using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CatanClient.Commands
{
    public class AsyncRelayCommand : ICommand
    {
        private readonly Func<object, Task> executeAsync;
        private readonly Predicate<object> canExecute;
        private readonly Func<Task> executeNoParamAsync;
        private bool isExecuting;

        public AsyncRelayCommand(Func<object, Task> executeAsync, Predicate<object> canExecute = null)
        {
            this.executeAsync = executeAsync ?? throw new ArgumentNullException(nameof(executeAsync));
            this.canExecute = canExecute;
        }

        public AsyncRelayCommand(Func<Task> executeNoParamAsync)
            : this(o => executeNoParamAsync(), null)
        {
            this.executeNoParamAsync = executeNoParamAsync ?? throw new ArgumentNullException(nameof(executeNoParamAsync));
        }

        public bool CanExecute(object parameter)
        {
            return !isExecuting && (canExecute == null || canExecute(parameter));
        }

        public async void Execute(object parameter)
        {
            isExecuting = true;
            RaiseCanExecuteChanged();

            try
            {
                if (executeAsync != null)
                {
                    await executeAsync(parameter);
                }
                else if (executeNoParamAsync != null)
                {
                    await executeNoParamAsync();
                }
            }
            finally
            {
                isExecuting = false;
                RaiseCanExecuteChanged();
            }
        }

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
