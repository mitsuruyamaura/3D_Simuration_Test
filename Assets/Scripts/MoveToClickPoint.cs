using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// アニメーションとナビゲーションの利用
//https://docs.unity3d.com/ja/2019.4/Manual/nav-CouplingAnimationAndNavigation.html

// 2D NavMesh - Unity で Tilemap に NavMesh をベイクしてみる
//https://www.matatabi-ux.com/entry/2021/03/04/100000

// 【Unity】NavMeshComponentsに2D対応ブランチが追加されていたので試してみた
//https://tsubakit1.hateblo.jp/entry/2019/11/10/180825

public class MoveToClickPoint : MonoBehaviour
{
    private NavMeshAgent agent;
    
    void Start()
    {
        TryGetComponent(out agent);
    }

    void Update()
    {
        if (agent && Input.GetMouseButtonDown(0)) {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100)) {
                agent.destination = hit.point;
            }
        }
    }
}
