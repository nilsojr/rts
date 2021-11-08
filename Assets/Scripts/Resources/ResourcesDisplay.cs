using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;

public class ResourcesDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text resourcesText = null;

    private RTSPlayer player;

    private void Start()
    {
        player = NetworkClient.connection.identity.GetComponent<RTSPlayer>();

        ClientHandleOnResourcesUpdated(player.GetResources());
            
        player.ClientOnResourcesUpdated += ClientHandleOnResourcesUpdated;
    }

    private void OnDestroy()
    {
        player.ClientOnResourcesUpdated -= ClientHandleOnResourcesUpdated;
    }

    private void ClientHandleOnResourcesUpdated(int resources)
    {
        resourcesText.text = $"Resources: {resources}";
    }
}
