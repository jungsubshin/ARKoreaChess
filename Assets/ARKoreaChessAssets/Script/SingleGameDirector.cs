using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.IO;

public class SingleGameDirector : MonoBehaviour {
    public GameObject Han_Cha, Han_Ma, Han_Po, Han_Sang, Han_Jol, Han_Sa, Han_King;
    public GameObject Cho_Cha, Cho_Ma, Cho_Po, Cho_Sang, Cho_Jol, Cho_Sa, Cho_King;

    public GameObject Han_Cha3d, Han_Ma3d, Han_Po3d, Han_Sang3d, Han_Jol3d, Han_Sa3d, Han_King3d;
    public GameObject Cho_Cha3d, Cho_Ma3d, Cho_Po3d, Cho_Sang3d, Cho_Jol3d, Cho_Sa3d, Cho_King3d;

    public GameObject Rbutton, Lbutton, ArrowMark3D, ArrowMark, Attackpoint3D, Attackpoint, PlaneGenerator;

    public Image GameFinish, JangGoon;
    //public GameObject me, enemy, controller, generator;
    public Text text;
    AudioSource aud;
    Vector3[,] mal_position = new Vector3[11, 11];
    GameObject mal_object, tmp_object, anchor, touchTarget, lastTouch, lastMeObj = null;
    GameObject[] enemy_objs = new GameObject[8];
    GameObject[] go = new GameObject[32];
    GameObject pre_mal, movedPosition = null, jangGoonMark = null;
    GiboData giboData;
    SensorControl sensorControl;
    PublicVariable publicVariable;
    string position_name = null, myTag = "Cho", sensor = "Sensor";
    public string nowTurn = "Cho";
    int i, j, k = 0, bot_count = 1;
    int game_turn = 0;
    bool isChoosing = false, isEnemy = false;
    bool[,] attackedPosition = new bool[11, 11];
    int stage = 1;
    int cho_score = 0, han_score = 0;
    string source = "";
    public string[,] gibo_pos = new string[200, 4];
    int cho_Set;
    int han_Set;
    bool endGame = false;
    int endPoint = 0;
    string[] tmp_str;

