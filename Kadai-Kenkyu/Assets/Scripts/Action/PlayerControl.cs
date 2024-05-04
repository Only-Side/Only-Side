using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    public float playerSpeed;     // プレイヤーの動くスピード

    private Vector2 playerVelocity;     // プレイヤーに加えられる力
    private Rigidbody2D rb = null;     // Rigidbody2Dのコンポーネントを取得するために必要

    private void Start()
    {
        // Rigidbody2Dのコンポーネントを取得
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // プレイヤ―に力を加える
        rb.velocity = playerVelocity * playerSpeed;
    }

    // 移動に必要なキー(InputSystem)を押したとき実行
    private void OnMove(InputValue value)
    {
        // 力の向きと大きさを取得する
        playerVelocity = value.Get<Vector2>();
    }
}
