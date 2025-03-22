using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    [SerializeField] private Image _upgradeImage;
    [SerializeField] private TMP_Text _costText;
    [SerializeField] private Button _upgradeButton;
    [SerializeField] private Button _DestroyButton;
    
    private float _upgradeCost;
    private Unit_Friendly _unitToUpgrade;

    private void Start()
    {
        _upgradeButton.onClick.AddListener(OnUpgradeButtonClicked);
        _DestroyButton.onClick.AddListener(OnDestroyClicked);
    }

    public void SetUpgradeCard(Unit_Friendly unitToUpgrade)
    {
        _unitToUpgrade = unitToUpgrade;
        _upgradeImage.sprite = unitToUpgrade.GetSprite();
        _upgradeCost = unitToUpgrade.GetCost();
        _costText.text = _upgradeCost.ToString();
    }

    private void OnUpgradeButtonClicked()
    {
        if (GameManager.instance.SpendMoney(_unitToUpgrade.GetCost()))
        {
            _unitToUpgrade.Upgrade();
        }
    }

    private void OnDestroyClicked()
    {
        _unitToUpgrade.DeleteUnit();
    }
}
