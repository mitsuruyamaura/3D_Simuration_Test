using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//UnityでSceneビューのような視点移動ができるカメラを作る(MouseUpdate の方)
//https://esprog.hatenablog.com/entry/2016/03/20/033322

//【Unity】スワイプ移動した分だけオブジェクトを移動させる(MoveCamera の方)
//https://zenn.dev/daichi_gamedev/articles/74b0a80dd836ac


public class SwipeMoveCamera : MonoBehaviour
{
    [SerializeField, Range(0.1f, 10.0f)]
    private float wheelSpeed = 1.0f;

    [SerializeField, Range(0.1f, 10.0f)]
    private float moveSpeed = 0.3f;

    [SerializeField, Range(0.1f, 10.0f)]
    private float rotateSpeed = 0.3f;

    private Vector3 prevMousePos;


    void Update()
    {
        MoveCamera();
        return;

        //MouseUpdate();
        //return;
    }

    /// <summary>
    /// スワイプした分だけカメラを上下左右に移動させる
    /// </summary>
    private void MoveCamera() {

        // 最初にタップした時
        if (Input.GetMouseButtonDown(0)) {

            // マウスの位置情報を記録
            prevMousePos = Input.mousePosition;
        }

        // スワイプ中
        if (Input.GetMouseButton(0)) {

            // 最新のマウスの位置情報を取得
            Vector3 currentPos = Input.mousePosition;

            // 前のマウスの位置情報と最新のマウスの位置情報の差分値を計算
            Vector3 diffDistance = currentPos - prevMousePos;

            // 移動距離の調整
            diffDistance = diffDistance * moveSpeed * Time.deltaTime;

            // カメラの移動(スワイプの逆に移動させるので減算処理)
            transform.position -= new Vector3(diffDistance.x, diffDistance.y, 0);

            // マウスの位置情報を更新
            prevMousePos = Input.mousePosition;
        }
    }


////////////////////***  未使用(動作は確認済で問題なし)　***////////////////////////


    /// <summary>
    /// マウス制御による更新処理
    /// </summary>
    private void MouseUpdate() {

        // ホイール入力の感知
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");

        // ホイール入力がある場合
        if (scrollWheel != 0) {

            // カメラの前後移動(画面的には拡大・縮小)
            MouseWheel(scrollWheel);
        }

        // マウスクリックの入力感知
        if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)) {

            // マウスの位置情報を記録
            prevMousePos = Input.mousePosition;
        }

        // ドラッグ(スワイプ)の処理
        MouseDrag(Input.mousePosition);
    }

    /// <summary>
    /// カメラの前後移動(画面的には拡大・縮小)
    /// </summary>
    /// <param name="delta"></param>
    private void MouseWheel(float delta) {
        // カメラの前後移動
        transform.position += transform.forward * delta * wheelSpeed;
        return;
    }

    /// <summary>
    /// ドラッグ(スワイプ)の処理
    /// </summary>
    /// <param name="mousePos"></param>
    private void MouseDrag(Vector3 mousePos) {

        // 前のマウスの位置情報と最新のマウスの位置情報の差分値を計算
        Vector3 diff = mousePos - prevMousePos;

        // diff のベクトルの長さ(平方根は求めない方)を取得し、有効桁数(1e-5未満)を満たしているか確認
        if (diff.sqrMagnitude < Vector3.kEpsilon) {
            // 満たしていない場合には処理しない
            return;
        }
   
        if (Input.GetMouseButton(2)) {
            // カメラの上下移動(ドラッグの逆に動く)
            transform.Translate(-diff * Time.deltaTime * moveSpeed);
        } else if (Input.GetMouseButton(1)) {
            // カメラの回転
            CameraRotate(new Vector2(-diff.y, diff.x) * rotateSpeed);
        }

        // マウスの位置情報を更新
        prevMousePos = mousePos;
    }

    /// <summary>
    /// カメラの回転
    /// </summary>
    /// <param name="angle"></param>
    public void CameraRotate(Vector2 angle) {
        // カメラの回転(軸ごと)
        transform.RotateAround(transform.position, transform.right, angle.x);
        transform.RotateAround(transform.position, Vector3.up, angle.y);
    }
}
