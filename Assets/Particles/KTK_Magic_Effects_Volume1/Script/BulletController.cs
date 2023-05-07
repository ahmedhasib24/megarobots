using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

	public float bulletSpeed = 5.0f;	//bullet speed
	private Rigidbody rb;
    public float attack = 0;
    public int partId = -1;

    public Vector3 OriPos;
    public Vector3 OriRot;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * bulletSpeed;
    }

    private void OnEnable()
    {
        //transform.localPosition = OriPos;
        //transform.localEulerAngles = OriRot;
    }

    public GameObject ImpactEffect;	//hit EffectPrefab

	// The processing when making a hit
	void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Robot"))
        {
            Debug.Log("Hitting robot");
            //GameObject hitEffPrefab = Instantiate(ImpactEffect, transform.position, Quaternion.identity) as GameObject;
            //hitEffPrefab.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);

            col.gameObject.GetComponent<RobotController>().GetDamage(partId, attack);
            Destroy(this.gameObject);
        }
	}
}

