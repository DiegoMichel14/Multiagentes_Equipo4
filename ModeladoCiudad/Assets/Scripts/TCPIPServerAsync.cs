using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class TCPIPServerAsync : MonoBehaviour
{

public static string receivedData;
public static string data;
// Use this for initialization

System.Threading.Thread SocketThread;
volatile bool keepReading = false;

void Start()
{
    Application.runInBackground = true;
    startServer();
}

void startServer()
{
    SocketThread = new System.Threading.Thread(networkCode);
    SocketThread.IsBackground = true;
    SocketThread.Start();
}



private string getIPAddress()
{
    IPHostEntry host;
    string localIP = "";
    host = Dns.GetHostEntry(Dns.GetHostName());
    foreach (IPAddress ip in host.AddressList)
    {
        if (ip.AddressFamily == AddressFamily.InterNetwork)
        {
            localIP = ip.ToString();
        }

    }
    return localIP;
}


Socket listener;
Socket handler;

void networkCode()
{
    

    // Data buffer for incoming data.
    byte[] bytes = new Byte[1024];

    // host running the application.
    //Create EndPoint
	 IPAddress IPAdr = IPAddress.Parse("127.0.0.1"); // Dirección IP
	 IPEndPoint localEndPoint = new IPEndPoint(IPAdr, 1104);

    // Create a TCP/IP socket.
    listener = new Socket(AddressFamily.InterNetwork,SocketType.Stream, ProtocolType.Tcp);

    // Bind the socket to the local endpoint and 
    // listen for incoming connections.

    try
    {
        listener.Bind(localEndPoint);
        listener.Listen(10);

        // Start listening for connections.
        while (true)
        {
            keepReading = true;

            // Program is suspended while waiting for an incoming connection.
            Debug.Log("Waiting for Connection");     //It works

            handler = listener.Accept();
            Debug.Log("Client Connected");     //It doesn't work
            data = null;
				
				byte[] SendBytes = System.Text.Encoding.Default.GetBytes("I will send key");
			   handler.Send(SendBytes); // dar al cliente
			
			

            // An incoming connection needs to be processed.
            while (keepReading)
            {
                bytes = new byte[1024];
                int bytesRec = handler.Receive(bytes);
                
                if (bytesRec <= 0)
                {
                    keepReading = false;
                    handler.Disconnect(true);
                    break;
                }

                data += System.Text.Encoding.ASCII.GetString(bytes, 0, bytesRec);
					 //Debug.Log("Received from Server: "+data);
                     receivedData = data;

                if (data.IndexOf("<EOF>") > -1)
                {
                    break;
                }

                System.Threading.Thread.Sleep(1);
            }

            System.Threading.Thread.Sleep(1);
        }
    }
    catch (Exception e)
    {
        Debug.Log(e.ToString());
    }
}

void stopServer()
{
    keepReading = false;

    //stop thread
    if (SocketThread != null)
    {
        SocketThread.Abort();
    }

    if (handler != null && handler.Connected)
    {
        handler.Disconnect(false);
        Debug.Log("Disconnected!");
    }
}

void OnDisable()
{
    stopServer();
}

void Update(){
    receivedData = data;
}
    
}
