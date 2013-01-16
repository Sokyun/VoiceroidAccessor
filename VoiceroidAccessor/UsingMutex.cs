using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace net.azworks.threading
{
    public class UsingMutex : IDisposable
    {
        private Mutex mutex;

        public UsingMutex(string mutexName)
        {
            mutex = new Mutex(false, mutexName);
            mutex.WaitOne();
        }

        public void Dispose()
        {
            mutex.ReleaseMutex();
            mutex.Close();
            mutex = null;
        }

        ~UsingMutex()
        {
            if (mutex != null)
            {
                this.Dispose();
            }
        }
    }
}
