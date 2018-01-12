using NetworkLibrary.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace NetworkLibrary.Handler.Action
{
    public class ActionHandler<T>
    {
        private IDictionary<Type, List<Action<T>>> actionList { get; }
        
        public ActionHandler()
        {
            actionList = new Dictionary<Type, List<Action<T>>>();
        }

        public void AddFunction(Type t, Action<T> item)
        {
            if (actionList.ContainsKey(t))
            {
                actionList[t].Add(item);
            }
            else
            {
                actionList.Add(t, new List<Action<T>> { item });
            }
        }

        public void InvokeFunction(Type t, T value)
        {
            try
            {
                actionList[t].ForEach(action => action(value));
            }
            catch (System.Collections.Generic.KeyNotFoundException ex)
            {
                //Add Logger
                Logger.Instance.WriteLog("No Method that handles recieved Type (" + ex.ToString() + ")");
            }
        }
    }
}
