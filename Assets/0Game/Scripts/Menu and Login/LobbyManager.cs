using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    private bool isQuitting = false;

    private void Start()
    {
        ConnectToLobby();
    }

    // Connect to the lobby when this scene starts
    private void ConnectToLobby()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinLobby();
        }
        else
        {
            Debug.LogWarning("Photon is not connected!");
            LoadLevel.LoadServerSelect();
        }
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
    }

    public void Quit()
    {
        isQuitting = true;
        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogError("Disconnected from Photon: " + cause);

        if (isQuitting)
        {
            isQuitting = false;
            LoadLevel.LoadServerSelect();
        }

        if (!PhotonNetwork.IsConnected)
        {
            Debug.LogWarning("Photon is disconnected. Skipping critical actions.");
            return;
        }       
    }
}
