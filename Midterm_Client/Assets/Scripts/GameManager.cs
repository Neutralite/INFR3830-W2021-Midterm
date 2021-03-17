using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// Lecture 4
using System;
using System.Net;
using System.Net.Sockets;

// Using code from https://youtu.be/IRAeJgGkjHk
public class GameManager : MonoBehaviour
{

    //networking variables
    public GameObject myCube;
    public string serverAddress;
    public static bool clientRunning = false;
    private static byte[] outBuffer = new byte[512];
    private static IPAddress ip;
    private static IPEndPoint remoteEP;
    private static Socket client_socket_udp;
    private static Socket client_socket_tcp;
    private static float[] pos = new float[3];
    private static float elapsedTime;
    private static float delayTime = 1f;

    //chat variables
    public GameObject chatPanel, textObject;
    public InputField chatBox;
    public int maxMessages = 25;
    [SerializeField]
    List<Message> messageList = new List<Message>();

    // Start is called before the first frame update
    void Start()
    {
        SendMessageToChat("Please enter SERVER IP address.");
        chatBox.ActivateInputField();
    }

    // Update is called once per frame
    void Update()
    {
        if (chatBox.text != "")
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SendMessageToChat(chatBox.text);
                chatBox.text = "";
            }
        }
        if (serverAddress == "" && messageList[1] != null)
        {
            serverAddress = messageList[1].text;
        }
        if (serverAddress != "" && clientRunning == false)
        {
            RunClient(serverAddress);
        }
        //if (!chatBox.isFocused)
        //{
        //    if (Input.GetKeyDown(KeyCode.Space))
        //    {
        //        SendMessageToChat("You pressed the space bar."/*, Message.MessageType.info*/);
        //        Debug.Log("Space");
        //        chatBox.text = "";
        //    }
        //}
        if (clientRunning)
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

                    client_socket_udp.SendTo(bpos, remoteEP);

                    Debug.Log("Sent Coordinates!");
                }
                elapsedTime = 0;
            }
        }

    }

    public void SendMessageToChat (string text)
    {
        if (messageList.Count >= maxMessages)
        {
            Destroy(messageList[0].textObject.gameObject);
            messageList.Remove(messageList[0]);
        }

        Message newMessage = new Message();

        newMessage.text = text;

        GameObject newText = Instantiate(textObject, chatPanel.transform);

        newMessage.textObject = newText.GetComponent<Text>();

        newMessage.textObject.text = newMessage.text;
        //newMessage.textObject.color = MessageTypeColor(messageType);

        messageList.Add(newMessage);
    }

    public static void RunClient(string _serverAddress)
    {

        //ip = IPAddress.Parse("127.0.0.1");
        ip = IPAddress.Parse(_serverAddress);

        remoteEP = new IPEndPoint(ip, 11112);

        client_socket_udp = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        //client_socket_tcp = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);


        clientRunning = true;
    }
}

[System.Serializable]
public class Message
{
    public string text;
    public Text textObject;
}