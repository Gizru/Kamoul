using Kamoul.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public void Connect()
    {
        NetworkManager.Instance.Connect(); 
    }
}
