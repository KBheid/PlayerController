using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class AttackState : State
{
	static GameObject chargeAttackPrefab = Resources.Load("Prefabs/IceAttack") as GameObject;

	float maxAttackTime = 3f;
	float attackTime = 0f;

	float attackLaunchSpeed = 450f;

	bool attackingLeft;
	GameObject attackObject;
	Vector3 offset;

	public AttackState(Rigidbody2D rb, Animator anim, StateChanged callback, bool attackLeft) : base(rb, anim, callback)
	{
		attackingLeft = attackLeft;

		float direction = (attackingLeft) ? -1 : 1;
		offset = new Vector3(direction * 0.5f, -0.5f);
	}

	public override void Update(float deltaTime)
	{
		base.Update(deltaTime);

		attackTime += deltaTime;
		//                                  (1, 1, 1)   *   maxSizeRatio             * undo scale from model
		attackObject.transform.localScale = Vector3.one * attackTime / maxAttackTime * 1 / _rb.transform.localScale.y;

		attackObject.transform.position = _rb.transform.position + offset;
		if (attackTime >= maxAttackTime)
		{
			TransitionStates(new WalkState(_rb, animator, _stateChangedCallback));
		}
	}

	public override void Input(KeyCode key, bool pressed)
	{
		base.Input(key, pressed);

		bool finishLeftAttack = key == KeyCode.LeftArrow && attackingLeft && !pressed;
		bool finishRightAttack = key == KeyCode.RightArrow && !attackingLeft && !pressed;
		if (finishRightAttack || finishLeftAttack)
		{
			TransitionStates(new WalkState(_rb, animator, _stateChangedCallback));
		}
	}

	protected override void OnEnterState()
	{
		base.OnEnterState();
		animator.SetTrigger("AttackSide");
		attackObject = GameObject.Instantiate(chargeAttackPrefab, _rb.transform.position, Quaternion.identity);
		attackObject.transform.localScale = Vector3.zero;
		attackObject.transform.SetParent(_rb.transform);

		// Manual offset
		attackObject.transform.position += offset;
	}

	protected override void OnExitState()
	{
		base.OnExitState();
		animator.SetTrigger("AttackEnd");

		float direction = (attackingLeft) ? -1 : 1;
		attackObject.transform.SetParent(null);
		attackObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(direction * (1 / attackTime / maxAttackTime) * attackLaunchSpeed, 0));
	}
}