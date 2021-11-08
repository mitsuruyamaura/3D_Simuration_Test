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
        // Debug用
        GenerateChara();
    }

    /// <summary>
    /// 
    /// </summary>
    public void GenerateChara() {

        // TODO ループ回数は後で変数に変更
        for (int i = 0; i < 5; i++) {

            CharaController chara = Instantiate(charaPrefab, charaGenerateTran.position, charaPrefab.transform.rotation);

            // 初期設定
            chara.SetUpCharaController(this);

            // ボタン生成
            GenerateCharaButton(chara);

            // Grid と Tilemap を設定
            chara.mapPoint.SetUpMapPoint(grid, tilemap);

            // 各リストに追加
            charasList.Add(chara);

            cameraManager.AddCharaCamerasList(chara.MyCamera);
        }

        /// <summary>
        /// キャラボタン生成
        /// </summary>
        void GenerateCharaButton(CharaController chara) {

            CharaButton charaButton = Instantiate(charaButtonPrefab, charaButtonTran);

            charaButton.SetUpCharaDetail(cameraManager, chara);

            charaButtonsList.Add(charaButton);
        }
    } 

    /// <summary>
    /// 選択したキャラをアクティブに切り替えて、他のキャラを非アクティブに切り替える
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
