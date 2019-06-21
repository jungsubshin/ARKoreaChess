using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorControl : MonoBehaviour {
    private bool isExist = false;
    private GameObject onCollisionObj;
    public bool getExistsFlag() {
        return isExist;
    }
    public GameObject getCollisionObj() {
        return onCollisionObj;
    }
    // Use this for initialization
    void Start() {

    }
    void OnTriggerEnter(Collider other) {
        isExist = true;
        onCollisionObj = other.gameObject;
    }
    void OnTriggerStay(Collider other) {
        isExist = true;
        onCollisionObj = other.gameObject;
    }

    void OnTriggerExit(Collider other) {
        isExist = false;
        onCollisionObj = null;
    }

    //void OnCollisionEnter(Collision other) {
    //    isExist = true;
    //    onCollisionObj = other.gameObject;
    //    Debug.Log("sen col: " + onCollisionObj.name);
    //}

    //void OnCollisionStay(Collision other) {
    //    isExist = true;
    //    onCollisionObj = other.gameObject;
    //}

    //void OnCollisionExit(Collision other) {
    //    isExist = false;
    //    onCollisionObj = null;
    //}

    // Update is called once per frame
    void Update() {

    }
}