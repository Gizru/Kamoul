using Riptide;
using Riptide.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kamoul.Core;

public class NetworkManager : Singleton<NetworkManager>
{
    protected override void Awake()
    {
        base.Awake();
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, true);
    }

    public Server Server;
    [SerializeField] private ushort m_Port = 7777;
    [SerializeField] private ushort m_MaxPlayers = 3;

    private void Start()
    {
        Server = new Server();
        Server.Start(m_Port, m_MaxPlayers);
    }

    private void FixedUpdate()
    {
        Server.Update();
    }
}
