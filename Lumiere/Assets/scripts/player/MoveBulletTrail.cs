using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBulletTrail : MonoBehaviour {

    public float m_moveSpeed = 230f;
    public float m_destroyAfterSeconds = 2f;

    //Damage to deal to whoever gets hit
    public double Damage = 0;

    void Start()
    {
        StartCoroutine(DestoryAfter(m_destroyAfterSeconds));
    }

	void Update ()
    {
        transform.Translate(Vector3.right * Time.deltaTime * m_moveSpeed);
	}

    IEnumerator DestoryAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(this.gameObject);
    }

    public void DestroyEarly()
    {
        Destroy(this.gameObject);
    }
}
