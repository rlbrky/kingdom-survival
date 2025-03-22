using UnityEngine;

public class AutoTarget : MonoBehaviour
{
    public LayerMask whatIsEnemy;
    public int fireFrameSetup;
    //public AudioSource firingSFX;

    [Header("Setup")] 
    [SerializeField] private float range;
    [SerializeField] private float fireRate;
    
    [Header("Ammo")]
    [SerializeField] private Transform firingPoint;
    [SerializeField] public GameObject _bullet;

    private float firingCounter = 0f;

    private void Update()
    {
        Attack();
    }

    private Collider CheckForEnemies()
    {
        Collider closestEnemy = null;
        Collider[] colliders = Physics.OverlapSphere(transform.position, range, whatIsEnemy);
        foreach (Collider collider in colliders)
        {
            if (closestEnemy == null)
            {
                closestEnemy = collider;
                continue;
            }
            if (Vector3.Distance(collider.transform.position, transform.position) <= Vector3.Distance(closestEnemy.transform.position, transform.position))
            {
                closestEnemy = collider;
            }
            else
                continue;
        }
        return closestEnemy;
    }

    private void Attack()
    {
        Collider enemy = null;
        if (Time.frameCount % fireFrameSetup == 0)
            enemy = CheckForEnemies();

        if (enemy && firingCounter >= fireRate)
        {
            firingCounter = 0f;
            transform.LookAt(new Vector3(enemy.transform.position.x, enemy.transform.position.y, enemy.transform.position.z));
            var bullet = Instantiate(_bullet, firingPoint.position, Quaternion.identity);
            bullet.transform.forward = transform.forward;
            //firingSFX.Play();
        }
        else
        {
            firingCounter += Time.deltaTime;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
