using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading;


namespace LServer
{
	class Program
	{
		static void Main(string[] args)
		{
			
			/*if (args.Length != 1 || !Regex.Match(args[0], @"^\d{1,5}$").Success)
			{
				Console.WriteLine("usage : LServer.exe port-nr");
				Environment.Exit(0);
			}*/
			
			
			
			AsymTCPServer lServer = new AsymTCPServer(11000); //int.Parse(args[])
			lServer.StartServer();
			
			Console.WriteLine("Hit Ctrl+C to stop the server.");
			while (lServer.IsRunning)
				Thread.Sleep(500);
		}
	}
	
	
	
	
	
	/*
     * 
     * 
     */
	class AsymTCPServer
	{
		private int mPort;
		private byte[] data = new byte[1024];
		private Socket mServer;
		
		public bool IsRunning { get; set; }
		
		
		
		/*
         * 
         * 
         */
		public AsymTCPServer(int pPort)
		{
			mPort = pPort;
			IsRunning = false;
		}
		
		
		/*
         * 
         * 
         */
		public void StartServer()
		{
			mServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, 
			                     ProtocolType.Tcp);
			IPEndPoint iep = new IPEndPoint(IPAddress.Any, mPort);
			mServer.Bind(iep);
			mServer.Listen(5);
			mServer.BeginAccept(new AsyncCallback(AcceptConn), mServer);
			
			IsRunning = true;
		}
		
		
		
		/*
         * 
         * 
         */
		void AcceptConn(IAsyncResult pAsyncRes)
		{
			Socket lClient = ((Socket)pAsyncRes.AsyncState).EndAccept(pAsyncRes);
			Console.WriteLine("Connected to : {0}", lClient.RemoteEndPoint.ToString());
			string stringData = "EHLO";
			byte[] message1 = Encoding.ASCII.GetBytes(stringData);
			lClient.BeginSend(message1, 0, message1.Length, SocketFlags.None,
			                  new AsyncCallback(SendData), lClient);
			
			mServer.BeginAccept(new AsyncCallback(AcceptConn), mServer);
		}
		
		
		
		/*
         * 
         * 
         */
		void SendData(IAsyncResult pAsyncRes)
		{
			Socket client = (Socket) pAsyncRes.AsyncState;
			int lBytesRead = client.EndSend(pAsyncRes);
			
			client.BeginReceive(data, 0, data.Length, SocketFlags.None,
			                    new AsyncCallback(ReceiveData), client);
		}
		
		
		
		/*
         * 
         * 
         */
		void ReceiveData(IAsyncResult pAsyncRes)
		{
			Socket lClient = (Socket) pAsyncRes.AsyncState;
			int lBytesRead = 0;
			byte[] lRecvBuff;
			string lDataRead;
			
			try
			{
				if ((lBytesRead = lClient.EndReceive(pAsyncRes)) == 0)
				{
					lClient.Close();
					return;
				}
				
				lDataRead = Encoding.ASCII.GetString(data, 0, lBytesRead).TrimEnd();
				lRecvBuff = Encoding.ASCII.GetBytes(lDataRead);

				Console.Write(lDataRead);


				if (lDataRead.Contains (";")) { 

					Console.WriteLine("One dropped: {0}",lClient.RemoteEndPoint.ToString());
					lClient.Close();
					return;
				}

				// Echo data back.
				lClient.BeginSend(lRecvBuff, 0, lRecvBuff.Length, SocketFlags.None,
				                  new AsyncCallback(SendData), lClient);



			}
			catch (Exception lEX)
			{
				Console.WriteLine("Exception occurred ({0}). Closing connection.", 
				                  lEX.ToString());
				lClient.Close();
				return;
			}
		}
	}
}
