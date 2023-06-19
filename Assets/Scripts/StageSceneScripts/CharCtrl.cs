using UnityEngine;
using System.Collections;
public class CharCtrl : MonoBehaviour
{
    private Vector2 _joyVector;
    public Animator anim;
    private Weapon equipWeapon;
    private Weapon[] others = new Weapon[3];
    Rigidbody rigid;

    float inputX;
    float inputZ;
    Vector3 mVector;

    //CharProperty
    private float movespeed;
    private bool inAction;
    private bool isDamage;
    public GameObject[] weapons;
    public GameObject[] body;

    public int ammo;
    public int coin;
    public int heart;
    public int score;

    public int maxAmmo;
    public int maxCoin;
    public int maxHeart;

    void Look()
    {

        if (inAction == false)
            transform.LookAt(transform.position + new Vector3(inputX, 0, inputZ));
    }

    void FreezeRot()
    {
        rigid.angularVelocity = Vector3.zero;
    }

    void GetInput()
    {
        _joyVector = GameObject.FindGameObjectWithTag("JoyStick").GetComponent<JoyCtrl>().Inputjoy();
        inputX = _joyVector.x;
        inputZ = _joyVector.y;
    }

    void CharMove()
    {
        mVector = new Vector3(inputX, 0, inputZ).normalized;
        if (inAction == false)
        {
            rigid.velocity = mVector * movespeed;
        }
        if (mVector != Vector3.zero)
        {
            anim.SetBool("Run", true);
        }
        else
        {
            anim.SetBool("Run", false);
        }
    }

    public void EndAction()
    {
        inAction = false;
    }

    void Attack()
    {
        if (equipWeapon.type == Weapon.Type.Hand)
        {
            anim.SetTrigger("Rj");
            equipWeapon.Use();
        }
        else if (equipWeapon.type == Weapon.Type.Melee)
        {
            anim.SetTrigger("SwordSlash");
            equipWeapon.Use();
        }
        else if (equipWeapon.type == Weapon.Type.Range && ammo > 0)
        {
            ammo--;
            anim.SetTrigger("SingleShot");
            equipWeapon.Use();
        }
        else
            return;
    }

    public void Btn(int index)
    {
        if (inAction == false)
        {
            inAction = true;
            switch (index)
            {
                case 0:
                    anim.SetTrigger("Lh");
                    others[0].Use();
                    break;
                case 1:
                    Attack();
                    break;
                case 2:
                    anim.SetTrigger("Lk");
                    others[1].Use();
                    break;
                case 3:
                    anim.SetTrigger("Rk");
                    others[2].Use();
                    break;
                case 4:
                    anim.SetTrigger("BackDodge");
                    break;
            }
        }
        return;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Weapon")
        {
            Item item = other.GetComponent<Item>();
            if (equipWeapon != null)
                equipWeapon.gameObject.SetActive(false);
            switch (item.value)
            {
                case 0:
                    equipWeapon = weapons[item.value].GetComponent<Weapon>();
                    equipWeapon.gameObject.SetActive(true);
                    break;
                case 1:
                    equipWeapon = weapons[item.value].GetComponent<Weapon>();
                    equipWeapon.gameObject.SetActive(true);
                    break;
                case 2:
                    equipWeapon = weapons[item.value].GetComponent<Weapon>();
                    ammo += 30;
                    equipWeapon.gameObject.SetActive(true);
                    break;
            }
            Destroy(other.gameObject);
        }
        else if (other.tag == "Item")
        {
            Item item = other.GetComponent<Item>();
            switch (item.type)
            {
                case Item.Type.Ammo:
                    ammo += item.value;
                    if (ammo > maxAmmo)
                        ammo = maxAmmo;
                    break;
                case Item.Type.Coin:
                    coin += item.value;
                    if (coin > maxCoin)
                        coin = maxCoin;
                    break;
                case Item.Type.Heart:
                    heart += item.value;
                    if (heart > maxHeart)
                        heart = maxHeart;
                    break;
            }
            Destroy(other.gameObject);
        }
        else if (other.tag == "EnemyBullet")
        {
            if (!isDamage)
            {
                Bullet enemyBullet = other.GetComponent<Bullet>();
                heart -= enemyBullet.damage;
                if (other.GetComponent<Rigidbody>() != null)
                    Destroy(other.gameObject);
                StartCoroutine(OnDamage());
            }
        }
    }

    IEnumerator OnDamage()
    {
        if(!isDamage)
        {
            inAction = true;
            isDamage = true;
            anim.SetTrigger("KnockBack");
            yield return new WaitForSeconds(1.5f);
            isDamage = false;
            inAction = false;
        }
    }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        equipWeapon = weapons[3].GetComponent<Weapon>();
        equipWeapon.gameObject.SetActive(true);
        for (int i = 0; i < body.Length; i++)
        {
            others[i] = body[i].GetComponent<Weapon>();
            others[i].gameObject.SetActive(true);
        }
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        movespeed = 30;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        FreezeRot();
        GetInput();
        CharMove();
        Look();
    }
}