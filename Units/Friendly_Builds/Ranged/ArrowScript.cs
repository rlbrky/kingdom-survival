using System.Collections;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private float arrowLifeTime = 3f;
    
    private Vector3 _travelDirection;
    private float _damage;
    
    public void SetDamage(float damage)
    {
        _damage = damage;
    }

    public void SetArrow(Vector3 direction)
    {
        _travelDirection = direction;
    }

    private IEnumerator ResetArrow()
    {
        yield return new WaitForSeconds(arrowLifeTime);
        Destroy(gameObject);
    }

    private void OnEnable()
    {
        StartCoroutine(ResetArrow());
    }

    private void Update()
    {
        transform.Translate(_travelDirection * (speed * Time.deltaTime));
    }

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<UnitBaseClass>().GetHit(_damage);   
        Destroy(gameObject);
    }
}
