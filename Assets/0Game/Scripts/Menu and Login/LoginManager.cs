using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.CloudSave;
using System.Collections.Generic;
using System.Threading.Tasks;
using Photon.Pun;
using Photon.Realtime;

public class LoginManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject loadingIndicator;

    [Header("TMPro")]
    [SerializeField] private TMP_InputField usernameInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private TMP_Text statusText;

    [Header("Buttons")]
    [SerializeField] private Button loginBTN;
    [SerializeField] private Button signupBTN;
    [SerializeField] private Button signOutBTN;
    [SerializeField] private Button guestLoginBTN;

    private bool isLoggingOut = false;

    public async void Start()
    {
        loadingIndicator.SetActive(false);
        loginBTN.interactable = true;
        signupBTN.interactable = true;
        signOutBTN.interactable = true;
        guestLoginBTN.interactable = true;
        await InitializeUnityServices();
    }

    private async Task InitializeUnityServices()
    {
        if (UnityServices.State == ServicesInitializationState.Initialized)
            return;

        await UnityServices.InitializeAsync();
    }

    [System.Obsolete]
    public async void JoinAsGuest()
    {
        try
        {
            guestLoginBTN.interactable = false;
            loadingIndicator.SetActive(true);
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            statusText.text = "Guest login successful! Connecting to Game...";

            await SavePlayerName("Guest_" + Random.Range(1000, 9999));

            LoadLevel.LoadMainMenu();
        }
        catch (RequestFailedException e)
        {
            statusText.text = "Guest login failed: " + e.Message;
            guestLoginBTN.interactable = true;
        }
        finally
        {
            loadingIndicator.SetActive(false);
        }
    }

    public async void SignupBTNPressed()
    {
        string username = usernameInput.text.Trim();
        string password = passwordInput.text.Trim();

        try
        {
            signupBTN.interactable = false;
            loadingIndicator.SetActive(true);
            await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);
            statusText.text = "Signup successful! Please sign out to log in.";

            // Disable login and signup buttons, show sign out button.
            loginBTN.interactable = false;
            signupBTN.interactable = false;
            signOutBTN.gameObject.SetActive(true); // Show sign-out button
        }
        catch (RequestFailedException e)
        {
            statusText.text = "Signup failed: " + e.Message;
            signupBTN.interactable = true;
        }
        finally
        {
            loadingIndicator.SetActive(false);
        }
    }

    [System.Obsolete]
    public async void LoginBTNPressed()
    {
        string username = usernameInput.text.Trim();
        string password = passwordInput.text.Trim();

        try
        {
            loginBTN.interactable = false;
            loadingIndicator.SetActive(true);
            await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(username, password);
            statusText.text = "Login successful! Connecting to Photon...";

            await SavePlayerName(username);
            LoadLevel.LoadMainMenu();
        }
        catch (RequestFailedException)
        {
            statusText.text = "Invalid username or password!";
            loginBTN.interactable = true;
        }
        finally
        {
            loadingIndicator.SetActive(false);
        }
    }

    public void SignOut()
    {
        AuthenticationService.Instance.SignOut();
        PhotonNetwork.Disconnect();
        isLoggingOut = true;

        statusText.text = "You have been signed out. Now you can log in.";

        // Hide sign-out button, show login button
        signOutBTN.gameObject.SetActive(false);
        loginBTN.gameObject.SetActive(true); // Show login button after sign out
    }

    [System.Obsolete]
    private async Task SavePlayerName(string playerName)
    {
        try
        {
            var data = new Dictionary<string, object> { { "PlayerName", playerName } };
            await CloudSaveService.Instance.Data.ForceSaveAsync(data);
            Debug.Log("Player name saved to Cloud Save.");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to save name: " + e.Message);
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected from Photon: " + cause);

        if (isLoggingOut)
        {
            isLoggingOut = false;
            PhotonNetwork.LoadLevel(0);
        }
    }
}
