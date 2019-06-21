using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameDirector : MonoBehaviour {

    public AudioClip move_Jol, move_Cha, move_King, winSound, loseSound, startSound;
    public AudioClip touch_Cha, touch_Ma, touch_Po, touch_Sang, touch_Jol, touch_Sa, touch_HanKing, touch_ChoKing;
    public AudioClip jangGoon1, jangGoon2, jangGoon3, jangGoon4, mungGoon1, mungGoon2, mungGoon3, mungGoon4, mungGoon5;

    public GameObject Han_Cha, Han_Ma, Han_Po, Han_Sang, Han_Jol, Han_Sa, Han_King;
    public GameObject Cho_Cha, Cho_Ma, Cho_Po, Cho_Sang, Cho_Jol, Cho_Sa, Cho_King;
    public GameObject Han_Cha3d, Han_Ma3d, Han_Po3d, Han_Sang3d, Han_Jol3d, Han_Sa3d, Han_King3d;
    public GameObject Cho_Cha3d, Cho_Ma3d, Cho_Po3d, Cho_Sang3d, Cho_Jol3d, Cho_Sa3d, Cho_King3d;

    public GameObject ArrowMark, Attackpoint, Handpoint, ArrowMark3D, Attackpoint3D, Handpoint3D;
    public GameObject controller, generator, WinPanel, LosePanel, PlayingSound, JangGoonSound;

    public Image GameStartImage;

    AudioSource aud, aud2;
    string[,] mal_position = new string[11, 11];
    SearchPath searchPath;
    GameObject mal_object, anchor, touchTarget, lastTouch, lastMeObj = null, lastMovedPosition = null;
    GameObject[] enemy_objs = new GameObject[8];
    GameObject[] go = new GameObject[32];
    GameObject field;
    PhotonView view;
    SensorControl sensorControl;
    PublicVariable publicVariable;
    JangGoonCheck jangGoonCheck;
    string position_name = null, myTag = "Cho", nowTurn = "Cho";
    string preSensor = "1", nowSensor;
    int i, j, k = 0;
    bool isChoosing = false, isEnemy = false, isJangGoon = false;
    bool[,] attackedPosition = new bool[11, 11];

    // Use this for initialization
    void Start() {
        this.aud = GetComponent<AudioSource>();
        view = GetComponent<PhotonView>();
        anchor = GameObject.Find("Anchor");
        searchPath = GameObject.Find("SearchPath").GetComponent<SearchPath>();
        publicVariable = GameObject.Find("PublicVariable").GetComponent<PublicVariable>();
        jangGoonCheck = GameObject.Find("JangGoonCheck").GetComponent<JangGoonCheck>();
        GameStartImage.gameObject.SetActive(true);
        publicVariable.setIsStart(true);
        StartCoroutine("GameStart");

        for (i = 1; i <= 9; i++)
            for (j = 1; j <= 10; j++) {
                position_name = "w" + i + "h" + j;
                mal_position[i, j] = position_name;
                attackedPosition[i, j] = false;
            }
        
        if (!PhotonNetwork.isMasterClient) {
            field = GameObject.Find("JangGiField(Clone)");
            field.transform.Rotate(0, 180.0f, 0);
            myTag = "Han";
        }

        CreateDefaultMal();

        aud.PlayOneShot(startSound);
        PlayingSound.SetActive(true);
    }


    void FixedUpdate() {

        if (Input.GetMouseButtonDown(0) && nowTurn.Equals(myTag)) {

            touchTarget = GetClickedObject();

            if (!touchTarget.tag.Equals("Field")) {
                removeAllEnemyObjs();
                if (lastMeObj != null) removeObj(lastMeObj);
                if (lastMovedPosition != null) removeObj(lastMovedPosition);
            }

            if (touchTarget.tag.Equals("Han") || touchTarget.tag.Equals("Cho")) { // 말 선택시

                if (isChoosing && !touchTarget.tag.Equals(myTag)) { // 말 선택 중 다른 팀 선택 했을때
                    if (touchTarget.GetComponent<MalScript>().getSensorObj().GetComponent<Renderer>().enabled) {
                        view.RPC("attack_RPC", PhotonTargets.All, lastTouch.GetComponent<MalScript>().getSensorObj().name, touchTarget.GetComponent<MalScript>().getSensorObj().name);
                        offAllSensors();
                        isChoosing = false;
                    }

                }

                else if (touchTarget.tag.Equals(myTag)) { // 자기 팀을 선택 하면
                    offAllSensors();
                    lastMeObj = setMeObj(touchTarget);
                    lastTouch = touchTarget;
                    if (publicVariable.getIsModeling()) touchVoice(touchTarget.name);
                    searchPath.searchPath(touchTarget.GetComponent<MalScript>().getSensorObj().name, touchTarget.name);
                    isChoosing = true;
                }

            }

            else if (touchTarget.tag.Equals("Sensor")) { // 센서 선택시
                if (isChoosing && touchTarget.GetComponent<Renderer>().enabled) { // 말 선택 중 이동가능한 센서 선택시
                    preSensor = lastTouch.GetComponent<MalScript>().getSensorObj().name;
                    view.RPC("move_RPC", PhotonTargets.All, preSensor, touchTarget.name);
                    isChoosing = false;
                    offAllSensors();
                }
            }
        }

        if (!publicVariable.getIsPlayer()) { // 상대나가면 승리
            aud.PlayOneShot(winSound);
            WinPanel.SetActive(true);
            PlayingSound.SetActive(false);
            JangGoonSound.SetActive(false);
        }
    }

    IEnumerator GameStart() {
        yield return new WaitForSeconds(2.0f);
        GameStartImage.gameObject.SetActive(false);
    }


    [PunRPC]
    void move_RPC(string lastTouchSensor, string touchTargetSensor) {
        GameObject lastTouch = GameObject.Find(lastTouchSensor).GetComponent<SensorControl>().getCollisionObj();
        GameObject touchTarget = GameObject.Find(touchTargetSensor);
        preSensor = lastTouchSensor;
        nowSensor = touchTargetSensor;
        lastTouch.transform.position = touchTarget.transform.position;
        lastMovedPosition = setMovePosition(touchTarget);
        moveSound(lastTouch);
        if (nowTurn.Equals("Han")) nowTurn = "Cho";
        else nowTurn = "Han";
        StartCoroutine("JangGoonCheck");
    }


    [PunRPC]
    void attack_RPC(string lastTouchSensor, string touchTargetSensor) {
        GameObject lastTouch = GameObject.Find(lastTouchSensor).GetComponent<SensorControl>().getCollisionObj();
        GameObject touchTarget = GameObject.Find(touchTargetSensor).GetComponent<SensorControl>().getCollisionObj();
        preSensor = lastTouchSensor;
        nowSensor = touchTargetSensor;
        lastTouch.transform.position = touchTarget.transform.position;
        lastMovedPosition = setMovePosition(touchTarget);
        moveSound(lastTouch);
        Destroy(touchTarget);
        if (nowTurn.Equals("Han")) nowTurn = "Cho";
        else nowTurn = "Han";

        if (touchTarget.name.Equals("Han_King(Clone)") || touchTarget.name.Equals("Cho_King(Clone)")) {
            if (touchTarget.name.Substring(0, 3).Equals(myTag)) { // 패배
                aud.PlayOneShot(loseSound);
                LosePanel.SetActive(true);
                PlayingSound.SetActive(false);
                JangGoonSound.SetActive(false);
            }
            else { // 승리
                aud.PlayOneShot(winSound);
                WinPanel.SetActive(true);
                PlayingSound.SetActive(false);
                JangGoonSound.SetActive(false);
            }
        }

        else {
            StartCoroutine("JangGoonCheck");
        }
    }


    IEnumerator JangGoonCheck() {
        yield return new WaitForSeconds(0.1f);
        GameObject movedMal = GameObject.Find(nowSensor).GetComponent<SensorControl>().getCollisionObj();

        if (isJangGoon) {
            isJangGoon = false;
            if (movedMal.tag.Equals("Han"))
                checkJangGoon("Cho");
            else
                checkJangGoon("Han");

            if (!isJangGoon)
                mungGoonSound();
        }
        else {
            checkJangGoon(movedMal.tag);
            if (isJangGoon) jangGoonSound();
        }

    }


    public void setIsJangGoon() {
        isJangGoon = true;
    }

    private void checkJangGoon(string tag) {
        if (tag.Equals("Cho")) {
            for (i = 16; i <= 31; i++) { // 초의 모든 말 검사
                if (go[i] == null) continue; // 오브젝트가 존재하는지
                else {
                    jangGoonCheck.jangGoonCheck(go[i].GetComponent<MalScript>().getSensorObj().name, go[i].name);
                }

            } // 서치돌려서 공격 가능한 부분 체크
        }
        else {
            for (i = 0; i <= 15; i++) { // 한의 모든 말 검사
                if (go[i] == null) continue;
                else {
                    jangGoonCheck.jangGoonCheck(go[i].GetComponent<MalScript>().getSensorObj().name, go[i].name);
                }


            }
        }
    }



    private GameObject setMeObj(GameObject touchTarget) {
        Vector3 vc;
        GameObject go;
        vc = touchTarget.transform.position;

        if (publicVariable.getIsModeling()) {
            vc.y += 0.11f;
            go = Instantiate(Handpoint3D, anchor.transform);
        }
        else {
            vc.y += 0.03f;
            go = Instantiate(Handpoint, anchor.transform);
        }
        go.transform.position = vc;
        return go;
    }

    private GameObject setMovePosition(GameObject touchTarget) {
        Vector3 vc;
        GameObject go;
        if (lastMovedPosition != null) Destroy(lastMovedPosition);

        vc = touchTarget.transform.position;

        if (publicVariable.getIsModeling()) {
            vc.y += 0.12f;
            go = Instantiate(ArrowMark3D, anchor.transform);
        }
        else {
            vc.y += 0.04f;
            go = Instantiate(ArrowMark, anchor.transform);
        }
        go.transform.position = vc;
        return go;
    }

    public void setEnemyObj(GameObject sensorObj) {
        Vector3 vc;
        vc = sensorObj.transform.position;
        if (publicVariable.getIsModeling()) {
            vc.y += 0.12f;
            enemy_objs[k] = Instantiate(Attackpoint3D, anchor.transform);
        }
        else {
            vc.y += 0.04f;
            enemy_objs[k] = Instantiate(Attackpoint, anchor.transform);
        }
        enemy_objs[k++].transform.position = vc;
        isEnemy = true;
    }

    private void removeAllEnemyObjs() {
        if (isEnemy) {
            for (k = 0; k <= 8; k++) {
                if (enemy_objs[k] == null)
                    break;
                else
                    Destroy(enemy_objs[k]);
            }
            k = 0;
        }
        isEnemy = false;
    }

    private void removeObj(GameObject go) {
        Destroy(go);
    }



    private bool checkAttackedPosition(string sensor_name) {
        int row, col;
        row = int.Parse(sensor_name.Substring(1, 1));
        if (sensor_name.Length == 4)
            col = int.Parse(sensor_name.Substring(3, 1));
        else
            col = int.Parse(sensor_name.Substring(3, 2));

        return attackedPosition[row, col];

    }
    private void setAttackedPosition(string tag) {
        resetAttackedPosition();
        if (tag.Equals("Han")) {
            for (i = 16; i <= 31; i++) { // 초의 모든 말 검사
                if (go[i] == null) continue; // 오브젝트가 존재하는지
                jangGoonCheck.jangGoonCheck(go[i].GetComponent<MalScript>().getSensorObj().name, go[i].name);

            } // 서치돌려서 공격 가능한 부분 체크
        }
        else {
            for (i = 0; i <= 15; i++) { // 한의 모든 말 검사
                if (go[i] == null) continue;
                jangGoonCheck.jangGoonCheck(go[i].GetComponent<MalScript>().getSensorObj().name, go[i].name);

            }
        }
    }

    private void resetAttackedPosition() {
        for (i = 4; i <= 6; i++)
            for (j = 1; j <= 10; j++) {
                attackedPosition[i, j] = false;
            }
    }

    public void setAttackedSensorPosition(int row, int col) {
        attackedPosition[row, col] = true;
    }

    private void moveSound(GameObject lastTouch) {
        if (lastTouch.name.Equals("Han_Jol(Clone)") || lastTouch.name.Equals("Cho_Jol(Clone)"))
            aud.PlayOneShot(move_Jol);
        else if (lastTouch.name.Equals("Han_King(Clone)") || lastTouch.name.Equals("Cho_King(Clone)"))
            aud.PlayOneShot(move_King);
        else
            aud.PlayOneShot(move_Cha);

    }

    private void touchVoice(string mal_name) {
        if (mal_name.Equals("Han_Jol(Clone)") || mal_name.Equals("Cho_Jol(Clone)")) {
            aud.Stop();
            aud.PlayOneShot(touch_Jol);
        }
        else if (mal_name.Equals("Han_Ma(Clone)") || mal_name.Equals("Cho_Ma(Clone)")) {
            aud.Stop();
            aud.PlayOneShot(touch_Ma);
        }
        else if (mal_name.Equals("Han_Sang(Clone)") || mal_name.Equals("Cho_Sang(Clone)")) {
            aud.Stop();
            aud.PlayOneShot(touch_Sang);
        }
        else if (mal_name.Equals("Han_Po(Clone)") || mal_name.Equals("Cho_Po(Clone)")) {
            aud.Stop();
            aud.PlayOneShot(touch_Po);
        }
        else if (mal_name.Equals("Han_Cha(Clone)") || mal_name.Equals("Cho_Cha(Clone)")) {
            aud.Stop();
            aud.PlayOneShot(touch_Cha);
        }
        else if (mal_name.Equals("Han_Sa(Clone)") || mal_name.Equals("Cho_Sa(Clone)")) {
            aud.Stop();
            aud.PlayOneShot(touch_Sa);
        }
        else if (mal_name.Equals("Han_King(Clone)")) {
            aud.Stop();
            aud.PlayOneShot(touch_HanKing);
        }
        else if (mal_name.Equals("Cho_King(Clone)")) {
            aud.Stop();
            aud.PlayOneShot(touch_ChoKing);
        }

    }

    public void jangGoonSound() {
        int i = UnityEngine.Random.Range(1,5);
        switch (i) {
            case 1:
                aud.PlayOneShot(jangGoon1);
                break;
            case 2:
                aud.PlayOneShot(jangGoon2);
                break;
            case 3:
                aud.PlayOneShot(jangGoon3);
                break;
            case 4:
                aud.PlayOneShot(jangGoon4);
                break;
        }
        JangGoonSound.SetActive(true);
        PlayingSound.SetActive(false);
    }

    public void mungGoonSound() {
        int i = UnityEngine.Random.Range(1, 6);
        switch (i) {
            case 1:
                aud.PlayOneShot(mungGoon1);
                break;
            case 2:
                aud.PlayOneShot(mungGoon2);
                break;
            case 3:
                aud.PlayOneShot(mungGoon3);
                break;
            case 4:
                aud.PlayOneShot(mungGoon4);
                break;
            case 5:
                aud.PlayOneShot(mungGoon5);
                break;
        }
        JangGoonSound.SetActive(false);
        PlayingSound.SetActive(true);
    }


    public void offAllSensors() {
        for (i = 1; i <= 9; i++)
            for (j = 1; j <= 10; j++) {
                position_name = "w" + i + "h" + j;
                mal_object = GameObject.Find(position_name);
                mal_object.GetComponent<Renderer>().enabled = false;
            }
    }

    public string getMyTag() {
        return myTag;
    }

    public void setMyTag(string s) {
        myTag = s;
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


    public void CreateDefaultMal() {

        if (publicVariable.getIsModeling()) {
            go[0] = Instantiate(Han_Cha3d, anchor.transform); // 한
            go[0].transform.position = GameObject.Find(mal_position[1, 10]).transform.position;

            go[1] = Instantiate(Han_Cha3d, anchor.transform);
            go[1].transform.position = GameObject.Find(mal_position[9, 10]).transform.position;

            go[2] = Instantiate(Han_Po3d, anchor.transform);
            go[2].transform.position = GameObject.Find(mal_position[2, 8]).transform.position;

            go[3] = Instantiate(Han_Po3d, anchor.transform);
            go[3].transform.position = GameObject.Find(mal_position[8, 8]).transform.position;

            go[4] = Instantiate(Han_Ma3d, anchor.transform);
            go[4].transform.position = GameObject.Find(publicVariable.Han_Ma1_defaultPosition).transform.position;

            go[5] = Instantiate(Han_Ma3d, anchor.transform);
            go[5].transform.position = GameObject.Find(publicVariable.Han_Ma2_defaultPosition).transform.position;

            go[6] = Instantiate(Han_Sang3d, anchor.transform);
            go[6].transform.position = GameObject.Find(publicVariable.Han_Sang1_defaultPosition).transform.position;

            go[7] = Instantiate(Han_Sang3d, anchor.transform);
            go[7].transform.position = GameObject.Find(publicVariable.Han_Sang2_defaultPosition).transform.position;

            go[8] = Instantiate(Han_Jol3d, anchor.transform);
            go[8].transform.position = GameObject.Find(mal_position[1, 7]).transform.position;

            go[9] = Instantiate(Han_Jol3d, anchor.transform);
            go[9].transform.position = GameObject.Find(mal_position[3, 7]).transform.position;

            go[10] = Instantiate(Han_Jol3d, anchor.transform);
            go[10].transform.position = GameObject.Find(mal_position[5, 7]).transform.position;

            go[11] = Instantiate(Han_Jol3d, anchor.transform);
            go[11].transform.position = GameObject.Find(mal_position[7, 7]).transform.position;

            go[12] = Instantiate(Han_Jol3d, anchor.transform);
            go[12].transform.position = GameObject.Find(mal_position[9, 7]).transform.position;

            go[13] = Instantiate(Han_King3d, anchor.transform);
            go[13].transform.position = GameObject.Find(mal_position[5, 9]).transform.position;


            go[14] = Instantiate(Han_Sa3d, anchor.transform);
            go[14].transform.position = GameObject.Find(mal_position[4, 10]).transform.position;

            go[15] = Instantiate(Han_Sa3d, anchor.transform);
            go[15].transform.position = GameObject.Find(mal_position[6, 10]).transform.position;





            go[16] = Instantiate(Cho_Cha3d, anchor.transform); // 초
            go[16].transform.position = GameObject.Find(mal_position[1, 1]).transform.position;

            go[17] = Instantiate(Cho_Cha3d, anchor.transform);
            go[17].transform.position = GameObject.Find(mal_position[9, 1]).transform.position;

            go[18] = Instantiate(Cho_Po3d, anchor.transform);
            go[18].transform.position = GameObject.Find(mal_position[2, 3]).transform.position;

            go[19] = Instantiate(Cho_Po3d, anchor.transform);
            go[19].transform.position = GameObject.Find(mal_position[8, 3]).transform.position;

            go[20] = Instantiate(Cho_Ma3d, anchor.transform);
            go[20].transform.position = GameObject.Find(publicVariable.Cho_Ma1_defaultPosition).transform.position;

            go[21] = Instantiate(Cho_Ma3d, anchor.transform);
            go[21].transform.position = GameObject.Find(publicVariable.Cho_Ma2_defaultPosition).transform.position;

            go[22] = Instantiate(Cho_Sang3d, anchor.transform);
            go[22].transform.position = GameObject.Find(publicVariable.Cho_Sang1_defaultPosition).transform.position;

            go[23] = Instantiate(Cho_Sang3d, anchor.transform);
            go[23].transform.position = GameObject.Find(publicVariable.Cho_Sang2_defaultPosition).transform.position;

            go[24] = Instantiate(Cho_Jol3d, anchor.transform);
            go[24].transform.position = GameObject.Find(mal_position[1, 4]).transform.position;

            go[25] = Instantiate(Cho_Jol3d, anchor.transform);
            go[25].transform.position = GameObject.Find(mal_position[3, 4]).transform.position;

            go[26] = Instantiate(Cho_Jol3d, anchor.transform);
            go[26].transform.position = GameObject.Find(mal_position[5, 4]).transform.position;

            go[27] = Instantiate(Cho_Jol3d, anchor.transform);
            go[27].transform.position = GameObject.Find(mal_position[7, 4]).transform.position;

            go[28] = Instantiate(Cho_Jol3d, anchor.transform);
            go[28].transform.position = GameObject.Find(mal_position[9, 4]).transform.position;

            go[29] = Instantiate(Cho_King3d, anchor.transform);
            go[29].transform.position = GameObject.Find(mal_position[5, 2]).transform.position;

            go[30] = Instantiate(Cho_Sa3d, anchor.transform);
            go[30].transform.position = GameObject.Find(mal_position[4, 1]).transform.position;

            go[31] = Instantiate(Cho_Sa3d, anchor.transform);
            go[31].transform.position = GameObject.Find(mal_position[6, 1]).transform.position;
        }




        else {
            go[0] = Instantiate(Han_Cha, anchor.transform); // 한
            go[0].transform.position = GameObject.Find(mal_position[1, 10]).transform.position;

            go[1] = Instantiate(Han_Cha, anchor.transform);
            go[1].transform.position = GameObject.Find(mal_position[9, 10]).transform.position;

            go[2] = Instantiate(Han_Po, anchor.transform);
            go[2].transform.position = GameObject.Find(mal_position[2, 8]).transform.position;

            go[3] = Instantiate(Han_Po, anchor.transform);
            go[3].transform.position = GameObject.Find(mal_position[8, 8]).transform.position;

            go[4] = Instantiate(Han_Ma, anchor.transform);
            go[4].transform.position = GameObject.Find(publicVariable.Han_Ma1_defaultPosition).transform.position;

            go[5] = Instantiate(Han_Ma, anchor.transform);
            go[5].transform.position = GameObject.Find(publicVariable.Han_Ma2_defaultPosition).transform.position;

            go[6] = Instantiate(Han_Sang, anchor.transform);
            go[6].transform.position = GameObject.Find(publicVariable.Han_Sang1_defaultPosition).transform.position;

            go[7] = Instantiate(Han_Sang, anchor.transform);
            go[7].transform.position = GameObject.Find(publicVariable.Han_Sang2_defaultPosition).transform.position;

            go[8] = Instantiate(Han_Jol, anchor.transform);
            go[8].transform.position = GameObject.Find(mal_position[1, 7]).transform.position;

            go[9] = Instantiate(Han_Jol, anchor.transform);
            go[9].transform.position = GameObject.Find(mal_position[3, 7]).transform.position;

            go[10] = Instantiate(Han_Jol, anchor.transform);
            go[10].transform.position = GameObject.Find(mal_position[5, 7]).transform.position;

            go[11] = Instantiate(Han_Jol, anchor.transform);
            go[11].transform.position = GameObject.Find(mal_position[7, 7]).transform.position;

            go[12] = Instantiate(Han_Jol, anchor.transform);
            go[12].transform.position = GameObject.Find(mal_position[9, 7]).transform.position;

            go[13] = Instantiate(Han_King, anchor.transform);
            go[13].transform.position = GameObject.Find(mal_position[5, 9]).transform.position;


            go[14] = Instantiate(Han_Sa, anchor.transform);
            go[14].transform.position = GameObject.Find(mal_position[4, 10]).transform.position;

            go[15] = Instantiate(Han_Sa, anchor.transform);
            go[15].transform.position = GameObject.Find(mal_position[6, 10]).transform.position;





            go[16] = Instantiate(Cho_Cha, anchor.transform); // 초
            go[16].transform.position = GameObject.Find(mal_position[1, 1]).transform.position;

            go[17] = Instantiate(Cho_Cha, anchor.transform);
            go[17].transform.position = GameObject.Find(mal_position[9, 1]).transform.position;

            go[18] = Instantiate(Cho_Po, anchor.transform);
            go[18].transform.position = GameObject.Find(mal_position[2, 3]).transform.position;

            go[19] = Instantiate(Cho_Po, anchor.transform);
            go[19].transform.position = GameObject.Find(mal_position[8, 3]).transform.position;

            go[20] = Instantiate(Cho_Ma, anchor.transform);
            go[20].transform.position = GameObject.Find(publicVariable.Cho_Ma1_defaultPosition).transform.position;

            go[21] = Instantiate(Cho_Ma, anchor.transform);
            go[21].transform.position = GameObject.Find(publicVariable.Cho_Ma2_defaultPosition).transform.position;

            go[22] = Instantiate(Cho_Sang, anchor.transform);
            go[22].transform.position = GameObject.Find(publicVariable.Cho_Sang1_defaultPosition).transform.position;

            go[23] = Instantiate(Cho_Sang, anchor.transform);
            go[23].transform.position = GameObject.Find(publicVariable.Cho_Sang2_defaultPosition).transform.position;

            go[24] = Instantiate(Cho_Jol, anchor.transform);
            go[24].transform.position = GameObject.Find(mal_position[1, 4]).transform.position;

            go[25] = Instantiate(Cho_Jol, anchor.transform);
            go[25].transform.position = GameObject.Find(mal_position[3, 4]).transform.position;

            go[26] = Instantiate(Cho_Jol, anchor.transform);
            go[26].transform.position = GameObject.Find(mal_position[5, 4]).transform.position;

            go[27] = Instantiate(Cho_Jol, anchor.transform);
            go[27].transform.position = GameObject.Find(mal_position[7, 4]).transform.position;

            go[28] = Instantiate(Cho_Jol, anchor.transform);
            go[28].transform.position = GameObject.Find(mal_position[9, 4]).transform.position;

            go[29] = Instantiate(Cho_King, anchor.transform);
            go[29].transform.position = GameObject.Find(mal_position[5, 2]).transform.position;

            go[30] = Instantiate(Cho_Sa, anchor.transform);
            go[30].transform.position = GameObject.Find(mal_position[4, 1]).transform.position;

            go[31] = Instantiate(Cho_Sa, anchor.transform);
            go[31].transform.position = GameObject.Find(mal_position[6, 1]).transform.position;
        }

        if (!PhotonNetwork.isMasterClient)
            for (i = 0; i < 32; i++)
                go[i].transform.Rotate(0, 180.0f, 0);


    }

}