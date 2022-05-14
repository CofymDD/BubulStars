using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class ConnectionToServer : MonoBehaviourPunCallbacks
{
    public static ConnectionToServer Instance;
    [SerializeField] private TMP_InputField inputRoomName;
    [SerializeField] private TMP_Text roomName;
    [SerializeField] private Transform transformRoomList;
    [SerializeField] private Transform transformPlayerList;
    [SerializeField] private GameObject roomItemPrefab; 
    [SerializeField] private GameObject playerListItem;
    [Space(10)]
    [SerializeField] private GameObject startGameButton;
    private void Awake() 
    {
        Instance = this;
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
       PhotonNetwork.JoinLobby();
       
    }
    public override void OnJoinedLobby()
    {
        WindowsManager.Layout.OpenLayout("MainMenu");
        Debug.Log("Connected to Lobby!");
    }
    public void CreateNewRoom()
    {
        if(string.IsNullOrEmpty(inputRoomName.text))
        {
            return;
        }
        RoomOptions currentRoom = new RoomOptions();
        currentRoom.IsOpen = true;
        currentRoom.MaxPlayers = 10;
        PhotonNetwork.CreateRoom(inputRoomName.text, currentRoom);
    }
    public override void OnCreatedRoom()
    {
        
    }
    public override void OnJoinedRoom()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        WindowsManager.Layout.OpenLayout("GameRoom");
        if(PhotonNetwork.IsMasterClient) startGameButton.SetActive(true);           
        else startGameButton.SetActive(false);
            
        roomName.text = PhotonNetwork.CurrentRoom.Name;
        Player[] players = PhotonNetwork.PlayerList;
        foreach (Transform trns in transformPlayerList)
        {
            Destroy(trns.gameObject);
        }
        for(int i = 0; i < players.Length; i++)
        {
         Instantiate(playerListItem, transformPlayerList).GetComponent<PlayerListItem>().SetUp(players[i]);   
        }
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if(PhotonNetwork.IsMasterClient) startGameButton.SetActive(true);           
        else startGameButton.SetActive(false);
    }
    public void ConnectToRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
       
    }
    public override void OnLeftRoom()
    {
        WindowsManager.Layout.OpenLayout("MainMenu");
    }
    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform trns in transformRoomList)
        {
           Destroy(trns.gameObject);
        }
        for(int i = 0; i < roomList.Count; i++)
        {
            Instantiate(roomItemPrefab, transformRoomList).GetComponent<RoomItem>().SetUp(roomList[i]);
        }
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(playerListItem, transformPlayerList).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }
    public void StartGameLevel(int levelIndex)
    {
        PhotonNetwork.LoadLevel(levelIndex);
    }
}
