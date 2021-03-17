using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Lecture 4
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;


public class client : MonoBehaviour
{

    public GameObject myCube;
    private static byte[] outBuffer = new byte[512];
    private static IPAddress ip;
    private static IPEndPoint remoteEP;
    private static Socket client_socket;
    private static float[] pos = new float[3];
    private static float elapsedTime;
    private static float delayTime = 1f;


    public static void RunClient()
    {
        
        ip = IPAddress.Parse("127.0.0.1"); 

        remoteEP = new IPEndPoint(ip, 11111);

        client_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
    }

    // Start is called before the first frame update
    void Start()
    {        
        RunClient();
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= delayTime)
        {
            if (pos[0] != myCube.transform.position.x || pos[1] != myCube.transform.position.y || pos[2] != myCube.transform.position.z)
            {
                pos[0] = myCube.transform.position.x;
                pos[1] = myCube.transform.position.y;
                pos[2] = myCube.transform.position.z;
                byte[] bpos = new byte[pos.Length * 4];

                // From https://answers.unity.com/questions/683693/converting-vector3-to-byte.html,
                // there's this nifty Buffer.BlockCopy trick.

                //Buffer.BlockCopy(BitConverter.GetBytes(myCube.transform.position.x), 0, outBuffer, 0 * sizeof(float), sizeof(float));
                //Buffer.BlockCopy(BitConverter.GetBytes(myCube.transform.position.y), 0, outBuffer, 1 * sizeof(float), sizeof(float));
                //Buffer.BlockCopy(BitConverter.GetBytes(myCube.transform.position.z), 0, outBuffer, 2 * sizeof(float), sizeof(float));

                Buffer.BlockCopy(pos, 0, bpos, 0, bpos.Length);

                client_socket.SendTo(bpos, remoteEP);

                Debug.Log("Sent Coordinates!");
            }
            elapsedTime = 0;
        }

    }
}
