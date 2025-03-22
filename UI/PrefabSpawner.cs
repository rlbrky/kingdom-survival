using UnityEngine;
using UnityEngine.UI;

public class PrefabSpawner : MonoBehaviour
{
    [SerializeField] GameObject prefab;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClickCall);
    }

    private void OnClickCall()
    {
        Player_Build.instance.SpawnPrefab(prefab);
    }
}
