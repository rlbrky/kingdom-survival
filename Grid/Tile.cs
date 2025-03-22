using DG.Tweening;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private static readonly int Ambient = Shader.PropertyToID("_Ambient");
    
    [SerializeField] private float highlightDuration = 1f, targetAmbientValue = 2;
    
    private float _startAmbientValue;
    private MeshRenderer _meshRenderer;
    private Tweener _tweener;
    
    private Unit_Friendly _friendly = null;

    public Unit_Friendly Unit
    {
        get => _friendly;
        set => _friendly = value;
    }
    
    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _startAmbientValue = _meshRenderer.material.GetFloat(Ambient);
    }

    public void TileClicked()
    {
        if (_friendly == null)
        {
            Player_Build.instance.UpdateSpawnLocation(this);
            Player_Build.instance.OpenBuildUI();
        }
        else
        {
            Player_Build.instance.UpdateSpawnLocation(this);
            Player_Build.instance.OpenUpgradeUI(_friendly);
        }
    }

    public void Deactivate()
    {
        _tweener.Kill();
        _meshRenderer.material.SetFloat(Ambient, _startAmbientValue);
    }
    
    public void HighlightTileLoop() //Visual effect for tile highlight.
    {
        _tweener = _meshRenderer.material.DOFloat(targetAmbientValue, Ambient, highlightDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }
}
