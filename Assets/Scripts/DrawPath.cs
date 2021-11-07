using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//[DefaultExecutionOrder(10)]
public class DrawPath : MonoBehaviour
{
    [SerializeField]
    private LineRenderer line;

    [SerializeField]
    private Transform startPos, endPos;

    private NavMeshPath path;

    void Awake() {
        path = new NavMeshPath();
    }

    void OnEnable() {

        // NavMeshBuilder2D �� Bake On Enable �Ƀ`�F�b�N�����邱�ƁB����Ȃ��� Bake �������� false �ɂȂ�
        bool result = NavMesh.CalculatePath(startPos.position, endPos.position, NavMesh.AllAreas, path);
        enabled = line.enabled = result;

        if (result) {
            var corners = path.corners;
            line.positionCount = corners.Length;
            line.SetPositions(corners);

            Debug.Log("Draw Line");
        }
    }
}
