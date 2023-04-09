using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class objFather : MonoBehaviour
{
    public int port; //3
    public Thread receiveThread; //1
	public UdpClient client; //2
    public Rigidbody Cube; //4

	public bool pickup; //6
    public bool inTargetPos;
    public static bool inTargetPosLED;
    public static bool inTargetPosRES;
    public static bool inTargetPosWIRE;
    public static bool inTargetPosBATT;

    public float targetX;
    public float targetY;
    public float targetZ;

    public float targetRX;
    public float targetRY;
    public float targetRZ;

    public float locX;
    public float locY;
    public float locZ;

    public float sizeX;
    public float sizeY;
    public float sizeZ;


    public float spacerConst;

    public string[] text2;

	public void InitUDP()
	{
		print ("UDP Initialized");

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
				if (text2[0] == "True") {
					pickup = true; //6
					// print (">> " + text2[1] + ", " + text2[2] + ", " + text2[3]);
                    locX = (float) Convert.ToDouble(text2[1]);
                    locY = (float) Convert.ToDouble(text2[2]);
                    locZ = (float) Convert.ToDouble(text2[3]);

				} else {
                    pickup = false;
                }


			} catch(Exception e)
			{
				print (e.ToString()); //7
			}
		}
	}

	// 5. Make the Player pickup
	public void translate()
	{

        float diffx = Math.Abs(transform.position.x - locX);
        float diffy = Math.Abs(transform.position.y - locY);
        float diffz = Math.Abs(transform.position.z - locZ);
        if (diffx < sizeX/2+1 && diffy < sizeY/2+1 && diffz < sizeZ/2+1){
            transform.position = new Vector3(locX, locY, locZ);
            Cube.constraints = RigidbodyConstraints.None;
        }

    }

    public void translateNoUnLock()
	{

        float diffx = Math.Abs(transform.position.x - locX);
        float diffy = Math.Abs(transform.position.y - locY);
        float diffz = Math.Abs(transform.position.z - locZ);
        if (diffx < sizeX/2+0.5 && diffy < sizeY/2+0.5 && diffz < sizeZ/2+0.5){
            transform.position = new Vector3(locX, locY, locZ);
            // Cube.constraints = RigidbodyConstraints.None;
            Cube.constraints &= ~RigidbodyConstraints.FreezePositionX;
            Cube.constraints &= ~RigidbodyConstraints.FreezePositionY;
            Cube.constraints &= ~RigidbodyConstraints.FreezePositionZ;
        }

    }

    public void inTarget()
    {

        float diffx = Math.Abs(transform.position.x - targetX);
        float diffy = Math.Abs(transform.position.y - targetY);
        float diffz = Math.Abs(transform.position.z - targetZ);

        if (diffx < 2 && diffy < 2 && diffz < 2 && !inTargetPos){
            inTargetPos = true;
            Cube.useGravity = false;
            transform.position = new Vector3(targetX, targetY, targetZ);
            Cube.constraints = RigidbodyConstraints.FreezePosition;
            transform.eulerAngles = new Vector3(targetRX, targetRY, targetRZ);
            Cube.constraints = RigidbodyConstraints.FreezeRotation;

        } else {
            inTargetPos = false;
        }
    }

}
