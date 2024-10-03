using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatanClient.UIHelpers
{
    internal class Mediator
    {
        private static IDictionary<string, Action<object>> _actions = new Dictionary<string, Action<object>>();

        public static void Register(string token, Action<object> callback)
        {
            if (!_actions.ContainsKey(token))
            {
                _actions[token] = callback;
            }
        }

        public static void Notify(string token, object args = null)
        {
            if (_actions.ContainsKey(token))
            {
                _actions[token](args);
            }
        }
    }
}
