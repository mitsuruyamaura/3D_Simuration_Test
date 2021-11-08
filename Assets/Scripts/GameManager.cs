using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private CharaController charaPrefab;

    [SerializeField]
    private CharaButton charaButtonPrefab;

    [SerializeField]
    private Transform charaGenerateTran;

    [SerializeField]
    private Transform charaButtonTran;

    [SerializeField]
    private CameraManager cameraManager;

    [SerializeField]
    private Grid grid;

    [SerializeField]
    private Tilemap tilemap;

    [SerializeField]
    private List<CharaController> charasList = new List<CharaController>();

    [SerializeField]
    private List<CharaButton> charaButtonsList = new List<CharaButton>();


    void Start()
    {
        // Debug�p
        GenerateChara();
    }

    /// <summary>
    /// 
    /// </summary>
    public void GenerateChara() {

        // TODO ���[�v�񐔂͌�ŕϐ��ɕύX
        for (int i = 0; i < 5; i++) {

            CharaController chara = Instantiate(charaPrefab, charaGenerateTran.position, charaPrefab.transform.rotation);

            // �����ݒ�
            chara.SetUpCharaController(this);

            // �{�^������
            GenerateCharaButton(chara);

            // Grid �� Tilemap ��ݒ�
            chara.mapPoint.SetUpMapPoint(grid, tilemap);

            // �e���X�g�ɒǉ�
            charasList.Add(chara);

            cameraManager.AddCharaCamerasList(chara.MyCamera);
        }

        /// <summary>
        /// �L�����{�^������
        /// </summary>
        void GenerateCharaButton(CharaController chara) {

            CharaButton charaButton = Instantiate(charaButtonPrefab, charaButtonTran);

            charaButton.SetUpCharaDetail(cameraManager, chara);

            charaButtonsList.Add(charaButton);
        }
    } 

    /// <summary>
    /// �I�������L�������A�N�e�B�u�ɐ؂�ւ��āA���̃L�������A�N�e�B�u�ɐ؂�ւ���
    /// </summary>
    public void SwitchActivateChara(CharaController activeChara) {

        charasList.Select(x => x == activeChara ? x.mapPoint.isActive = true : x.mapPoint.isActive = false).ToList();

        //foreach (CharaController chara in charasList) {
        //    if (chara.Equals(activeChara)) {
        //        chara.mapPoint.isActive = true;
        //    } else {
        //        chara.mapPoint.isActive = false;
        //    }
        //}
    }
}
