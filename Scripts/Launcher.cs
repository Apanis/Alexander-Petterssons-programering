using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Launcher : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    public static Launcher Instance;
    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text logText;
    [SerializeField] Image logImage;
    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListItemPrefab;
    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject playerListItemPrefab;
    [SerializeField] GameObject MasterClient;
    [SerializeField] GameObject Client;
    private AudioSource audioSource;
    public AudioClip playerLeft;
    public static Launcher instance;
    public Slider MaxPlayerSlider;
    public Toggle IsPrivateToggle;

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "0.0.0";
        Pause.paused = false;
        instance = this;
        audioSource = GetComponent<AudioSource>();
        logImage.enabled = true;
        Pause.paused = false;
        Instance = this;
    }
    void Start()
    {
        Debug.Log("Connected to master");
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to master");
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        MenuManager.Instance.OpenMenu("Title");
        Debug.Log("Joined lobby");
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        audioSource.PlayOneShot(playerLeft);
        //Instantiate(logText, logImage.transform);
        //logText.text = otherPlayer.NickName + " has left the room.";
        //logText.color = Color.red;
        
    }
    public override void OnPlayerEnteredRoom(Player otherPlayer)
    {
        base.OnPlayerEnteredRoom(otherPlayer);
        audioSource.PlayOneShot(playerLeft);
        //Instantiate(logText, logImage.transform);
        //logText.text = otherPlayer.NickName + " has joined the room.";
        //logText.color = Color.green;
        //logImage.enabled = true;
        Instantiate(playerListItemPrefab, playerListContent).GetComponent<playerListItem>().SetUp(otherPlayer);
    }

    public void CreateRoom()
    {
        if(string.IsNullOrEmpty(roomNameInputField.text))
            PhotonNetwork.CreateRoom("Room " + Random.Range(1, 20));
        else
            PhotonNetwork.CreateRoom(roomNameInputField.text);

        //RoomOptions options = new RoomOptions();
        //options.MaxPlayers = (byte)MaxPlayerSlider.value;
        //options.IsVisible = IsPrivateToggle.isOn;

        MenuManager.Instance.OpenMenu("Loading");
        
    }
    
    public override void OnJoinedRoom()
    {
        MenuManager.Instance.OpenMenu("Rooms");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name.ToUpper();

        

        Player[] players = PhotonNetwork.PlayerList;

        foreach(Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }

        for(int i = 0; i < players.Count(); i++)
		{
			Instantiate(playerListItemPrefab, playerListContent).GetComponent<playerListItem>().SetUp(players[i]);
		}

        

        MasterClient.SetActive(PhotonNetwork.IsMasterClient);
        Client.SetActive(!PhotonNetwork.IsMasterClient);
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        MasterClient.SetActive(PhotonNetwork.IsMasterClient);
        Client.SetActive(!PhotonNetwork.IsMasterClient);
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Room Creation Failed: " + message;
        MenuManager.Instance.OpenMenu("Errors");
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("Loading");
    }
    public override void OnLeftRoom()
    {
        MenuManager.Instance.OpenMenu("Title");
        base.OnLeftRoom();
    }
    
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach(Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }
        for(int i = 0; i < roomList.Count; i++)
        {
            if(roomList[i].RemovedFromList)
                continue;
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<roomListItem>().SetUp(roomList[i]);
        }
    }
    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.Instance.OpenMenu("Loading");
    }
    
    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
    public void Leave()
    {
        PhotonNetwork.LeaveRoom();
        Pause.disconnecting = true;
    }
}
