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

    private Animator anim;
    private int currentCornerIndex;

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
        TryGetComponent(out anim);
    }

    /// <summary>
    /// �ړ������ƃA�j���̘A��
    /// </summary>
    /// <param name="corners"></param>
    /// <returns></returns>
    public IEnumerator SetAnime(Vector3[] corners) {

        Debug.Log("�A�j���J�n");

        currentCornerIndex = 0;

        while (true) {

            Debug.Log(Vector2.Distance(transform.position, corners[currentCornerIndex]));

            // Vector3 ���Ə�肭�����Ȃ�(Z ����������ƌv�Z�l���ς�邽��)
            if (Vector2.Distance(transform.position, corners[currentCornerIndex]) <= 0.3f) {

                currentCornerIndex++;
                Debug.Log(currentCornerIndex);

                if (currentCornerIndex >= corners.Length) {
                    Debug.Log("�A�j���I��");
                    yield break;
                }

                // Vector3 ���Ə�肭�����Ȃ�
                Vector2 direction = (corners[currentCornerIndex] - transform.position).normalized;

                anim.SetFloat("X", direction.x);
                anim.SetFloat("Y", direction.y);
            }

            yield return null;
        }
    }
}
