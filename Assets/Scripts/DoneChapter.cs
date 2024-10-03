using UnityEngine;
using UnityEngine.SceneManagement;

public class DoneChapter : MonoBehaviour, ISaveManager
{
    [SerializeField] private int chapterCurrentcy;
    [SerializeField] private bool trigger;

    void Update()
    {
        if (trigger && Input.GetKeyDown(KeyCode.E))
        {
            chapterCurrentcy++;
            SaveManager.instance.SaveGame();
            SceneManager.LoadScene(chapterCurrentcy);
        }
    }

    public void LoadData(GameData _data)
    {
        chapterCurrentcy = _data.chapter;
    }

    public void SaveData(ref GameData _data)
    {
        _data.chapter = chapterCurrentcy;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            trigger = true;
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            trigger = false;
        }
    }
}