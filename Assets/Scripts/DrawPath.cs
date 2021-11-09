using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.AI;

//[DefaultExecutionOrder(10)]
public class DrawPath : MonoBehaviour
{
    [SerializeField]
    private LineRenderer line;

    //[SerializeField]
    //private Transform startPos, endPos;

    //private NavMeshPath path;

    //void Awake() {
    //    path = new NavMeshPath();
    //}

    //void OnEnable() {

    //    // NavMeshBuilder2D の Bake On Enable にチェックを入れること。入れないと Bake が消えて false になる
    //    bool result = NavMesh.CalculatePath(startPos.position, endPos.position, NavMesh.AllAreas, path);
    //    enabled = line.enabled = result;

    //    if (result) {
    //        var corners = path.corners;
    //        line.positionCount = corners.Length;
    //        line.SetPositions(corners);

    //        Debug.Log("Draw Line");
    //    }
    //}

    /// <summary>
    /// 経路の生成
    /// </summary>
    /// <param name="corners"></param>
    /// <returns></returns>
    public IEnumerator DrawCornersLine(Vector3[] corners) {

        // 経路の頂点数の設定
        line.positionCount = corners.Length;

        // 経路の太さ調整
        line.startWidth = 0.3f;
        line.endWidth = 0.3f;

        // 経路の表示(描画)
        line.SetPositions(corners);

        Debug.Log("Draw Line");

        yield return new WaitForSeconds(1.0f);

        // 経路破棄
        Destroy(gameObject);
    }
}
