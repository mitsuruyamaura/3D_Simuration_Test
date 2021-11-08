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
    /// キャラボタンの初期設定
    /// </summary>
    /// <param name="cameraManager"></param>
    public void SetUpCharaDetail(CameraManager cameraManager, CharaController charaController) {

        SwitchActiveteCharaButton(false);

        // TODO 画像、コストなどの設定

        btnChara?.onClick.AddListener(OnClickCharaButton);

        this.cameraManager = cameraManager;
        this.charaController = charaController;

        // コストが支払える場合にはボタンを押せるようにする
        SwitchActiveteCharaButton(CheckCost(0));
    }

    /// <summary>
    /// キャラボタンを押した際の処理
    /// </summary>
    private void OnClickCharaButton() {

        // このキャラのカメラをメインに切り替える
        cameraManager.SetCurrentCharaCamera(charaController.MyCamera);

        // このキャラをアクティブ状態にする
        charaController.gameManager.SwitchActivateChara(charaController);

        Debug.Log("キャラ選択");
    }

    /// <summary>
    /// コスト支払い有無の確認とボタンの活性化切り替え
    /// </summary>
    /// <param name="cost"></param>
    public bool CheckCost(int cost) {

        // TODO コストの値が支払えるなら(後で修正する)
        return cost <= 100 ? true : false;
    }

    /// <summary>
    /// ボタンの活性化切り替え
    /// </summary>
    /// <param name="isSwitch"></param>
    public void SwitchActiveteCharaButton(bool isSwitch) {
        btnChara.interactable = isSwitch;
    }
}
