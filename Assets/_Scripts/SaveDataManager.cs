using System.IO;
using UnityEngine;

public static class SaveDataManager
{
    public static void SaveData<T>(this T data, string relativePath, string encryptionString = null)
    {
        string path = Application.persistentDataPath + relativePath;

        if (!File.Exists(path))
        {
            File.Create(path).Close();
        }

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(path, json);
    }

    public static T LoadData<T>(string relativePath, string decryptionString = null)
    {
        string path = Application.persistentDataPath + relativePath;

        if (!File.Exists(path))
        {
            return default;
        }

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<T>(json);
    }
}
