using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyController : MonoBehaviour, IDestroyable
{
    public int hp;
    public float speed;
    public int moveCount;
    public int movePattern;
    public float attackDelay;
    public int attackPattern;
    public float bulletFireRate;
    public int bulletAtOnce = 5;
    //public float fireCoolTime;
    public Sprite onDamagedSprite;
    public int firePattern;
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
    }

    void Update()
    {
        TransitionDirector();
        if (transform.position.y < -5.5)
            Destroy(gameObject);
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
            yield return new WaitForSeconds(attackDelay);
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
            case 0:
                GameObject bulletClone = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, 180)));
                //bulletClone.transform.LookAt(-bulletClone.transform.up);
                break;
            case 1:
                GameObject bulletClone1 = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, 150)));
                GameObject bulletClone2 = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, 180)));
                GameObject bulletClone3 = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, 210)));
                break;
            case 2:
                break;
        }
    }

    IEnumerator OnDamaged()
    {
        spriteRenderer.sprite = onDamagedSprite;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.sprite = idleImage;
    }


    private void TransitionDirector()
    {
        switch (movePattern)
        {
            case 0:
                transform.position += new Vector3(0.0f, -speed * Time.deltaTime, 0.0f);
                break;

            case 1:
                transitionTarget = GameManager.Inst.loop1[moveCount];
                if (transform.position == GameManager.Inst.loop1[moveCount])
                    moveCount++;
                if (moveCount >= GameManager.Inst.loop1.Count)
                    moveCount = 0;
                transform.position = Vector2.MoveTowards(transform.position, transitionTarget, speed * Time.deltaTime);
                break;

            case 2:
                transitionTarget = GameManager.Inst.loop2[moveCount];
                if (transform.position == GameManager.Inst.loop2[moveCount])
                    moveCount++;
                if (moveCount >= GameManager.Inst.loop2.Count)
                    moveCount = 0;
                transform.position = Vector2.MoveTowards(transform.position, transitionTarget, speed * Time.deltaTime);
                break;

            case 3:

                break;
        }


    }

    public void SetParameter(int _hp, float _speed, int _movePattern, float _attackDelay, int _attackPattern)
    {
        hp = _hp;
        speed = _speed;
        movePattern = _movePattern;
        attackDelay = _attackDelay;
        attackPattern = _attackPattern;
        switch (attackPattern)
        {
            case 0:
                bulletAtOnce = 1;
                bulletFireRate = 0.0f;
                firePattern = 0;
                break;
            case 1:
                bulletAtOnce = 3;
                bulletFireRate = 0.5f;
                firePattern = 1;
                break;

        }
    }
}


/*
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
    public int targetLoop = 0;

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
            GameManager.enemyCount -= 1;
        }
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        idleImage = spriteRenderer.sprite;
        StartCoroutine(Fire());

        StartCoroutine(TransitionDirector(targetLoop));
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
                break;
            case EnemyBulletPattern.Spiral:
                for (int i = 0; i < 5; i++)
                {
                    Instantiate(bulletPrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, 150 + 15 * i +360)));
                }
                break;
            case EnemyBulletPattern.Cruise:
                break;
        }
    }

    IEnumerator OnDamaged()
    {
        if (onDamagedSprite == null)
            yield break;

        if (TryGetComponent<Animator>(out Animator animController))
            animController.enabled = false;

        spriteRenderer.sprite = onDamagedSprite;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.sprite = idleImage;

        if (animController != null)
            animController.enabled = true;
    }

    IEnumerator TransitionDirector(int targetLoop = 0)
    {
        int i = 0;
        while (true)
        {
            transitionTarget = GameManager.Inst.loops[targetLoop].positions[i].position;
            yield return new WaitForSeconds(3.5f);
            i++;
            if (i >= GameManager.Inst.loops[targetLoop].positions.Length)
                i = 0;
        }
    }
}
*/