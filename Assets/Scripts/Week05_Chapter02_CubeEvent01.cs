using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections;       // IEnumerator のための参照
using UnityEngine.Networking;   // UnityWebRequest のための参照
using System;                   // Serializable のための参照

using System.Collections.Generic; // List のための参照

public class Week05_Chapter02_CubeEvent01 : MonoBehaviour, IPointerClickHandler
{
    // API の接続先
    // 今回は サーバーURL + /api/get を読み込む
    string urlAPI = "https://super-duper-potato-jrw4r76996fq44x-1880.app.github.dev/sample01";

    // 受信した JSON データを Unity で扱うデータにする ResponseData ベースクラス
    // 中には ResponseDataItem で構成されています
    //
    //{
    //  "data": [
    //    {
    //      "id": "33d3",
    //      "title": "A"
    //    },
    //    {
    //      "id": "230d",
    //      "title": "B"
    //    },
    //    {
    //      "id": "93b3",
    //      "title": "C"
    //    },
    //    {
    //      "id": "4fe7",
    //      "title": "D"
    //    }
    //  ]
    //}
    // という data のオブジェクトの中に配列が入っています
    [Serializable]
    public class ResponseData
    {
        public List<ResponseDataItem> data;
    }

    // ResponseDataItem で data の中を id, title のリストで取得できるよう構造を作ります
    [Serializable]
    public class ResponseDataItem
    {

    }

    void Start()
    {

    }

    void Update()
    {

    }
    public void OnPointerClick(PointerEventData eventData)
    {
        // マウスクリックイベント

        // HTTP リクエストを非同期処理を待つためコルーチンとして呼び出す
        StartCoroutine("GetData");
    }

    IEnumerator GetData()
    {
        // HTTP リクエストする(GET メソッド) UnityWebRequest を呼び出し
        // アクセスする先は変数 urlGitHub で設定
        UnityWebRequest request = UnityWebRequest.Get(urlAPI);

        // リクエスト開始
        yield return request.SendWebRequest();

        // 結果によって分岐
        switch (request.result)
        {
            case UnityWebRequest.Result.InProgress:
                Debug.Log("リクエスト中");
                break;

            case UnityWebRequest.Result.Success:
                Debug.Log("リクエスト成功");

                // コンソールに表示
                Debug.Log($"responseData: {request.downloadHandler.text}");

                // ResponseData クラスで Unity で扱えるデータ化
                ResponseData response = JsonUtility.FromJson<ResponseData>(request.downloadHandler.text);


                break;
        }


    }
}