using Photon.Pun;
using UnityEngine;

public class CameraFollow : MonoBehaviourPunCallbacks
{
    public Transform player;  // Reference to the player object (local player's transform)
    public Vector3 offset;    // Offset from the player (camera's position relative to player)

    private void Start()
    {
        // If this camera is not for the local player, disable it.
        if (!photonView.IsMine)
        {
            gameObject.SetActive(false);
            return;
        }

        // Ensure the camera starts in the correct position relative to the player
        if (player != null)
        {
            transform.position = player.position + offset; // Start at the correct position with the offset
        }
    }

    private void Update()
    {
        // Only move the camera if it's for the local player
        if (photonView.IsMine && player != null)
        {
            // Update the camera's position based on the local player's position
            transform.position = player.position + offset;
        }
    }

    public void SetPlayer(Transform newPlayer)
    {
        // Set the player reference after the player is instantiated
        player = newPlayer;

        // Recalculate the offset so that the camera maintains its position relative to the player
        offset = transform.position - player.position; // Calculate the offset from the player's initial position
    }
}
