
using UnityEngine;

public class Unit_Friendly : UnitBaseClass
{
    private Tile _startedTile = null;
    
    public Tile StartedTile
    {
        get => _startedTile; 
        set => _startedTile = value;
        
    }
    
    [Header("Pricing")]
    [SerializeField] protected float unitCost;
    [Header("Upgrade Effects")] 
    [SerializeField] protected Sprite[] uiSprites = new Sprite[2];
    [SerializeField] protected GameObject level1_Look, level2_Look, level3_Look;
    [SerializeField] protected float upgradeCost, damageScaling, upgradeCostScaling;
    
    private int _level = 1;

    protected override void Awake()
    {
        base.Awake();
        transform.rotation = Quaternion.Euler(0, 90, 0);
        _animator = level1_Look.GetComponent<Animator>();
    }
    
    protected void Start()
    {
        _startedTile.Unit = this;
    }
    
    public void DeleteUnit()
    {
        GameManager.instance.SpendMoney(-unitCost / 2); //Refund half this unit's value.
        StartedTile.Unit = null;
        Destroy(gameObject);
    }

    #region PUBLIC FUNCTIONS
    public void Upgrade()
    {
        _level++;
        damage += damageScaling;
        unitCost += upgradeCost; //Update unit total price.
        upgradeCost += upgradeCostScaling; //Update upgrade cost.
        switch (_level)
        {
            case 2:
                level1_Look.SetActive(false);
                level2_Look.SetActive(true);
                _animator = level2_Look.GetComponent<Animator>();
                break;
            case 3:
                level2_Look.SetActive(false);
                level3_Look.SetActive(true);
                _animator = level3_Look.GetComponent<Animator>();
                break;
        }
    }    
    
    public Sprite GetSprite()
    {
        return uiSprites[_level - 1];
    }
    
    public float GetCost()
    {
        return upgradeCost;
    }
    #endregion
}
