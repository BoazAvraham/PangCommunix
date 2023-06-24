using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUIManager : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private Slider volumeSlider; 
    [SerializeField] private TMP_Text highscoreText; 
    private void Start()
    {
        settingsPanel.SetActive(false);
        highscoreText.text = "High score: " + GameManager.Instance.GetHighScore();
    }
    
    public void OnPlayClicked()
    {
        SceneManager.LoadScene(1);
    }
    
    
    public void OnSettingsClicked()
    {
        volumeSlider.SetValueWithoutNotify(SoundManager.Instance.GetVolume());
        settingsPanel.SetActive(true);
    }

    public void OnClosePanel()
    {
        GameManager.Instance.SaveVolumeSettins(volumeSlider.value);
        settingsPanel.SetActive(false);
    }
}
