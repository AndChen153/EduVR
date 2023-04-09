using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class hand : MonoBehaviour
{
    Thread receiveThread; //1
	UdpClient client; //2
	int port; //3

    string[] text2;
    bool inFrame;
    int frameCounter;
    void Start()
    {
        port = 5060; //1
		InitUDP(); //4
        inFrame = false;
        frameCounter = 0;

    }
    // 3. InitUDP
	private void InitUDP()
	{
		receiveThread = new Thread (new ThreadStart(ReceiveData)); //1
		receiveThread.IsBackground = true; //2
		receiveThread.Start (); //3
	}

	// 4. Receive Data
	private void ReceiveData()
	{
		client = new UdpClient (port); //1
		while (true) //2
		{
			try
			{
				IPEndPoint anyIP = new IPEndPoint(IPAddress.Parse("0.0.0.0"), port); //3
				byte[] data = client.Receive(ref anyIP); //4

				string text = Encoding.UTF8.GetString(data); //5
				text2 = text.Split(',');

				// print(">>HAND: " + text2[1] + ", " + text2[2] + ", " + text2[3]);
                inFrame = true;
                frameCounter = 0;


			} catch(Exception e)
			{
				print(e.ToString()); //7
			}
		}
	}

	// 5. Make the Player Jump
	public void translate()
	{
        float locX = (float) Convert.ToDouble(text2[1]);
        float locY = (float) Convert.ToDouble(text2[2]);
        float locZ = (float) Convert.ToDouble(text2[3]);
        transform.position = new Vector3(locX, locY, locZ);

    }

    public void zero() {
        transform.position = new Vector3(-100, -100, -100);
    }

    // Update is called once per frame
    void Update()
    {
        // print(frameCounter);
        if (inFrame){
            translate();
            inFrame = false;
        }
        else if (frameCounter > 50) {
            zero();
            frameCounter = 0;
        }
        else {
            frameCounter++;
        }
    }
}
