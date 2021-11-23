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
    /// デバッグ用
    /// </summary>
    //void Start() {
    //    for (int i = 0; i < enemyTrans.Length; i++) {
    //        GenerateEnemy(0, i);
    //    }
    //}

    /// <summary>
    /// 敵の生成
    /// </summary>
    /// <param name="enemyNo"></param>
    /// <param name="enemyTranNo"></param>
    public void GenerateEnemy() {    // int enemyNo, int enemyTranNo = 0

        //EnemyBaseController enemy = Instantiate(enemies[enemyNo], enemyTrans[enemyTranNo].localPosition, Quaternion.identity);

        EnemyBaseController enemy = Instantiate(enemyBaseData.enemies[currentEnemyCount], transform);

        enemiesList.Add(enemy);
    }

    /// <summary>
    /// 初期設定
    /// </summary>
    /// <param name="enemyBaseData"></param>
    public void SetUpEnemyGenerator(int enemyBaseNo) {  // TODO あとでステージ番号も追加する

        // TODO no から検索してデータを取得する。今は手動で入れておく
        // スクリプタブル・オブジェクトから取得する場合 StageData/EnemyBaseData

        currentEnemyCount = 0;
        IsComplete = false;

        StartCoroutine(PrepareGenerateEnemy());
    }

    /// <summary>
    /// 敵の生成準備
    /// </summary>
    /// <returns></returns>
    private IEnumerator PrepareGenerateEnemy() {

        Debug.Log("生成開始");

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

        Debug.Log("すべての敵の生成完了。総生成数 : " + currentEnemyCount);
        IsComplete = true;
    }
}
