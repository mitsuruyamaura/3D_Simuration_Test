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
/// ゲームシーンの制御・管理用
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField]
    private CharaController charaPrefab;

    [SerializeField]
    private CharaButton charaButtonPrefab;

    [SerializeField]
    private Transform charaGenerateTran;        // 一旦 GameManager をアサイン。後で拠点に変更する

    [SerializeField]
    private Transform charaButtonGenerateTran;  // Content をアサイン

    [SerializeField]
    private UnityEngine.UI.Scrollbar scrollbar;

    [SerializeField]
    private CameraManager cameraManager;

    [SerializeField]
    private Grid grid; // 移動可能なタイルマップの方の Grid をアサイン 

    [SerializeField]
    private Tilemap tilemap;　// 移動可能なタイルマップの Tilemap をアサイン

    [SerializeField]
    private List<CharaController> charasList = new List<CharaController>();  // 生成したキャラのリスト

    [SerializeField]
    private List<CharaButton> charaButtonsList = new List<CharaButton>();    // 生成したキャラボタンのリスト

    [SerializeField]
    private int generateCharaCount;  // キャラの生成数。後で別の情報から参照するのでデバッグ用

    public ReactiveProperty<bool> IsTimeStopped = new ReactiveProperty<bool>(false);  // 時間が流れているかどうかを判定するための ReactiveProperty

    [SerializeField]
    private UnityEngine.UI.Button btnTimeTransition;

    public ReactiveProperty<int> OurTime;
    private int maxTime = 24;

    public TimeType currentTimeType;


    void Start()
    {
        // Debug用
        GenerateChara();

        // 時間の流れの切り替えボタンの設定
        btnTimeTransition!.OnClickAsObservable()
            .TakeUntilDestroy(gameObject)
            .ThrottleFirst(TimeSpan.FromSeconds(0.5f))
            .Subscribe(_ => OnClickSwitchTimeTransition());

        Debug.Log($"時間の流れ : {(IsTimeStopped.Value ? "停止" : "開始")}");

        int targetTime = 3;

        // 時間計測
        Observable.Timer(TimeSpan.FromSeconds(targetTime), TimeSpan.FromSeconds(targetTime))
            .Subscribe(_ => KeepTime());  // Subscribe 内に複数の処理を書く場合は Subscribe(_ => { }) とする

        // 時間を監視して TimeType を自動で変更
        OurTime.Subscribe(x => ChangeTimeType(x));

        OurTime.Value = 6;
    }

    /// <summary>
    /// 時間経過計測
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
    /// TimeTypeの変更
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

        // 明るさとライトの範囲の調整
        if (our >= 17 || our >= 5) {
            cameraManager.ChangeLightIntensity(our);
            cameraManager.ChangePointLightOuterRadius(our);
        }
    }

    /// <summary>
    /// キャラ生成
    /// </summary>
    public void GenerateChara() {

        // TODO ループ回数は後で別の変数に変更
        for (int i = 0; i < generateCharaCount; i++) {

            // キャラ生成
            CharaController chara = Instantiate(charaPrefab, charaGenerateTran.position, charaPrefab.transform.rotation);

            // キャラの初期設定
            chara.SetUpCharaController(this);

            // ボタン生成
            GenerateCharaButton(chara);

            // タイルマップの移動用の情報として Grid と Tilemap を設定
            chara.TilemapMove.SetUpTilemapMove(grid, tilemap, chara, this);

            // 各リストに追加
            charasList.Add(chara);
            cameraManager.AddCharaCamerasList(chara.MyCamera);
        }

        /// <summary>
        /// キャラボタン生成
        /// </summary>
        /// <param name="chara"></param>
        void GenerateCharaButton(CharaController chara) {

            // キャラボタン生成(GridLayoutGroup を使って自動的に並べる)
            CharaButton charaButton = Instantiate(charaButtonPrefab, charaButtonGenerateTran);
            
            // キャラボタンの初期設定
            charaButton.SetUpCharaButton(cameraManager, chara);

            // キャラボタンのリストに追加
            charaButtonsList.Add(charaButton);
        }

        // すべてのキャラボタン作成後、スクロールバーを一番先頭にフォーカスする
        scrollbar.value = 1.0f;
    }

    /// <summary>
    /// 選択したキャラをアクティブに切り替えて、他のキャラを非アクティブに切り替える
    /// </summary>
    /// <param name="activeChara">アクティブ状態にするキャラ</param>
    public void SwitchActivateChara(CharaController activeChara) {

        // アクティブキャラのみアクティブ状態にし、他のキャラは非アクティブ状態にする
        charasList.Select(x => x == activeChara ? x.TilemapMove.isActive = true : x.TilemapMove.isActive = false).ToList();

        // アクティブキャラのみフレームを表示する
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
    /// 選択しているキャラの解除
    /// </summary>
    public void InactivateChara() {
        charasList.Select(x => x.TilemapMove.isActive = false).ToList();

        charaButtonsList.Select(x => x.SwitchActivateFrame(false)).ToList();
    }

    /// <summary>
    /// 時間の流れの切り替え
    /// </summary>
    private void OnClickSwitchTimeTransition() {

        IsTimeStopped.Value = !IsTimeStopped.Value;

        Debug.Log($"時間の流れ : {(IsTimeStopped.Value ? "停止" : "開始")}");
    }
}
