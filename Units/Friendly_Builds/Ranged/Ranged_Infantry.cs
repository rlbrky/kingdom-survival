using UnityEngine;

public class Ranged_Infantry : Unit_Friendly
{
    [Header("Ranged Properties")]
    [SerializeField] private ArrowScript arrowPrefab;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private Vector3 arrowDirection;
    
    protected override void Awake()
    {
        base.Awake();
        _animator.SetBool(_idle, true);
    }
    
    public void FireArrow() //This function is an Animation Event.
    {
        ArrowScript arrow = Instantiate(arrowPrefab, firingPoint.position, Quaternion.identity);
        if (Mathf.Approximately(transform.rotation.eulerAngles.y, 90f))
        {
            arrow.transform.rotation = Quaternion.Euler(new Vector3(90, -90, 0));
        }
        else
        {
            arrow.transform.rotation = Quaternion.Euler(new Vector3(90, 90, 0));
        }
        arrow.SetArrow(-transform.up.normalized); 
        arrow.SetDamage(damage);
    }
    
     private void Update()
    {
        if (_deathRoutine != null)
            return;
        
        if (_target != null)
        {
            _activeState = State.Attacking;
        }
        
        switch (_activeState)
        {
            case State.Attacking:
                if (_attackCounter < attackCD)
                {
                    _attackCounter += Time.deltaTime;
                }
                
                if(_target != null && _attackCounter >= attackCD && Mathf.Abs(transform.position.x - _target.transform.position.x) <= attackRange)
                {
                    transform.LookAt(new Vector3(_target.transform.position.x, transform.position.y, transform.position.z));
                    _animator.SetTrigger(_attack);
                    _attackCounter = 0;
                }
                else if(_target == null)
                {
                    _activeState = State.Default;
                }
                break;
            case State.Damaged:
                if (_animator.GetCurrentAnimatorStateInfo(0).IsTag("Damaged"))
                {
                    _attackCounter += Time.deltaTime;
                }
                break;
        }
    }

    private void FixedUpdate()
    {
        switch (_activeState)
        {
            case State.Default:
                if (_target == null && CheckForEnemies(out _enemyCol))
                {
                    _target = _enemyCol.GetComponent<UnitBaseClass>();
                }
                break;
            
            case State.Attacking:                
                if (_target != null && Mathf.Abs(transform.position.x - _target.transform.position.x) > attackRange)
                {
                    if(!CheckForEnemies(out _enemyCol)) //Couldn't find collider.
                        _target = null;
                    else
                    {
                        _animator.SetBool(_idle, false);
                        Move(_target.transform.position);
                    }
                }
                break;
        }
    }
}
