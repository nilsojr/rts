using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Health : NetworkBehaviour
{
    [SerializeField] private int maxHealth = 100;

    [SyncVar(hook = nameof(HandleHealthUpdated))]
    private int currentHealth;

    public event Action ServerOnDie;

    public event Action<int, int> ClientOnHealthUpdated;

    #region Server
    
    public override void OnStartServer()
    {
        currentHealth = maxHealth;
        Debug.Log("current " + currentHealth);
        UnitBase.ServerOnPlayerDie += ServerHandlePlayerDie;
    }

    public override void OnStopServer()
    {
        UnitBase.ServerOnPlayerDie -= ServerHandlePlayerDie;
    }

    [Server]
    private void ServerHandlePlayerDie(int connectionId)
    {
        if (connectionId != connectionToClient.connectionId) return;

        DealDamage(currentHealth);
    }

    [Server]
    public void DealDamage(int amount)
    {
        Debug.Log("taking damage");
        
        if (currentHealth == 0) { return; }

        currentHealth = Mathf.Max(currentHealth - amount, 0);

        if (currentHealth != 0) { return; }

        ServerOnDie?.Invoke();
    }
    
    #endregion

    #region Client

    private void HandleHealthUpdated(int oldHealth, int newHealth)
    {
        ClientOnHealthUpdated?.Invoke(newHealth, maxHealth);
    }

    #endregion
}
