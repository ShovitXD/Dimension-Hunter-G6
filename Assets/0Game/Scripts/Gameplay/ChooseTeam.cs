using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TeamModelSelector : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text TeamNumber;
    [SerializeField] private GameObject Pannel, ToogleTipON, ToogleTipOFF;
    [SerializeField] private Button[] teamButtons;
    private bool IsItActive = false;

    private int selectedSlot = -1;

    private void Start()
    {
        // Ensure the TeamNumber text is initialized to "No Team Selected" at the start
        if (photonView.IsMine)
        {
            TeamNumber.text = "No Team Selected";
        }
    }

    private void Update()
    {
        TogglePannel();
    }

    private void TogglePannel()
    {
        if (Input.GetKeyDown(KeyCode.H) && !IsItActive)
        {
            Pannel.SetActive(true);
            ToogleTipON.SetActive(false);
            ToogleTipOFF.SetActive(true);
            IsItActive = true; 
            Cursor.lockState = CursorLockMode.None; 
            Cursor.visible = true; 
        }
        else if (Input.GetKeyDown(KeyCode.H) && IsItActive)
        {
            Pannel.SetActive(false);
            ToogleTipON.SetActive(true);
            ToogleTipOFF.SetActive(false);
            IsItActive = false; 
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void R1()
    {
        SelectSlot(0, 0);
    }

    public void R2()
    {
        SelectSlot(1, 0);
    }

    public void B1()
    {
        SelectSlot(2, 1);
    }

    public void B2()
    {
        SelectSlot(3, 1);
    }

    private void SelectSlot(int slot, int team)
    {
        // Debugging GameManager instance and teamButtons array
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager instance is null!");
        }

        if (teamButtons == null || teamButtons.Length == 0)
        {
            Debug.LogError("teamButtons array is not initialized or empty!");
        }

        // If player selected the same slot again, unassign it
        if (selectedSlot == slot)
        {
            photonView.RPC("EnableButtonRPC", RpcTarget.AllBuffered, slot);
            GameManager.Instance.AssignTeam(-1);
            selectedSlot = -1;

            // Update the TeamNumber to reflect no team
            photonView.RPC("UpdateTeamNumber", RpcTarget.AllBuffered, "No Team Selected");
        }
        else
        {
            // If another slot was selected, re-enable the previous slot's button
            if (selectedSlot != -1)
            {
                photonView.RPC("EnableButtonRPC", RpcTarget.AllBuffered, selectedSlot);
            }

            // Disable the newly selected slot button and assign the player to that team
            photonView.RPC("DisableButtonRPC", RpcTarget.AllBuffered, slot);
            GameManager.Instance.AssignTeam(team);
            selectedSlot = slot;

            // Update the TeamNumber based on the selected team
            string teamName = (team == 0) ? "Red Team" : "Blue Team";
            photonView.RPC("UpdateTeamNumber", RpcTarget.AllBuffered, teamName);
        }
    }

    [PunRPC]
    private void UpdateTeamNumber(string teamName)
    {
        if (photonView.IsMine)
        {
            TeamNumber.text = teamName;  // Update the TeamNumber text for this specific player
        }
    }

    [PunRPC]
    private void DisableButtonRPC(int slot)
    {
        if (slot >= 0 && slot < teamButtons.Length && teamButtons[slot] != null)
        {
            teamButtons[slot].interactable = false;
        }
        else
        {
            Debug.LogError($"Button at slot {slot} is null or out of range.");
        }
    }

    [PunRPC]
    private void EnableButtonRPC(int slot)
    {
        if (slot >= 0 && slot < teamButtons.Length && teamButtons[slot] != null)
        {
            teamButtons[slot].interactable = true;
        }
        else
        {
            Debug.LogError($"Button at slot {slot} is null or out of range.");
        }
    }
}
