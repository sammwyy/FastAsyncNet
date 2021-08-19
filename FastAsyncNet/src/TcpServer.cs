using System.Collections.Generic;
using System.Net;
using System;
using System.Net.Sockets;
using System.Threading;

namespace FastAsyncNet
{
    public class TcpServer : IDisposable
    {
        private TcpListener _listener;

        private Thread _thread;

        private Thread[] _workers;

        private ManualResetEvent _stop, _ready;
        private Queue<TcpClient> _queue;
        private ServerHandler _handler;

        public TcpServer(int port, string host, int threadPoolSize)
        {
            this._stop = new ManualResetEvent(false);
            this._ready = new ManualResetEvent(false);
            this._listener = new TcpListener(IPAddress.Parse(host), port);
            this._thread = new Thread(Handle);
            this._queue = new Queue<TcpClient>();
            this._workers = new Thread[threadPoolSize];
        }

        public TcpServer(int port, string host) : this(port, host, 128) { }
        public TcpServer(int port) : this(port, "127.0.0.1", 128) { }

        public void SetHandler(ServerHandler handler)
        {
            this._handler = handler;
        }

        private void AcceptClientCallback(IAsyncResult ar)
        {
            try
            {
                lock (this._queue)
                {
                    this._queue.Enqueue(this._listener.EndAcceptTcpClient(ar));
                    this._ready.Set();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void Handle()
        {
            while (true)
            {
                var context = this._listener.BeginAcceptTcpClient(this.AcceptClientCallback, null);
                if (0 == WaitHandle.WaitAny(new[] { this._stop, context.AsyncWaitHandle }))
                {
                    return;
                }
            }
        }

        private void Worker()
        {
            WaitHandle[] wait = new[] { this._ready, this._stop };
            while (0 == WaitHandle.WaitAny(wait))
            {
                TcpClient client;
                lock (this._queue)
                {
                    if (this._queue.Count > 0)
                    {
                        client = this._queue.Dequeue();
                    }
                    else
                    {
                        this._ready.Reset();
                        continue;
                    }
                }

                try
                {
                    TcpConnection connection = new TcpConnection(client, this._handler);
                    this._handler.HandleConnection(connection);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public void Listen()
        {
            if (this._handler == null)
            {
                throw new Exception("No handler defined in Server. You maybe forgot to use server.SetHandler(handler:ServerHandler)?");
            };

            this._listener.Start();
            this._thread.Start();

            for (int i = 0; i < this._workers.Length; i++)
            {
                this._workers[i] = new Thread(this.Worker);
                this._workers[i].Start();
            }
        }

        public void Stop()
        {
            this._listener.Stop();
        }

        public void Dispose()
        {
            this.Stop();
        }
    }
}
