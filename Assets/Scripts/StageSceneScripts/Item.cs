using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum Type { Ammo, Coin, Heart, Weapon };
    public Type type;
    public int value;

    private void Update()
    {
        if (type == Type.Weapon)
        {
            transform.Rotate(Vector3.up * 50 * Time.deltaTime);
        }
    }
}