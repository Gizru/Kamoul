using Riptide;
using Riptide.Utils;
using System;
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

    public Client Client;
    [SerializeField] private string m_Ip = "127.0.0.1";
    [SerializeField] private ushort m_Port = 7777;

    private void Start()
    {
        Client = new Client();
        Client.Connected += OnClientConnected;
    }

    private void OnClientConnected(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    public void Connect()
    {
        Client.Connect($"{m_Ip}:{m_Port}");
    }

    private void FixedUpdate()
    {
        Client.Update();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        Client.Connected -= OnClientConnected;
    }
}
