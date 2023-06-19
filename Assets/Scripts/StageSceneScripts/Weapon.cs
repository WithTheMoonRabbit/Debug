using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum Type { Hand, Melee, Range };
    public Type type;
    public int damage;
    public float rate;
    public BoxCollider meleeArea;
    public TrailRenderer trailEffect;

    public Transform bulletPos;
    public GameObject bullet;

    public void Use()
    {
        if (type == Type.Hand)
        {
            StopCoroutine("HandAtk");
            StartCoroutine("HandAtk");
        }
        else if (type == Type.Melee)
        {
            StopCoroutine("MeleeAtk");
            StartCoroutine("MeleeAtk");
        }
        else if (type == Type.Range)
        {
            StartCoroutine("Shot");
        }
    }

    IEnumerator HandAtk()
    {
        yield return new WaitForSeconds(0.2f);
        meleeArea.enabled = true;

        yield return new WaitForSeconds(0.6f);
        meleeArea.enabled = false;
    }

    IEnumerator MeleeAtk()
    {
        yield return new WaitForSeconds(0.2f);
        meleeArea.enabled = true;
        trailEffect.enabled = true;

        yield return new WaitForSeconds(0.6f);
        meleeArea.enabled = false;

        yield return new WaitForSeconds(0.3f);
        trailEffect.enabled = false;
    }

    IEnumerator Shot()
    {
        yield return new WaitForSeconds(0.5f);
        GameObject instantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRigid = instantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletPos.forward * 50;
        yield return null;
    }
}