    GameObject col_obj;
    // Use this for initialization
    void Start() {
        anchor = GameObject.Find("Anchor");
        giboData = GameObject.Find("GiboData").GetComponent<GiboData>();
        publicVariable = GameObject.Find("PublicVariable").GetComponent<PublicVariable>();
        PlaneGenerator.SetActive(false);
        for (i = 1; i <= 9; i++)
            for (j = 1; j <= 10; j++) {
                position_name = "w" + i + "h" + j;
                mal_object = GameObject.Find(position_name);
                mal_position[i, j] = mal_object.transform.position;
                attackedPosition[i, j] = false;
            }
        ReadDate();
        CreateDefaultMal();
    }
    void Update() {
        if (endGame == true) { // 대국종료
            GameFinish.gameObject.SetActive(true);
            JangGoon.gameObject.SetActive(false);
            Rbutton.GetComponent<Button>().interactable = false;
        }
        else {
            GameFinish.gameObject.SetActive(false);
            Rbutton.GetComponent<Button>().interactable = true;
            if (game_turn == 0) Lbutton.GetComponent<Button>().interactable = false;
            else Lbutton.GetComponent<Button>().interactable = true;

            if (gibo_pos[game_turn, 3] == "장군") {
                JangGoon.gameObject.SetActive(true);
                if (game_turn % 2 == 1) {
                    GameObject go = GameObject.Find("Han_King(Clone)");
                    setAttackedPosition(go);
                }
                else {
                    GameObject go = GameObject.Find("Cho_King(Clone)");
                    setAttackedPosition(go);
                }
            }
            else {
                if (jangGoonMark != null)
                    Destroy(jangGoonMark);
                JangGoon.gameObject.SetActive(false);
            }
        }

        

        

        text.text = game_turn.ToString();
    }
    public void ReadDate() {
        int i = 0, turn = 1;
        int selectedGibo = publicVariable.getSelectedGibo();
        string[] tmp_text;
        string from_gibo, to_gibo;
        while (true) {
            if (i == giboData.getGiboDataSize(selectedGibo)) {
                break;
            }
            source = giboData.getGiboData(selectedGibo, i++);
            if (source == null) {
                break;
            }
            if (source != "") {
                if (source.Contains("초차림")) {
                    if (source.Contains("마상마상")) cho_Set = 1;
                    else if (source.Contains("마상상마")) cho_Set = 2;
                    else if (source.Contains("상마상마")) cho_Set = 3;
                    else if (source.Contains("상마마상")) cho_Set = 4;
                    Debug.Log("초차림" + cho_Set);
                }
                if (source.Contains("한차림")) {
                    if (source.Contains("마상마상")) han_Set = 1;
                    else if (source.Contains("마상상마")) han_Set = 2;
                    else if (source.Contains("상마상마")) han_Set = 3;
                    else if (source.Contains("상마마상")) han_Set = 4;
                    Debug.Log("한차림" + han_Set);
                }
                if (source[0] != '[') {
                    tmp_text = source.Split('.');
                    for (int j = 1; j < tmp_text.Length; j++) {
                        if (tmp_text[j][1] == '0')
                            from_gibo = "w" + tmp_text[j][2] + "h10";
                        else
                            from_gibo = "w" + tmp_text[j][2] + "h" + tmp_text[j][1];
                        if (tmp_text[j][4] == '0')
                            to_gibo = "w" + tmp_text[j][5] + "h10";
                        else
                            to_gibo = "w" + tmp_text[j][5] + "h" + tmp_text[j][4];
                        gibo_pos[turn, 0] = from_gibo;
                        gibo_pos[turn, 1] = to_gibo;

                        if (tmp_text[j].Contains("장군"))
                            gibo_pos[turn, 3] = "장군";
                        else
                            gibo_pos[turn, 3] = "x";
                        turn++;//턴 증가
                    }
                }
            }
            // Debug.Log(result);

        }
        endPoint = turn - 1;
        Debug.Log("총 수:" + endPoint + "기보 입력 완료");
    }

    private void setMovePosition(GameObject touchTarget) {
        Vector3 vc;
        if (movedPosition != null) Destroy(movedPosition);

        vc = touchTarget.transform.position;

        if (publicVariable.getIsModeling()) {
            vc.y += 0.12f;
            movedPosition = Instantiate(ArrowMark3D, anchor.transform);
        }
        else {
            vc.y += 0.04f;
            movedPosition = Instantiate(ArrowMark, anchor.transform);
        }
        movedPosition.transform.position = vc;
    }

    private void setAttackedPosition(GameObject touchTarget) {
        Vector3 vc;
        if (jangGoonMark != null) Destroy(jangGoonMark);

        vc = touchTarget.transform.position;

        if (publicVariable.getIsModeling()) {
            vc.y += 0.12f;
            jangGoonMark = Instantiate(Attackpoint3D, anchor.transform);
        }
        else {
            vc.y += 0.04f;
            jangGoonMark = Instantiate(Attackpoint, anchor.transform);
        }
        jangGoonMark.transform.position = vc;
    }

