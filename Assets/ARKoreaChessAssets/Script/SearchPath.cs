using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchPath : MonoBehaviour {
    GameObject tmp_obj, go, touch_mal;
    string tmp_name; // 경로위의 말

    GameDirector gameDirector;
    int row, col, i, j, GO = 1, STOP = 2;
    Vector3 vc;
    string malName; // 터치한 말

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void searchPath(string sensor_name, string mal_name) {
        gameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();
        touch_mal = GameObject.Find(sensor_name).GetComponent<SensorControl>().getCollisionObj();
        row = int.Parse(sensor_name.Substring(1, 1));
        if (sensor_name.Length == 4)
            col = int.Parse(sensor_name.Substring(3, 1));
        else
            col = int.Parse(sensor_name.Substring(3, 2));

        malName = mal_name;

        if (mal_name.Equals("Han_Jol(Clone)") || mal_name.Equals("Cho_Jol(Clone)")) {
            jolSearch(row, col, mal_name);
        }
        else if (mal_name.Equals("Han_Ma(Clone)") || mal_name.Equals("Cho_Ma(Clone)")) {
            maSearch(row, col);
        }
        else if (mal_name.Equals("Han_Sang(Clone)") || mal_name.Equals("Cho_Sang(Clone)")) {
            sangSearch(row, col);
        }
        else if (mal_name.Equals("Han_Po(Clone)") || mal_name.Equals("Cho_Po(Clone)")) {
            poSearch(row, col);
        }
        else if (mal_name.Equals("Han_Cha(Clone)") || mal_name.Equals("Cho_Cha(Clone)")) {
            chaSearch(row, col);
        }
        else if (mal_name.Equals("Han_King(Clone)") || mal_name.Equals("Cho_King(Clone)")) {
            kingSearch(row, col);
        }
        else if (mal_name.Equals("Han_Sa(Clone)") || mal_name.Equals("Cho_Sa(Clone)")) {
            saSearch(row, col);
        }
    }

    private void jolSearch(int row, int col, string mal_name) {
        searchPath(row + 1, col, STOP);
        searchPath(row - 1, col, STOP);
        if (mal_name == "Cho_Jol(Clone)")
            searchPath(row, col + 1, STOP);
        else
            searchPath(row, col - 1, STOP);

        switch (row) {
            case 4:
                if (col == 8 && mal_name == "Cho_Jol(Clone)")
                    searchPath(row + 1, col + 1, STOP);
                if (col == 3 && mal_name == "Han_Jol(Clone)")
                    searchPath(row + 1, col - 1, STOP);
                break;
            case 5:
                if (col == 2 && mal_name == "Han_Jol(Clone)") {
                    searchPath(row - 1, col - 1, STOP);
                    searchPath(row + 1, col - 1, STOP);
                }
                if (col == 9 && mal_name == "Cho_Jol(Clone)") {
                    searchPath(row - 1, col + 1, STOP);
                    searchPath(row + 1, col + 1, STOP);
                }
                break;
            case 6:
                if (col == 8 && mal_name == "Cho_Jol(Clone)")
                    searchPath(row - 1, col + 1, STOP);
                if (col == 3 && mal_name == "Han_Jol(Clone)")
                    searchPath(row - 1, col - 1, STOP);
                break;
            default:
                break;
        }
    }

    private void maSearch(int row, int col) {
        if (searchPath(row, col + 1, GO)) { // 위
            searchPath(row - 1, col + 2, STOP);
            searchPath(row + 1, col + 2, STOP);
        }
        //아래쪽
        if (searchPath(row, col - 1, GO)) {
            searchPath(row + 1, col - 2, STOP);
            searchPath(row - 1, col - 2, STOP);
        }
        //오른쪽
        if (searchPath(row + 1, col, GO)) {
            searchPath(row + 2, col + 1, STOP);
            searchPath(row + 2, col - 1, STOP);
        }
        //왼쪽
        if (searchPath(row - 1, col, GO)) {
            searchPath(row - 2, col + 1, STOP);
            searchPath(row - 2, col - 1, STOP);
        }
    }

    private void sangSearch(int row, int col) {
        if (searchPath(row, col + 1, 1)) { // 위
            if (searchPath(row + 1, col + 2, GO))
                searchPath(row + 2, col + 3, STOP);
            if (searchPath(row - 1, col + 2, GO))
                searchPath(row - 2, col + 3, STOP);
        }
        //아래쪽
        if (searchPath(row, col - 1, 1)) {
            if (searchPath(row + 1, col - 2, GO))
                searchPath(row + 2, col - 3, STOP);
            if (searchPath(row - 1, col - 2, GO))
                searchPath(row - 2, col - 3, STOP);
        }
        //오른쪽
        if (searchPath(row + 1, col, 1)) {
            if (searchPath(row + 2, col + 1, GO))
                searchPath(row + 3, col + 2, STOP);
            if (searchPath(row + 2, col - 1, GO))
                searchPath(row + 3, col - 2, STOP);
        }
        //왼쪽
        if (searchPath(row - 1, col, 1)) {
            if (searchPath(row - 2, col + 1, GO))
                searchPath(row - 3, col + 2, STOP);
            if (searchPath(row - 2, col - 1, GO))
                searchPath(row - 3, col - 2, STOP);
        }
    }

    private void poSearch(int row, int col) {
        tmp_name = null;
        for (int k = 1; k <= 10; k++) {
            if (!searchPath(row + k, col, 1)) {
                for (int l = k + 1; l <= 10; l++) {
                    if (!searchPath(row + l, col, 2)) {
                        break;
                    }
                }
                break;
            }
            if (tmp_name == "Han_Po(Clone)" || tmp_name == "Cho_Po(Clone)") break;
        }
        tmp_name = null;
        for (int k = 1; k <= 10; k++) {
            if (!searchPath(row - k, col, 1)) {
                for (int l = k + 1; l <= 10; l++) {
                    if (!searchPath(row - l, col, 2)) {
                        break;
                    }
                }
                break;
            }
            if (tmp_name == "Han_Po(Clone)" || tmp_name == "Cho_Po(Clone)") break;
        }
        tmp_name = null;
        for (int n = 1; n <= 10; n++) {
            if (!searchPath(row, col + n, 1)) {
                for (int l = n + 1; l <= 10; l++) {
                    if (!searchPath(row, col + l, 2)) {
                        break;
                    }
                }
                break;
            }
            if (tmp_name == "Han_Po(Clone)" || tmp_name == "Cho_Po(Clone)") break;
        }
        tmp_name = null;
        for (int n = 1; n <= 10; n++) {
            if (!searchPath(row, col - n, 1)) {
                for (int l = n + 1; l <= 10; l++) {
                    if (!searchPath(row, col - l, 2)) {
                        break;
                    }
                }
                break;
            }
            if (tmp_name == "Han_Po(Clone)" || tmp_name == "Cho_Po(Clone)") break;
        }
        switch (row) {
            case 4:
                if (col == 1 || col == 8) {
                    if (!searchPath(row + 1, col + 1, 1)) {
                        searchPath(row + 2, col + 2, 2);
                    }
                }
                else if (col == 3 || col == 10) {
                    if (!searchPath(row + 1, col - 1, 1)) {
                        searchPath(row + 2, col - 2, 2);
                    }
                }
                break;
            case 5:
                if (col == 2 || col == 9) {
                    if (searchPath(row + 1, col + 1, 2))
                        searchPath(row + 1, col - 1, 2);
                    if (searchPath(row - 1, col + 1, 2))
                        searchPath(row - 1, col - 1, 2);
                }
                break;
            case 6:
                if (col == 1 || col == 8) {
                    if (!searchPath(row - 1, col + 1, 1)) {
                        searchPath(row - 2, col + 2, 2);
                    }
                }
                else if (col == 3 || col == 10) {
                    if (!searchPath(row - 1, col - 1, 1)) {
                        searchPath(row + 2, col - 2, 2);
                    }
                }
                break;
            default:
                break;
        }
    }

    private void chaSearch(int row, int col) {
        //차 서치
        for (int k = 1; k <= 10; k++) {
            if (!searchPath(row + k, col, 2))
                break;
        }
        for (int k = 1; k <= 10; k++) {
            if (!searchPath(row - k, col, 2))
                break;
        }
        for (int n = 1; n <= 10; n++) {
            if (!searchPath(row, col + n, 2))
                break;
        }
        for (int n = 1; n <= 10; n++) {
            if (!searchPath(row, col - n, 2))
                break;
        }
        if (row >= 4 && row <= 6) {
            switch (row) {
                case 4:
                    if (col == 1 || col == 8) {
                        if (searchPath(row + 1, col + 1, 2))
                            searchPath(row + 2, col + 2, 2);
                    }
                    else if (col == 3 || col == 10) {
                        if (searchPath(row + 1, col - 1, 2))
                            searchPath(row + 2, col - 2, 2);
                    }
                    break;
                case 5:
                    if (col == 2 || col == 9) {
                        if (searchPath(row + 1, col + 1, 2))
                            searchPath(row + 1, col - 1, 2);
                        if (searchPath(row - 1, col + 1, 2))
                            searchPath(row - 1, col - 1, 2);
                    }
                    break;
                case 6:
                    if (col == 1 || col == 8) {
                        if (searchPath(row - 1, col + 1, 2))
                            searchPath(row - 2, col + 2, 2);
                    }
                    else if (col == 3 || col == 10) {
                        if (searchPath(row - 1, col - 1, 2))
                            searchPath(row - 2, col - 2, 2);
                    }
                    break;
                default:
                    break;
            }
        }
    }

    private void kingSearch(int row, int col) {
        switch (row) {
            case 4:
                if (col == 1 || col == 8) {
                    searchPath(row + 1, col + 1, 2);
                    searchPath(row, col + 1, 2);
                    searchPath(row + 1, col, 2);
                }
                else if (col == 2 || col == 9) {
                    searchPath(row + 1, col, 2);
                    searchPath(row, col + 1, 2);
                    searchPath(row, col - 1, 2);
                }
                else {
                    searchPath(row + 1, col - 1, 2);
                    searchPath(row + 1, col, 2);
                    searchPath(row, col - 1, 2);
                }
                break;
            case 5:
                if (col == 1 || col == 8) {
                    searchPath(row + 1, col, 2);
                    searchPath(row - 1, col, 2);
                    searchPath(row, col + 1, 2);
                }
                else if (col == 2 || col == 9) {
                    searchPath(row - 1, col + 1, 2);
                    searchPath(row, col + 1, 2);
                    searchPath(row + 1, col + 1, 2);
                    searchPath(row + 1, col, 2);
                    searchPath(row + 1, col - 1, 2);
                    searchPath(row, col - 1, 2);
                    searchPath(row - 1, col - 1, 2);
                    searchPath(row - 1, col, 2);
                }
                else {
                    searchPath(row + 1, col, 2);
                    searchPath(row - 1, col, 2);
                    searchPath(row, col - 1, 2);
                }
                break;
            case 6:
                if (col == 1 || col == 8) {
                    searchPath(row - 1, col, 2);
                    searchPath(row - 1, col + 1, 2);
                    searchPath(row, col + 1, 2);
                }
                else if (col == 2 || col == 9) {
                    searchPath(row, col - 1, 2);
                    searchPath(row, col + 1, 2);
                    searchPath(row - 1, col, 2);
                }
                else {
                    searchPath(row - 1, col, 2);
                    searchPath(row - 1, col - 1, 2);
                    searchPath(row, col - 1, 2);
                }
                break;
            default:
                break;
        }
    }

    private void saSearch(int row, int col) {
        switch (row) {
            case 4:
                if (col == 1 || col == 8) {
                    searchPath(row + 1, col + 1, 2);
                    searchPath(row, col + 1, 2);
                    searchPath(row + 1, col, 2);
                }
                else if (col == 2 || col == 9) {
                    searchPath(row + 1, col, 2);
                    searchPath(row, col + 1, 2);
                    searchPath(row, col - 1, 2);
                }
                else {
                    searchPath(row + 1, col - 1, 2);
                    searchPath(row + 1, col, 2);
                    searchPath(row, col - 1, 2);
                }
                break;
            case 5:
                if (col == 1 || col == 8) {
                    searchPath(row + 1, col, 2);
                    searchPath(row - 1, col, 2);
                    searchPath(row, col + 1, 2);
                }
                else if (col == 2 || col == 9) {
                    searchPath(row - 1, col + 1, 2);
                    searchPath(row, col + 1, 2);
                    searchPath(row + 1, col - 1, 2);
                    searchPath(row + 1, col, 2);
                    searchPath(row + 1, col - 1, 2);
                    searchPath(row, col - 1, 2);
                    searchPath(row - 1, col - 1, 2);
                    searchPath(row - 1, col, 2);
                }
                else {
                    searchPath(row + 1, col, 2);
                    searchPath(row - 1, col, 2);
                    searchPath(row, col - 1, 2);
                }
                break;
            case 6:
                if (col == 1 || col == 8) {
                    searchPath(row - 1, col, 2);
                    searchPath(row - 1, col + 1, 2);
                    searchPath(row, col + 1, 2);
                }
                else if (col == 2 || col == 9) {
                    searchPath(row, col - 1, 2);
                    searchPath(row, col + 1, 2);
                    searchPath(row - 1, col, 2);
                }
                else {
                    searchPath(row - 1, col, 2);
                    searchPath(row - 1, col - 1, 2);
                    searchPath(row, col - 1, 2);
                }
                break;
            default:
                break;
        }
    }


    bool searchPath(int row, int col, int dir) {
        string tmp_string;
        if (row <= 0 || row >= 10 || col <= 0 || col >= 11) { // 범위 밖
            return false;
        }
        else {
            tmp_string = "w" + row + "h" + col;
            tmp_obj = GameObject.Find(tmp_string);
            if (tmp_obj.GetComponent<SensorControl>().getExistsFlag()) { // 센서 위에 뭐가 있으면
                tmp_name = tmp_obj.GetComponent<SensorControl>().getCollisionObj().name;
                if (dir == GO) { // 경로
                    if (malName == "Han_Ma(Clone)" || malName == "Han_Sang(Clone)" || malName == "Cho_Ma(Clone)" || malName == "Cho_Sang(Clone)")
                        return false;
                    else if (tmp_name == "Han_Po(Clone)" || tmp_name == "Cho_Po(Clone)")
                        return true;
                    else
                        return false;
                }
                else { // 도착지

                    if (tmp_obj.GetComponent<SensorControl>().getCollisionObj().tag == touch_mal.tag) { // 아군

                    }
                    else { // 적군
                        if (tmp_name == "Han_Po(Clone)" || tmp_name == "Cho_Po(Clone)") {  // 포는 포끼리 못먹음
                            if (malName == "Han_Po(Clone)" || malName == "Cho_Po(Clone)")
                                tmp_obj.GetComponent<Renderer>().enabled = false;
                            else
                                tmp_obj.GetComponent<Renderer>().enabled = true;
                        }

                        else if (tmp_name == "Han_King(Clone)" || tmp_name == "Cho_King(Clone)") { // 왕일때 장군
                            tmp_obj.GetComponent<Renderer>().enabled = true;
                            gameDirector.setEnemyObj(tmp_obj);
                        }
                        else {// 나머지 말
                            tmp_obj.GetComponent<Renderer>().enabled = true;
                            gameDirector.setEnemyObj(tmp_obj);
                        }

                    }
                    return false;
                }
            }
            else {
                if (dir == STOP) { // 도착지점
                    tmp_obj.GetComponent<Renderer>().enabled = true; // 불들어옴

                }
                return true;
            }
        }
    }
}
