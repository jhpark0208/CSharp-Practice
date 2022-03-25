using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Reference_HttpServer
{
    class Reference_HttpServer
    {
        static void Main(string[] args)
        {
            HttpServer server = new HttpServer();
            server.Run();
        }
    }

    class HttpServer
    {
        HttpListener Listener;
        public HttpServer()
        {
            Listener = new HttpListener();
            Listener.Prefixes.Add("http://127.0.0.1:8080/");
        }

        public void Run()
        {
            Listener.Start();
            while (true)
            {
                var ctx = Listener.GetContext();

                // Using Task
                Task.Factory.StartNew(() =>
                {
                    Route.TaskRun(ctx);
                });

                // Using Thread
                //Thread thread = new Thread(Route.ThreadRun);
                //thread.Start(ctx);
            }
        }
    }

    class Route
    {
        public Route()
        {
        }

        public static Task TaskRun(HttpListenerContext ctx)
        {
            Console.WriteLine("Using Task");
            if (ctx.Request.HttpMethod == "GET")
                GET.execute(ctx);
            else if (ctx.Request.HttpMethod == "POST")
                POST.execute(ctx);

            return Task.CompletedTask;
        }

        public static void ThreadRun(object parameter)
        {
            Console.WriteLine("Using Thread");
            HttpListenerContext ctx = parameter as HttpListenerContext;
            if (ctx.Request.HttpMethod == "GET")
                GET.execute(ctx);
            else if (ctx.Request.HttpMethod == "POST")
                POST.execute(ctx);
        }
    }

    class GET
    {
        public static void execute(HttpListenerContext ctx)
        {
            string requestURL = ctx.Request.Url.ToString().Replace("http://localhost:8080/", "");
            string query = string.Empty;
            if (requestURL.Contains('?'))
            {
                requestURL = requestURL.Split('?')[0];
                query = requestURL.Split('?')[1];
            }

            /// Request URL에 따라 분기
            /// Example : http://localhost:8080/get -> get
            /// static 함수 추가하고, if문으로 분기 태우면 됨
            if (requestURL == "get")
                SampleGET(ctx, query);
        }
        public static void SampleGET(HttpListenerContext ctx, string query)
        {
            Console.WriteLine("Sample GET");

            byte[] data = Encoding.UTF8.GetBytes("Sample GET");
            ctx.Response.OutputStream.Write(data);
            ctx.Response.StatusCode = 200;
            ctx.Response.Close();
        }

        // ADD GET Function
    }

    class POST
    {
        public static void execute(HttpListenerContext ctx)
        {
            string requestURL = ctx.Request.Url.ToString().Replace("http://localhost:8080/", "");

            /// Request URL에 따라 분기
            /// Example : http://localhost:8080/get -> get
            /// static 함수 추가하고, if문으로 분기 태우면 됨
            /// POST의 경우 context에 Request Content가 포함 되어있음
            if (requestURL == "post")
                SamplePOST(ctx);
        }
        public static void SamplePOST(HttpListenerContext ctx)
        {
            Console.WriteLine("Sample POST");
            StreamReader reader = new StreamReader(ctx.Request.InputStream);
            Console.WriteLine(reader.ReadToEnd());

            byte[] data = Encoding.UTF8.GetBytes("Sample POST");
            ctx.Response.OutputStream.Write(data);
            ctx.Response.StatusCode = 200;
            ctx.Response.Close();
        }

        // ADD POST Function
    }
}
