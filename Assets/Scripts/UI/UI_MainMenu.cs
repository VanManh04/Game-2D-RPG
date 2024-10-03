using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour,ISaveManager
{
    [SerializeField] private string sceneName = "MainScene";
    [SerializeField] private GameObject continueButton;
    [SerializeField] UI_FadeScreen fadeScreen;

    [SerializeField] private int sceneLoad;
    private void Start()
    {
        if (SaveManager.instance.HasSavedData() == false)
            continueButton.SetActive(false);
    }

    public void ContinueGame()
    {
        StartCoroutine(LoadSceneWithFadeEffect(1.5f,sceneLoad));
    }

    public void NewGame()
    {
        StartCoroutine(LoadSceneWithFadeEffect(1.5f,1));
        SaveManager.instance.DeleteSavedData();
    }

    public void ExitGame()
    {
        Debug.Log("Exit game");
        //Application.Quit();
    }

    IEnumerator LoadSceneWithFadeEffect(float _delay,int sceneNumber)
    {
        fadeScreen.FadeOut();

        yield return new WaitForSeconds(_delay);

        SceneManager.LoadScene(sceneNumber);
    }

    public void LoadData(GameData _data)
    {
        sceneLoad = _data.chapter;
    }

    public void SaveData(ref GameData _data)
    {
    }
}
