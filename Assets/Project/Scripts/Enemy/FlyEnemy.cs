using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEnemy : MonoBehaviour
{
    Animator ani;
    CircleCollider2D colli2D;
    SpriteRenderer spriteRenderer;

    public Andras Andras;
    public Paimon Paimon;

    public float AggroRadius;
    public float Speed;
    public int ExplodeRadius;
    public int Health;
    public int MaxHealth;

    public LayerMask AndrasLayerMask;
    public LayerMask PaimonLayerMask;
    public LayerMask WallLayerMask;

    Vector3 target;

    Rigidbody2D rb2D;

    bool isDead;

    private void Awake()
    {
        ani = GetComponent<Animator>();
        colli2D = GetComponent<CircleCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        ani.Play("Idle" + Random.Range(1, 4));
    }

    private void Update()
    {
        CheckAggro();

        CheckPlayerTriggerEnter();

        if (!isDead)
            CheckWallTriggerEnter();
    }

    private void CheckAggro()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, AggroRadius, AndrasLayerMask);
        if (colliders.Length > 0)
            target = Andras.transform.position;
        else
            target = Paimon.transform.position;

        rb2D.velocity = ((target - transform.position).normalized * Speed);
    }

    private void CheckPlayerTriggerEnter()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, colli2D.radius, AndrasLayerMask | PaimonLayerMask);
        if (colliders.Length > 0)
            OnDeath();
    }

    private void CheckWallTriggerEnter()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, colli2D.radius, WallLayerMask);
        if (colliders.Length > 0)
        {
            TakeDamage();
            if (Health <= 0)
            {
                OnDeath();
            }
        }  
    }

    private void TakeDamage()
    {
        Vector3Int gridPos = ColorWallCreater.i.previewMap.WorldToCell(transform.position);
        Health = Mathf.Clamp(Health - ColorWallCreater.i.ClearItem(gridPos, (int)colli2D.radius), 0, MaxHealth);
        float value = (float)((float)Health / (float)MaxHealth);
        Debug.Log(value);
        spriteRenderer.color = new Color(1, value, value, 1);
    }

    private void OnDeath()
    {
        isDead = true;
        Vector3Int gridPos = ColorWallCreater.i.previewMap.WorldToCell(transform.position);
        ColorWallCreater.i.ClearItem(gridPos, ExplodeRadius);
        Destroy(gameObject);
    }
}
