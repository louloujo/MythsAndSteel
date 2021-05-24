using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class POPO : MonoBehaviour
{

	public float camSpeed = 15f;
	public float camPosMax = 1.5625f;
	public float margin = 64f;


    void Start()
    {

    }


    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 camPos = this.transform.position;

        // print(camPos.y);
        // print(mousePos.y);

        if (mousePos.y > 1080f - margin && camPos.y < camPosMax) {
        	this.transform.position = camPos + Vector3.up*camSpeed*Time.deltaTime;
        }

        if (mousePos.y < margin && camPos.y > -camPosMax) {
        	this.transform.position = camPos + Vector3.down*camSpeed*Time.deltaTime;
        }
        
    }
}