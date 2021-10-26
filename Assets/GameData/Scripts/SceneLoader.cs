using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private string next_stage;
    private bool ispushed = false;

    public void Move_to_MenuScene()
    {
        if (ispushed) return;
        ispushed = true;
        SceneManager.LoadScene("Menu");
    }

    public void Move_to_RuleScene()
    {
        if (ispushed) return;
        ispushed = true;
        SceneManager.LoadScene("Rule");
    }

    public void Move_to_SettingScene()
    {
        if (ispushed) return;
        ispushed = true;
        SceneManager.LoadScene("Setting");
    }

    public void Move_to_GameScene(string stage_name)
    {
        if (ispushed) return;
        ispushed = true;
        next_stage = stage_name;
        SceneManager.sceneLoaded += GameSceneLoaded;
        SceneManager.LoadScene("GamePlay");

    }

    private void GameSceneLoaded(Scene next, LoadSceneMode mode)
    {
        var gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        gameManager.Stage = next_stage;
        SceneManager.sceneLoaded -= GameSceneLoaded;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
