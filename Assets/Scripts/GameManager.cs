using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using System;

public enum TimeType {
    Night = 0,       // 0 - 5
    Morning = 6,     // 6 -11
    Afternoon = 12,  // 12 - 17
    Evening = 18,    // 18 - 23
}


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
    private UnityEngine.UI.Scrollbar scrollbar;

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

    public ReactiveProperty<bool> IsTimeStopped = new ReactiveProperty<bool>(false);  // ���Ԃ�����Ă��邩�ǂ����𔻒肷�邽�߂� ReactiveProperty

    [SerializeField]
    private UnityEngine.UI.Button btnTimeTransition;

    public ReactiveProperty<int> OurTime;
    private int maxTime = 24;

    public TimeType currentTimeType;


    void Start()
    {
        // Debug�p
        GenerateChara();

        // ���Ԃ̗���̐؂�ւ��{�^���̐ݒ�
        btnTimeTransition!.OnClickAsObservable()
            .TakeUntilDestroy(gameObject)
            .ThrottleFirst(TimeSpan.FromSeconds(0.5f))
            .Subscribe(_ => OnClickSwitchTimeTransition());

        Debug.Log($"���Ԃ̗��� : {(IsTimeStopped.Value ? "��~" : "�J�n")}");

        int targetTime = 3;

        // ���Ԍv��
        Observable.Timer(TimeSpan.FromSeconds(targetTime), TimeSpan.FromSeconds(targetTime))
            .Subscribe(_ => KeepTime());  // Subscribe ���ɕ����̏����������ꍇ�� Subscribe(_ => { }) �Ƃ���

        // ���Ԃ��Ď����� TimeType �������ŕύX
        OurTime.Subscribe(x => ChangeTimeType(x));

        OurTime.Value = 6;
    }

    /// <summary>
    /// ���Ԍo�ߌv��
    /// </summary>
    private void KeepTime() {

        OurTime.Value++;

        OurTime.Value = OurTime.Value % maxTime;
        
        //float timer = 0;
        //int targetTime = 3;

        //timer += Time.deltaTime;
        //if (timer >= targetTime) {
        //    timer = 0;
            
        //}
    }

    /// <summary>
    /// TimeType�̕ύX
    /// </summary>
    /// <param name="our"></param>
    private void ChangeTimeType(int our) {

        currentTimeType = our switch {
            0 => TimeType.Night,
            6 => TimeType.Morning,
            12 => TimeType.Afternoon,
            18 => TimeType.Evening,
            _ => currentTimeType
        };

        // ���邳�ƃ��C�g�͈̔͂̒���
        if (our >= 17 || our >= 5) {
            cameraManager.ChangeLightIntensity(our);
            cameraManager.ChangePointLightOuterRadius(our);
        }
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
            chara.TilemapMove.SetUpTilemapMove(grid, tilemap, chara, this);

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

        // ���ׂẴL�����{�^���쐬��A�X�N���[���o�[����Ԑ擪�Ƀt�H�[�J�X����
        scrollbar.value = 1.0f;
    }

    /// <summary>
    /// �I�������L�������A�N�e�B�u�ɐ؂�ւ��āA���̃L�������A�N�e�B�u�ɐ؂�ւ���
    /// </summary>
    /// <param name="activeChara">�A�N�e�B�u��Ԃɂ���L����</param>
    public void SwitchActivateChara(CharaController activeChara) {

        // �A�N�e�B�u�L�����̂݃A�N�e�B�u��Ԃɂ��A���̃L�����͔�A�N�e�B�u��Ԃɂ���
        charasList.Select(x => x == activeChara ? x.TilemapMove.isActive = true : x.TilemapMove.isActive = false).ToList();

        // �A�N�e�B�u�L�����̂݃t���[����\������
        charaButtonsList.Select(x => x.GetCharaController() == activeChara ? x.SwitchActivateFrame(true) : x.SwitchActivateFrame(false)).ToList();

        //foreach (CharaController chara in charasList) {
        //    if (chara.Equals(activeChara)) {
        //        chara.mapPoint.isActive = true;
        //    } else {
        //        chara.mapPoint.isActive = false;
        //    }
        //}

        //foreach (CharaButton charaButton in charaButtonsList) {
        //    if (charaButton.GetCharaController() == activeChara) {
        //        charaButton.SwitchActivateFrame(true);
        //    } else {
        //        charaButton.SwitchActivateFrame(false);
        //    }
        //}
    }

    /// <summary>
    /// �I�����Ă���L�����̉���
    /// </summary>
    public void InactivateChara() {
        charasList.Select(x => x.TilemapMove.isActive = false).ToList();

        charaButtonsList.Select(x => x.SwitchActivateFrame(false)).ToList();
    }

    /// <summary>
    /// ���Ԃ̗���̐؂�ւ�
    /// </summary>
    private void OnClickSwitchTimeTransition() {

        IsTimeStopped.Value = !IsTimeStopped.Value;

        Debug.Log($"���Ԃ̗��� : {(IsTimeStopped.Value ? "��~" : "�J�n")}");
    }
}
