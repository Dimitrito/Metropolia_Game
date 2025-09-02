using System.IO;
using UnityEngine;

public static class SaveLoadManager
{
    private static string savePath => Path.Combine(Application.persistentDataPath, "save.json");

    public static void SaveGame(GameSave save)
    {
        string json = JsonUtility.ToJson(save, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Game saved to: " + savePath);
    }

    public static GameSave LoadGame()
    {
        if (!File.Exists(savePath))
        {
            Debug.LogWarning("Save file not found!");
            return null;
        }

        string json = File.ReadAllText(savePath);
        GameSave save = JsonUtility.FromJson<GameSave>(json);
        Debug.Log("Game loaded from: " + savePath);
        return save;
    }
}
