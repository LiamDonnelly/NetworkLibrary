using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkLibrary.Observer
{

    public interface ISubject
    {
        void AddObserver(IObserver observer);
        void RemoveObserver(IObserver observer);
        void Update(object obj);
    }


    public class Subject : ISubject
    {
        IList<IObserver> observerList = new List<IObserver>();

        public void AddObserver(IObserver observer)
        {
            observerList.Add(observer);
        }

        public void RemoveObserver(IObserver observer)
        {
            observerList.Remove(observer);
        }

        public void Update(object obj)
        {
            observerList.ToList().ForEach(obs => obs.Update(obj));
        }
    }

    public interface IObserver
    {
        void Update(object obj);
    }

    public class NetworkHealth : IObserver
    {
        public string title;
        public bool connected;
        

        public NetworkHealth(string name, bool connected)
        {
            this.connected = connected;
            title = name;
        }

        public void Update(object obj)
        {
            if(connected)
            {
                connected = false;
            }
            else
            {
                connected = true;
            }
        }
    }
}
