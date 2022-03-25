using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Reference_SocketServer
{
    class Reference_SocketServer
    {
        static void Main(string[] args)
        {
            SocketServer server = new SocketServer("127.0.0.1", 9999);
            server.run();
        }
    }

    class SocketServer
    {
        TcpListener Listener;
        IPAddress IP;

        public SocketServer(string ip, int port)
        {
            IP = IPAddress.Parse(ip);
            Listener = new TcpListener(IP, port);
        }

        public void run()
        {
            Listener.Start();
            while (true)
            {
                TcpClient client = Listener.AcceptTcpClient();

                // Using Task
                Task.Factory.StartNew(() =>
                {
                    SocketReceiver.Receive(client);
                });

                // Using thread
                //Thread thread = new Thread(SocketReceiver.Receive);
                //thread.Start(client);
            }
        }
    }

    class SocketReceiver
    {
        public static void Receive(Object parameter)
        {
            TcpClient client = parameter as TcpClient;
            NetworkStream networkStream = client.GetStream();
            StreamReader streamReader = new StreamReader(networkStream, Encoding.UTF8);
            StreamWriter streamWriter = new StreamWriter(networkStream, Encoding.UTF8);

            try
            {
                while (client.Connected)
                {
                    string clientMessage = streamReader.ReadLine();
                    Console.WriteLine("Client Command : " + clientMessage);

                    // Process About client Message

                    streamWriter.WriteLine("Server Result : " + DateTime.Now.ToString());
                    streamWriter.Flush();
                }
            }
            catch(Exception e)
            {
                // Console.WriteLine(e);
            }
            finally
            {
                client.Close();
                networkStream.Close();
                streamReader.Close();
                streamWriter.Close();
            }
        }
    }
}
