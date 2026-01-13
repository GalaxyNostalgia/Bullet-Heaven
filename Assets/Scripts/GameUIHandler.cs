using System;
using UnityEngine;
using UnityEngine.UIElements;

public class GameUIHandler : MonoBehaviour
{
    private GameObject _player;
    private HealthComponent _healthComponent;
    private UIDocument UIDoc;
    private Label m_HealthLabel;
    private VisualElement m_HealthBarMask;
    private Label m_ScoreNumber;

    private void Start()
    {
        UIDoc = GetComponent<UIDocument>();
        _player = GameObject.FindWithTag("Player");
        if (_player)
        {
            _healthComponent = _player.GetComponent<HealthComponent>();
        }
        m_HealthLabel = UIDoc.rootVisualElement.Q<Label>("HealthLabel");
        m_HealthBarMask = UIDoc.rootVisualElement.Q<VisualElement>("HealthBarMask");
        m_ScoreNumber = UIDoc.rootVisualElement.Q<Label>("ScoreNumber");
        
    }

    private void Update()
    {
        HealthChanged();
        CheckScore();
    }

    private void HealthChanged()
    {
        m_HealthLabel.text = $"{_healthComponent.Health}/{_healthComponent.MaxHealth}";
        float healthRatio = (float)_healthComponent.Health /_healthComponent.MaxHealth;
        float healthPercent = Mathf.Lerp(6, 66, healthRatio);
        m_HealthBarMask.style.width = Length.Percent(healthPercent);
    }

    private void CheckScore()
    {
        m_ScoreNumber.text = $"{HealthComponent.ScoreStatic}";
    }

}