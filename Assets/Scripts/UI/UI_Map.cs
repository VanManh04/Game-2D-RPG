using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Map : MonoBehaviour
{
    [SerializeField] private string sceneNameMap1 = "MainMap1";
    [SerializeField] private string sceneNameMap2 = "MainMap2";
    [SerializeField] private GameObject continueButton;
    [SerializeField] UI_FadeScreen fadeScreen;

    private void Start()
    {
        if (SaveManager.instance.HasSavedData() == false)
            continueButton.SetActive(false);
    }

    public void ContinueGame()
    {
        StartCoroutine(LoadSceneWithFadeEffect(1.5f));
    }

    public void NewGame()
    {
        StartCoroutine(LoadSceneWithFadeEffect(1.5f));
        SaveManager.instance.DeleteSavedData();
    }

    public void ExitGame()
    {
        Debug.Log("Exit game");
        //Application.Quit();
    }

    IEnumerator LoadSceneWithFadeEffect(float _delay)
    {
        fadeScreen.FadeOut();

        yield return new WaitForSeconds(_delay);

        //SceneManager.LoadScene(sceneName);
    }
}
