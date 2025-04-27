using Photon.Pun;
using TMPro;
using UnityEngine;

public class SpawnIn : MonoBehaviourPunCallbacks
{
    [Header("Player Prefab and Spawn Points")]
    public GameObject playerPrefab;  // The player prefab to spawn
    public Transform[] spawnPoints;  // Array of spawn points
    public TMP_Text roomText;  // Room display text

    void SpawnPlayer()
    {
        // Check if playerPrefab and spawnPoints are assigned
        if (playerPrefab == null)
        {
            Debug.LogError("Player prefab is not assigned!");
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("Spawn points are not assigned or empty!");
            return;
        }

        // Calculate the spawn point index based on the number of players in the room
        int spawnIndex = (PhotonNetwork.CurrentRoom.PlayerCount - 1) % spawnPoints.Length;

        // Spawn the player at the calculated spawn point
        GameObject _player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoints[spawnIndex].position, spawnPoints[spawnIndex].rotation);

        // Initialize the local player
        _player.GetComponent<PlayerSetup>().IsLocalPlayer();

        // Store the reference of the local player in the GameManager------
        if (_player.GetComponent<PhotonView>().IsMine)
        {
            GameManager.Instance.LocalPlayerInstance = _player;
        }
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room successfully.");
        roomText.text = PhotonNetwork.CurrentRoom.Name;
        SpawnPlayer();
    }
}
