using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwordMan : MonoBehaviour//플레이어 코드
{
    Animator animator;
    public int maxHp;//최대체력
    public int nowHp;//현재체력
    public int atkDmg;//공격력
    public float atkSpeed = 1; //공격속도 변수
    public bool attacked = false; //공격중인지 여부 파악 -> 없을시 닿아도 피닳을수 있음.
    public Image nowHpbar;

    bool inputRight = false;//우측 이동여부
    bool inputLeft = false;//좌측 이동여부
    Rigidbody2D rigid2D;

    public float jumpPower = 5.0f;// 점프높이
    bool inputJump = false; //점프 여부

    BoxCollider2D col2D;

    // Start is called before the first frame update
    void Start()
    {
        maxHp = 50;
        nowHp = 50;
        atkDmg = 10;

        transform.position = new Vector3(10,4.6f,0);//최초 생성 위치 지정
        animator = GetComponent<Animator>();//애니메이션관련 데이터 받아오기
        rigid2D = GetComponent<Rigidbody2D>();//중력관련함수 받아오기

        SetAttackSpeed(1.5f); //최초 공격속도 지정

        col2D = GetComponent<BoxCollider2D>();

    }

    // Update is called once per frame
    
    float speed = 3;
    void Update()
    {
        nowHpbar.fillAmount = (float)nowHp/(float)maxHp;

        RaycastHit2D raycastHit = Physics2D.BoxCast(col2D.bounds.center, col2D.bounds.size, 0f, Vector2.down, 0.1f, LayerMask.GetMask("Ground"));
        if (raycastHit.collider != null)
        {

            animator.SetBool("jumping", false);
        }
        else animator.SetBool("jumping", true);

        if (Input.GetKey(KeyCode.D))
        {
            inputRight = true;
            transform.localScale = new Vector3(-1, 1, 1);//우측면 바라보기
            animator.SetBool("moving",true);//이동시 애니메이션 작동
            if (inputRight)
            {
                inputRight = false;
                rigid2D.velocity = new Vector2(speed, rigid2D.velocity.y);
            }
        }
        else if (Input.GetKey(KeyCode.A))
        {
            inputLeft = true;
            transform.localScale = new Vector3(1, 1, 1);//좌측면 바라보기
            animator.SetBool("moving",true);
            
            if (inputLeft)
            {
                inputLeft = false;
                rigid2D.velocity = new Vector2(-speed, rigid2D.velocity.y);
            }
            
        }
        else animator.SetBool("moving",false);

        if (Input.GetKey(KeyCode.J) && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {

            animator.SetTrigger("attack");//공격 애니메이션 작동

        }
        if (Input.GetKeyDown(KeyCode.Space) && !animator.GetBool("jumping"))
        {
            inputJump = true;
        }
    }
    void FixedUpdate()
    {
        if(inputJump)
        {
            inputJump = false;
            rigid2D.AddForce(Vector2.up * jumpPower);
            //rigid2D.velocity = new Vector2(rigid2D.velocity.x, jumpPower);
        }
        
    }

    void AttackTrue()//공격 활성화
    {
	    attacked = true;
    }
    void AttackFalse()//공격 비활성화
    {
	    attacked = false;
    }
    void SetAttackSpeed(float speed)
    {
	    animator.SetFloat("attackSpeed", speed);
	    atkSpeed = speed;
    }
    
}
