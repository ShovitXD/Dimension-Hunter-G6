using TMPro;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using System;

public class WaitingRoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text RoomCodeTXT; // Text to display the room code
    [SerializeField] private GameObject Pannel; // Panel for character selection
    [SerializeField] private Button[] charBTN; // Array of character selection buttons
    private int UIIndex; // Index for UI navigation

    private void Start()
    {
        if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
        {
            // Set the room code text to the current room's name
            RoomCodeTXT.text = PhotonNetwork.CurrentRoom.Name;
        }
        else
        {
            Debug.LogWarning("Not connected to a room.");
        }

        Pannel.SetActive(false); // Hide the panel initially
    }

    private void Update()
    {
        CharacterSelection();
    }

    // Toggle character select and team select panel
    private void CharacterSelection()
    {
        // Toggle Panel
        if (Input.GetKeyDown(KeyCode.H))
        {
            Pannel.SetActive(!Pannel.activeSelf);
            SetNotReady();
        }

        if (!Pannel.activeSelf) return;

        // Scroll through UI
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            UIIndex = (UIIndex - 1 + charBTN.Length) % charBTN.Length;
            UpdateDisplay();
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            UIIndex = (UIIndex + 1) % charBTN.Length;
            UpdateDisplay();
        }

        // Confirm selection
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ConfirmSelection();
            SetReady();
            SetNotReady();
        }
    }

    private void SetNotReady()
    {
        // Implement logic to set the player as "not ready"
        throw new NotImplementedException();
    }

    private void SetReady()
    {
        // Implement logic to set the player as "ready"
        throw new NotImplementedException();
    }

    private void ConfirmSelection()
    {
        // Implement logic to confirm the character selection
        throw new NotImplementedException();
    }

    private void UpdateDisplay()
    {
        // Implement logic to update the UI display based on the current UIIndex
        throw new NotImplementedException();
    }
}