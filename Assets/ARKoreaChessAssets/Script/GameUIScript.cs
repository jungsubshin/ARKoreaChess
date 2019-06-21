using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUIScript : MonoBehaviour {

    public Button ReadyButton, MaSangButton, CancelButton, MSMSButton, MSSMButton, SMSMButton, SMMSButton, FixYesButton, FixNoButton;
    public Button ExitButton, ExitButton2;
    public GameObject GameDirector, MaSangPanel, FieldFixPanel, WinPanel, LosePanel;
    public Image waitingImage;
    ReadyManager readyManager;
    PublicVariable publicVariable;
    bool readyOn = false;

    // Use this for initialization
    void Start () {
        readyManager = GameObject.Find("ReadyManager").GetComponent<ReadyManager>();
        publicVariable = GameObject.Find("PublicVariable").GetComponent<PublicVariable>();
        ReadyButton.onClick.AddListener(readyFunc);
        MaSangButton.onClick.AddListener(masangFunc);
        CancelButton.onClick.AddListener(cancelFunc);
        FixYesButton.onClick.AddListener(fixYesFunc);
        FixNoButton.onClick.AddListener(fixNoFunc);
        ExitButton.onClick.AddListener(exitFunc);
        ExitButton2.onClick.AddListener(exitFunc);
        MSMSButton.onClick.AddListener(msmsFunc);
        MSSMButton.onClick.AddListener(mssmFunc);
        SMSMButton.onClick.AddListener(smsmFunc);
        SMMSButton.onClick.AddListener(smmsFunc);

       
    }
	
	// Update is called once per frame
	void Update () {
        if (!readyOn) {
            if ((publicVariable.getIsPlayer() && publicVariable.getMasangDone()) || (!PhotonNetwork.isMasterClient && publicVariable.getMasangDone())) {
                publicVariable.setIsPlayer(true);
                ReadyButton.gameObject.SetActive(true);
            }
        }

        if (publicVariable.getIsPlayer() || !PhotonNetwork.isMasterClient || publicVariable.getIsStart()) {
            waitingImage.gameObject.SetActive(false);
        }
        else {
            waitingImage.gameObject.SetActive(true);
        }

    }



    void exitFunc() {
        SceneManager.LoadScene("MainScene");
    }
    

    void readyFunc() {
        readyOn = true;
        CancelButton.gameObject.SetActive(true);
        ReadyButton.gameObject.SetActive(false);
        MaSangButton.gameObject.SetActive(false);
        //ModelChangeButton.gameObject.SetActive(false);
        readyManager.setReadyState(PhotonNetwork.isMasterClient, publicVariable.getMSPosition());
    }

    void masangFunc() {
        MaSangPanel.SetActive(true);
        MaSangButton.gameObject.SetActive(false);
        ReadyButton.gameObject.SetActive(false);
        publicVariable.setMasangDone(false);
    }

    void cancelFunc() {
        readyOn = false;
        CancelButton.gameObject.SetActive(false);
        ReadyButton.gameObject.SetActive(true);
        MaSangButton.gameObject.SetActive(true);
        publicVariable.setMasangDone(false);
        //ModelChangeButton.gameObject.SetActive(true);
        readyManager.setCancelState();
    }

    void fixYesFunc() {
        FieldFixPanel.gameObject.SetActive(false);
        MaSangPanel.SetActive(true);
        publicVariable.setIsField(true);
        //MaSangButton.gameObject.SetActive(true);
        GameObject.Find("Plane Generator").SetActive(false);
    }

    void fixNoFunc() {
        FieldFixPanel.gameObject.SetActive(false);
        publicVariable.setIsField(false);
        var anchor = GameObject.Find("Anchor");
        Destroy(anchor);
    }

    void msmsFunc() {
        publicVariable.setMSPosition(1);
        publicVariable.setMasangDone(true);
        MaSangPanel.SetActive(false);
        MaSangButton.gameObject.SetActive(true);
    }

    void mssmFunc() {
        publicVariable.setMSPosition(2);
        publicVariable.setMasangDone(true);
        MaSangPanel.SetActive(false);
        MaSangButton.gameObject.SetActive(true);
    }

    void smsmFunc() {
        publicVariable.setMSPosition(3);
        publicVariable.setMasangDone(true);
        MaSangPanel.SetActive(false);
        MaSangButton.gameObject.SetActive(true);
    }

    void smmsFunc() {
        publicVariable.setMSPosition(4);
        publicVariable.setMasangDone(true);
        MaSangPanel.SetActive(false);
        MaSangButton.gameObject.SetActive(true);
    }

}
