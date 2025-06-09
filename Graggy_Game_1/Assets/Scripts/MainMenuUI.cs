using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public GameObject settingsPanel;

    public void OnPlayButton()
    {
        Debug.Log("Burda ilk sahneye atmas� laz�m");
        SceneManager.LoadScene("Scene_1");
    }

    public void OnSettingsButton()
    {
        Debug.Log("Settings panelinin a��lmas� laz�m");
        settingsPanel.SetActive(true);
    }

    public void CloseSettingsPanel()
    {
        settingsPanel.SetActive(false);
    }

    public void OnExitButton()
    {
        Debug.Log("Oyundan ��k�l�yor");
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
