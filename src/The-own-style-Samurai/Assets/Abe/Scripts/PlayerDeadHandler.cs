// ----- ----- ----- ----- -----
//
// PlayerDeadHandler
//
// 作成日：2016/4/20
// 作成者：阿部
//
// <概要>
// プレイヤーの死亡を通知するイベントハンドラです
//
// ----- ----- ----- ----- -----

using UnityEngine.EventSystems;

public interface PlayerDeadHandler : IEventSystemHandler
{
    void OnPlayerDead();
}
