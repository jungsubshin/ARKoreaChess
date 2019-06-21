using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MalScript : MonoBehaviour {
    private GameObject sensor_obj;
    private bool changeState = true;
    // Use this for initialization
    void Start() {
    }
    public GameObject getSensorObj() {
        return sensor_obj;
    }

    public bool getChangeState() {
        return changeState;
    }

    void OnTriggerEnter(Collider other) {
        sensor_obj = other.gameObject;
        changeState = true;
    }

    void OnTriggerStay(Collider other) {
        sensor_obj = other.gameObject;
        changeState = true;
    }

    void OnTriggerExit(Collider other) {
        sensor_obj = null;
        changeState = false;
    }

    //void OnCollisionEnter(Collision other) {
    //    sensor_obj = other.gameObject;
    //}

    //void OnCollisionStay(Collision other) {
    //    sensor_obj = other.gameObject;
    //}

    //void OnCollisionExit(Collision other) {
    //    sensor_obj = null;
    //}

    // Update is called once per frame
    void Update() {

    }
}