using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Lecture 4
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;


public class server : MonoBehaviour
{

    public GameObject myCube;

    //private static byte[] outBuffer = new byte[512];
    //private static Socket client_socket;

    private static byte[] buffer = new byte[512];
    private static IPHostEntry host;
    private static IPAddress ip;
    private static IPEndPoint localEP;
    private static Socket server_socket;
    private static IPEndPoint remoteEP;
    private static EndPoint remoteClient;
    private static int rec = 0;
    //private float posx = 0f;
    //private float posy = 0f;
    //private float posz = 0f;

    public static void RunServer()
    {

        host = Dns.GetHostEntry(Dns.GetHostName());
        //ip = host.AddressList[1];
        ip = IPAddress.Parse("127.0.0.1"); 

        Debug.Log("Server name: " + host.HostName + "  IP: " + ip);

        localEP = new IPEndPoint(ip, 11111);

        server_socket = new Socket(ip.AddressFamily, SocketType.Dgram, ProtocolType.Udp);

        remoteEP = new IPEndPoint(IPAddress.Any, 0); // 0 represents any available port
        remoteClient = (EndPoint)remoteEP;

        server_socket.Bind(localEP);

        Debug.Log("Waiting for data...");
    }

    // Start is called before the first frame update
    void Start()
    {
        myCube = GameObject.Find("Cube");
        //posx = myCube.transform.position.x;
        //posy = myCube.transform.position.y;
        //posz = myCube.transform.position.z;

        RunServer();

        // non-blocking socket mode
        server_socket.Blocking = false;
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            rec = server_socket.ReceiveFrom(buffer, ref remoteClient);

            //Debug.Log("Received x: " + Encoding.ASCII.GetString(buffer, 0, rec) + "  from Client: " + remoteClient.ToString());

            //rec = server_socket.ReceiveFrom(buffer, ref remoteClient);

            //Debug.Log("Received y: " + Encoding.ASCII.GetString(buffer, 0, rec) + "  from Client: " + remoteClient.ToString());
        }
        catch (SocketException e)
        {
            Debug.Log("Exception: " + e.ToString());
        }

        //float[] pos = new float[rec / 4];
        float[] pos = new float[3];
        Buffer.BlockCopy(buffer, 0, pos, 0, rec);
        //posx = BitConverter.ToSingle(buffer, 0);
        //posy = BitConverter.ToSingle(buffer, 1 * sizeof(float));
        //posz = BitConverter.ToSingle(buffer, 2 * sizeof(float));

        //Debug.Log("Received: X: " + posx + "  Y: " + posy + "  Z: " + posz + "  from Client: " + remoteClient.ToString());

        myCube.transform.position = new Vector3(pos[0], pos[1], pos[2]);
    }







}
