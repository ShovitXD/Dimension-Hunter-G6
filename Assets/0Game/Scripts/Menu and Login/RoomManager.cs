using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text ErrorTXT;
    [SerializeField] private TMP_InputField RoomCodeInp;
    [SerializeField] private GameObject RoomPannel;
    private string RoomCode;

    private void Start()
    {
        // Get the Room Code from GameManager
        if (GameManager.Instance != null)
        {
            RoomCode = GameManager.Instance.RoomCode;
            Debug.Log(RoomCode);
        }
        else
        {
            Debug.LogError("GameManager instance not found!");
            return;
        }

        RoomPannel.SetActive(false);
    }

    // Create a new room
    public void CreateRoom()
    {
        if (!string.IsNullOrEmpty(RoomCode))
        {
            RoomOptions options = new RoomOptions { MaxPlayers = 4 };
            LoadLevel.LoadLobby();
            PhotonNetwork.CreateRoom(RoomCode, options);
            Debug.Log("Tried to create a room");
        }
        else
        {
            Debug.LogError("Room code is empty.");
        }
    }
    public override void OnCreatedRoom()
    {
        Debug.Log("Successfully created room.");
    }

    // Join a room by input code
    public void JoinRoom()
    {
        if (!string.IsNullOrEmpty(RoomCodeInp.text))
        {
            LoadLevel.LoadLobby();
            PhotonNetwork.JoinRoom(RoomCodeInp.text);
            Debug.Log("Tried to join a room");
           
        }
        else
        {
            ErrorTXT.text = "Please input a valid code.";
        }
    }
   
    // Toggle the join room panel
    public void ToggleMainMenu()
    {
        RoomPannel.SetActive(!RoomPannel.activeSelf);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Room creation failed: " + message);
        ErrorTXT.text = "Room creation failed: " + message;
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Failed to join room: " + message);

        if (returnCode == ErrorCode.GameFull)
        {
            ErrorTXT.text = "Room is full";
        }
        else
        {
            ErrorTXT.text = "Failed to join room:" + message;
        }
    }

}
