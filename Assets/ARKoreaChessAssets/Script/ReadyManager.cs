using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadyManager : MonoBehaviour {

    public GameObject GameDirector;
    public GameObject MaSangPanel;
    public Text text;
    public Button cancelButton;
    PhotonView view;
    PublicVariable publicVariable;
    int readyCount=0;
	// Use this for initialization
	void Start () {
        publicVariable = GameObject.Find("PublicVariable").GetComponent<PublicVariable>();
        view = GetComponent<PhotonView>();
    }
	
	// Update is called once per frame
	void Update () {

	}

    public void setReadyState(bool master, int ms_position) {
        view.RPC("ready_RPC", PhotonTargets.All, master, ms_position);
    }

    public void setCancelState() {
        view.RPC("cancel_RPC", PhotonTargets.All);
    }

    [PunRPC]
    void ready_RPC(bool master, int ms_position) {
        readyCount++;

        if (master)
            publicVariable.setChoPosition(ms_position);
        else
            publicVariable.setHanPosition(ms_position);

        if (readyCount == 2) {
            GameDirector.SetActive(true);
            cancelButton.gameObject.SetActive(false);
        }
    }

    [PunRPC]
    void cancel_RPC() {
        readyCount--;
    }


}