    public void LButtonDown() {
        endGame = false;
        if (gibo_pos[game_turn, 2] != null) {
            Debug.Log(gibo_pos[game_turn, 2].ToString());
            pre_mal = Instantiate(Resources.Load(gibo_pos[game_turn, 2], typeof(GameObject)), anchor.transform) as GameObject;
            pre_mal.transform.position = GameObject.Find(gibo_pos[game_turn, 1]).transform.position;
            pre_mal.transform.Rotate(0, 180.0f, 0);

        }
        Debug.Log(game_turn + " " + gibo_pos[game_turn, 0].ToString());
        mal_object = GameObject.Find(gibo_pos[game_turn, 1]).GetComponent<SensorControl>().getCollisionObj();
        mal_object.transform.position = GameObject.Find(gibo_pos[game_turn, 0]).transform.position;
        setMovePosition(mal_object);
        game_turn--;
    }
    public void RButtonDown() {

        game_turn++;
        Debug.Log(game_turn + " " + gibo_pos[game_turn, 0].ToString());
        mal_object = GameObject.Find(gibo_pos[game_turn, 0]).GetComponent<SensorControl>().getCollisionObj();

        if (GameObject.Find(gibo_pos[game_turn, 1]).GetComponent<SensorControl>().getExistsFlag() == true) {
            tmp_str = GameObject.Find(gibo_pos[game_turn, 1]).GetComponent<SensorControl>().getCollisionObj().ToString().Split('(');
            gibo_pos[game_turn, 2] = tmp_str[0];
            Destroy(GameObject.Find(gibo_pos[game_turn, 1]).GetComponent<SensorControl>().getCollisionObj());
        }
        mal_object.transform.position = GameObject.Find(gibo_pos[game_turn, 1]).transform.position;
        setMovePosition(mal_object);
        if (game_turn == endPoint) {
            endGame = true;
        }
    }
    public void DoMinMax(int stage, int depth) {
        if (depth == 0) {
            switch (stage) {
                case 2:
                    if (GameObject.Find("w9h4").GetComponent<SensorControl>().getExistsFlag() == false) {
                        go[8].transform.position = GameObject.Find("w2h7").transform.position;
                    }
                    else {
                        go[12].transform.position = GameObject.Find("w8h7").transform.position;
                    }
                    break;
                case 4: {
                        go[4].transform.position = GameObject.Find("w3h8").transform.position;
                    }
                    break;

            }
        }
        else if (han_score >= cho_score) {
            depth--;
            DoMinMax(stage, depth);
        }
        else if (han_score < cho_score) {

            depth--;
        }

    }
    /* 점수계산 함수
     * 졸 1점 사 1점 상 2점 포 3점 마 4점 차5점 왕 10점
     *
    */
    public GameObject getTouchObject() {//다른 스크립트에 현재 선택한 장기말 오브젝트 반환
        return touchTarget;
    }
    private void Calc_score() {
        cho_score = 0;
        han_score = 0;
        for (i = 1; i <= 9; i++) {
            for (j = 1; j <= 10; j++) {
                position_name = "w" + i + "h" + j;
                tmp_object = GameObject.Find(position_name);
                if (tmp_object.GetComponent<SensorControl>().getExistsFlag() == true) {

                    if (tmp_object.GetComponent<SensorControl>().getCollisionObj().tag.Equals("Han")) {
                        Debug.Log(tmp_object.GetComponent<SensorControl>().getCollisionObj().name);
                        switch (tmp_object.GetComponent<SensorControl>().getCollisionObj().name) {
                            case "Han_Jol(Clone)":
                                han_score += 1;
                                break;
                            case "Han_Po(Clone)":
                                han_score += 3;
                                break;
                            case "Han_Ma(Clone)":
                                han_score += 4;
                                break;
                            case "Han_Cha(Clone)":
                                han_score += 5;
                                break;
                            case "Han_Sang(Clone)":
                                han_score += 2;
                                break;
                            case "Han_Sa(Clone)":
                                han_score += 1;
                                break;
                            case "Han_King(Clone)":
                                han_score += 10;
                                break;
                        }
                    }
                    else if (tmp_object.GetComponent<SensorControl>().getCollisionObj().tag.Equals("Cho")) {
                        switch (tmp_object.GetComponent<SensorControl>().getCollisionObj().name) {
                            case "Cho_Jol(Clone)":
                                cho_score += 1;
                                break;
                            case "Cho_Po(Clone)":
                                cho_score += 3;
                                break;
                            case "Cho_Ma(Clone)":
                                cho_score += 4;
                                break;
                            case "Cho_Cha(Clone)":
                                cho_score += 5;
                                break;
                            case "Cho_Sang(Clone)":
                                cho_score += 2;
                                break;
                            case "Cho_Sa(Clone)":
                                cho_score += 1;
                                break;
                            case "Cho_King(Clone)":
                                cho_score += 10;
                                break;
                        }
                    }
                    Debug.Log(han_score);
                }
            }
        }
        text.text = han_score.ToString() + " " + cho_score.ToString();
        if (han_score >= cho_score)
            text.text += "win";
        else
            text.text += "lose";
    }
    public string changeTurn(string now_turn) {
        if (now_turn.Equals("Han")) now_turn = "Cho";
        else now_turn = "Han";
        stage++;
        text.text = stage.ToString();
        Calc_score();
        return now_turn;
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
            go[0].transform.position = mal_position[1, 10];

            go[1] = Instantiate(Han_Cha3d, anchor.transform);
            go[1].transform.position = mal_position[9, 10];

            go[2] = Instantiate(Han_Po3d, anchor.transform);
            go[2].transform.position = mal_position[2, 8];

            go[3] = Instantiate(Han_Po3d, anchor.transform);
            go[3].transform.position = mal_position[8, 8];

            //상차림 체크
            if (cho_Set == 1)//마상마상
            {
                go[4] = Instantiate(Han_Ma3d, anchor.transform);
                go[4].transform.position = mal_position[3, 10];

                go[5] = Instantiate(Han_Ma3d, anchor.transform);
                go[5].transform.position = mal_position[8, 10];

                go[6] = Instantiate(Han_Sang3d, anchor.transform);
                go[6].transform.position = mal_position[2, 10];

                go[7] = Instantiate(Han_Sang3d, anchor.transform);
                go[7].transform.position = mal_position[7, 10];
            }
            else if (cho_Set == 2)//마상상마
            {
                go[4] = Instantiate(Han_Ma3d, anchor.transform);
                go[4].transform.position = mal_position[2, 10];

                go[5] = Instantiate(Han_Ma3d, anchor.transform);
                go[5].transform.position = mal_position[8, 10];

                go[6] = Instantiate(Han_Sang3d, anchor.transform);
                go[6].transform.position = mal_position[3, 10];

                go[7] = Instantiate(Han_Sang3d, anchor.transform);
                go[7].transform.position = mal_position[7, 10];
            }
            else if (cho_Set == 3)//상마상마
            {
                go[4] = Instantiate(Han_Ma3d, anchor.transform);
                go[4].transform.position = mal_position[2, 10];

                go[5] = Instantiate(Han_Ma3d, anchor.transform);
                go[5].transform.position = mal_position[7, 10];

                go[6] = Instantiate(Han_Sang3d, anchor.transform);
                go[6].transform.position = mal_position[3, 10];

                go[7] = Instantiate(Han_Sang3d, anchor.transform);
                go[7].transform.position = mal_position[8, 10];
            }
            else if (cho_Set == 4)//상마마상
            {
                go[4] = Instantiate(Han_Ma3d, anchor.transform);
                go[4].transform.position = mal_position[3, 10];

                go[5] = Instantiate(Han_Ma3d, anchor.transform);
                go[5].transform.position = mal_position[7, 10];

                go[6] = Instantiate(Han_Sang3d, anchor.transform);
                go[6].transform.position = mal_position[2, 10];

                go[7] = Instantiate(Han_Sang3d, anchor.transform);
                go[7].transform.position = mal_position[8, 10];
            }
            //여기까지

            go[8] = Instantiate(Han_Jol3d, anchor.transform);
            go[8].transform.position = mal_position[1, 7];

            go[9] = Instantiate(Han_Jol3d, anchor.transform);
            go[9].transform.position = mal_position[3, 7];

            go[10] = Instantiate(Han_Jol3d, anchor.transform);
            go[10].transform.position = mal_position[5, 7];

            go[11] = Instantiate(Han_Jol3d, anchor.transform);
            go[11].transform.position = mal_position[7, 7];

            go[12] = Instantiate(Han_Jol3d, anchor.transform);
            go[12].transform.position = mal_position[9, 7];

            go[13] = Instantiate(Han_King3d, anchor.transform);
            go[13].transform.position = mal_position[5, 9];


            go[14] = Instantiate(Han_Sa3d, anchor.transform);
            go[14].transform.position = mal_position[4, 10];

            go[15] = Instantiate(Han_Sa3d, anchor.transform);
            go[15].transform.position = mal_position[6, 10];





            go[16] = Instantiate(Cho_Cha3d, anchor.transform); // 초
            go[16].transform.position = mal_position[1, 1];

            go[17] = Instantiate(Cho_Cha3d, anchor.transform);
            go[17].transform.position = mal_position[9, 1];

            go[18] = Instantiate(Cho_Po3d, anchor.transform);
            go[18].transform.position = mal_position[2, 3];

            go[19] = Instantiate(Cho_Po3d, anchor.transform);
            go[19].transform.position = mal_position[8, 3];

            //상차림 체크
            if (han_Set == 1)//마상마상
            {
                go[20] = Instantiate(Cho_Ma3d, anchor.transform);
                go[20].transform.position = mal_position[2, 1];

                go[22] = Instantiate(Cho_Sang3d, anchor.transform);
                go[22].transform.position = mal_position[3, 1];

                go[21] = Instantiate(Cho_Ma3d, anchor.transform);
                go[21].transform.position = mal_position[7, 1];

                go[23] = Instantiate(Cho_Sang3d, anchor.transform);
                go[23].transform.position = mal_position[8, 1];
            }
            else if (han_Set == 2)//마상상마
            {
                go[20] = Instantiate(Cho_Ma3d, anchor.transform);
                go[20].transform.position = mal_position[2, 1];

                go[22] = Instantiate(Cho_Sang3d, anchor.transform);
                go[22].transform.position = mal_position[3, 1];

                go[21] = Instantiate(Cho_Ma3d, anchor.transform);
                go[21].transform.position = mal_position[8, 1];

                go[23] = Instantiate(Cho_Sang3d, anchor.transform);
                go[23].transform.position = mal_position[7, 1];
            }
            else if (han_Set == 3)//상마상마
            {
                go[20] = Instantiate(Cho_Ma3d, anchor.transform);
                go[20].transform.position = mal_position[3, 1];

                go[22] = Instantiate(Cho_Sang3d, anchor.transform);
                go[22].transform.position = mal_position[2, 1];

                go[21] = Instantiate(Cho_Ma3d, anchor.transform);
                go[21].transform.position = mal_position[8, 1];

                go[23] = Instantiate(Cho_Sang3d, anchor.transform);
                go[23].transform.position = mal_position[7, 1];
            }
            else if (han_Set == 3)//상마마상
            {
                go[20] = Instantiate(Cho_Ma3d, anchor.transform);
                go[20].transform.position = mal_position[3, 1];

                go[22] = Instantiate(Cho_Sang3d, anchor.transform);
                go[22].transform.position = mal_position[2, 1];

                go[21] = Instantiate(Cho_Ma3d, anchor.transform);
                go[21].transform.position = mal_position[7, 1];

                go[23] = Instantiate(Cho_Sang3d, anchor.transform);
                go[23].transform.position = mal_position[8, 1];
            }
            //여기까지

            go[24] = Instantiate(Cho_Jol3d, anchor.transform);
            go[24].transform.position = mal_position[1, 4];

            go[25] = Instantiate(Cho_Jol3d, anchor.transform);
            go[25].transform.position = mal_position[3, 4];

            go[26] = Instantiate(Cho_Jol3d, anchor.transform);
            go[26].transform.position = mal_position[5, 4];

            go[27] = Instantiate(Cho_Jol3d, anchor.transform);
            go[27].transform.position = mal_position[7, 4];

            go[28] = Instantiate(Cho_Jol3d, anchor.transform);
            go[28].transform.position = mal_position[9, 4];

            go[29] = Instantiate(Cho_King3d, anchor.transform);
            go[29].transform.position = mal_position[5, 2];

            go[30] = Instantiate(Cho_Sa3d, anchor.transform);
            go[30].transform.position = mal_position[4, 1];

            go[31] = Instantiate(Cho_Sa3d, anchor.transform);
            go[31].transform.position = mal_position[6, 1];
        }



