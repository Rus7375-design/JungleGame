using System.IO;
using UnityEngine;

public static class SaveSystem
{
    public static string FilePath =>
        Path.Combine(Application.persistentDataPath, "save.json");

    static string TmpPath =>
        Path.Combine(Application.persistentDataPath, "save.tmp");

    public static void Save(SaveData data)
    {
        try
        {
            File.WriteAllText(TmpPath, JsonUtility.ToJson(data, true));
            if (File.Exists(FilePath))
                File.Replace(TmpPath, FilePath, null);
            else
                File.Move(TmpPath, FilePath);
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("Save failed: " + e.Message);  
        }
    }

    public static SaveData Load()
    {
        if (!File.Exists(FilePath)) return new SaveData();

        try
        {
            string json = File.ReadAllText(FilePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);  

            if (data == null || data.version != SaveData.CurrentVersion)
            {
                Debug.LogWarning("Save is empty or has another version, using defaults");
                return new SaveData();
            }

            data.Validate();
            return data;
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("Broken save: " + e.Message);
            return new SaveData();
        }
    }

    public static void Delete()
    {
        try
        {
            if (File.Exists(FilePath)) File.Delete(FilePath);
            if (File.Exists(TmpPath)) File.Delete(TmpPath);
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("Delete failed: " + e.Message);
        }
    }
}