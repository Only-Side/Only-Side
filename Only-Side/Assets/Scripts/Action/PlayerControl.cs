using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    public float playerSpeed;     // プレイヤーの動くスピード

    private Vector2 playerVelocity;     // プレイヤーに加えられる力
    private Vector2 lastMove;
    private Vector2 autoMoveTarget;
    private Rigidbody2D rb = null;     // Rigidbody2Dのコンポーネントを取得するために必要
    private Animator anim = null;
    private bool isAutoMove = false;

    private void Start()
    {
        // Rigidbody2Dのコンポーネントを取得
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        // プレイヤ―に力を加える（自動移動中かどうかで異なる速度を使用）
        if (isAutoMove)
        {
            // 自動移動の場合、ターゲット位置に向かって移動
            rb.velocity = (autoMoveTarget - rb.position).normalized * playerSpeed;
            // ターゲット位置に近づいたら自動移動を終了
            if (Vector2.Distance(rb.position, autoMoveTarget) < 0.1f)
            {
                rb.velocity = Vector2.zero;
                isAutoMove = false;
            }
        }
        else
        {
            // 通常のプレイヤー操作による移動
            rb.velocity = playerVelocity * playerSpeed;
        }
    }

    private void Update()
    {

    }

    // 移動に必要なキー(InputSystem)を押したとき実行
    private void OnMove(InputValue value)
    {
        // 自動移動中はプレイヤーの操作を無効にする
        if (isAutoMove) return;

        // 力の向きと大きさを取得する
        playerVelocity = value.Get<Vector2>();
        // 力のベクトルの大きさを半径1.0の円に制限
        playerVelocity = Vector2.ClampMagnitude(playerVelocity, 1);
        // アニメーションのパラメータを更新
        anim.SetFloat("Horizontal", playerVelocity.x);
        anim.SetFloat("Vertical", playerVelocity.y);
        anim.SetFloat("Speed", playerVelocity.sqrMagnitude);

        // 移動が発生した場合、最後の移動方向を記録
        if (playerVelocity != Vector2.zero)
        {
            lastMove = playerVelocity;
            anim.SetFloat("LastMoveX", lastMove.x);
            anim.SetFloat("LastMoveY", lastMove.y);
        }
    }

    // 移動キーを離したとき実行
    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        // 自動移動中はプレイヤーの操作を無効にする
        if (isAutoMove) return;

        // プレイヤーの速度をゼロにして停止させる
        playerVelocity = Vector2.zero;

        // 停止時に最後の方向を維持する
        anim.SetFloat("Horizontal", 0);
        anim.SetFloat("Vertical", 0);
        anim.SetFloat("Speed", 0);
        anim.SetFloat("LastMoveX", lastMove.x);
        anim.SetFloat("LastMoveY", lastMove.y);
    }

    // プレイヤーを部屋の中心に自動的に移動させるメソッド
    public void AutoMoveTo(Vector2 targetPosition)
    {
        isAutoMove = true;  // 自動移動を開始
        autoMoveTarget = targetPosition;  // ターゲット位置を設定

        // アニメーションのパラメータを更新
        Vector2 moveDirection = (targetPosition - rb.position).normalized;
        anim.SetFloat("Horizontal", moveDirection.x);
        anim.SetFloat("Vertical", moveDirection.y);
        anim.SetFloat("Speed", 1f);  // 自動移動中は常に移動アニメーションを再生
    }
}