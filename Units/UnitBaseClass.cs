using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public abstract class UnitBaseClass : MonoBehaviour
{
    [Header("Defaults")]
    [SerializeField] protected float attackRange;
    [SerializeField] protected float damage;
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float speed;
    [SerializeField] protected float attackCD;
    [SerializeField] protected Slider healthbar;
    //public Image healthFill;  // Assign this in the Inspector

    [Header("Detection")]
    [SerializeField] protected LayerMask whatIsEnemy;
    [SerializeField] protected Vector3 detectionRange;
    [SerializeField] protected Vector3 detectionOffset;

    
    protected float targetEdgeX;
    protected float _currentHealth;
    protected Collider _collider;
    protected Collider _enemyCol;
    protected Coroutine _deathRoutine = null;
    protected UnitBaseClass _target = null;
    protected float _attackCounter = 0;
    
    #region Anim&State Setup

    protected Animator _animator;
    
    //Anim Hashes
    protected readonly int _idle = Animator.StringToHash("Idle");
    protected readonly int _attack = Animator.StringToHash("Attack");
    private readonly int _gotHit = Animator.StringToHash("gotHit");
    private readonly int _die = Animator.StringToHash("DIE");
    
    protected enum State
    {
        Default,
        Attacking,
        Damaged,
    }
    protected State _activeState = State.Default;

    #endregion
    
    private IEnumerator SmoothHealthChange(float targetValue)
    {
        float currentValue = healthbar.value;
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * 5; // Adjust speed
            healthbar.value = Mathf.Lerp(currentValue, targetValue, t);
            yield return null;
        }
    }

    protected virtual void Awake()
    {
        healthbar = GetComponentInChildren<Slider>();
        _collider = GetComponent<Collider>();
        _currentHealth = maxHealth;
        healthbar.maxValue = maxHealth;
        healthbar.value = _currentHealth;
    }

    /// <summary>
    /// Returns true if enemy is in attack range.
    /// </summary>
    protected bool CheckAttackRange()
    {
        if (_target == null) return false;

        if (_enemyCol != null)
        {
            // Determine which edge is relevant.
            targetEdgeX = transform.position.x < _enemyCol.bounds.center.x
                ? _enemyCol.bounds.min.x
                : _enemyCol.bounds.max.x;
        
            // If the distance between our position and the target edge is less than or equal to attackRange, we’re in range.
            return Mathf.Abs(transform.position.x - targetEdgeX) <= attackRange;
        }
        else
        {
            // Fallback: use the target's transform position if no collider exists.
            return Mathf.Abs(transform.position.x - _target.transform.position.x) <= attackRange;
        }
    }
    
    protected bool CheckForEnemies(out Collider closestEnemy)
    {
        closestEnemy = null; // Initialize the out parameter

        Collider[] colliders = Physics.OverlapBox(transform.position + detectionOffset, detectionRange / 2, Quaternion.identity, whatIsEnemy);
    
        foreach (Collider collider in colliders)
        {
            if (closestEnemy == null || 
                Vector3.Distance(collider.transform.position, transform.position) < Vector3.Distance(closestEnemy.transform.position, transform.position))
            {
                closestEnemy = collider;
            }
        }

        return closestEnemy != null; // Return true if an enemy was found
    }

    protected void Move(Vector3 targetPos)
    {
        //float moveTime = Mathf.Abs(transform.position.x - targetPos) / speed;
        transform.LookAt(new Vector3(targetPos.x, transform.position.y, targetPos.z));
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        //transform.DOMoveX(targetPos, moveTime).SetEase(Ease.OutBounce);
    }
    
    protected void Attack()
    {
        _animator.SetBool(_idle, true);
        _animator.SetTrigger(_attack);
        if (!_target.GetHit(damage))
        {
            _target = null;
            _enemyCol = null;
        }
        _attackCounter = 0;
    }
    
    protected void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + detectionOffset, detectionRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    protected virtual IEnumerator DeathCoroutine()
    {
        _animator.SetTrigger(_die);
        Destroy(_collider);
        Destroy(healthbar.gameObject);
        _target = null;
        _enemyCol = null;
        yield return new WaitForSeconds(1f);
        //TODO: PLAY SOME VFX ?
        Destroy(gameObject);
    }
    protected void UpdateHealth(float currentHP)
    {
        StartCoroutine(SmoothHealthChange(currentHP));
    }
    
    #region PUBLIC FUNCTIONS
    public virtual bool GetHit(float incDamage)
    {
        if (_currentHealth > 0)
        {
            _currentHealth -= incDamage;
            UpdateHealth(_currentHealth);
            //healthbar.value = _currentHealth;
            _animator.SetTrigger(_gotHit);
            _activeState = State.Damaged;
            return true;
        }
        else if(_deathRoutine == null)
        {
            _deathRoutine = StartCoroutine(DeathCoroutine());   
            return false;
        }

        return false;
    }

    #endregion
}
