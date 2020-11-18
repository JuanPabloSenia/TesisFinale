using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeepLogic : MonoBehaviour {

    public bool isStatic;
    public bool hideOnPlay;

	void Start () {
        transform.position = new Vector3(transform.position.x, -transform.position.z / 2, transform.position.z);
        if (hideOnPlay) gameObject.SetActive(false);
	}
	
	void Update () {
		if (!isStatic)
            transform.position = new Vector3(transform.position.x, -transform.position.z / 2, transform.position.z);
    }
}
