using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] AudioClip audioClip;
    
    private UIDocument _uiDocument;
    private Button _playButton;
    private Button _settingsButton;
    private Button _creditsButton;
    
    void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
        if (!_uiDocument)
        {
            Debug.LogError("No UIDocument found on MainMenuManager.");
        }

        _playButton = _uiDocument.rootVisualElement.Q<Button>("Play_Button");
        _playButton.RegisterCallback<ClickEvent>(LoadGameScene);
        
        _settingsButton = _uiDocument.rootVisualElement.Q<Button>("Settings_Button");
        _settingsButton.RegisterCallback<ClickEvent>(ShowSettingsUI);
        
        _creditsButton = _uiDocument.rootVisualElement.Q<Button>("Credits_Button");
        _creditsButton.RegisterCallback<ClickEvent>(ShowCreditsUI);
    }

    private void Start()
    {
        SoundManager.Instance.ChangeMusic(audioClip);
    }

    private void LoadGameScene(ClickEvent e)
    {
        SceneManager.LoadScene("Game");
    }
    
    private void ShowSettingsUI(ClickEvent evt)
    {
        SettingsUIManager.Instance.SetVisible();
    }
    
    private void ShowCreditsUI(ClickEvent evt)
    {
        CreditsUIManager.Instance.SetVisible();
    }

}
