using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveManager
{
    public static GameManager instance;

    [SerializeField] private Checkpoint[] checkpoints;
    [SerializeField] private string closestCheckpointId;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            RestartScene();
    }

    private void Start()
    {
        checkpoints = FindObjectsOfType<Checkpoint>();
    }

    public void RestartScene()
    {
        SaveManager.instance.SaveGame();
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void LoadData(GameData _data)
    {
        //Debug.Log(1);
        foreach (KeyValuePair<string, bool> pair in _data.checkpoints)
        {
            foreach (Checkpoint checkpoint in checkpoints)
            {
                if (checkpoint.id == pair.Key && pair.Value == true)
                    checkpoint.ActivateCheckpoint();
            }
        }

        closestCheckpointId = _data.closestCheckpointId;//-----------------------------------------
        Invoke("PlacePlayerAtClosestCheckpoint", .1f);
    }

    private void PlacePlayerAtClosestCheckpoint()
    {
        foreach (Checkpoint checkpointToSpawn in checkpoints)
        {
            if (closestCheckpointId == checkpointToSpawn.id)
                PlayerManager.instance.player.transform.position = checkpointToSpawn.transform.position;
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.closestCheckpointId = FindClosestCheckpoint().id;
        _data.checkpoints.Clear();

        foreach (Checkpoint checkpoint in checkpoints)
        {
            _data.checkpoints.Add(checkpoint.id, checkpoint.activationStatus);
        }
    }

    private Checkpoint FindClosestCheckpoint()
    {
        //tim khoang cach toi check poin gan nhat
        float closestDistance = Mathf.Infinity;
        Checkpoint closestCheckpoint = null;

        foreach (var checkpoint in checkpoints)
        {
            float distanceToCheckpoin = Vector2.Distance(PlayerManager.instance.player.transform.position,checkpoint.transform.position);

            if (distanceToCheckpoin < closestDistance && checkpoint.activationStatus == true)
            {
                closestDistance = distanceToCheckpoin;
                closestCheckpoint = checkpoint;
            }
        }

        return closestCheckpoint;
    }
}
