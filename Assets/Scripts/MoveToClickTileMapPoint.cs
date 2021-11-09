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

    [SerializeField]
    private DrawPath drawPathPrefab;

    private CharaController charaController;
    //private IEnumerator coroutine;


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
        Vector3 nextPos = new Vector3(gridPos.x + 0.5f, gridPos.y + 0.5f, 0);

        // 比較する情報の Z 軸を両方とも 0 に指定しておかないと、2Dの経路計算ができない
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        //Debug.Log(nextPos);

        // 目的地の更新
        //agent.SetDestination(nextPos);  // どちらでも問題なし
        agent.destination = nextPos;

        //Debug.Log(agent.hasPath);
        //Debug.Log(agent.isOnNavMesh);

        // 経路の生成
        StartCoroutine(GenerateCornerLineFromPath(transform.position, nextPos));

        //Debug.Log("移動");

        /// <summary>
        /// 経路の生成
        /// </summary>
        /// <param name="startPos"></param>
        /// <param name="endPos"></param>
        IEnumerator GenerateCornerLineFromPath(Vector3 startPos, Vector3 endPos) {

            //Debug.Log(startPos);
            //Debug.Log(endPos);
            //Debug.Log(agent.hasPath);

            // パスを取得するまで待機
            yield return new WaitUntil(() => agent.hasPath);
            //Debug.Log(agent.hasPath);
            //Debug.Log(agent.isOnNavMesh);

            NavMeshPath path = new NavMeshPath();

            // 丁寧に書く場合
            //bool result = NavMesh.CalculatePath(startPos, endPos, NavMesh.AllAreas, path);
            //Debug.Log(result);

            // 経路の情報を取得できているか確認
            if (NavMesh.CalculatePath(startPos, endPos, NavMesh.AllAreas, path)) {   // if(result){ }  丁寧に書く場合

                // 経路情報がある場合、経路表示用のゲームオブジェクト作成
                DrawPath drawPath = Instantiate(drawPathPrefab);

                // 経路の座標情報取得
                Vector3[] corners = path.corners;

                //if (coroutine != null) {
                //    StopCoroutine(coroutine);
                //    coroutine = null;
                //}

                //coroutine = charaController.SetAnime(corners);

                //StartCoroutine(coroutine);

                //path.GetCornersNonAlloc(corners);
                Debug.Log("GetCornersNonAlloc : " + path.GetCornersNonAlloc(corners));  // 最初の地点もいれて数える

                // 経路の作成
                StartCoroutine(drawPath.DrawCornersLine(corners));

                Debug.Log(" 経路の作成 開始");
            }
        }
    }

    /// <summary>
    /// 初期設定
    /// </summary>
    /// <param name="grid"></param>
    /// <param name="tilemap"></param>
    public void SetUpTilemapMove(Grid grid, Tilemap tilemap, CharaController charaController) {
        this.grid = grid;
        this.tilemap = tilemap;
        this.charaController = charaController;

        isActive = false;
    }
}
