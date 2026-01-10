using System;
using UnityEngine;
using UnityEngine.UIElements;

public class GameUIHandler : MonoBehaviour
{
    private GameObject _player;
    private HealthComponent _healthComponent;
    private UIDocument _uiDoc;
    private Label m_HealthLabel;

    private void Start()
    {
        _uiDoc = GetComponent<UIDocument>();
        _player = GameObject.FindWithTag("Player");
        if (_player)
        {
            _healthComponent = _player.GetComponent<HealthComponent>();
        }
        m_HealthLabel = _uiDoc.rootVisualElement.Q<Label>("HealthLabel");
        
    }

    private void Update()
    {
        HealthChanged();
    }

    private void HealthChanged()
    {
        m_HealthLabel.text = $"{_healthComponent.Health}/{_healthComponent.MaxHealth}";
    }

}