﻿using System.Collections;
using UnityEngine;

public class RoomTransition : MonoBehaviour
{
    public Transform targetRoomTransform; // 移動先の部屋のTransform
    public string bgmName; // 遷移時に再生するBGM名
    public float moveSpeed = 2f; // プレイヤーが部屋の中心に移動する速度

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

            PlayerControl playerControl = collision.GetComponent<PlayerControl>();
            if (playerControl != null)
            {
                playerControl.isRigidMove = true;
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
            PlayerControl playerControl = player.GetComponent<PlayerControl>();
            if(playerControl != null)
            {
                Animator anim = playerControl.GetComponent<Animator>();
                playerControl.isRigidMove = false;
                if(anim != null)
                {
                    anim.SetFloat("Speed", 0);
                }

            }
            // プレイヤーをターゲットルームにワープ
            player.position = targetRoomTransform.position;

            if (bgmName != "")
            {
                // BGM再生
                AudioManager.instance.PlayBGM(bgmName);
            }

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