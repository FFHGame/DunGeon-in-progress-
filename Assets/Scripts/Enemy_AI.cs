using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AI : MonoBehaviour
{
    public Transform target;
    float attackDelay;

    Pig_Enemy enemy;
    Animator enemyAnimator;
    void Start()
    {
        enemy = GetComponent<Pig_Enemy>();
        enemyAnimator = enemy.enemyAnimator;
    }

    void Update()
    {
        attackDelay -= Time.deltaTime;
        if (attackDelay < 0) attackDelay = 0;
        //타겟 거리확인
        float distance = Vector3.Distance(transform.position, target.position);

        //공격딜레이 없을시, 시야 범위안에 존재여부
        if (attackDelay == 0 && distance <= enemy.fieldOfVision)
        {
            FaceTarget();//타겟 바라보기

            if (distance <= enemy.atkRange)
            {
                AttackTarget();
            }
            else//공격 실행중이 아닐때 이동
            {
                if (!enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                {
                    MoveToTarget();
                }
            }
        }
        else
        {
            enemyAnimator.SetBool("moving", false);
        }
    }

    void MoveToTarget()
    {
        float dir = target.position.x - transform.position.x;
        dir = (dir < 0) ? -1 : 1;
        transform.Translate(new Vector2(dir, 0) * enemy.moveSpeed * Time.deltaTime);
        enemyAnimator.SetBool("moving", true);
    }

    void FaceTarget()
    {
        if (target.position.x - transform.position.x < 0) // 타겟이 왼쪽에 있을 때
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else // 타겟이 오른쪽에 있을 때
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void AttackTarget()
    {
        target.GetComponent<SwordMan>().nowHp -= enemy.atkDmg;
        enemyAnimator.SetTrigger("attack"); // 공격 애니메이션 실행
        attackDelay = enemy.atkSpeed; // 딜레이 충전
    }
}
