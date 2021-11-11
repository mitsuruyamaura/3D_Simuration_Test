using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//Unity��Scene�r���[�̂悤�Ȏ��_�ړ����ł���J���������(MouseUpdate �̕�)
//https://esprog.hatenablog.com/entry/2016/03/20/033322

//�yUnity�z�X���C�v�ړ������������I�u�W�F�N�g���ړ�������(MoveCamera �̕�)
//https://zenn.dev/daichi_gamedev/articles/74b0a80dd836ac

/// <summary>
/// �t���[�̃J�����̐���p
/// </summary>
public class SwipeMoveCamera : MonoBehaviour
{
    [SerializeField, Range(0.1f, 10.0f)]
    private float wheelSpeed = 1.0f;

    [SerializeField, Range(0.1f, 10.0f)]
    private float moveSpeed = 0.3f;

    [SerializeField, Range(0.1f, 10.0f)]
    private float rotateSpeed = 0.3f;

    private Vector3 prevMousePos;


    void Start() {
#if UNITY_EDITOR
        moveSpeed = 1f;
#else   // �X�}�z�p
        moveSpeed = 0.3f;
#endif
    }


    void Update()
    {
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
        MoveCamera();
        return;

        //MouseUpdate();
        //return;
    }

    /// <summary>
    /// �X���C�v�����������J�������㉺���E�Ɉړ�������
    /// </summary>
    private void MoveCamera() {

        // �ŏ��Ƀ^�b�v������
        if (Input.GetMouseButtonDown(0)) {

            // �}�E�X�̈ʒu�����L�^
            prevMousePos = Input.mousePosition;
        }

        // �X���C�v��
        if (Input.GetMouseButton(0)) {

            // �ŐV�̃}�E�X�̈ʒu�����擾
            Vector3 currentPos = Input.mousePosition;

            // �O�̃}�E�X�̈ʒu���ƍŐV�̃}�E�X�̈ʒu���̍����l���v�Z
            Vector3 diffDistance = currentPos - prevMousePos;

            // �ړ������̒���
            diffDistance = diffDistance * moveSpeed * Time.deltaTime;

            // �J�����̈ړ�(�X���C�v�̋t�Ɉړ�������̂Ō��Z����)
            transform.position -= new Vector3(diffDistance.x, diffDistance.y, 0);

            // �}�E�X�̈ʒu�����X�V
            prevMousePos = Input.mousePosition;
        }
    }


////////////////////***  ���g�p(����͊m�F�ςŖ��Ȃ�)�@***////////////////////////


    /// <summary>
    /// �}�E�X����ɂ��X�V����
    /// </summary>
    private void MouseUpdate() {

        // �z�C�[�����͂̊��m
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");

        // �z�C�[�����͂�����ꍇ
        if (scrollWheel != 0) {

            // �J�����̑O��ړ�(��ʓI�ɂ͊g��E�k��)
            MouseWheel(scrollWheel);
        }

        // �}�E�X�N���b�N�̓��͊��m
        if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)) {

            // �}�E�X�̈ʒu�����L�^
            prevMousePos = Input.mousePosition;
        }

        // �h���b�O(�X���C�v)�̏���
        MouseDrag(Input.mousePosition);
    }

    /// <summary>
    /// �J�����̑O��ړ�(��ʓI�ɂ͊g��E�k��)
    /// </summary>
    /// <param name="delta"></param>
    private void MouseWheel(float delta) {
        // �J�����̑O��ړ�(3D)
        transform.position += transform.forward * delta * wheelSpeed;

        // 2D�̏ꍇ�� Size �𓮂������ƂŊg��E�k��������
        Camera.main.orthographicSize += delta * wheelSpeed;
        return;
    }

    /// <summary>
    /// �h���b�O(�X���C�v)�̏���
    /// </summary>
    /// <param name="mousePos"></param>
    private void MouseDrag(Vector3 mousePos) {

        // �O�̃}�E�X�̈ʒu���ƍŐV�̃}�E�X�̈ʒu���̍����l���v�Z
        Vector3 diff = mousePos - prevMousePos;

        // diff �̃x�N�g���̒���(�������͋��߂Ȃ���)���擾���A�L������(1e-5����)�𖞂����Ă��邩�m�F
        if (diff.sqrMagnitude < Vector3.kEpsilon) {
            // �������Ă��Ȃ��ꍇ�ɂ͏������Ȃ�
            return;
        }
   
        if (Input.GetMouseButton(2)) {
            // �J�����̏㉺�ړ�(�h���b�O�̋t�ɓ���)
            transform.Translate(-diff * Time.deltaTime * moveSpeed);
        } else if (Input.GetMouseButton(1)) {
            // �J�����̉�]
            CameraRotate(new Vector2(-diff.y, diff.x) * rotateSpeed);
        }

        // �}�E�X�̈ʒu�����X�V
        prevMousePos = mousePos;
    }

    /// <summary>
    /// �J�����̉�]
    /// </summary>
    /// <param name="angle"></param>
    public void CameraRotate(Vector2 angle) {
        // �J�����̉�](������)
        transform.RotateAround(transform.position, transform.right, angle.x);
        transform.RotateAround(transform.position, Vector3.up, angle.y);
    }
}
