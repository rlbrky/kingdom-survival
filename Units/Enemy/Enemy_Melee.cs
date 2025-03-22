using UnityEngine;

public class Enemy_Melee : Unit_Enemy
{
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
                
                if(_attackCounter >= attackCD && CheckAttackRange())
                {
                    Attack();
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
        if (_deathRoutine != null)
            return;
        
        switch (_activeState)
        {
            case State.Default:
                _animator.SetBool(_idle, false);
                Move(transform.position + new Vector3(-1, 0, 0));
                if (_target == null && CheckForEnemies(out _enemyCol))
                {
                    if (_enemyCol.TryGetComponent(out Unit_Friendly playerUnit))
                    {
                        _target = playerUnit;
                    }
                    else if (_enemyCol.TryGetComponent(out Nexus_Player playerNexus))
                    {
                        _target = playerNexus;
                    }
                    //Determine if the right or left edge is closer to this unit.
                    targetEdgeX = transform.position.x < _enemyCol.bounds.center.x
                            ? _enemyCol.bounds.min.x
                            : _enemyCol.bounds.max.x;
                }
                break;
            
            case State.Attacking:
            {
                if (_enemyCol != null)
                {
                    // Determine the distance from our position to the target edge.
                    float distanceToEdge = Mathf.Abs(transform.position.x - targetEdgeX);

                    // If we are farther than the desired attack distance (using a threshold), keep moving.
                    if (distanceToEdge > attackRange)
                    {
                        _animator.SetBool(_idle, false);
                        // Move toward the target edge.
                        Move(new Vector3(targetEdgeX, transform.position.y, transform.position.z));
                    }
                    else
                    {
                        // We're close enough to the edge, so stop moving.
                        _animator.SetBool(_idle, true);
                    }
                }
                break;
            }
        }
    }
}
