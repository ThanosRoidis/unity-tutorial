using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {


    public GameObject player;

    private Vector3 offset;

	// Use this for initialization
	void Start () {
        offset = transform.position - player.transform.position;
	}
	
	// Update is called once per frame
	void LateUpdate () {

        float rot_dir = 0;

        if (Input.GetKey(KeyCode.E))
            rot_dir = -1;
        if (Input.GetKey(KeyCode.Q))
            rot_dir = 1;

        rot_dir = Input.GetAxis("Mouse X");
        offset = Quaternion.Euler(0, rot_dir * 360 * Time.deltaTime, 0) * offset;


        float mouse_scroll = Input.GetAxis("Mouse ScrollWheel");
        offset *= (1 - mouse_scroll);

        transform.position = player.transform.position + offset;
        transform.LookAt(player.transform.position);
        //transform.position = transform.position - player.transform.position

	}
}
