using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseData_EnemyBase
{
    public int appearTime;         // １体生成するまでの待機時間

    public EnemyBaseController[] enemies;   // 生成される敵の種類。 生成するエネミーの総数

    // TODO 他にもあれば追加する

}
