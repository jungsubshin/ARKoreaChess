using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviour {

    PublicVariable publicVariable;
    //public GameObject CannotJoinPanel;
    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(gameObject);
        publicVariable = GameObject.Find("PublicVariable").GetComponent<PublicVariable>();
        PhotonNetwork.ConnectUsingSettings("v01");
        
    }
	
	// Update is called once per frame
	void Update () {

        //if (PhotonNetwork.connectionStateDetailed.ToString() != "Joined")
        //{
        //    TextInfos.text = PhotonNetwork.connectionStateDetailed.ToString();
        //}
        //else
        //{
        //    TextInfos.text = "Connected to " + PhotonNetwork.room.Name + "Player(s) Online " + PhotonNetwork.room.PlayerCount;
        //    //Test.SetActive(true);
        //}


	}

    void OnConnectedToMaster() {
        Debug.Log("Connected with Master");
        PhotonNetwork.JoinLobby();
    }

    void OnJoinedLobby() {
        RoomOptions MyRoomOptions = new RoomOptions();
        MyRoomOptions.MaxPlayers = 2;
        Debug.Log("Connected with Lobby");

        if (publicVariable.getIsCreate() == true) {
            if (!PhotonNetwork.CreateRoom(publicVariable.getRoomName(), MyRoomOptions, TypedLobby.Default))
                Debug.Log("생성실패");
        }
        else {
            Debug.Log("접속중... " + publicVariable.getRoomName());
            PhotonNetwork.JoinRoom(publicVariable.getRoomName());
        }
    }

    void OnPhotonJoinRoomFailed() {
        Debug.Log("접속실패");
        //CannotJoinPanel.SetActive(true);
    }



    void OnJoinedRoom() {
        Debug.Log("Connected with Room, Roomname : " + publicVariable.getRoomName());
        SceneManager.LoadScene("GameScene");
    }

    void OnPhotonPlayerConnected(PhotonPlayer newPlayer) {
        publicVariable.setIsPlayer(true);
        publicVariable.setIsLeft(false);
        Debug.Log("newbie : " + newPlayer.ID);
        
    }

    void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer) {
        publicVariable.setIsPlayer(false);
        publicVariable.setIsLeft(true);
        Debug.Log("bye : " + otherPlayer.ID);
    }
}
 