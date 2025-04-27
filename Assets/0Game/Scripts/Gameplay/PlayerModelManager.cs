using Photon.Pun;
using UnityEngine;

public class PlayerModelManager : MonoBehaviourPunCallbacks, IPunObservable
{
    // Array to hold clothing objects (limit to 4 as per your requirement)
    private GameObject[] clothingObjects = new GameObject[4];

    // A reference to the PhotonView to handle object ownership
    private PhotonView photonView;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();

        // Find all child objects named "Clothing" and assign them to the array (max 4 objects)
        clothingObjects[0] = transform.Find("Clothing/Clothing1")?.gameObject;
        clothingObjects[1] = transform.Find("Clothing/Clothing2")?.gameObject;
        clothingObjects[2] = transform.Find("Clothing/Clothing3")?.gameObject;
        clothingObjects[3] = transform.Find("Clothing/Clothing4")?.gameObject;

        // Print names of all clothing objects added to the array
        for (int i = 0; i < clothingObjects.Length; i++)
        {
            if (clothingObjects[i] != null)
            {
                Debug.Log($"Clothing object {i + 1}: {clothingObjects[i].name}");
            }
            else
            {
                Debug.Log($"Clothing object {i + 1} is missing!");
            }
        }

        // Initially, disable all clothing items for every player
        foreach (var clothing in clothingObjects)
        {
            if (clothing != null)
                clothing.SetActive(false);
        }
    }

    // Public methods to enable and disable clothing objects for this specific player
    public void EnableClothing1()
    {
        if (photonView.IsMine)
            clothingObjects[0]?.SetActive(true);
    }

    public void DisableClothing1()
    {
        if (photonView.IsMine)
            clothingObjects[0]?.SetActive(false);
    }

    public void EnableClothing2()
    {
        if (photonView.IsMine)
            clothingObjects[1]?.SetActive(true);
    }

    public void DisableClothing2()
    {
        if (photonView.IsMine)
            clothingObjects[1]?.SetActive(false);
    }

    public void EnableClothing3()
    {
        if (photonView.IsMine)
            clothingObjects[2]?.SetActive(true);
    }

    public void DisableClothing3()
    {
        if (photonView.IsMine)
            clothingObjects[2]?.SetActive(false);
    }

    public void EnableClothing4()
    {
        if (photonView.IsMine)
            clothingObjects[3]?.SetActive(true);
    }

    public void DisableClothing4()
    {
        if (photonView.IsMine)
            clothingObjects[3]?.SetActive(false);
    }

    // This method will ensure that the changes are synchronized across the network
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Sync the state of all clothing items for this player
            for (int i = 0; i < clothingObjects.Length; i++)
            {
                if (clothingObjects[i] != null)
                {
                    stream.SendNext(clothingObjects[i].activeSelf);
                }
            }
        }
        else
        {
            // Receive the clothing state changes from the network
            for (int i = 0; i < clothingObjects.Length; i++)
            {
                if (clothingObjects[i] != null)
                {
                    bool isActive = (bool)stream.ReceiveNext();
                    clothingObjects[i].SetActive(isActive);
                }
            }
        }
    }
}
