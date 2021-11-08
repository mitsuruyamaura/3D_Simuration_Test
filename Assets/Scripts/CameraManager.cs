using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Linq;

// Unity 2D����Cinemachine��G���Ă݂��̂Ń���
//https://www.subarunari.com/entry/2018/05/19/2D%E7%92%B0%E5%A2%83%E3%81%A7Cinemachine%E3%82%92%E8%A7%A6%E3%81%A3%E3%81%A6%E3%81%BF%E3%81%9F%E3%81%AE%E3%81%A7%E3%83%A1%E3%83%A2

/// <summary>
/// �Q�[�����̃J�����̐���E�Ǘ��p
/// </summary>
public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.Button btnSwitchCamera;  // ���[���h�J�����؂�ւ��{�^���̐���p

    [SerializeField]
    private CinemachineVirtualCamera worldCamera;  // ���՗p�J�������A�T�C��

    //[SerializeField]  // Debug �p�B�m�F��ꂽ�� SerializeField ���O��
    private CinemachineVirtualCamera currentCharaCamera;  // �A�N�e�B�u��Ԃ̃L�����̃J���������̓s�x�A�T�C�����ĕύX

    private bool isWorldCamera;  // ���[���h�J�����ɂȂ��Ă��邩�̊m�F�Btrue �Ȃ烏�[���h�J�����g�p���

    [SerializeField]
    private List<CinemachineVirtualCamera> charaCamerasList = new List<CinemachineVirtualCamera>();  // �e�L�����̃J�����̃��X�g


    void Start()
    {
        // TODO ���[���h�J�����������ʒu�ɂ��邩�ݒ�


        // �{�^���̐ݒ�
        btnSwitchCamera.onClick.AddListener(SwitchWorldCamera);

        isWorldCamera = true;
    }

    /// <summary>
    /// ���[���h�J�����ƌ��ݑI�����Ă���L�����̃J�����̐؂�ւ�
    /// </summary>
    public void SwitchWorldCamera() {

        if (isWorldCamera) {
            worldCamera.Priority = 10;
            currentCharaCamera.Priority = 5;
        } else {
            worldCamera.Priority = 5;
            currentCharaCamera.Priority = 10;
        }

        isWorldCamera = !isWorldCamera;
    }

    /// <summary>
    /// �I�𒆂̃L�����̃J�������Z�b�g
    /// </summary>
    /// <param name="charaCamera"></param>
    public void SetCurrentCharaCamera(CinemachineVirtualCamera charaCamera) {
        currentCharaCamera = charaCamera;
        isWorldCamera = true;

        SetActiveCharaCamera(charaCamera);
    }

    /// <summary>
    /// �L�����̃J�������폜
    /// </summary>
    public void RemoveCurrentCharaCamera() {
        currentCharaCamera = null;
        isWorldCamera = false;
    }

    /// <summary>
    /// �I�𒆂̃L�����̃J���������C���̃J�����ɐ؂�ւ�
    /// </summary>
    /// <param name="charaCamera"></param>
    public void SetActiveCharaCamera(CinemachineVirtualCamera charaCamera) {

        charaCamerasList.Select(x => x == x.Equals(charaCamera) ? x.Priority = 10 : x.Priority = 5).ToList();

        //foreach (CinemachineVirtualCamera camera in charaCamerasList) {
        //    if (camera.Equals(charaCamera)) {
        //        camera.Priority = 10;
        //    } else {
        //        camera.Priority = 5;
        //    }
        //}
    }

    /// <summary>
    /// �L�����̃J���������X�g�ɒǉ�
    /// </summary>
    /// <param name="camera"></param>
    public void AddCharaCamerasList(CinemachineVirtualCamera camera) {
        charaCamerasList.Add(camera);
    }
}
