using System;
using UnityEngine;

public interface ISteamClient
{
    void StartHost(int maxMembers, Action onSuccess, Action<string> onFail);

    void JoinWithId(ulong id, Action onSuccess, Action<string> onFail);
}


public interface ISteamPlayer : IPlayer
{
    
}