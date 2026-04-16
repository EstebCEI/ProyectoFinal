using UnityEngine;
using System.Collections;
using System.IO;

public class GameDataJSON : MonoBehaviour
{
    public static string fileName = "GameData.json";
    public static string GetPath()
    {
        return Path.Combine(Application.persistentDataPath, fileName);
    }
    
    public static void SaveGameData(GameData data)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(GetPath(), json);
    }

    public static GameData Load()
    {
        string path = GetPath();

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<GameData>(json);
        }
        return new GameData(100f, Vector3.zero, 12, false);
    }

    public static void Delete()
    {
        if (File.Exists(GetPath()))
        {
            File.Delete(GetPath());
        }
    }
}


