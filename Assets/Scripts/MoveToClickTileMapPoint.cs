using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.AI;
using UnityEngine.EventSystems;

//�yUnity��2DRPG�zNPC��NavMeshAgent�œ������Ă݂� (�L�̖`��)
//https://a1026302.hatenablog.com/entry/2020/11/21/014727

//�yUnity�z�{�^�����������Ƃ��ɉ�ʃN���b�N�͖�������
//https://nn-hokuson.hatenablog.com/entry/2017/07/12/220302

/// <summary>
/// �^�C���}�b�v���^�b�v���ăi�r���b�V���ňړ������邽�߂̃N���X
/// </summary>
public class MoveToClickTilemapPoint : MonoBehaviour {

    [SerializeField]
    private Grid grid;

    [SerializeField]
    private Tilemap tilemap;

    private NavMeshAgent agent;

    public bool isActive;

    [SerializeField]
    private DrawPath drawPathPrefab;

    private CharaController charaController;
    //private IEnumerator coroutine;


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

#if UNITY_EDITOR
        // UI ���^�b�v���ꂽ�Ƃ��͏������Ȃ�(UI �̃{�^�����������炻����݂̂𔽉�������)
        if (EventSystem.current.IsPointerOverGameObject()) {
            return;
        }
#else   // �X�}�z�p
        if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) {
            return;
        }
#endif

        if (!isActive) {
            return;
        }

        // NavMeshAgent �����p�ł����ԂŃ^�C���}�b�v���^�b�v(�}�E�X�N���b�N)������
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
        Vector3 nextPos = new Vector3(gridPos.x + 0.5f, gridPos.y + 0.5f, 0);

        // ��r������� Z ���𗼕��Ƃ� 0 �Ɏw�肵�Ă����Ȃ��ƁA2D�̌o�H�v�Z���ł��Ȃ�
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        //Debug.Log(nextPos);

        // �ړI�n�̍X�V
        //agent.SetDestination(nextPos);  // �ǂ���ł����Ȃ�
        agent.destination = nextPos;

        //Debug.Log(agent.hasPath);
        //Debug.Log(agent.isOnNavMesh);

        // �o�H�̐���
        StartCoroutine(GenerateCornerLineFromPath(transform.position, nextPos));

        //Debug.Log("�ړ�");

        /// <summary>
        /// �o�H�̐���
        /// </summary>
        /// <param name="startPos"></param>
        /// <param name="endPos"></param>
        IEnumerator GenerateCornerLineFromPath(Vector3 startPos, Vector3 endPos) {

            //Debug.Log(startPos);
            //Debug.Log(endPos);
            //Debug.Log(agent.hasPath);

            // �p�X���擾����܂őҋ@
            yield return new WaitUntil(() => agent.hasPath);
            //Debug.Log(agent.hasPath);
            //Debug.Log(agent.isOnNavMesh);

            NavMeshPath path = new NavMeshPath();

            // ���J�ɏ����ꍇ
            //bool result = NavMesh.CalculatePath(startPos, endPos, NavMesh.AllAreas, path);
            //Debug.Log(result);

            // �o�H�̏����擾�ł��Ă��邩�m�F
            if (NavMesh.CalculatePath(startPos, endPos, NavMesh.AllAreas, path)) {   // if(result){ }  ���J�ɏ����ꍇ

                // �o�H��񂪂���ꍇ�A�o�H�\���p�̃Q�[���I�u�W�F�N�g�쐬
                DrawPath drawPath = Instantiate(drawPathPrefab);

                // �o�H�̍��W���擾
                Vector3[] corners = path.corners;

                //if (coroutine != null) {
                //    StopCoroutine(coroutine);
                //    coroutine = null;
                //}

                //coroutine = charaController.SetAnime(corners);

                //StartCoroutine(coroutine);

                //path.GetCornersNonAlloc(corners);
                Debug.Log("GetCornersNonAlloc : " + path.GetCornersNonAlloc(corners));  // �ŏ��̒n�_������Đ�����

                // �o�H�̍쐬
                StartCoroutine(drawPath.DrawCornersLine(corners));

                Debug.Log(" �o�H�̍쐬 �J�n");
            }
        }
    }

    /// <summary>
    /// �����ݒ�
    /// </summary>
    /// <param name="grid"></param>
    /// <param name="tilemap"></param>
    public void SetUpTilemapMove(Grid grid, Tilemap tilemap, CharaController charaController) {
        this.grid = grid;
        this.tilemap = tilemap;
        this.charaController = charaController;

        isActive = false;
    }
}
