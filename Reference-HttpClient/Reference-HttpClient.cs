using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Reference_HttpClient
{
    class Reference_HttpClient
    {
        static void Main(string[] args)
        {
            Client client = new Client();

            for (int i = 0; i < 10; i++)
            {
                // Using Task
                Task.Factory.StartNew(() =>
                {
                    client.TaskGet();
                });

                // Using Thread
                Thread thread = new Thread(client.ThreadGet);
                thread.IsBackground = true;
                thread.Start();
            }
            Console.ReadLine();
        }


    }

    class Client
    {
        public Task TaskGet()
        {
            Console.WriteLine("Task start");
            string responseString = string.Empty;
            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    Task<HttpResponseMessage> httpResponse = httpClient.GetAsync("http://localhost:8080/get");
                    HttpResponseMessage msg = httpResponse.Result;
                    responseString = msg.Content.ReadAsStringAsync().Result;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            Console.WriteLine("Task end");
            Console.WriteLine(responseString);

            return Task.CompletedTask;
        }

        public void ThreadGet(object state)
        {
            Console.WriteLine("Thread start");
            string responseString = string.Empty;
            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    Task<HttpResponseMessage> httpResponse = httpClient.GetAsync("http://localhost:8080/get");
                    HttpResponseMessage msg = httpResponse.Result;
                    responseString = msg.Content.ReadAsStringAsync().Result;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
            Console.WriteLine("Thread end");
            Console.WriteLine(responseString);
        }
    }
}
