using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{

    public enum mMaterial { Mesh, Skinned }
    public mMaterial MMaterial;
    public enum Type { Normal, Charge, Range}
    public Type EnemyType;
    public Transform target;
    public Vector3 ttarget;
    public Vector3 offset;

    public float maxHealth;
    public float curHealth;
    public float speed;
    public bool green;

    public GameObject healthBar;
    public Slider slider;
    public BoxCollider meleeArea;
    public GameObject bullet;
    public bool isAttack;
    public bool isStopped;

    [Header("knockbackforce")]
    public int kbf;

    Material mat;

    Rigidbody rigid;
    BoxCollider boxCollider;
    private float targetRadius;
    private float targetRange;

    private void OnTriggerEnter(Collider other)
    {
        Vector3 reactVect = transform.position - other.transform.position;
        if (other.tag == "Melee" || other.tag == "Hand")
        {
            Weapon weapon = other.GetComponent<Weapon>();
            curHealth -= weapon.damage;
            
            StartCoroutine(OnDamage(reactVect));
        }
        else if(other.tag == "Bullet")
        {
            Bullet bullet = other.GetComponent<Bullet>();
            curHealth -= bullet.damage;

            StartCoroutine(OnDamage(reactVect));
        }
    }

    IEnumerator OnDamage(Vector3 reactVect)
    {

        mat.color = Color.red;
        yield return new WaitForSeconds(0.1f);

        if (curHealth > 0)
        {
            if(green == true)
            {
                mat.color = new Color(9/255f, 150/255f, 52/255f, 255/255f);
            }
            else
            {
                mat.color = Color.clear;
            }
            rigid.AddForce(reactVect.normalized * kbf, ForceMode.Impulse);
        }
        else
        {
            mat.color = Color.gray;
            gameObject.layer = 10;
            Destroy(gameObject, 1);
        }
    }

    void Targeting()
    {
        switch (EnemyType)
        {
            case Type.Normal:
                targetRadius = 1.5f;
                targetRange = 3f;
                break;
            case Type.Charge:
                targetRadius = 1f;
                targetRange = 60f;
                break;
            case Type.Range:
                targetRadius = 0.5f;
                targetRange = 200f;
                break;
        }
        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange, LayerMask.GetMask("character"));
        if(rayHits.Length > 0 && !isAttack)
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        isAttack = true;
        isStopped = true;

        switch (EnemyType)
        {
            case Type.Normal:
                yield return new WaitForSeconds(0.2f);
                meleeArea.enabled = true;
                yield return new WaitForSeconds(1f);
                meleeArea.enabled = false;
                break;

            case Type.Charge:
                yield return new WaitForSeconds(0.2f);
                rigid.AddForce(transform.forward * 50f, ForceMode.Impulse);
                meleeArea.enabled = true;
                yield return new WaitForSeconds(0.5f);
                rigid.velocity = Vector3.zero;
                meleeArea.enabled = false;
                yield return new WaitForSeconds(2f);
                break;

            case Type.Range:
                yield return new WaitForSeconds(0.2f);
                GameObject instantBullet = Instantiate(bullet, transform.position, transform.rotation);
                Rigidbody rigidBullet = instantBullet.GetComponent<Rigidbody>();
                rigidBullet.velocity = transform.forward * 20;
                yield return new WaitForSeconds(2f);
                break;
        }

        isAttack = false;
        isStopped = false;

    }
    float CalculateHealth()
    {
        return curHealth / maxHealth;
    }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        if (MMaterial == mMaterial.Mesh)
        {
            mat = GetComponentInChildren<MeshRenderer>().material;
        }
        else if(MMaterial == mMaterial.Skinned)
        {
            mat = GetComponentInChildren<SkinnedMeshRenderer>().material;
        }
    }

    void Start()
    {
        curHealth = maxHealth;
        slider.value = CalculateHealth();
    }
    void Update()
    {

        slider.value = CalculateHealth();

        if(curHealth < maxHealth)
        {
            healthBar.SetActive(true);
        }

        if (curHealth > maxHealth)
        {
            curHealth = maxHealth;
        }

        if (EnemyType == Type.Range)
        {
            ttarget = new Vector3(target.position.x, 5, target.position.z);
            transform.LookAt(ttarget);
        }
        else
        {
            transform.LookAt(target);
        }

        if (!isStopped&&(EnemyType == Type.Normal))
        {
            rigid.position = Vector3.MoveTowards(gameObject.transform.position,target.position + offset, speed);
        }
        if (EnemyType == Type.Range)
        {
            rigid.position = Vector3.MoveTowards(gameObject.transform.position, new Vector3(-target.position.x, 1, -target.position.z), speed);
        }
    }

    private void FixedUpdate()
    {
        Targeting();
    }
}
