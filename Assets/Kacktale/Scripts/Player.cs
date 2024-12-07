using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("기본적인 세팅")]
    public float MoveSpeed;
    public float JumpForce;
    private Rigidbody2D rigidbody;
    public bool IsGround = false;
    public LayerMask LayerMask;
    [Header("공격")]
    public float AttackPower;
    [SerializeField]
    private List<GameObject> Enemys;
    public LayerMask AttackLayerMask;
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");

        if(h > 0)
        {
            transform.rotation = Quaternion.Euler(0,0,0);
        }
        else if(h < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        transform.position += new Vector3 (h, 0, 0) * MoveSpeed * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        RayCast();
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            Jump();
        }
        if(Input.GetKeyDown(KeyCode.Z)||Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    //땅 & 공격범위 확인용
    void RayCast()
    {
        float RayDistance = 1.1f;
        RaycastHit2D MidRay = Physics2D.Raycast(transform.position, Vector2.down, RayDistance, LayerMask);
        RaycastHit2D LeftRay = Physics2D.Raycast(transform.position + new Vector3(-0.5f, 0), Vector2.down, RayDistance, LayerMask);
        RaycastHit2D RightRay = Physics2D.Raycast(transform.position + new Vector3(0.5f, 0), Vector2.down, RayDistance, LayerMask);
        Debug.DrawRay(transform.position, Vector2.down * RayDistance);
        Debug.DrawRay(transform.position + new Vector3(-0.5f, 0), Vector2.down * RayDistance);
        Debug.DrawRay(transform.position + new Vector3(0.5f, 0), Vector2.down * RayDistance);

        //Debug.Log(MidRay.collider.gameObject.name);

        if (MidRay.collider || LeftRay.collider || RightRay.collider)
        {
            IsGround = true;
        }


        Enemys = new List<GameObject>();
        float AttackDistance = 1.4f;
        if (gameObject.transform.rotation == Quaternion.Euler(0,-180,0))
        {
            RaycastHit2D AttackRangeL = Physics2D.Raycast(transform.position + new Vector3(0, -0.8f), Vector2.left, AttackDistance, AttackLayerMask);
            Debug.DrawRay(transform.position + new Vector3(0, -0.8f), Vector2.left * AttackDistance);

            if (AttackRangeL.collider)
            {
                if (AttackRangeL.collider.gameObject.CompareTag("Enemy"))
                {
                    Enemys.Add(AttackRangeL.collider.gameObject);
                }
            }
        }
        else if(gameObject.transform.rotation == Quaternion.Euler(0, 0, 0))
        {
            RaycastHit2D AttackRangeR = Physics2D.Raycast(transform.position + new Vector3(0,-0.8f), Vector2.right, AttackDistance, AttackLayerMask);
            Debug.DrawRay(transform.position + new Vector3(0, -0.8f), Vector2.right * AttackDistance);

            if (AttackRangeR.collider)
            {
                if (AttackRangeR.collider.gameObject.CompareTag("Enemy"))
                {
                    Enemys.Add(AttackRangeR.collider.gameObject);
                }
            }
        }
    }

    void Jump()
    {
        if(IsGround)
        {
            rigidbody.AddForce(Vector2.up * JumpForce,ForceMode2D.Impulse);
            IsGround = false;
        }
    }

    void Attack()
    {
        foreach(var EnemyHp in Enemys)
        {
            Enemy enemy = EnemyHp.GetComponent<Enemy>();
            enemy.Damage(AttackPower);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        IsGround = false;
    }
}
