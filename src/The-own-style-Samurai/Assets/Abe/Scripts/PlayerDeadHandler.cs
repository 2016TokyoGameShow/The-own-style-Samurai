// ----- ----- ----- ----- -----
//
// PlayerDeadHandler
//
// 作成日：
// 作成者：
//
// <概要>
//
//
// ----- ----- ----- ----- -----

using UnityEngine.EventSystems;

public interface PlayerDeadHandler : IEventSystemHandler
{
    void OnPlayerDead();
}
