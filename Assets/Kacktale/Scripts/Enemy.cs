using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("½ºÅÝ")]
    public float Hp;
    public float Def;

    public float MoveSpeed;
    public float AttackSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Damage(float Damage)
    {
        if(Damage - Def <= 0)
        {
            return;
        }
        Hp -= Damage + Def;
        if (Hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
