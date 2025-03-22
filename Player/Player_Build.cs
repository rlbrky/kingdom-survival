using DG.Tweening;
using UnityEngine;

public class Player_Build : MonoBehaviour
{
    public static Player_Build instance;
    
    [SerializeField] private GameObject _buildUI;
    [SerializeField] private Upgrade _upgradeUI;
    
    private Tile _selectedTile;
    
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
        
        _buildUI.SetActive(false);
        _upgradeUI.gameObject.SetActive(false);
    }

    public void OpenBuildUI()
    {
        if (_upgradeUI.gameObject.activeInHierarchy)
        {
            _upgradeUI.transform.DOMoveY(_upgradeUI.transform.position.y - 300f, .5f)
                .SetEase(Ease.InOutSine)
                .onComplete += () =>
            {
                _upgradeUI.gameObject.SetActive(false);
                Open_BuildUI();
            };
        }
        else
        {
            Open_BuildUI();
        }
    }
    
    public void OpenUpgradeUI(Unit_Friendly unit)
    {
        if (_buildUI.activeInHierarchy)
        {
            _buildUI.transform.DOMoveY(_buildUI.transform.position.y - 300f, .5f)
                .SetEase(Ease.InOutSine)
                .onComplete += () =>
            {
                _buildUI.SetActive(false);
                Open_UpgradeUI(unit);
            };
        }
        else
        {
            Open_UpgradeUI(unit);
        }
    }

    public void CloseUI()
    {
        if (_buildUI.activeInHierarchy)
        {
            _selectedTile.Deactivate();
            
            _buildUI.transform.DOMoveY(_buildUI.transform.position.y - 300f, .5f)
                .SetEase(Ease.InOutSine)
                .onComplete += () =>
            {
                _buildUI.transform.localPosition = new Vector3(0, -700);
                _buildUI.SetActive(false);
            };
        }

        if (_upgradeUI.gameObject.activeInHierarchy)
        {
            _selectedTile.Deactivate();
            
            _upgradeUI.transform.DOMoveY(_upgradeUI.transform.position.y - 300f, .5f)
                .SetEase(Ease.InOutSine)
                .onComplete += () =>
            {
                _upgradeUI.transform.localPosition = new Vector3(0, -700f);
                _upgradeUI.gameObject.SetActive(false);
            };
        }
    }
    
    public void SpawnPrefab(GameObject prefab)
    {
        Unit_Friendly obj = Instantiate(prefab, _selectedTile.transform.position, Quaternion.identity).GetComponent<Unit_Friendly>();
        obj.StartedTile = _selectedTile;
        _selectedTile.Deactivate();
        _selectedTile = null;
    }
    
    /// <summary>
    /// Checks if the current tile matches the previous tile. If not updates the tile.
    /// </summary>
    /// <param name="selectedTile"></param>
    /// <returns></returns>
    public void UpdateSpawnLocation(Tile selectedTile)
    {
        if (CompareSelectedTile(selectedTile))
        {
            if (_selectedTile != null)
            {
                _selectedTile.Deactivate();
            }
            
            _selectedTile = selectedTile;
            _selectedTile.HighlightTileLoop();
        }
        else
        {
            if (_selectedTile != null)
            {
                _selectedTile.Deactivate();
                _selectedTile.HighlightTileLoop();
            }
        }
    }

    private void Open_UpgradeUI(Unit_Friendly unit)
    {
        if (!_upgradeUI.gameObject.activeInHierarchy)
        {
            _upgradeUI.gameObject.SetActive(true);
            _upgradeUI.transform.DOMoveY(_upgradeUI.transform.position.y + 300, 0.5f)
                .SetEase(Ease.InOutSine)
                .onComplete += () =>
            {
                _upgradeUI.transform.localPosition = new Vector3(0, -400);
            };;
            _upgradeUI.SetUpgradeCard(unit);
        }
    }

    private void Open_BuildUI()
    {
        if (!_buildUI.activeInHierarchy)
        {
            _buildUI.gameObject.SetActive(true);
            _buildUI.transform.DOMoveY(_buildUI.transform.position.y + 300f, 0.5f)
                .SetEase(Ease.InOutSine)
                .onComplete += () =>
            {
                _buildUI.transform.localPosition = new Vector3(0, -400);
            };
        }
    }
    
    private bool CompareSelectedTile(Tile selectedTile)
    {
        return !(_selectedTile == selectedTile);
    }
}
