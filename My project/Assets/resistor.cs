using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class resistor : objFather
{

    void Start()
    {
        port = 7400; //1
		pickup = false; //2
        spacerConst = 1.5F;
        inTargetPos = false;

		InitUDP(); //4

        Cube = GetComponent<Rigidbody>();

        targetX = 17.4F;
        targetY = 1.5F;
        targetZ = 2.72F;

        targetRX = 0F;
        targetRY = 90F;
        targetRZ = 0F;

        sizeX = 2F;
        sizeY = 1.5F;
        sizeZ = 1.5F;

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

        if (inTargetPos) inTargetPosRES = true;
    }
}
