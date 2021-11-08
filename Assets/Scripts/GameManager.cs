using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

/// <summary>
/// �Q�[���V�[���̐���E�Ǘ��p
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField]
    private CharaController charaPrefab;

    [SerializeField]
    private CharaButton charaButtonPrefab;

    [SerializeField]
    private Transform charaGenerateTran;        // ��U GameManager ���A�T�C���B��ŋ��_�ɕύX����

    [SerializeField]
    private Transform charaButtonGenerateTran;  // Content ���A�T�C��

    [SerializeField]
    private CameraManager cameraManager;

    [SerializeField]
    private Grid grid; // �ړ��\�ȃ^�C���}�b�v�̕��� Grid ���A�T�C�� 

    [SerializeField]
    private Tilemap tilemap;�@// �ړ��\�ȃ^�C���}�b�v�� Tilemap ���A�T�C��

    [SerializeField]
    private List<CharaController> charasList = new List<CharaController>();  // ���������L�����̃��X�g

    [SerializeField]
    private List<CharaButton> charaButtonsList = new List<CharaButton>();    // ���������L�����{�^���̃��X�g

    [SerializeField]
    private int generateCharaCount;  // �L�����̐������B��ŕʂ̏�񂩂�Q�Ƃ���̂Ńf�o�b�O�p


    void Start()
    {
        // Debug�p
        GenerateChara();
    }

    /// <summary>
    /// �L��������
    /// </summary>
    public void GenerateChara() {

        // TODO ���[�v�񐔂͌�ŕʂ̕ϐ��ɕύX
        for (int i = 0; i < generateCharaCount; i++) {

            // �L��������
            CharaController chara = Instantiate(charaPrefab, charaGenerateTran.position, charaPrefab.transform.rotation);

            // �L�����̏����ݒ�
            chara.SetUpCharaController(this);

            // �{�^������
            GenerateCharaButton(chara);

            // �^�C���}�b�v�̈ړ��p�̏��Ƃ��� Grid �� Tilemap ��ݒ�
            chara.TilemapMove.SetUpTilemapMove(grid, tilemap);

            // �e���X�g�ɒǉ�
            charasList.Add(chara);
            cameraManager.AddCharaCamerasList(chara.MyCamera);
        }

        /// <summary>
        /// �L�����{�^������
        /// </summary>
        /// <param name="chara"></param>
        void GenerateCharaButton(CharaController chara) {

            // �L�����{�^������(GridLayoutGroup ���g���Ď����I�ɕ��ׂ�)
            CharaButton charaButton = Instantiate(charaButtonPrefab, charaButtonGenerateTran);
            
            // �L�����{�^���̏����ݒ�
            charaButton.SetUpCharaButton(cameraManager, chara);

            // �L�����{�^���̃��X�g�ɒǉ�
            charaButtonsList.Add(charaButton);
        }
    }

    /// <summary>
    /// �I�������L�������A�N�e�B�u�ɐ؂�ւ��āA���̃L�������A�N�e�B�u�ɐ؂�ւ���
    /// </summary>
    /// <param name="activeChara">�A�N�e�B�u��Ԃɂ���L����</param>
    public void SwitchActivateChara(CharaController activeChara) {

        // �A�N�e�B�u�L�����̂݃A�N�e�B�u��Ԃɂ��A���̃L�����͔�A�N�e�B�u��Ԃɂ���
        charasList.Select(x => x == activeChara ? x.TilemapMove.isActive = true : x.TilemapMove.isActive = false).ToList();

        //foreach (CharaController chara in charasList) {
        //    if (chara.Equals(activeChara)) {
        //        chara.mapPoint.isActive = true;
        //    } else {
        //        chara.mapPoint.isActive = false;
        //    }
        //}
    }
}
