using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendMessageDelegate : MonoBehaviour
{
    public delegate void MessageEvent(); // Does this need to return some sort of object id?
    public static event MessageEvent OnMessageSent;

    public void SendBroadcast()
    {
        OnMessageSent?.Invoke();
    }
}
