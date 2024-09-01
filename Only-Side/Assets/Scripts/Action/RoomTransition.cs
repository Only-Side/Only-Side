using System.Collections;
using UnityEngine;

public class RoomTransition : MonoBehaviour
{
    public Transform targetRoomTransform; // �ړ���̕�����Transform
    private bool playerInTransition = false; // �v���C���[���J�ڒ����ǂ���

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �v���C���[���g���K�[�]�[���ɓ������Ƃ�
        if (!playerInTransition && collision.CompareTag("Player"))
        {
            RoomTransition otherTransition = targetRoomTransform.GetComponentInParent<RoomTransition>();
            if (otherTransition != null)
            {
                otherTransition.playerInTransition = true; // ���̃g���K�[�̏�Ԃ�ݒ�
            }

            // �����̑J�ڂ��J�n����R���[�`�������s
            StartCoroutine(TransitionRoom(collision.transform, otherTransition));
        }
    }

    private IEnumerator TransitionRoom(Transform player, RoomTransition otherTransition)
    {
        playerInTransition = true; // �v���C���[���J�ڒ��ł��邱�Ƃ�ݒ�

        // �t�F�[�h�A�E�g
        yield return new WaitForSeconds(0.1f); // �t�F�[�h�J�n�O�ɏ����ҋ@
        FadeManager.Instance.FadeOut(() =>
        {
            // �v���C���[���^�[�Q�b�g���[���Ƀ��[�v
            player.position = targetRoomTransform.position;

            // �����̊ԑҋ@
            StartCoroutine(WaitAndFadeIn(otherTransition));
        });
    }

    private IEnumerator WaitAndFadeIn(RoomTransition otherTransition)
    {
        // �v���C���[���g���K�[�]�[�����痣���̂��m�F����
        yield return new WaitForSeconds(0.1f);

        // �t�F�[�h�C��
        FadeManager.Instance.FadeIn(() =>
        {
            // �g���K�[�̍ėL����
            playerInTransition = false;
            if (otherTransition != null)
            {
                otherTransition.playerInTransition = false;
            }
        });
    }
}