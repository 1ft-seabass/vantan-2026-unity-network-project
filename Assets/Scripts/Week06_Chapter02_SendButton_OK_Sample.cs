using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections;       // IEnumerator のための参照
using UnityEngine.Networking;   // UnityWebRequest のための参照
using System;                   // Serializable のための参照
using System.Text;              // Encoding のための参照
using UnityEngine.UI;           // InputField のための参照

public class Week06_Chapter02_SendButton_OK_Sample : MonoBehaviour, IPointerClickHandler
{

    // 送信する Unity データを JSON データ化する PointRequestData ベースクラス
    [Serializable]
    public class PointRequestData
    {
        // point というプロパティ名で int 型で変換
        public int point;
        // name というプロパティ名で int 型で変換
        public string name;
    }

    // アクセスする URL
    // サーバーURL + /pointlist でアクセス
    string urlGitHub = "";

    public void OnPointerClick(PointerEventData eventData)
    {
        // HTTP リクエストを非同期処理を待つためコルーチンとして呼び出す
        StartCoroutine("PostPointData");
    }

    // POST リクエストする本体
    IEnumerator PostPointData()
    {
        // HTTP リクエストする(POST メソッド) UnityWebRequest を呼び出し
        // アクセスする先は変数 urlGitHub で設定
        UnityWebRequest request = new UnityWebRequest(urlGitHub, "POST");

        // PointRequestData ベースクラスを器として呼び出す
        PointRequestData pointRequestData = new PointRequestData();
        // データを設定
        // 現在のポイントを得る
        pointRequestData.point = GameObject.Find("ClickPart").GetComponent<Week06_Chapter02_ClickPart>().currentPoint;
        // 自分の名前を登録
        // 固定値でなく InputField から取得する
        pointRequestData.name = "Seigo";

        // 送信データを JsonUtility.ToJson で JSON 文字列を作成
        // pointRequestData の構造に基づいて変換してくれる
        string strJSON = JsonUtility.ToJson(pointRequestData);
        Debug.Log($"strJSON : {strJSON}");
        // 送信データを Encoding.UTF8.GetBytes で byte データ化
        byte[] bodyRaw = Encoding.UTF8.GetBytes(strJSON);

        // アップロード（Unity→サーバ）のハンドラを作成
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        // ダウンロード（サーバ→Unity）のハンドラを作成
        request.downloadHandler = new DownloadHandlerBuffer();

        // JSON で送ると HTTP ヘッダーで宣言する
        request.SetRequestHeader("Content-Type", "application/json");

        // リクエスト開始
        yield return request.SendWebRequest();
        Debug.Log("リクエスト開始");

        // 結果によって分岐
        switch (request.result)
        {
            case UnityWebRequest.Result.InProgress:
                Debug.Log("リクエスト中");
                break;

            case UnityWebRequest.Result.Success:
                Debug.Log("リクエスト成功");

                // 送信したらリセット
                GameObject.Find("ClickPart").GetComponent<Week06_Chapter02_ClickPart>().ResetPoint();

                // コンソールに表示
                Debug.Log($"responseData: {request.downloadHandler.text}");

                // RankingMessage で最新データを取得
                // GetDataCore を動かす
                GameObject.Find("RankingMessage").GetComponent<Week06_Chapter02_RankingMessage_OK_Sample>().GetDataCore();


                break;
        }

    }
}
