using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cinemachine;

public class CharaButton : MonoBehaviour
{
    [SerializeField]
    private Button btnChara;

    [SerializeField]
    private Image imgCharaIcon;

    [SerializeField]
    private Text txtCost;

    private CameraManager cameraManager;
    private CharaController charaController;


    /// <summary>
    /// �L�����{�^���̏����ݒ�
    /// </summary>
    /// <param name="cameraManager"></param>
    public void SetUpCharaDetail(CameraManager cameraManager, CharaController charaController) {

        SwitchActiveteCharaButton(false);

        // TODO �摜�A�R�X�g�Ȃǂ̐ݒ�

        btnChara?.onClick.AddListener(OnClickCharaButton);

        this.cameraManager = cameraManager;
        this.charaController = charaController;

        // �R�X�g���x������ꍇ�ɂ̓{�^����������悤�ɂ���
        SwitchActiveteCharaButton(CheckCost(0));
    }

    /// <summary>
    /// �L�����{�^�����������ۂ̏���
    /// </summary>
    private void OnClickCharaButton() {

        // ���̃L�����̃J���������C���ɐ؂�ւ���
        cameraManager.SetCurrentCharaCamera(charaController.MyCamera);

        // ���̃L�������A�N�e�B�u��Ԃɂ���
        charaController.gameManager.SwitchActivateChara(charaController);

        Debug.Log("�L�����I��");
    }

    /// <summary>
    /// �R�X�g�x�����L���̊m�F�ƃ{�^���̊������؂�ւ�
    /// </summary>
    /// <param name="cost"></param>
    public bool CheckCost(int cost) {

        // TODO �R�X�g�̒l���x������Ȃ�(��ŏC������)
        return cost <= 100 ? true : false;
    }

    /// <summary>
    /// �{�^���̊������؂�ւ�
    /// </summary>
    /// <param name="isSwitch"></param>
    public void SwitchActiveteCharaButton(bool isSwitch) {
        btnChara.interactable = isSwitch;
    }
}
