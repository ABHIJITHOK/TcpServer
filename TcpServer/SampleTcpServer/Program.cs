using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SampleTcpServer
{
    public class Program
    {
        private const int portNum = 13;

        public static int Main(string[] args)
        {
            bool done = false;

            TcpListener listener = new TcpListener(Dns.GetHostAddresses(Environment.MachineName)[0], portNum);

            TcpClient client = null;

            NetworkStream ns = null;

            byte[] byteTime = Encoding.ASCII.GetBytes(DateTime.Now.ToString());
            byte[] bytes = null;
            listener.Start();

            while (!done)
            {
                byteTime = Encoding.ASCII.GetBytes(DateTime.Now.ToString());
                try
                {
                    Console.Write("Waiting for connection...");
                    client = listener.AcceptTcpClient();

                    Console.WriteLine($"Connection accepted.");
                    ns = client.GetStream();
                    
                    //Send data to Client
                    Console.WriteLine("Sending current date time to Client");
                    ns.Write(byteTime, 0, byteTime.Length);

                    //Read data from Client
                    bytes = new byte[1024];
                    int bytesRead = ns.Read(bytes, 0, bytes.Length);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Received message from Client:\n{Encoding.ASCII.GetString(bytes, 0, bytesRead)}");
                    Console.ForegroundColor = ConsoleColor.White;
                    ns.Close();
                    client.Close();
                }
                catch (Exception e1)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(e1.ToString());
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            listener.Stop();
            return 0;
        }

    }
}
