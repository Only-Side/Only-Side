using System.Collections;
using UnityEngine;

public class RoomTransition : MonoBehaviour
{
    public Transform targetRoomTransform; // 移動先の部屋のTransform
    public string bgmName; // 遷移時のBGM名

    private bool playerInTransition = false; // プレイヤーが遷移中かどうか

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // プレイヤーがトリガーゾーンに入ったとき
        if (!playerInTransition && collision.CompareTag("Player"))
        {
            RoomTransition otherTransition = targetRoomTransform.GetComponentInParent<RoomTransition>();
            if (otherTransition != null)
            {
                otherTransition.playerInTransition = true; // 他のトリガーの状態を設定
            }

            // 部屋の遷移を開始するコルーチンを実行
            StartCoroutine(TransitionRoom(collision.transform, otherTransition));
        }
    }

    private IEnumerator TransitionRoom(Transform player, RoomTransition otherTransition)
    {
        playerInTransition = true; // プレイヤーが遷移中であることを設定

        // フェードアウト
        yield return new WaitForSeconds(0.1f); // フェード開始前に少し待機
        FadeManager.Instance.FadeOut(() =>
        {
            // プレイヤーをターゲットルームにワープ
            player.position = targetRoomTransform.position;

            if (bgmName != null)
            {
                // すべてのBGMを止める
                AudioManager.instance.StopBGM();
                // BGM再生
                AudioManager.instance.Play(bgmName);
            }

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
