using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

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


    void Start()
    {
        // Debug用
        GenerateChara();
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
            chara.TilemapMove.SetUpTilemapMove(grid, tilemap);

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
    }

    /// <summary>
    /// 選択したキャラをアクティブに切り替えて、他のキャラを非アクティブに切り替える
    /// </summary>
    /// <param name="activeChara">アクティブ状態にするキャラ</param>
    public void SwitchActivateChara(CharaController activeChara) {

        // アクティブキャラのみアクティブ状態にし、他のキャラは非アクティブ状態にする
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
