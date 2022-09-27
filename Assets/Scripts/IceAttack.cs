using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceAttack : MonoBehaviour
{
    public float maxDistance;
    private Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if ( (transform.position - startPos).magnitude > maxDistance )
		{
            Destroy(gameObject);
		}
    }

	private void OnCollisionEnter2D(Collision2D collision)
	{
        StartCoroutine(nameof(DestroyAfterDelay));
	}

    IEnumerator DestroyAfterDelay()
	{
        yield return new WaitForEndOfFrame();
        Destroy(gameObject);
    }
}
