using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PublicVariable : MonoBehaviour {

    private bool isCreate = true, isPlayer = false, isField = false, masangDone = false, isLeft = false, playerJoin = false, isModeling = true, isStart = false;
    private int ms_position, selectedGibo=1;
    private string roomName = "room1";
    private string han_Ma1_defaultPosition = "w2h10";
    private string han_Ma2_defaultPosition = "w7h10";
    private string han_Sang1_defaultPosition = "w3h10";
    private string han_Sang2_defaultPosition = "w8h10";
    private string cho_Ma1_defaultPosition = "w2h1";
    private string cho_Ma2_defaultPosition = "w7h1";
    private string cho_Sang1_defaultPosition = "w3h1";
    private string cho_Sang2_defaultPosition = "w8h1";
    
    void Awake() {
        DontDestroyOnLoad(gameObject);
    }

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setIsPlayer(bool b) {
        isPlayer = b;
    }

    public bool getIsPlayer() {
        return isPlayer;
    }

    public void setSelectedGibo(int i) {
        selectedGibo = i;
    }

    public int getSelectedGibo() {
        return selectedGibo;
    }

    public void setIsStart(bool b) {
        isStart = b;
    }

    public bool getIsStart() {
        return isStart;
    }


    public void setRoomName(string s) {
        roomName = s;
    }

    public string getRoomName() {
        return roomName;
    }

    public void setIsCreate(bool b) {
        isCreate = b;
    }

    public bool getIsCreate() {
        return isCreate;
    }

    public void setIsField(bool b) {
        isField = b;
    }

    public bool getIsField() {
        return isField;
    }

    public void setMasangDone(bool b) {
        masangDone = b;
    }

    public bool getMasangDone() {
        return masangDone;
    }

    public void setIsLeft(bool b) {
        isLeft = b;
    }

    public bool getIsLeft() {
        return isLeft;
    }

    public void setPlayerJoin(bool b) {
        playerJoin = b;
    }

    public bool getPlayerJoin() {
        return playerJoin;
    }

    public void setIsModeling(bool b) {
        isModeling = b;
    }

    public bool getIsModeling() {
        return isModeling;
    }

    public void setMSPosition(int i) {
        ms_position = i;
    }

    public int getMSPosition() {
        return ms_position;
    }

    public void setChoPosition(int i) {
        switch(i) {
            case 1: // msms
                Cho_Ma1_defaultPosition = "w2h1";
                Cho_Ma2_defaultPosition = "w7h1";
                Cho_Sang1_defaultPosition = "w3h1";
                Cho_Sang2_defaultPosition = "w8h1";
            break;

            case 2: // mssm
                Cho_Ma1_defaultPosition = "w2h1";
                Cho_Ma2_defaultPosition = "w8h1";
                Cho_Sang1_defaultPosition = "w3h1";
                Cho_Sang2_defaultPosition = "w7h1";

            break;

            case 3: // smsm
                Cho_Ma1_defaultPosition = "w3h1";
                Cho_Ma2_defaultPosition = "w8h1";
                Cho_Sang1_defaultPosition = "w2h1";
                Cho_Sang2_defaultPosition = "w7h1";
            break;

            case 4: // smms
                Cho_Ma1_defaultPosition = "w3h1";
                Cho_Ma2_defaultPosition = "w7h1";
                Cho_Sang1_defaultPosition = "w2h1";
                Cho_Sang2_defaultPosition = "w8h1";
            break;

        }
    }

    public void setHanPosition(int i) {
        switch (i) {
            case 1: // msms
                Han_Ma1_defaultPosition = "w8h10";
                Han_Ma2_defaultPosition = "w3h10";
                Han_Sang1_defaultPosition = "w7h10";
                Han_Sang2_defaultPosition = "w2h10";
                break;

            case 2: // mssm
                Han_Ma1_defaultPosition = "w8h10";
                Han_Ma2_defaultPosition = "w2h10";
                Han_Sang1_defaultPosition = "w7h10";
                Han_Sang2_defaultPosition = "w3h10";

                break;

            case 3: // smsm
                Han_Ma1_defaultPosition = "w7h10";
                Han_Ma2_defaultPosition = "w2h10";
                Han_Sang1_defaultPosition = "w8h10";
                Han_Sang2_defaultPosition = "w3h10";
                break;

            case 4: // smms
                Han_Ma1_defaultPosition = "w7h10";
                Han_Ma2_defaultPosition = "w3h10";
                Han_Sang1_defaultPosition = "w8h10";
                Han_Sang2_defaultPosition = "w2h10";
                break;
        }
    }

    public string Han_Ma1_defaultPosition {
        get {
            return han_Ma1_defaultPosition;
        }

        set {
            han_Ma1_defaultPosition = value;
        }
    }

    public string Han_Ma2_defaultPosition {
        get {
            return han_Ma2_defaultPosition;
        }

        set {
            han_Ma2_defaultPosition = value;
        }
    }

    public string Han_Sang1_defaultPosition {
        get {
            return han_Sang1_defaultPosition;
        }

        set {
            han_Sang1_defaultPosition = value;
        }
    }

    public string Han_Sang2_defaultPosition {
        get {
            return han_Sang2_defaultPosition;
        }

        set {
            han_Sang2_defaultPosition = value;
        }
    }

    public string Cho_Ma1_defaultPosition {
        get {
            return cho_Ma1_defaultPosition;
        }

        set {
            cho_Ma1_defaultPosition = value;
        }
    }

    public string Cho_Ma2_defaultPosition {
        get {
            return cho_Ma2_defaultPosition;
        }

        set {
            cho_Ma2_defaultPosition = value;
        }
    }

    public string Cho_Sang1_defaultPosition {
        get {
            return cho_Sang1_defaultPosition;
        }

        set {
            cho_Sang1_defaultPosition = value;
        }
    }

    public string Cho_Sang2_defaultPosition {
        get {
            return cho_Sang2_defaultPosition;
        }

        set {
            cho_Sang2_defaultPosition = value;
        }
    }

}
