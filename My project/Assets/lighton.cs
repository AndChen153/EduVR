using UnityEngine;

public class lighton : MonoBehaviour
{
    Light myLight;
    void Start()
    {
        myLight = GetComponent<Light>();

    }

    void Update() {
        if (objFather.inTargetPosBATT && objFather.inTargetPosLED && objFather.inTargetPosRES && objFather.inTargetPosWIRE) {
            myLight.intensity = 8;
        } else {
            myLight.intensity = 1;
        }
    }
}