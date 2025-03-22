using DG.Tweening;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    [SerializeField] private float startingMoney = 100f;
    //[SerializeField] private float ;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private GameObject _cantSpendMoney_UI;
    
    private float currentMoney;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        _cantSpendMoney_UI.SetActive(false);
    }

    private void Start()
    {
        currentMoney = startingMoney;
        moneyText.text = currentMoney.ToString();
    }

    public bool SpendMoney(float amount)
    {
        if (currentMoney - amount >= 0)
        {
            currentMoney -= amount;
            moneyText.text = currentMoney.ToString();
            return true;
        }
        
        PlayCantSpendMoneyAnimation();
        return false;
    }

    private void PlayCantSpendMoneyAnimation()
    {
        _cantSpendMoney_UI.SetActive(true);
        _cantSpendMoney_UI.transform.DOMoveY(_cantSpendMoney_UI.transform.position.y + 50f, 1f)
            .SetEase(Ease.InOutSine)
            .onComplete += () =>
        {
            _cantSpendMoney_UI.transform.localPosition = new Vector3(0, 0, 0);
            _cantSpendMoney_UI.SetActive(false);
        };
    }
}
