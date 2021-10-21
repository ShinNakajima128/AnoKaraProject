using UnityEngine;
using System.IO;

namespace AnoKara
{
    class LocalData
    { 
        public static void Save<T>(string file, T data)
        {
            StreamWriter writer;
            var json = JsonUtility.ToJson(data);
            writer = new StreamWriter(Application.dataPath + "/" +  file, false);
            writer.Write(json);
            writer.Flush();
            writer.Close();
        }

        public static T Load<T>(string file)
        {
            string datastr;
            StreamReader reader;

            reader = new StreamReader(Application.dataPath + "/" + file);
            datastr = reader.ReadToEnd();
            reader.Close();

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