        else {
            go[0] = Instantiate(Han_Cha, anchor.transform); // 한
            go[0].transform.position = mal_position[1, 10];

            go[1] = Instantiate(Han_Cha, anchor.transform);
            go[1].transform.position = mal_position[9, 10];

            go[2] = Instantiate(Han_Po, anchor.transform);
            go[2].transform.position = mal_position[2, 8];

            go[3] = Instantiate(Han_Po, anchor.transform);
            go[3].transform.position = mal_position[8, 8];
            //상차림 체크
            if (cho_Set == 1)//마상마상
            {
                go[4] = Instantiate(Han_Ma, anchor.transform);
                go[4].transform.position = mal_position[3, 10];

                go[5] = Instantiate(Han_Ma, anchor.transform);
                go[5].transform.position = mal_position[8, 10];

                go[6] = Instantiate(Han_Sang, anchor.transform);
                go[6].transform.position = mal_position[2, 10];

                go[7] = Instantiate(Han_Sang, anchor.transform);
                go[7].transform.position = mal_position[7, 10];
            }
            else if (cho_Set == 2)//마상상마
            {
                go[4] = Instantiate(Han_Ma, anchor.transform);
                go[4].transform.position = mal_position[2, 10];

                go[5] = Instantiate(Han_Ma, anchor.transform);
                go[5].transform.position = mal_position[8, 10];

                go[6] = Instantiate(Han_Sang, anchor.transform);
                go[6].transform.position = mal_position[3, 10];

                go[7] = Instantiate(Han_Sang, anchor.transform);
                go[7].transform.position = mal_position[7, 10];
            }
            else if (cho_Set == 3)//상마상마
            {
                go[4] = Instantiate(Han_Ma, anchor.transform);
                go[4].transform.position = mal_position[2, 10];

                go[5] = Instantiate(Han_Ma, anchor.transform);
                go[5].transform.position = mal_position[7, 10];

                go[6] = Instantiate(Han_Sang, anchor.transform);
                go[6].transform.position = mal_position[3, 10];

                go[7] = Instantiate(Han_Sang, anchor.transform);
                go[7].transform.position = mal_position[8, 10];
            }
            else if (cho_Set == 4)//상마마상
            {
                go[4] = Instantiate(Han_Ma, anchor.transform);
                go[4].transform.position = mal_position[3, 10];

                go[5] = Instantiate(Han_Ma, anchor.transform);
                go[5].transform.position = mal_position[7, 10];

                go[6] = Instantiate(Han_Sang, anchor.transform);
                go[6].transform.position = mal_position[2, 10];

                go[7] = Instantiate(Han_Sang, anchor.transform);
                go[7].transform.position = mal_position[8, 10];
            }
            //여기까지
            go[8] = Instantiate(Han_Jol, anchor.transform);
            go[8].transform.position = mal_position[1, 7];

            go[9] = Instantiate(Han_Jol, anchor.transform);
            go[9].transform.position = mal_position[3, 7];

            go[10] = Instantiate(Han_Jol, anchor.transform);
            go[10].transform.position = mal_position[5, 7];

            go[11] = Instantiate(Han_Jol, anchor.transform);
            go[11].transform.position = mal_position[7, 7];

            go[12] = Instantiate(Han_Jol, anchor.transform);
            go[12].transform.position = mal_position[9, 7];

            go[13] = Instantiate(Han_King, anchor.transform);
            go[13].transform.position = mal_position[5, 9];


            go[14] = Instantiate(Han_Sa, anchor.transform);
            go[14].transform.position = mal_position[4, 10];

            go[15] = Instantiate(Han_Sa, anchor.transform);
            go[15].transform.position = mal_position[6, 10];




            go[16] = Instantiate(Cho_Cha, anchor.transform); // 초
            go[16].transform.position = mal_position[1, 1];

            go[17] = Instantiate(Cho_Cha, anchor.transform);
            go[17].transform.position = mal_position[9, 1];

            go[18] = Instantiate(Cho_Po, anchor.transform);
            go[18].transform.position = mal_position[2, 3];

            go[19] = Instantiate(Cho_Po, anchor.transform);
            go[19].transform.position = mal_position[8, 3];
            //상차림 체크
            if (han_Set == 1)//마상마상
            {
                go[20] = Instantiate(Cho_Ma, anchor.transform);
                go[20].transform.position = mal_position[2, 1];

                go[22] = Instantiate(Cho_Sang, anchor.transform);
                go[22].transform.position = mal_position[3, 1];

                go[21] = Instantiate(Cho_Ma, anchor.transform);
                go[21].transform.position = mal_position[7, 1];

                go[23] = Instantiate(Cho_Sang, anchor.transform);
                go[23].transform.position = mal_position[8, 1];
            }
            else if (han_Set == 2)//마상상마
            {
                go[20] = Instantiate(Cho_Ma, anchor.transform);
                go[20].transform.position = mal_position[2, 1];

                go[22] = Instantiate(Cho_Sang, anchor.transform);
                go[22].transform.position = mal_position[3, 1];

                go[21] = Instantiate(Cho_Ma, anchor.transform);
                go[21].transform.position = mal_position[8, 1];

                go[23] = Instantiate(Cho_Sang, anchor.transform);
                go[23].transform.position = mal_position[7, 1];
            }
            else if (han_Set == 3)//상마상마
            {
                go[20] = Instantiate(Cho_Ma, anchor.transform);
                go[20].transform.position = mal_position[3, 1];

                go[22] = Instantiate(Cho_Sang, anchor.transform);
                go[22].transform.position = mal_position[2, 1];

                go[21] = Instantiate(Cho_Ma, anchor.transform);
                go[21].transform.position = mal_position[8, 1];

                go[23] = Instantiate(Cho_Sang, anchor.transform);
                go[23].transform.position = mal_position[7, 1];
            }
            else if (han_Set == 3)//상마마상
            {
                go[20] = Instantiate(Cho_Ma, anchor.transform);
                go[20].transform.position = mal_position[3, 1];

                go[22] = Instantiate(Cho_Sang, anchor.transform);
                go[22].transform.position = mal_position[2, 1];

                go[21] = Instantiate(Cho_Ma, anchor.transform);
                go[21].transform.position = mal_position[7, 1];

                go[23] = Instantiate(Cho_Sang, anchor.transform);
                go[23].transform.position = mal_position[8, 1];
            }
            //여기까지
            go[24] = Instantiate(Cho_Jol, anchor.transform);
            go[24].transform.position = mal_position[1, 4];

            go[25] = Instantiate(Cho_Jol, anchor.transform);
            go[25].transform.position = mal_position[3, 4];

            go[26] = Instantiate(Cho_Jol, anchor.transform);
            go[26].transform.position = mal_position[5, 4];

            go[27] = Instantiate(Cho_Jol, anchor.transform);
            go[27].transform.position = mal_position[7, 4];

            go[28] = Instantiate(Cho_Jol, anchor.transform);
            go[28].transform.position = mal_position[9, 4];

            go[29] = Instantiate(Cho_King, anchor.transform);
            go[29].transform.position = mal_position[5, 2];

            go[30] = Instantiate(Cho_Sa, anchor.transform);
            go[30].transform.position = mal_position[4, 1];

            go[31] = Instantiate(Cho_Sa, anchor.transform);
            go[31].transform.position = mal_position[6, 1];
        }

        for (i = 0; i < 32; i++) {
            go[i].transform.Rotate(0, 180.0f, 0);
        }

    }

}