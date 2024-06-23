using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    [SerializeField] private string filename;

    private GameData gameData;
    private List<ISaveManager> saveManager;
    private FileDataHandler dataHandler;

    [ContextMenu("Delete save file")]
    private void DeleteSavedData()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath,filename);
        dataHandler.Delete();
    }

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, filename);
        saveManager = FindAllSaveManager();
        LoadGame();
    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        //game data = data from data handler
        gameData = dataHandler.Load();
        //game data = data from data handler

        if (this.gameData == null)
        {
            Debug.Log("No saved data found !");
            NewGame();
        }

        foreach (ISaveManager saveManager in saveManager)
        {
            saveManager.LoadData(gameData);
        }
        //Debug.Log("Loaded currency " + gameData.currency);
    }

    public void SaveGame()
    {
        //data handler save gameData
        //Debug.Log("Game was saved !");

        foreach (ISaveManager saveManager in saveManager)
        {
            saveManager.SaveData(ref gameData);
        }
        dataHandler.Save(gameData);
        //Debug.Log("Saved currency " + gameData.currency);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    //lay danh sach tat ca ISaveManager
    private List<ISaveManager> FindAllSaveManager()
    {
        IEnumerable<ISaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();

        return new List<ISaveManager>(saveManagers);
    }

}
