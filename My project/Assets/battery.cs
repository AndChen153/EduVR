using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class battery : objFather
{

    void Start()
    {
        port = 5421; //1
		pickup = false; //2
        spacerConst = 1.5F;
        inTargetPos = false;

		InitUDP(); //4

        Cube = GetComponent<Rigidbody>();

        targetX = 11.91F;
        targetY = 2F;
        targetZ = 5.7F;

        targetRX = 0F;
        targetRY = 0F;
        targetRZ = 0F;

        sizeX = 2F;
        sizeY = 2F;
        sizeZ = 2F;

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
        if(inTargetPos) inTargetPosBATT = true;
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

// public class cubePlayer : MonoBehaviour
// {
//     Thread receiveThread; //1
// 	UdpClient client; //2
// 	int port; //3

// 	public Rigidbody Cube; //4
// 	AudioSource pickupSound; //5
// 	bool pickup; //6
//     bool inTargetPos;
//     float targetX;
//     float targetY;
//     float targetZ;

//     float locX;
//     float locY;
//     float locZ;

//     float const;

//     public string[] text2;
//     // Start is called before the first frame update
//     void Start()
//     {
//         port = 5065; //1
// 		pickup = false; //2
//         inTargetPos = false;
// 		pickupSound = gameObject.GetComponent<AudioSource>(); //3

// 		InitUDP(); //4

//         Cube = GetComponent<Rigidbody>();

//         targetX = 17F;
//         targetY = 1F;
//         targetZ = 2F;

//         locX = 0.0F;
//         locY = 0.0F;
//         locZ = 0.0F;
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
// 					pickup = true; //6
// 					print (">> " + text2[1] + ", " + text2[2] + ", " + text2[3]);
//                     locX = (float) Convert.ToDouble(text2[1]);
//                     locY = (float) Convert.ToDouble(text2[2]);
//                     locZ = (float) Convert.ToDouble(text2[3]);

// 				} else {
//                     pickup = false;
//                 }


// 			} catch(Exception e)
// 			{
// 				print (e.ToString()); //7
// 			}
// 		}
// 	}

// 	// 5. Make the Player pickup
// 	public void translate()
// 	{
//         const = 1.5;
//         float diffx = Math.Abs(transform.position.x - locX);
//         float diffy = Math.Abs(transform.position.y - locY);
//         float diffz = Math.Abs(transform.position.z - locZ);
//         if (diffx < const && diffy < const && diffz < const){
//             transform.position = new Vector3(locX, locY, locZ);
//         }



//     }

//     public void inTarget()
//     {

//         float diffx = Math.Abs(transform.position.x - targetX);
//         float diffy = Math.Abs(transform.position.y - targetY);
//         float diffz = Math.Abs(transform.position.z - targetZ);
//         if (diffx < 2 && diffy < 2 && diffz < 2 && !inTargetPos){
//             transform.position = new Vector3(targetX, targetY, targetZ);
//             inTargetPos = true;
//         } else {
//             inTargetPos = false;
//         }
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         if (pickup == true)
// 		{
//             Cube.useGravity = false;
// 			translate();
// 		} else {
//             Cube.useGravity = true;
//             inTarget();
//         }
//     }
// }
