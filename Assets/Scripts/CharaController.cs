using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CharaController : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera myCamera;

    public CinemachineVirtualCamera MyCamera { get => myCamera; }

    public MoveToClickTileMapPoint mapPoint;

    public GameManager gameManager;

    // TODO �L�����f�[�^����������


    void Start() {
        //myCamera = transform.GetComponentInChildren<CinemachineVirtualCamera>();
    }


    public void SetUpCharaController(GameManager gameManager) {
        this.gameManager = gameManager;
    }
}
