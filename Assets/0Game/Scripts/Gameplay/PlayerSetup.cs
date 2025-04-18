using UnityEngine;

public class PlayerSetup : MonoBehaviour
{
    public Movement movement;
    public GameObject cam; // Reference to the Camera component on the player prefab

    private void Awake()
    {
        movement.enabled = false;
        cam.SetActive(false);
    }
    public void IsLocalPlayer()
    {
        movement.enabled = true; // Enable movement for the local playe
        cam.SetActive(true);
    }
}
