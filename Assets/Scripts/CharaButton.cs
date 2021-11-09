using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// キャラボタンの制御用
/// </summary>
public class CharaButton : MonoBehaviour
{
    [SerializeField]
    private Button btnChara;　　// ボタンの Interactable については、プレファブの初期設定で切ってある

    [SerializeField]
    private Image imgCharaIcon;

    [SerializeField]
    private Text txtCost;

    [SerializeField]
    private Image imgActiveFrame;

    private CameraManager cameraManager;
    private CharaController charaController;


    /// <summary>
    /// キャラボタンの初期設定
    /// </summary>
    /// <param name="cameraManager"></param>
    /// <param name="charaController"></param>
    public void SetUpCharaButton(CameraManager cameraManager, CharaController charaController) {

        // TODO 画像、コストなどの設定

        imgActiveFrame.enabled = false;

        // ボタンのアサインがある場合のみ、ボタンにメソッドを登録
        btnChara?.onClick.AddListener(OnClickCharaButton);

        this.cameraManager = cameraManager;
        this.charaController = charaController;

        // TODO コストが支払える場合にはボタンを押せるようにする(後で、コストの値はキャラのデータから参照するように変える)
        SwitchActiveteCharaButton(CheckCost(0));
    }

    /// <summary>
    /// キャラボタンを押した際の処理
    /// </summary>
    private void OnClickCharaButton() {

        // このキャラのカメラをメインに切り替えて、このキャラにフォーカスする
        cameraManager.SetCurrentCharaCamera(charaController.MyCamera);

        // このキャラをアクティブ状態にしてタイルマップの移動を許可する。他のキャラは非アクティブ状態にする
        charaController.GameManager.SwitchActivateChara(charaController);

        Debug.Log("キャラ選択");
    }

    /// <summary>
    /// コスト支払い有無の確認
    /// </summary>
    /// <param name="cost"></param>
    public bool CheckCost(int cost) {

        // TODO コストの値が支払えるなら(後で 100 の部分をコスト支払い用変数に修正する)
        return cost <= 100 ? true : false;
    }

    /// <summary>
    /// ボタンの活性化切り替え
    /// </summary>
    /// <param name="isSwitch"></param>
    public void SwitchActiveteCharaButton(bool isSwitch) {
        btnChara.interactable = isSwitch;
    }

    /// <summary>
    /// キャラの情報取得
    /// </summary>
    /// <returns></returns>
    public CharaController GetCharaController() {
        return charaController;
    }

    /// <summary>
    /// アクティブ状態時のフレームのオンオフ切り替え
    /// </summary>
    /// <param name="isSwitch"></param>
    public bool SwitchActivateFrame(bool isSwitch) {
        return imgActiveFrame.enabled = isSwitch;
    }
}
