using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

// Unity 2D����Cinemachine��G���Ă݂��̂Ń���
//https://www.subarunari.com/entry/2018/05/19/2D%E7%92%B0%E5%A2%83%E3%81%A7Cinemachine%E3%82%92%E8%A7%A6%E3%81%A3%E3%81%A6%E3%81%BF%E3%81%9F%E3%81%AE%E3%81%A7%E3%83%A1%E3%83%A2

public class ChangeCamera : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.Button btnSwitchCamera;

    [SerializeField]
    private CinemachineVirtualCamera worldCamera;

    [SerializeField]  // Debug �p�B�m�F��ꂽ�� SerializeField ���O��
    private CinemachineVirtualCamera currentCharaCamera;

    private bool isWorldCamera;

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
    /// �L�����̃J�������Z�b�g
    /// </summary>
    /// <param name="charaCamera"></param>
    public void SetCurrentCharaCamera(CinemachineVirtualCamera charaCamera) {
        currentCharaCamera = charaCamera;
        isWorldCamera = true;
    }

    /// <summary>
    /// �L�����̃J�������폜
    /// </summary>
    public void RemoveCurrentCharaCamera() {
        currentCharaCamera = null;
        isWorldCamera = false;
    }
}
