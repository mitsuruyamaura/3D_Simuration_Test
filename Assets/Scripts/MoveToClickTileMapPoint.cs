using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.AI;

//�yUnity��2DRPG�zNPC��NavMeshAgent�œ������Ă݂� (�L�̖`��)
//https://a1026302.hatenablog.com/entry/2020/11/21/014727
public class MoveToClickTileMapPoint : MonoBehaviour {

    [SerializeField]
    private Grid grid;

    [SerializeField]
    private Tilemap tilemap;

    private NavMeshAgent agent;

    void Start() {

        if (TryGetComponent(out agent)) {

            // �C���X�^���X�Ή��B���O�ɓ���Ă����ƁABake ���Ă���n�_��F���ł����ɃG���[�ɂȂ邽��
            agent.enabled = true;

            // 2D �Ȃ̂ŁA���ꂪ�Ȃ��ƁA�ςȈʒu�ɏ���Ɉړ�����
            agent.updateRotation = false;
            agent.updateUpAxis = false;

            // �����ړI�n�ݒ�(���ꂪ�Ȃ��Ə����ʒu����Y����)
            agent.destination = transform.position;
        }
    }

    void Update() {

        // NavMeshAgent �����p�ł����ԂŃ^�b�v(�}�E�X�N���b�N)������
        if (agent && Input.GetMouseButtonDown(0)) {

            // �^�b�v�̈ʒu���擾���ă��[���h���W�ɕϊ����A���������Ƀ^�C���̃Z�����W�ɕϊ�
            Vector3Int gridPos = grid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            //Debug.Log(gridPos);

            // �^�b�v�����^�C�����ړ��s�̃^�C���łȂ����
            if (tilemap.GetColliderType(gridPos) != Tile.ColliderType.None) {

                // �^�b�v�����^�C���}�b�v��ړI�n�Ƃ��Ĉړ�
                SetPathAndMove(gridPos);
            }
        }

        //// �Ώۂ������ꍇ�́A���ł��
        //float now = Vector2.Distance(transform.position, nextPos);

        //if (nextPos.magnitude < now) {
        //    // �Čv�Z
        //}
    }

    /// <summary>
    /// �^�b�v(�}�E�X�N���b�N)�����^�C���}�b�v��ړI�n�Ƃ��Ĉړ�
    /// </summary>
    /// <param name="gridPos"></param>
    private void SetPathAndMove(Vector3Int gridPos) {

        // �ړI�n�_�̐ݒ�B�������邱�ƂŁA�^�C���̒����Ɉړ�������
        Vector2 nextPos = new Vector2(gridPos.x + 0.5f, gridPos.y + 0.5f);

        //Debug.Log(nextPos);

        // �ړI�n�̍X�V
        agent.destination = nextPos;

        //Debug.Log("�ړ�");
    }
}