using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class DeathScreenUIManager : MonoBehaviour
{
    
    private UIDocument _uiDocument;
    private Button _resetButton;
    private Button _mainMenuButton;
    
    void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
        if (!_uiDocument)
        {
            Debug.LogError("No UIDocument found on MainMenuManager.");
        }

        _resetButton = _uiDocument.rootVisualElement.Q<Button>("RestartButton");
        _resetButton.RegisterCallback<ClickEvent>(LoadGameScene);
        
        _mainMenuButton = _uiDocument.rootVisualElement.Q<Button>("MainMenuButton");
        _mainMenuButton.RegisterCallback<ClickEvent>(LoadMainMenuScene);
        
        
    }

    private void LoadGameScene(ClickEvent e)
    {
        SceneManager.LoadScene("Game");
    }
    
    private void LoadMainMenuScene(ClickEvent e)
    {
        SceneManager.LoadScene("MainMenu");
    }
    
}
