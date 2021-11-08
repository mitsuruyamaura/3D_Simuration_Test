using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Linq;

// Unity 2D環境でCinemachineを触ってみたのでメモ
//https://www.subarunari.com/entry/2018/05/19/2D%E7%92%B0%E5%A2%83%E3%81%A7Cinemachine%E3%82%92%E8%A7%A6%E3%81%A3%E3%81%A6%E3%81%BF%E3%81%9F%E3%81%AE%E3%81%A7%E3%83%A1%E3%83%A2

/// <summary>
/// ゲーム内のカメラの制御・管理用
/// </summary>
public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.Button btnSwitchCamera;  // ワールドカメラ切り替えボタンの制御用

    [SerializeField]
    private CinemachineVirtualCamera worldCamera;  // 俯瞰用カメラをアサイン

    //[SerializeField]  // Debug 用。確認取れたら SerializeField を外す
    private CinemachineVirtualCamera currentCharaCamera;  // アクティブ状態のキャラのカメラをその都度アサインして変更

    private bool isWorldCamera;  // ワールドカメラになっているかの確認。true ならワールドカメラ使用状態

    [SerializeField]
    private List<CinemachineVirtualCamera> charaCamerasList = new List<CinemachineVirtualCamera>();  // 各キャラのカメラのリスト


    void Start()
    {
        // TODO ワールドカメラを初期位置にするか設定


        // ボタンの設定
        btnSwitchCamera.onClick.AddListener(SwitchWorldCamera);

        isWorldCamera = true;
    }

    /// <summary>
    /// ワールドカメラと現在選択しているキャラのカメラの切り替え
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
    /// 選択中のキャラのカメラをセット
    /// </summary>
    /// <param name="charaCamera"></param>
    public void SetCurrentCharaCamera(CinemachineVirtualCamera charaCamera) {
        currentCharaCamera = charaCamera;
        isWorldCamera = true;

        SetActiveCharaCamera(charaCamera);
    }

    /// <summary>
    /// キャラのカメラを削除
    /// </summary>
    public void RemoveCurrentCharaCamera() {
        currentCharaCamera = null;
        isWorldCamera = false;
    }

    /// <summary>
    /// 選択中のキャラのカメラをメインのカメラに切り替え
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
    /// キャラのカメラをリストに追加
    /// </summary>
    /// <param name="camera"></param>
    public void AddCharaCamerasList(CinemachineVirtualCamera camera) {
        charaCamerasList.Add(camera);
    }
}
