using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static void Save(int saveFileNumber)
    {
        Dictionary<string, object> saveDict = new Dictionary<string, object>();
        foreach (var savObject in FindObjectsOfType<SaveableObject>())
        {
            saveDict[savObject.name] = savObject.SaveState();
        }


        string path = Application.persistentDataPath + "/player" + saveFileNumber + ".zombie";
        using (FileStream stream = File.Open(path, FileMode.Create))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, saveDict);
        }
    }

    public static bool Load(int saveFileNumber)
    {
        string path = Application.persistentDataPath + "/player" + saveFileNumber + ".zombie";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            Dictionary<string, object> loadDict = formatter.Deserialize(stream) as Dictionary<string, object>;
            stream.Close();
            foreach (var savObject in FindObjectsOfType<SaveableObject>())
            {
                if (loadDict.ContainsKey(savObject.name))
                {
                    savObject.LoadState(loadDict[savObject.name]);
                }
            }

            return true;
        }

        Debug.LogError("Save file not found in " + path);
        return false;
    }

    public static void Delete(int saveFileNumber)
    {
        string path = Application.persistentDataPath + "/player" + saveFileNumber + ".zombie";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}