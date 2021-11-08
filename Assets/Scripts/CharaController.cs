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
    }
}
