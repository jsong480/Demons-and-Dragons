using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeChaseState : BaseState
{
    private Attack attack;
    private Vector3 target;
    private Vector3 moveDir;
    private bool isAttack;
    private float attackRateCounter = 0;
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
        attack = enemy.GetComponent<Attack>();

        currentEnemy.lostTargetTimeCounter = currentEnemy.lostTargetTime;
        currentEnemy.anim.SetBool("chase", true);
    }
    public override void LogicUpdate()
    {
        if (currentEnemy.lostTargetTimeCounter <= 0)
            currentEnemy.SwitchState(NPCState.Patrol);

        attackRateCounter -= Time.deltaTime;
        target = new Vector3(currentEnemy.attacker.position.x, currentEnemy.attacker.position.y + 0.7f, 0);

        if (Mathf.Abs(target.x - currentEnemy.transform.position.x) <= attack.attackRange && Mathf.Abs(target.y - currentEnemy.transform.position.y) <= attack.attackRange) 
        {
            isAttack = true;
            if (!currentEnemy.isHurt)
            {
                currentEnemy.rb.velocity = Vector2.zero;
            }

            
            if (attackRateCounter <= 0)
            {
                currentEnemy.anim.SetTrigger("attack");
                attackRateCounter = attack.attackRate;
            }
        }
        else
        {
            isAttack = false;
        }

        moveDir = (target - currentEnemy.transform.position).normalized;

        if (moveDir.x > 0)
        {
            currentEnemy.transform.localScale = new Vector3(-1, 1, 1);
        }
        if (moveDir.x < 0)
        {
            currentEnemy.transform.localScale = new Vector3(1, 1, 1);
        }
    }


    public override void PhysicsUpdate()
    {
        if (!currentEnemy.isHurt && !currentEnemy.isDead && !isAttack)
        {
            currentEnemy.rb.velocity = moveDir * currentEnemy.currentSpeed * Time.deltaTime;
        }
    }
    public override void OnExit()
    {
        currentEnemy.anim.SetBool("chase", false);
    }

}
