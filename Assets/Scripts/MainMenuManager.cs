using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuManager : MonoBehaviour
{

    private UIDocument _uiDocument;
    private Button _playButton;
    private Button _settingsButton;
    private Button _creditsButton;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
        if (!_uiDocument)
        {
            Debug.LogError("No UIDocument found on MainMenuManager.");
        }

        _playButton = _uiDocument.rootVisualElement.Q<Button>("Play_Button");
        _settingsButton = _uiDocument.rootVisualElement.Q<Button>("Settings_Button");
        _creditsButton = _uiDocument.rootVisualElement.Q<Button>("Credits_Button");
        
        _playButton.RegisterCallback<ClickEvent>(LoadGameScene);
    }

    private void LoadGameScene(ClickEvent e)
    {
        SceneManager.LoadScene("Game");
    }
}
