using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class wire : objFather
{

    void Start()
    {
        port = 7262; //1
		pickup = false; //2
        spacerConst = 1.5F;
        inTargetPos = false;

		InitUDP(); //4

        Cube = GetComponent<Rigidbody>();

        targetX = 16.41F;
        targetY = 2F;
        targetZ = 8.09F;

        targetRX = 0F;
        targetRY = 0F;
        targetRZ = 90F;

        sizeX = 2F;
        sizeY = 0.5F;
        sizeZ = 0.5F;

        locX = 0.0F;
        locY = 0.0F;
        locZ = 0.0F;
        Cube.constraints = RigidbodyConstraints.FreezeRotation;

    }

    void Update()
    {

        if (pickup == true)
		{
            Cube.useGravity = false;
			translateNoUnLock();
		} else {
            if (!inTargetPos) {
                Cube.useGravity = true;
            } else {
                Cube.useGravity = false;
            }
            inTarget();
        }
        if(inTargetPos) inTargetPosWIRE=true;
    }
}


// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using System;
// using System.Net;
// using System.Net.Sockets;
// using System.Text;
// using System.Threading;

// public class cubePlayer2 : MonoBehaviour
// {
//     Thread receiveThread; //1
// 	UdpClient client; //2
// 	int port; //3

// 	public Rigidbody Cube; //4
// 	AudioSource jumpSound; //5
// 	bool jump; //6
//     public string[] text2;
//     // Start is called before the first frame update
//     void Start()
//     {
//         port = 7272; //1
// 		jump = false; //2
// 		jumpSound = gameObject.GetComponent<AudioSource>(); //3

// 		InitUDP(); //4

//         Cube = GetComponent<Rigidbody>();
//     }
//     // 3. InitUDP
// 	private void InitUDP()
// 	{
// 		print ("UDP Initialized");

// 		receiveThread = new Thread (new ThreadStart(ReceiveData)); //1
// 		receiveThread.IsBackground = true; //2
// 		receiveThread.Start (); //3

// 	}

// 	// 4. Receive Data
// 	private void ReceiveData()
// 	{
// 		client = new UdpClient (port); //1
// 		while (true) //2
// 		{
// 			try
// 			{
// 				IPEndPoint anyIP = new IPEndPoint(IPAddress.Parse("0.0.0.0"), port); //3
// 				byte[] data = client.Receive(ref anyIP); //4

// 				string text = Encoding.UTF8.GetString(data); //5
// 				text2 = text.Split(',');
// 				if (text2[0] == "True") {
// 					jump = true; //6
// 					print (">> " + text2[1] + ", " + text2[2] + ", " + text2[3]);
// 				} else {
//                     jump = false;
//                 }


// 			} catch(Exception e)
// 			{
// 				print (e.ToString()); //7
// 			}
// 		}
// 	}

// 	// 5. Make the Player Jump
// 	public void translate()
// 	{

//         float locX = (float) Convert.ToDouble(text2[1]);
//         float locY = (float) Convert.ToDouble(text2[2]);
//         float locZ = (float) Convert.ToDouble(text2[3]);

//         float diffx = Math.Abs(transform.position.x - locX);
//         float diffy = Math.Abs(transform.position.y - locY);
//         float diffz = Math.Abs(transform.position.z - locZ);

//         if (diffx < 2 && diffy < 2 && diffz < 2){
//             transform.position = new Vector3(locX, locY, locZ);
//         }

//     }

//     // Update is called once per frame
//     void Update()
//     {
//         if (jump == true)
// 		{
//             Cube.useGravity = false;
// 			translate();
// 		} else {
//             Cube.useGravity = true;
//         }
//     }
// }
