using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class jumpset : MonoBehaviour {
    protected Animator animator;
    Rigidbody rigidbody;
    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {

	}
    public void setChk() 
    {
        animator.SetTrigger("jumpChk");
    }
}
