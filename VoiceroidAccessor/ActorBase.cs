using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Threading;

namespace net.azworks.threading
{
    abstract class ActorBase<T>
    {
        public T State { get; set; }

        public void SendMessage(T message)
        {
            this.allMessages.Enqueue(message);

            lock (this)
            {
                if (this.activeTask != null)
                    return;

                activeTask = new Thread(new ThreadStart(ExecuteActions));
                activeTask.SetApartmentState(ApartmentState.STA);
                activeTask.Start();
            }
        }

        private readonly ConcurrentQueue<T> allMessages = new ConcurrentQueue<T>();

        private Thread activeTask;

        public event EventHandler<UnhandledExceptionEventArgs> OnError;

        abstract protected void Act(T message);

        private void ExecuteActions()
        {
            T sample;
            while (allMessages.TryDequeue(out sample))
            {
                this.State = sample;
                try
                {
                    this.Act(sample);
                }
                catch (Exception e)
                {
                    UnhandledExceptionEventArgs except = new UnhandledExceptionEventArgs(e, false);
                    if (this.OnError != null)
                        this.OnError.Invoke(this, except);
                }
            }

            lock (this)
            {
                this.activeTask = null;
            }
        }
    }
}
