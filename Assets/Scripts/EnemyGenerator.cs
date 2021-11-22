using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField]
    private EnemyBaseController[] enemies;

    [SerializeField]
    private Transform[] enemyTrans;
    
    public List<EnemyBaseController> enemiesList = new List<EnemyBaseController>();


    /// <summary> 
    /// デバッグ用
    /// </summary>
    void Start() {
        for (int i = 0; i < enemyTrans.Length; i++) {
            GenerateEnemy(0, i);
        }
    }

    /// <summary>
    /// 敵の生成
    /// </summary>
    /// <param name="enemyNo"></param>
    /// <param name="enemyTranNo"></param>
    public void GenerateEnemy(int enemyNo, int enemyTranNo) {

        EnemyBaseController enemy = Instantiate(enemies[enemyNo], enemyTrans[enemyTranNo].localPosition, Quaternion.identity);

        enemiesList.Add(enemy);
    }
}
