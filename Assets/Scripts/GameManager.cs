using System.IO;
using UnityEngine;
using Leguar.TotalJSON;

public class GameManager : MonoBehaviour
{
    public static GameManager shared;
    public PlayerData playerData;
    public string filePath;

    public void Start()
    {
        LoadPlayerData();
    }

    public void Awake()
    {
        if (shared == null)
            shared = this;
        else
            Destroy(this);
    }

    public void SavePlayerData()
    {
        string serialisedPlayerData = JSON.Serialize(playerData).CreateString();
        File.WriteAllText(filePath, serialisedPlayerData);
    }

    public void LoadPlayerData()
    {
        if (File.Exists(filePath))
        {
            string fileContents = File.ReadAllText(filePath);
            playerData = JSON.ParseString(filePath).Deserialize<PlayerData>();
        }
        else
        {
            playerData = new PlayerData();
            SavePlayerData();
        }
    }
}
