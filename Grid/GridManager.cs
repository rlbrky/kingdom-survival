using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int _widthCount, _heightCount;
    [SerializeField] private Tile _originalTilePrefab;
    [SerializeField] private Tile _offsetTilePrefab;
    
    private float _posY = 0;
    
    private Dictionary<Vector2, Tile> _tileDictionary;
    private void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        _tileDictionary = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < _heightCount; x++)
        {
            float posX = 0;
            for (int y = 0; y < _widthCount; y++)
            {
                if ((x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0)) //Is Offset ?
                {
                    var spawnedTile = Instantiate(_offsetTilePrefab, new Vector3(posX, 0, _posY), Quaternion.identity);
                    spawnedTile.name = $"Tile_{x}_{y}";
                    _tileDictionary[new Vector2(x, y)] = spawnedTile;
                }
                else
                {
                    var spawnedTile = Instantiate(_originalTilePrefab, new Vector3(posX, 0, _posY), Quaternion.identity);
                    spawnedTile.name = $"Tile_{x}_{y}";
                    _tileDictionary[new Vector2(x, y)] = spawnedTile;
                }
                posX += _offsetTilePrefab.transform.localScale.x;
            }
            _posY += _offsetTilePrefab.transform.localScale.z;
        }
    }

    public Tile GetTile(Vector2 position)
    {
        if (_tileDictionary.TryGetValue(position, out Tile tile))
        {
            return tile;
        }

        return null;
    }
}
