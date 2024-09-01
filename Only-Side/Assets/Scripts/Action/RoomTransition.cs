using System.Collections;
using UnityEngine;

public class RoomTransition : MonoBehaviour
{
    public Transform targetRoomTransform;
    private bool playerInTransition = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!playerInTransition && collision.CompareTag("Player"))
        {
            RoomTransition otherTransition = targetRoomTransform.GetComponentInParent<RoomTransition>();
            if (otherTransition != null)
            {
                otherTransition.playerInTransition = true;
            }

            StartCoroutine(TransitionRoom(collision.transform, otherTransition));
        }
    }

    private IEnumerator TransitionRoom(Transform player, RoomTransition otherTransition)
    {
        playerInTransition = true;

        // フェードアウト
        yield return new WaitForSeconds(0.1f); // フェード開始前の短い待機時間
        FadeManager.Instance.FadeOut(() =>
        {
            // プレイヤーをターゲットルームにワープ
            player.position = targetRoomTransform.position;

            // 少しの間待機
            StartCoroutine(WaitAndFadeIn(otherTransition));
        });
    }

    private IEnumerator WaitAndFadeIn(RoomTransition otherTransition)
    {
        // プレイヤーがトリガーゾーンから離れるのを確認する
        yield return new WaitForSeconds(0.1f);

        // フェードイン
        FadeManager.Instance.FadeIn(() =>
        {
            // トリガーの再有効化
            playerInTransition = false;
            if (otherTransition != null)
            {
                otherTransition.playerInTransition = false;
            }
        });
    }
}
