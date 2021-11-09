using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// �L�����{�^���̐���p
/// </summary>
public class CharaButton : MonoBehaviour
{
    [SerializeField]
    private Button btnChara;�@�@// �{�^���� Interactable �ɂ��ẮA�v���t�@�u�̏����ݒ�Ő؂��Ă���

    [SerializeField]
    private Image imgCharaIcon;

    [SerializeField]
    private Text txtCost;

    [SerializeField]
    private Image imgActiveFrame;

    private CameraManager cameraManager;
    private CharaController charaController;


    /// <summary>
    /// �L�����{�^���̏����ݒ�
    /// </summary>
    /// <param name="cameraManager"></param>
    /// <param name="charaController"></param>
    public void SetUpCharaButton(CameraManager cameraManager, CharaController charaController) {

        // TODO �摜�A�R�X�g�Ȃǂ̐ݒ�

        imgActiveFrame.enabled = false;

        // �{�^���̃A�T�C��������ꍇ�̂݁A�{�^���Ƀ��\�b�h��o�^
        btnChara?.onClick.AddListener(OnClickCharaButton);

        this.cameraManager = cameraManager;
        this.charaController = charaController;

        // TODO �R�X�g���x������ꍇ�ɂ̓{�^����������悤�ɂ���(��ŁA�R�X�g�̒l�̓L�����̃f�[�^����Q�Ƃ���悤�ɕς���)
        SwitchActiveteCharaButton(CheckCost(0));
    }

    /// <summary>
    /// �L�����{�^�����������ۂ̏���
    /// </summary>
    private void OnClickCharaButton() {

        // ���̃L�����̃J���������C���ɐ؂�ւ��āA���̃L�����Ƀt�H�[�J�X����
        cameraManager.SetCurrentCharaCamera(charaController.MyCamera);

        // ���̃L�������A�N�e�B�u��Ԃɂ��ă^�C���}�b�v�̈ړ���������B���̃L�����͔�A�N�e�B�u��Ԃɂ���
        charaController.GameManager.SwitchActivateChara(charaController);

        Debug.Log("�L�����I��");
    }

    /// <summary>
    /// �R�X�g�x�����L���̊m�F
    /// </summary>
    /// <param name="cost"></param>
    public bool CheckCost(int cost) {

        // TODO �R�X�g�̒l���x������Ȃ�(��� 100 �̕������R�X�g�x�����p�ϐ��ɏC������)
        return cost <= 100 ? true : false;
    }

    /// <summary>
    /// �{�^���̊������؂�ւ�
    /// </summary>
    /// <param name="isSwitch"></param>
    public void SwitchActiveteCharaButton(bool isSwitch) {
        btnChara.interactable = isSwitch;
    }

    /// <summary>
    /// �L�����̏��擾
    /// </summary>
    /// <returns></returns>
    public CharaController GetCharaController() {
        return charaController;
    }

    /// <summary>
    /// �A�N�e�B�u��Ԏ��̃t���[���̃I���I�t�؂�ւ�
    /// </summary>
    /// <param name="isSwitch"></param>
    public bool SwitchActivateFrame(bool isSwitch) {
        return imgActiveFrame.enabled = isSwitch;
    }
}
