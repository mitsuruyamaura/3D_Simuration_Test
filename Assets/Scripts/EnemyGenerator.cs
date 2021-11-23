using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    //[SerializeField]
    //private EnemyBaseController[] enemies;

    //[SerializeField]
    //private Transform[] enemyTrans;
    
    public List<EnemyBaseController> enemiesList = new List<EnemyBaseController>();

    private bool isComplete;

    public bool IsComplete
    {
        set => isComplete = value;
        get => isComplete;
    }

    [SerializeField]
    private BaseData_EnemyBase enemyBaseData;

    private int currentEnemyCount;


    /// <summary> 
    /// �f�o�b�O�p
    /// </summary>
    //void Start() {
    //    for (int i = 0; i < enemyTrans.Length; i++) {
    //        GenerateEnemy(0, i);
    //    }
    //}

    /// <summary>
    /// �G�̐���
    /// </summary>
    /// <param name="enemyNo"></param>
    /// <param name="enemyTranNo"></param>
    public void GenerateEnemy() {    // int enemyNo, int enemyTranNo = 0

        //EnemyBaseController enemy = Instantiate(enemies[enemyNo], enemyTrans[enemyTranNo].localPosition, Quaternion.identity);

        EnemyBaseController enemy = Instantiate(enemyBaseData.enemies[currentEnemyCount], transform);

        enemiesList.Add(enemy);
    }

    /// <summary>
    /// �����ݒ�
    /// </summary>
    /// <param name="enemyBaseData"></param>
    public void SetUpEnemyGenerator(int enemyBaseNo) {  // TODO ���ƂŃX�e�[�W�ԍ����ǉ�����

        // TODO no ���猟�����ăf�[�^���擾����B���͎蓮�œ���Ă���
        // �X�N���v�^�u���E�I�u�W�F�N�g����擾����ꍇ StageData/EnemyBaseData

        currentEnemyCount = 0;
        IsComplete = false;

        StartCoroutine(PrepareGenerateEnemy());
    }

    /// <summary>
    /// �G�̐�������
    /// </summary>
    /// <returns></returns>
    private IEnumerator PrepareGenerateEnemy() {

        Debug.Log("�����J�n");

        float timer = 0;

        while (currentEnemyCount < enemyBaseData.enemies.Length) {

            timer += Time.deltaTime;

            if (timer > enemyBaseData.appearTime) {

                GenerateEnemy();

                timer = 0;
                currentEnemyCount++;
            }

            yield return null;
        }

        Debug.Log("���ׂĂ̓G�̐��������B�������� : " + currentEnemyCount);
        IsComplete = true;
    }
}
