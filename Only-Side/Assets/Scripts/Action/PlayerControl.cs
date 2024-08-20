using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    public float playerSpeed;     // プレイヤーの動くスピード

    private Vector2 playerVelocity;     // プレイヤーに加えられる力
    private Rigidbody2D rb = null;     // Rigidbody2Dのコンポーネントを取得するために必要
    private enum Direction
    {
        Front = 0,
        Back = 1,
        Left = 2,
        Right = 3
    }
    private Direction direction = 0; // Front

    private enum MoveType
    {
        Stop = 0,
        Walk = 1
    }
    private MoveType moveType = 0; // Stop

    private void Start()
    {
        // Rigidbody2Dのコンポーネントを取得
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        moveType = 0;
        // プレイヤ―に力を加える
        rb.velocity = playerVelocity * playerSpeed;
    }

    private void Update()
    {
        //print(moveType);
    }

    // 移動に必要なキー(InputSystem)を押したとき実行
    private void OnMove(InputValue value)
    {
        print("ugokuyo");
        // 力の向きと大きさを取得する
        playerVelocity = value.Get<Vector2>();
        // 力のベクトルの大きさを半径1.0の円に制限
        playerVelocity = Vector2.ClampMagnitude(playerVelocity, 1);
        moveType = MoveType.Walk;
    }
}
