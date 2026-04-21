using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections;       // IEnumerator のための参照
using UnityEngine.Networking;   // UnityWebRequest のための参照

public class Week01_Chapter04_CubeEvent : MonoBehaviour, IPointerClickHandler
{
    // アクセスする URL
    // サーバーURL + page/index
    string urlGitHub = "ここにサーバーURLをいれる";

    public void OnPointerClick(PointerEventData eventData)
    {
        // マウスクリックイベント
        // Debug.Log($"オブジェクト {this.name} がクリックされた！");

        // HTTP GET リクエストを非同期で待つためコルーチンとして呼び出す
        StartCoroutine(GetGitHubData());
    }

    // GET リクエストする本体
    IEnumerator GetGitHubData()
    {
        // HTTP リクエスト処理(GET メソッド) UnityWebRequest を呼び出す
        // アクセス先は変数 urlGitHub で設定
        UnityWebRequest request = UnityWebRequest.Get(urlGitHub);

        // リクエスト開始
        yield return request.SendWebRequest();

        // 結果によって分岐
        switch (request.result)
        {
            case UnityWebRequest.Result.Success:
                Debug.Log("リクエスト成功");

                // コンソールに表示
                Debug.Log($"responseData: {request.downloadHandler.text}");

                break;
        }

        request.Dispose();
    }
}
