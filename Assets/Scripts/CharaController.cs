using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary>
/// キャラ制御用
/// </summary>
public class CharaController : MonoBehaviour
{
    private CinemachineVirtualCamera myCamera;
    public CinemachineVirtualCamera MyCamera { get => myCamera; }

    private MoveToClickTilemapPoint tilemapMove;
    public MoveToClickTilemapPoint TilemapMove { get => tilemapMove; }

    private GameManager gameManager;
    public GameManager GameManager { get => gameManager; }

    private Animator anim;
    private int currentCornerIndex;

    // TODO キャラデータを持たせる


    void Start() {

        // Debug 用
        //myCamera = transform.GetComponentInChildren<CinemachineVirtualCamera>();
    }

    /// <summary>
    /// 初期設定
    /// </summary>
    /// <param name="gameManager"></param>
    public void SetUpCharaController(GameManager gameManager) {
        myCamera = transform.GetComponentInChildren<CinemachineVirtualCamera>();
        this.gameManager = gameManager;
        TryGetComponent(out tilemapMove);
        TryGetComponent(out anim);
    }

    /// <summary>
    /// 移動方向とアニメの連動
    /// </summary>
    /// <param name="corners"></param>
    /// <returns></returns>
    public IEnumerator SetAnime(Vector3[] corners) {

        Debug.Log("アニメ開始");

        currentCornerIndex = 0;

        while (true) {

            Debug.Log(Vector2.Distance(transform.position, corners[currentCornerIndex]));

            // Vector3 だと上手くいかない(Z 成分があると計算値が変わるため)
            if (Vector2.Distance(transform.position, corners[currentCornerIndex]) <= 0.3f) {

                currentCornerIndex++;
                Debug.Log(currentCornerIndex);

                if (currentCornerIndex >= corners.Length) {
                    Debug.Log("アニメ終了");
                    yield break;
                }

                // Vector3 だと上手くいかない
                Vector2 direction = (corners[currentCornerIndex] - transform.position).normalized;

                anim.SetFloat("X", direction.x);
                anim.SetFloat("Y", direction.y);
            }

            yield return null;
        }
    }
}
