using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Pig_Enemy : MonoBehaviour
{
    public GameObject prfHpBar;
    public GameObject canvas;
    

    public string enemyName;
    public int maxHp;
    public int nowHp;
    public int atkDmg;

    public float atkSpeed;
    public float moveSpeed;
    public float atkRange;
    public float fieldOfVision;

    public Animator enemyAnimator;

    public SwordMan sword_man;
    Image nowHpbar;

    RectTransform hpBar;

    public float height = 1;

    private void SetEnemyStatus(string _enemyName, int _maxHp, int _atkDmg, float _atkSpeed, float _moveSpeed, float _atkRange, float _fieldOfVision)//각 적별로 정보기입용 함수
    {
	    enemyName = _enemyName;
	    maxHp = _maxHp;
	    nowHp = _maxHp;
	    atkDmg = _atkDmg;
	    atkSpeed = _atkSpeed;
        moveSpeed = _moveSpeed;
        atkRange = _atkRange;
        fieldOfVision = _fieldOfVision;
    }

    // Start is called before the first frame update
    void Start()
    {
        hpBar = Instantiate(prfHpBar, canvas.transform).GetComponent<RectTransform>();
        if(name.Equals("Enemy1"))
        {
            SetEnemyStatus("Enemy1",100,10,3f, 2, 1.7f, 4f);
        }
        nowHpbar = hpBar.transform.GetChild(0).GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 _hpBarPos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x-1, transform.position.y + height, 0));//체력바 위치조정
        hpBar.position = _hpBarPos;
        nowHpbar.fillAmount = (float)nowHp/(float)maxHp;//현재체력 조정
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if (sword_man.attacked)
            {
                nowHp -= sword_man.atkDmg;//적 체력 감소
                Debug.Log(nowHp);
                sword_man.attacked = false;

                if (nowHp <= 0) // 적 사망
                {
                    Die();
                }
            }
        }
    }
    void Die()
    {
        enemyAnimator.SetTrigger("die");            // die 애니메이션 실행
        GetComponent<Enemy_AI>().enabled = false;    // 추적 비활성화
        GetComponent<Collider2D>().enabled = false; // 충돌체 비활성화
        Destroy(GetComponent<Rigidbody2D>());       // 중력 비활성화
        Destroy(gameObject, 3);                     // 3초후 제거
        Destroy(hpBar.gameObject, 3);               // 3초후 체력바 제거
    }
    void SetAttackSpeed(float speed)
    {
        enemyAnimator.SetFloat("attackSpeed", speed);
    }
}
