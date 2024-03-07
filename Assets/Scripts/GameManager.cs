using System.IO;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Material CurrentSkin;
    public string PlayerName;
    public string BestPlayerName;
    public int BestScore = 0;
    public int Score = 0;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadData();
    }

    [System.Serializable]
    private class SaveData
    {
        public string Name;
        public int Score;
    }

    public void SavePlayer()
    {
        if (Score > BestScore)
        {
            SaveData data = new SaveData();
            data.Name = PlayerName;
            data.Score = Score;
            string json = JsonUtility.ToJson(data);
            File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
        }
    }

    private void LoadData()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            BestPlayerName = data.Name;
            BestScore = data.Score;
        }
    }
}
