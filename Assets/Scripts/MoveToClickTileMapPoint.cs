using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.AI;
using UnityEngine.EventSystems;

//【Unityで2DRPG】NPCをNavMeshAgentで動かしてみる (猫の冒険)
//https://a1026302.hatenablog.com/entry/2020/11/21/014727

//【Unity】ボタンを押したときに画面クリックは無視する
//https://nn-hokuson.hatenablog.com/entry/2017/07/12/220302

/// <summary>
/// タイルマップをタップしてナビメッシュで移動させるためのクラス
/// </summary>
public class MoveToClickTilemapPoint : MonoBehaviour {

    [SerializeField]
    private Grid grid;

    [SerializeField]
    private Tilemap tilemap;

    private NavMeshAgent agent;

    public bool isActive;


    void Start() {

        if (TryGetComponent(out agent)) {

            // インスタンス対応。事前に入れておくと、Bake している地点を認識できずにエラーになるため
            agent.enabled = true;

            // 2D なので、これがないと、変な位置に勝手に移動する
            agent.updateRotation = false;
            agent.updateUpAxis = false;

            // 初期目的地設定(これがないと初期位置からズレる)
            agent.destination = transform.position;
        }
    }

    void Update() {

#if UNITY_EDITOR
        // UI がタップされたときは処理しない(UI のボタンを押したらそちらのみを反応させる)
        if (EventSystem.current.IsPointerOverGameObject()) {
            return;
        }
#else   // スマホ用
        if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) {
            return;
        }
#endif

        if (!isActive) {
            return;
        }

        // NavMeshAgent が利用できる状態でタイルマップをタップ(マウスクリック)したら
        if (agent && Input.GetMouseButtonDown(0)) {

            // タップの位置を取得してワールド座標に変換し、それをさらにタイルのセル座標に変換
            Vector3Int gridPos = grid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            //Debug.Log(gridPos);

            // タップしたタイルが移動不可のタイルでなければ
            if (tilemap.GetColliderType(gridPos) != Tile.ColliderType.None) {

                // タップしたタイルマップを目的地として移動
                SetPathAndMove(gridPos);
            }
        }

        //// 対象が動く場合は、他でやる
        //float now = Vector2.Distance(transform.position, nextPos);

        //if (nextPos.magnitude < now) {
        //    // 再計算
        //}
    }

    /// <summary>
    /// タップ(マウスクリック)したタイルマップを目的地として移動
    /// </summary>
    /// <param name="gridPos"></param>
    private void SetPathAndMove(Vector3Int gridPos) {

        // 目的地点の設定。調整することで、タイルの中央に移動させる
        Vector2 nextPos = new Vector2(gridPos.x + 0.5f, gridPos.y + 0.5f);

        //Debug.Log(nextPos);

        // 目的地の更新
        agent.destination = nextPos;

        //Debug.Log("移動");
    }

    /// <summary>
    /// 初期設定
    /// </summary>
    /// <param name="grid"></param>
    /// <param name="tilemap"></param>
    public void SetUpTilemapMove(Grid grid, Tilemap tilemap) {
        this.grid = grid;
        this.tilemap = tilemap;

        isActive = false;
    }
}
