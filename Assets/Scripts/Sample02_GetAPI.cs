using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections;           // IEnumerator のための参照
using UnityEngine.Networking;       // UnityWebRequest のための参照
using System;                       // Serializable のための参照
using System.Collections.Generic;   // List のための参照

public class Sample02_GetAPI : MonoBehaviour
{
    // アクセスする URL
    string urlAPI = "";

    void Start()
    {
        StartCoroutine(GetAPI(urlAPI));
    }

    // テクスチャ読み込み
    IEnumerator GetAPI(string url)
    {
        // テクスチャを GET リクエストで読み込む。ブラウザでも見れる。
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);

        // リクエスト開始
        yield return request.SendWebRequest();

        Debug.Log("リクエスト開始");

        // 結果によって分岐
        switch (request.result)
        {
            case UnityWebRequest.Result.InProgress:
                Debug.Log("リクエスト中");
                break;

            case UnityWebRequest.Result.ConnectionError:
                Debug.Log("ConnectionError");
                Debug.Log(request.error);
                break;

            case UnityWebRequest.Result.ProtocolError:
                Debug.Log("ProtocolError");
                Debug.Log(request.responseCode);
                Debug.Log(request.error);
                break;

            case UnityWebRequest.Result.Success:
                Debug.Log("リクエスト成功");

                // テクスチャに割り当て
                Texture loadedTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;

                GameObject.Find("Tile0").GetComponent<MeshRenderer>().material.SetTexture("_MainTex", loadedTexture);

                break;
        }
    }
}
