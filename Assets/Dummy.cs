using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dummy : MonoBehaviour
{
	public Text damageText;
	public float textDisplayDuration = 1f;

	private void OnCollisionEnter2D(Collision2D collision)
	{
		damageText.text = (collision.collider.transform.localScale.y).ToString();
		StartCoroutine(nameof(ResetText));
	}

	IEnumerator ResetText()
	{
		yield return new WaitForSeconds(textDisplayDuration);
		damageText.text = "";
	}
}
