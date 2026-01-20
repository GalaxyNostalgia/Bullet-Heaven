using UnityEngine;
using UnityEngine.UIElements;

public class SettingsUIManager : MonoBehaviour
{
    private static SettingsUIManager _instance;

    public static SettingsUIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("SettingsUIManager is null");
            }

            return _instance;
        }
    }
    
    private UIDocument _uiDocument;
    private VisualElement _root;
    private Slider _volumeSlider;
    private Toggle _muteToggle;
    private Button _okButton;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this;
        
        _uiDocument = GetComponent<UIDocument>();
        if (_uiDocument == null)
        {
            Debug.LogError("No UIDocument found on SettingsUIManager.");
        }
        
        _root = _uiDocument.rootVisualElement.Q<VisualElement>("Root");
        _volumeSlider = _root.Q<Slider>("Volume_Slider");
        _muteToggle = _root.Q<Toggle>("Mute_Toggle");
        _okButton = _root.Q<Button>("OK_Button");

        _volumeSlider.RegisterValueChangedCallback(OnVolumeSliderValueChanged);
        _muteToggle.RegisterValueChangedCallback(OnMuteToggleValueChanged);
        _okButton.RegisterCallback<ClickEvent>(_ => SetInvisible());
        
        SetInvisible();
    }
    
    public void SetVisible()
    {
        _root.RemoveFromClassList("hide");
    }

    private void SetInvisible()
    {
        _root.AddToClassList("hide");
    }

    private void OnVolumeSliderValueChanged(ChangeEvent<float> evt)
    {
        SoundManager.Instance.AdjustSoundVolume(evt.newValue);
    }
    
    private void OnMuteToggleValueChanged(ChangeEvent<bool> evt)
    {
        SoundManager.Instance.ToggleMute(evt.newValue);
    }
}
