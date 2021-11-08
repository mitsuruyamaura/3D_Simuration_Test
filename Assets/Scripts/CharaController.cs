using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary>
/// �L��������p
/// </summary>
public class CharaController : MonoBehaviour
{
    private CinemachineVirtualCamera myCamera;
    public CinemachineVirtualCamera MyCamera { get => myCamera; }

    private MoveToClickTilemapPoint tilemapMove;
    public MoveToClickTilemapPoint TilemapMove { get => tilemapMove; }

    private GameManager gameManager;
    public GameManager GameManager { get => gameManager; }

    // TODO �L�����f�[�^����������


    void Start() {

        // Debug �p
        //myCamera = transform.GetComponentInChildren<CinemachineVirtualCamera>();
    }

    /// <summary>
    /// �����ݒ�
    /// </summary>
    /// <param name="gameManager"></param>
    public void SetUpCharaController(GameManager gameManager) {
        myCamera = transform.GetComponentInChildren<CinemachineVirtualCamera>();
        this.gameManager = gameManager;
        TryGetComponent(out tilemapMove);
    }
}
