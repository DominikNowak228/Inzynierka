using System;
using UnityEngine;
using System.Collections;

public class ManaSystem : Singleton<ManaSystem>
{
    [SerializeField] private ManaUI manaUI;
    private PlayerStats _playerStats;

    private void OnEnable()
    {
        ActionSystem.AttachPerformer<SpendManaGA>(SpendManaPerformer);
    }
    private void OnDisable()
    {
        ActionSystem.DetachPerformer<SpendManaGA>();
    }

    private void Update()
    {
        if (_playerStats != null)
        {
            _playerStats.RegenerateMana();
            manaUI.UpdateManaBar(_playerStats.GetCurrentMana(), _playerStats.MaxMana);
        }
    }
    public void Setup(PlayerStats playerStats)
    {
        _playerStats = playerStats;
    }

    public bool HasEnoughMana(int amount)
    {
        if (_playerStats == null) return false;
        return _playerStats.HasEnoughMana(amount);
    }

    public IEnumerator SpendManaPerformer(SpendManaGA spendMana)
    {
        int spendAmount = spendMana.Amount;
        _playerStats.UseMana(spendAmount);
        yield return null;
    }
}

public class PlayerStats
{
    public int MaxMana { get; private set; } = 10;

    private float _currentMana;
    private float _manaRegenRate;

    public PlayerStats(int maxMana, float manaRegenRate = 1f)
    {
        MaxMana = maxMana;
        _currentMana = (float)maxMana / 2;

        _manaRegenRate = manaRegenRate;
    }

    // Call this method to check if there's enough mana
    public bool HasEnoughMana(int amount)
    {
        return Mathf.Floor(_currentMana) >= amount;
    }
    // Call this method to use mana
    public void UseMana(int amount)
    {
        _currentMana -= amount;
        if (_currentMana < 0) _currentMana = 0;
    }

    // Call this method in Update() to regenerate mana over time
    public void RegenerateMana()
    {
        if (_currentMana >= MaxMana) return;
        _currentMana += Time.deltaTime * _manaRegenRate;
    }

    // Returns mana percentage between 0 and 1
    public float GetManaPercentage()
    {
        return _currentMana / MaxMana;
    }

    public float GetCurrentMana()
    {
        return _currentMana;
    }


}