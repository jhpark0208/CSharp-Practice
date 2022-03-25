using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Reference_SocketClient
{
    class Reference_SocketClient
    {
        static void Main(string[] args)
        {
            Client.echo();
        }
    }

    class Client
    {
        public static void echo()
        {
            TcpClient client = new TcpClient("127.0.0.1", 9999);
            NetworkStream networkStream = client.GetStream();
            StreamReader streamReader = new StreamReader(networkStream, Encoding.UTF8);
            StreamWriter streamWriter = new StreamWriter(networkStream, Encoding.UTF8);
            try
            {
                string cmd = string.Empty;
                while ((cmd = Console.ReadLine()) != null)
                {
                    streamWriter.WriteLine(cmd);
                    streamWriter.Flush();

                    string serverMessage = streamReader.ReadLine();
                    Console.WriteLine(serverMessage);
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
