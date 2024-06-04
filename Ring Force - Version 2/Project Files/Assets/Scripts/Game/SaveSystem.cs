using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SavePlayer (PlayerData playerData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = System.IO.Directory.GetCurrentDirectory() + "/player.data";
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData saveData = new SaveData(playerData);

        formatter.Serialize(stream, saveData);
        stream.Close();
    }

    public static SaveData LoadPlayer()
    {
        string path = System.IO.Directory.GetCurrentDirectory() + "/player.data";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData saveData = formatter.Deserialize(stream) as SaveData;

            stream.Close();

            return saveData;
        }
        else
        {
            Debug.Log("ERROR: Save file not found in " + path);
            return null;
        }
    }
}
