using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Projectile_Direct : Projectile 
{
    public float projectileSpeed = 10f;

    public int damageAmount = 10;
    public string damageFunctionName = "DealDamage";

    public float timeToDie = 5f;

    private Rigidbody _rigidbody;

    public void Start()
    {
        _rigidbody = this.GetComponent<Rigidbody>();

        StartCoroutine(Timout());
    }

    void Update () 
    {
        this.transform.Translate(this.transform.forward * projectileSpeed * Time.deltaTime, Space.World);
	}

    public void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.SendMessageUpwards(damageFunctionName, damageAmount, SendMessageOptions.DontRequireReceiver);

        Destroy(this.gameObject);
    }

    IEnumerator Timout ()
    {
        yield return new WaitForSeconds(timeToDie);

        Destroy(this.gameObject);
    }

}
