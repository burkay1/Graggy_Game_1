using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public GameObject settingsPanel;

    public void OnPlayButton()
    {
        Debug.Log("Burda ilk sahneye atmasý lazým");
        SceneManager.LoadScene("Scene_1");
    }

    public void OnSettingsButton()
    {
        Debug.Log("Settings panelinin açýlmasý lazým");
        settingsPanel.SetActive(true);
    }

    public void CloseSettingsPanel()
    {
        settingsPanel.SetActive(false);
    }

    public void OnExitButton()
    {
        Debug.Log("Oyundan çýkýlýyor");
        Application.Quit();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && settingsPanel.activeSelf)
        {
            CloseSettingsPanel();
        }
    }
}
