using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDestroyable
{
    public int hp;
    public float speed = 1f;
    public float bulletFireRate;
    public int bulletAtOnce = 5;
    public float fireCoolTime;
    public Sprite onDamagedSprite;
    public EnemyBulletPattern firePattern;
    public GameObject bulletPrefab;

    private Vector3 transitionTarget;
    private SpriteRenderer spriteRenderer;
    private Sprite idleImage;

    public void Demolish()
    {
        GameObject effectClone = Instantiate(GameManager.Inst.destroyEffect, transform.position, Quaternion.identity);
        Destroy(effectClone, 0.6f);
        Instantiate(GameManager.Inst.coin, transform.position, Quaternion.identity);
        Destroy(gameObject);
        GameManager.Inst.score += 1000;
    }

    public void RecieveDamage(int value)
    {
        hp -= value;
        StartCoroutine(OnDamaged());
        if (hp <= 0)
        {
            Demolish();
        }
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        idleImage = spriteRenderer.sprite;
        StartCoroutine(Fire());

        StartCoroutine(TransitionDirector());
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, transitionTarget, speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("PlayerBullet"))
        {
            RecieveDamage(PlayerController.damage);
            Destroy(other.gameObject);
        }
    }

    IEnumerator Fire()
    {
        while (true)
        {
            int i = 0;
            yield return new WaitForSeconds(fireCoolTime);
            while (i < bulletAtOnce)
            {
                i++;
                BulletFire();
                yield return new WaitForSeconds(bulletFireRate);
            }
        }
    }

    private void BulletFire()
    {
        switch (firePattern)
        {
            case EnemyBulletPattern.Direct:
                GameObject bulletClone = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, 180)));
                //bulletClone.transform.LookAt(-bulletClone.transform.up);
                break;
            case EnemyBulletPattern.Spiral:
                break;
            case EnemyBulletPattern.Cruise:
                break;
        }
    }

    IEnumerator OnDamaged()
    {
        spriteRenderer.sprite = onDamagedSprite;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.sprite = idleImage;
    }

    IEnumerator TransitionDirector()
    {
        int i = 0;
        while (true)
        {
            transitionTarget = GameManager.Inst.loop1[i].position;
            yield return new WaitForSeconds(3.5f);
            i++;
            if (i >= GameManager.Inst.loop1.Length)
                i = 0;
        }
    }
}
