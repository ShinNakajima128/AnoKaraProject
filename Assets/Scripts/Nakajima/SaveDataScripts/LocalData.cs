using UnityEngine;
using System.IO;
using UnityEngine.Networking;

namespace AnoKara
{
    class LocalData
    {
        /// <summary>
        /// ローカルファイルにデータを保存する
        /// </summary>
        /// <typeparam name="T"> データの型 </typeparam>
        /// <param name="file"> 保存先のファイル名 </param>
        /// <param name="data"> 現在のデータ </param>
        public static void Save<T>(string file, T data)
        {
            var json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(file, json);
            Debug.Log($"データを保存しました。キー：{file},データ：{data}");
        }

        public static T Load<T>(string file)
        {
            string datastr;
            datastr = PlayerPrefs.GetString(file);

            Debug.Log(datastr);

            if (datastr == null || datastr == "")
            {
                Debug.Log("データがありませんでした");
                return default;
            }
            
            var gameData = JsonUtility.FromJson<T>(datastr); // ロードしたデータで上書き

            if (gameData != null)
            {
                Debug.Log(gameData + "のデータをロードしました");
                return gameData;
            }
            else
            {
                return default;
            }
        }
    }
}

