using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    public static bool isPlayerControl = true;

    public GameObject collidedItem;  // 衝突したアイテムを保持
    public float playerSpeed;     // プレイヤーの動くスピード
    public bool isRigidMove = false;
    public bool isItemCollision = false;     // アイテムと衝突しているかどうかのフラグ

    private Vector2 playerVelocity;     // プレイヤーに加えられる力
    private Vector2 lastMove;
    private Vector2 autoMoveTarget;
    private Rigidbody2D rb = null;     // Rigidbody2Dのコンポーネントを取得するために必要
    private Animator anim = null;
    private bool isAutoMove = false;     // 自動移動中かどうかのフラグ

    private void Start()
    {
        // Rigidbody2Dのコンポーネントを取得
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        // プレイヤーに力を加える（自動移動中かどうかで異なる速度を使用）
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
        // isRigidMove が true の場合のみ FixedMove を呼び出す
        else if (isRigidMove)
        {
            RigidMove(lastMove);
        }
        else
        {
            anim.SetFloat("Horizontal", playerVelocity.x);
            anim.SetFloat("Vertical", playerVelocity.y);
            anim.SetFloat("Speed", playerVelocity.sqrMagnitude);
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
        if (isAutoMove || !isPlayerControl) return;

        // 力の向きと大きさを取得する
        playerVelocity = value.Get<Vector2>();
        // 長さ1の単位ベクトルに変換する
        playerVelocity = playerVelocity.normalized;
        // アニメーションのパラメータを更新
        anim.SetFloat("Horizontal", playerVelocity.x);
        anim.SetFloat("Vertical", playerVelocity.y);
        anim.SetFloat("Speed", playerVelocity.sqrMagnitude);

        if (isRigidMove) return;

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

        // 停止時に最後の方向を維持する
        anim.SetFloat("Horizontal", 0);
        anim.SetFloat("Vertical", 0);
        anim.SetFloat("Speed", 0);
        anim.SetFloat("LastMoveX", lastMove.x);
        anim.SetFloat("LastMoveY", lastMove.y);
        
        if(isRigidMove) return;
        // プレイヤーの速度をゼロにして停止させる
        playerVelocity = Vector2.zero;
    }

    private void OnSelect()
    {
        // 自動移動中は操作を無効にする
        if (isAutoMove) return;

        if (isItemCollision && collidedItem != null)
        {
            Item item = collidedItem.GetComponent<Item>();
            if(item != null)
            {
                item.PickupItem();
            }
        }
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

    private void RigidMove(Vector2 velocity)
    {
        rb.velocity = lastMove * playerSpeed;
        // アニメーションのパラメータを更新
        anim.SetFloat("Horizontal", lastMove.x);
        anim.SetFloat("Vertical", lastMove.y);
        anim.SetFloat("Speed", lastMove.sqrMagnitude);
        anim.SetFloat("LastMoveX", lastMove.x);
        anim.SetFloat("LastMoveY", lastMove.y);
        if (!isRigidMove)
        {
            anim.SetFloat("Speed", 0);
            anim.SetFloat("LastMoveX", lastMove.x);
            anim.SetFloat("LastMoveY", lastMove.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Item")
        {
            isItemCollision = true;
            collidedItem = collision.gameObject;  // 衝突したアイテムを保持
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Item")
        {
            isItemCollision = false;
            collidedItem = null;  // アイテムが衝突していない場合、参照をクリア
        }
    }
}
