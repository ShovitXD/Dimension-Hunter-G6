using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Photon.Pun;
using System;

public class RemoveAccount : MonoBehaviourPunCallbacks
{
    [Header("UI Elements")]
    [SerializeField] private GameObject confirmationPopup;
    [SerializeField] private GameObject loadingIndicator;

    private void Start()
    {
        // Ensure UI elements are inactive initially
        if (confirmationPopup == null || loadingIndicator == null)
        {
            Debug.LogError("UI Elements not assigned in the inspector.");
            return;
        }
        //Setting all the Confirmation UI inactive
        confirmationPopup.SetActive(false);
        loadingIndicator.SetActive(false);
    }

    public void ShowConfirmation()
    {
        //To show confirmatuin before deleting the account
        if (confirmationPopup != null)
        {
            confirmationPopup.SetActive(true);
        }
    }

    public void HideConfirmation()
    {
        //Hide confirmation window if pressed no
        if (confirmationPopup != null)
        {
            confirmationPopup.SetActive(false);
        }
    }

    [Obsolete]
    public async void DeleteAccountAsync()
    {
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            Debug.LogWarning("User is not signed in.");
            return;
        }

        try
        {
            loadingIndicator.SetActive(true);
            confirmationPopup.SetActive(false);

            // Remove player data from Cloud Save
            await CloudSaveService.Instance.Data.Player.DeleteAsync("PlayerName");

            // Delete Unity Authentication account
            await AuthenticationService.Instance.DeleteAccountAsync();
            //signout  cause it worked in the signup things
            AuthenticationService.Instance.SignOut();
            // Load back to the login screen
            LoadLevel.LoadLoginScreen(); 
        }
        catch (Exception ex)
        {
            // Handle errors
            Debug.LogError("Error while deleting account: " + ex.Message);
        }
        finally
        {
            loadingIndicator.SetActive(false);
        }
    }

    public override void OnDisconnected(Photon.Realtime.DisconnectCause cause)
    {
        Debug.Log("Disconnected from Photon: " + cause);
    }
}
