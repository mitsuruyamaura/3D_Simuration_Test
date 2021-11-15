using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using System.Linq;
using UnityEngine.Experimental.Rendering.Universal;

// Unity 2D環境でCinemachineを触ってみたのでメモ
//https://www.subarunari.com/entry/2018/05/19/2D%E7%92%B0%E5%A2%83%E3%81%A7Cinemachine%E3%82%92%E8%A7%A6%E3%81%A3%E3%81%A6%E3%81%BF%E3%81%9F%E3%81%AE%E3%81%A7%E3%83%A1%E3%83%A2

/// <summary>
/// ゲーム内のカメラの制御・管理用
/// </summary>
public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private Button btnSwitchCamera;  // ワールドカメラ切り替えボタンの制御用

    [SerializeField]
    private CinemachineVirtualCamera worldCamera;  // 俯瞰用カメラをアサイン

    //[SerializeField]  // Debug 用。確認取れたら SerializeField を外す
    private CinemachineVirtualCamera currentCamera;  // アクティブ状態のキャラのカメラか、フリーカメラをその都度アサインして変更

    private bool isWorldCamera;  // ワールドカメラになっているかの確認。true ならワールドカメラ使用状態

    [SerializeField]
    private List<CinemachineVirtualCamera> charaCamerasList = new List<CinemachineVirtualCamera>();  // 各キャラのカメラのリスト

    [SerializeField]
    private Button btnFreeCamera;  // フリーカメラ切り替えボタンの制御用

    [SerializeField]
    private CinemachineVirtualCamera freeCamera;  // フリーカメラをアサイン

    [SerializeField]
    private Light2D light2D;

    private float currentOuterRadius;


    void Start()
    {
        // TODO ワールドカメラを初期位置にするか設定


        // ボタンの設定
        btnSwitchCamera?.onClick.AddListener(SwitchWorldCamera);
        btnFreeCamera?.onClick.AddListener(SetFreeCamera);

        isWorldCamera = true;

        SetFreeCamera();

        currentOuterRadius = 20;
    }

    /// <summary>
    /// ワールドカメラと現在選択しているキャラのカメラの切り替え
    /// </summary>
    public void SwitchWorldCamera() {

        if (isWorldCamera) {
            worldCamera.Priority = 10;
            currentCamera.Priority = 5;

            light2D.lightType = Light2D.LightType.Global;
        } else {
            worldCamera.Priority = 5;
            currentCamera.Priority = 10;

            light2D.lightType = Light2D.LightType.Point;
        }

        // ポイントライトの調整
        if (currentOuterRadius != 100) {
            ChangePointLightOuterRadius((int)currentOuterRadius);
        }

        isWorldCamera = !isWorldCamera;
    }

    /// <summary>
    /// 選択中のキャラのカメラをセット
    /// </summary>
    /// <param name="charaCamera"></param>
    public void SetCurrentCharaCamera(CinemachineVirtualCamera charaCamera) {
        currentCamera = charaCamera;
        isWorldCamera = true;

        // フリーカメラのボタンを押せるようにする
        btnFreeCamera.interactable = true;

        SetActiveCharaCamera(charaCamera);
    }

    /// <summary>
    /// キャラのカメラを削除
    /// </summary>
    public void RemoveCurrentCameraFromChara() {
        currentCamera = null;
        isWorldCamera = true;
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

    /// <summary>
    /// 現在のカメラをフリーカメラに設定
    /// </summary>
    public void SetFreeCamera() {

        // 現在のカメラの設定がある場合
        if (currentCamera != null) {

            // 表示している位置情報を引き継ぐ
            freeCamera.transform.position = currentCamera.transform.position;

            // キャラを解除(元からフリーカメラの場合には何もしないようにする)
            if (currentCamera.transform.parent.gameObject.TryGetComponent(out CharaController chara)){
                chara.GameManager.InactivateChara();
            }
            // キャラのカメラの優先度を下げる
            SetActiveCharaCamera(freeCamera);
        }

        // 現在のカメラをフリーカメラに設定
        currentCamera = freeCamera;

        // フリーカメラのボタンを押せないようにする
        btnFreeCamera.interactable = false;

        freeCamera.Priority = 10;

        // ポイントライトの調整
        if (currentOuterRadius != 100) {
            ChangePointLightOuterRadius((int)currentOuterRadius);
        }
    }

    /// <summary>
    /// ライトの明るさ調整
    /// </summary>
    /// <param name="our"></param>
    public void ChangeLightIntensity(int our) {

        // 明るさ調整
        light2D.intensity = our switch {
            5 => 0.8f,
            6 => 0.9f,
            7 => 1.0f,
            17 => 0.9f,
            18 => 0.8f,
            19 => 0.7f,
            _ => light2D.intensity
        };
    }

    /// <summary>
    /// ポイントライトの範囲の調整
    /// </summary>
    /// <param name="our"></param>
    public void ChangePointLightOuterRadius(int our) {

        currentOuterRadius = our;

        if (light2D.lightType == Light2D.LightType.Point) {

            light2D.pointLightOuterRadius = our switch {
                5 => 13f,
                6 => 16f,
                7 => 20f,
                17 => 16f,
                18 => 13f,
                19 => 10f,
                _ => light2D.pointLightOuterRadius
            };
        }
    }
}
