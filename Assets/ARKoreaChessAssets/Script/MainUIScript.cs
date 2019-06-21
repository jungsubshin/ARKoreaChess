using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainUIScript : MonoBehaviour {

    public GameObject MainPanel, CreatePanel, JoinPanel, SinglePanel, SettingPage, NetWorkManager, MainMovie;
    public Button CreateButton, JoinButton, MoveCreatePanelButton, MoveJoinPanelButton, MoveSettingPageButton, MoveSingleSceneButton, CancelButton_create, CancelButton_join, BackButton;
    public Button GiboButton1, GiboButton2, GiboButton3, GiboButton4, GiboButton5, BackButton2;
    public Toggle toggle3D, toggleBasic;
    public InputField CreateInputField, JoinInputField;
    public AudioClip ButtonSound;
    public Image BlinkImage, MainImage;
    AudioSource aud;
    PublicVariable publicVariable;

    private GameObject touchTarget;
    private bool flag = true;

    // Use this for initialization
    void Start() {
        publicVariable = GameObject.Find("PublicVariable").GetComponent<PublicVariable>();
        this.aud = GetComponent<AudioSource>();
        CreateButton.onClick.AddListener(createFunc);
        JoinButton.onClick.AddListener(joinFunc);
        MoveCreatePanelButton.onClick.AddListener(moveCreatePanelFunc);
        MoveJoinPanelButton.onClick.AddListener(moveJoinPanelFunc);
        MoveSettingPageButton.onClick.AddListener(moveSettingPageFunc);
        MoveSingleSceneButton.onClick.AddListener(moveSingleSceneFunc);
        //CannotJoinButton.onClick.AddListener(cannotJoinPanelFunc);
        CancelButton_create.onClick.AddListener(cancelFunc);
        CancelButton_join.onClick.AddListener(cancelFunc);
        BackButton.onClick.AddListener(backFunc);
        BackButton2.onClick.AddListener(backFunc);
        GiboButton1.onClick.AddListener(gibo1Func);
        GiboButton2.onClick.AddListener(gibo2Func);
        GiboButton3.onClick.AddListener(gibo3Func);
        GiboButton4.onClick.AddListener(gibo4Func);
        GiboButton5.onClick.AddListener(gibo5Func);
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKey(KeyCode.Escape)) {
            Application.Quit();
        }
        if (Input.GetMouseButtonDown(0) && flag) {
            BlinkImage.gameObject.SetActive(false);
            MainImage.gameObject.SetActive(true);
            MainPanel.SetActive(true);
            flag = false;


        }
    }


    private GameObject GetClickedObject() {

        RaycastHit hit;

        GameObject target = null;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //마우스 포인트 근처 좌표를 만든다. 


        if (Physics.Raycast(ray, out hit))   //오브젝트가 있는지 확인
        {
            //있으면 오브젝트를 저장한다.
            target = hit.collider.gameObject;
        }


        return target;
    }

    


    public void toggle3DFunc() {
        if (toggle3D.isOn) {
            publicVariable.setIsModeling(true);
            Debug.Log("온");
        }
        else {
            publicVariable.setIsModeling(false);
            Debug.Log("오프");
        }
   }
    
    void gibo1Func() {
        publicVariable.setSelectedGibo(1);
        SceneManager.LoadScene("SingleScene");
    }
    void gibo2Func() {
        publicVariable.setSelectedGibo(2);
        SceneManager.LoadScene("SingleScene");
    }
    void gibo3Func() {
        publicVariable.setSelectedGibo(3);
        SceneManager.LoadScene("SingleScene");
    }
    void gibo4Func() {
        publicVariable.setSelectedGibo(4);
        SceneManager.LoadScene("SingleScene");
    }
    void gibo5Func() {
        publicVariable.setSelectedGibo(5);
        SceneManager.LoadScene("SingleScene");
    }

    void moveSingleSceneFunc() {
        aud.PlayOneShot(ButtonSound);
        SinglePanel.SetActive(true);
        MainPanel.SetActive(false);

    }

    void cancelFunc() {
        CreatePanel.SetActive(false);
        JoinPanel.SetActive(false);
        MainPanel.SetActive(true);
        aud.PlayOneShot(ButtonSound);
    }

    void backFunc() {
        MainPanel.SetActive(true);
        SettingPage.SetActive(false);
        SinglePanel.SetActive(false);
        aud.PlayOneShot(ButtonSound);
    }

    void createFunc() {
        publicVariable.setRoomName(CreateInputField.text);
        publicVariable.setIsCreate(true);
        NetWorkManager.SetActive(true);
        aud.PlayOneShot(ButtonSound);

    }
    void joinFunc() {
        publicVariable.setRoomName(JoinInputField.text);
        publicVariable.setIsCreate(false);
        NetWorkManager.SetActive(true);
        aud.PlayOneShot(ButtonSound);
    }
    void moveCreatePanelFunc() {
        MainPanel.SetActive(false);
        CreatePanel.SetActive(true);
        aud.PlayOneShot(ButtonSound);

    }
    void moveJoinPanelFunc() {
        MainPanel.SetActive(false);
        JoinPanel.SetActive(true);
        aud.PlayOneShot(ButtonSound);

    }
    void moveSettingPageFunc() {
        MainPanel.SetActive(false);
        SettingPage.SetActive(true);
        aud.PlayOneShot(ButtonSound);
    }

    //void cannotJoinPanelFunc() {
    //    CannotJoinPanel.SetActive(false);
    //    aud.PlayOneShot(ButtonSound);
    //}
}
