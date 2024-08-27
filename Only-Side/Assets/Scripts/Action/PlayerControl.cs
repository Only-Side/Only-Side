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
    private Rigidbody2D rb = null;     // Rigidbody2Dのコンポーネントを取得するために必要
    private Animator anim = null;

    private void Start()
    {
        // Rigidbody2Dのコンポーネントを取得
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        // プレイヤ―に力を加える
        rb.velocity = playerVelocity * playerSpeed;
    }

    private void Update()
    {

    }

    // 移動に必要なキー(InputSystem)を押したとき実行
    private void OnMove(InputValue value)
    {
        // 力の向きと大きさを取得する
        playerVelocity = value.Get<Vector2>();
        // 力のベクトルの大きさを半径1.0の円に制限
        playerVelocity = Vector2.ClampMagnitude(playerVelocity, 1);
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
        // プレイヤーの速度をゼロにして停止させる
        playerVelocity = Vector2.zero;

        // 停止時に最後の方向を維持する
        anim.SetFloat("Horizontal", 0);
        anim.SetFloat("Vertical", 0);
        anim.SetFloat("Speed", 0);
        anim.SetFloat("LastMoveX", lastMove.x);
        anim.SetFloat("LastMoveY", lastMove.y);
    }
}