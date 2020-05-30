using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    //[SerializeField] Button[] btn;
    [SerializeField] private GameObject btnRoom;
    [SerializeField] private GameObject content;

    [SerializeField] private Text logTxt, logCreateTxt;

    [SerializeField] private GameObject LoadScreen, MenuScreen, CreateNewRoomScreen;

    [SerializeField] private InputField NameRoom;

    private List<RoomInfo> roomList;



    #region PUN Settings
    void Start()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            //Change the Text
            logTxt.text = "Проверьте подключение к интернету и перезайдите";
        }
        else
        {
            LoadingMenu();
        }
    }

    private void LoadingMenu()
    {
        PhotonNetwork.NickName = "Player " + Random.Range(1000, 10000);

        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "1";
        PhotonNetwork.ConnectUsingSettings();
    }


    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        MenuScreen.SetActive(true);
        LoadScreen.SetActive(false);
    }

    private void CreateRoom(string nameRoom)
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 2;

        PhotonNetwork.CreateRoom(nameRoom, options);
    }

    private void ClearRoomList()
    {
        foreach(Transform a in content.transform)
        {
            Destroy(a.gameObject);
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> list)
    {
        roomList = list;
        ClearRoomList();

        foreach(RoomInfo a in roomList)
        {
            GameObject newRoomBtn = Instantiate(btnRoom, content.transform) as GameObject;

            newRoomBtn.transform.Find("Name").GetComponent<Text>().text = a.Name;

            if (a.PlayerCount == 0)
            {
                Destroy(newRoomBtn);
            }

            newRoomBtn.transform.Find("Players").GetComponent<Text>().text = a.PlayerCount + " / " + a.MaxPlayers;

            newRoomBtn.GetComponent<Button>().onClick.AddListener(delegate { JoinRoom(newRoomBtn.transform); });
        }

        //base.OnRoomListUpdate(roomList);
    }

    public void JoinRoom(Transform btn)
    {
        string nameRoom = btn.transform.Find("Name").GetComponent<Text>().text;
        PhotonNetwork.JoinRoom(nameRoom);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Game"); //сцена с игрой
    }
    #endregion

    #region other menu settings

    public void BackToMenu()
    {
        MenuScreen.SetActive(true);
        CreateNewRoomScreen.SetActive(false);
    }

    public void ShowSettingsForCreateRoom()
    {
        CreateNewRoomScreen.SetActive(true);
        MenuScreen.SetActive(false);
    }

    public void CreateCustomRoom()
    {
        if(NameRoom.text != "" || NameRoom.text != " ")
        {
            CreateRoom(NameRoom.text);
        }
        else
        {
            logCreateTxt.text = "Недопустимое имя для комнаты!";
        }
    }

    public void ShowCreateSettingRoom()
    {
        CreateNewRoomScreen.SetActive(true);
        MenuScreen.SetActive(false);
    }



    #endregion
}
