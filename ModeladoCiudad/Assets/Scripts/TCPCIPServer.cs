using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;

public class TCPCIPServer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		 // Crear un socket, 
		 Socket Listefd = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp); 
		 IPAddress IPAdr = IPAddress.Parse("127.0.0.1"); // Dirección IP
		 IPEndPoint ipep = new IPEndPoint(IPAdr, 1103); // Dirección IP y número de puerto
		 Listefd.Bind(ipep); // unir el socket
		 Listefd.Listen(0); // Open Monitor, espera a la conexión
		 Debug.Log("SERVER START");
		while (true)
		{
			// Aceptar la conexión del cliente, los métodos de socket en este ejemplo son métodos bloqueadores
			Socket confd = Listefd.Accept(); 
			Debug.Log("Conexion de cliente");
			byte[] bytes = System.Text.Encoding.Default.GetBytes("I will send key");
			confd.Send(bytes); // dar al cliente
			
			byte[] ReadBuff = new byte[1024];
			int Count = confd.Receive (ReadBuff); // La información recibida está en Readbuff
			string str = System.Text.Encoding.UTF8.GetString(ReadBuff, 0, Count); // Traducir bytes a cadenas
			Debug.Log("servidor recibe: " + str);
			
			if (str.IndexOf("$") > -1)
			 {
				 confd.Shutdown(SocketShutdown.Both);  
             confd.Close();  
				 break;
			 }

      }  
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
