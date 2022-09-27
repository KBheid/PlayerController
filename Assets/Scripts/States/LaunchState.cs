using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class LaunchState : State
{
	private float _delayBeforeJump = 0.25f;
	private float _delayed = 0f;
	private bool _forceApplied = false;

	private bool _wasMovingLeft;

	public LaunchState(Rigidbody2D rb, Animator anim, StateChanged callback, bool movingLeft) : base(rb, anim, callback)
	{
		_wasMovingLeft = movingLeft;
	}

	public override void Update(float deltaTime)
	{
		base.Update(deltaTime);

		_delayed += deltaTime;

		if (_delayed < _delayBeforeJump)
			return;

		if (!_forceApplied)
		{
			float xMod = (_wasMovingLeft) ? 1 : -1;
			_rb.AddForce(new Vector2(xMod * 300, 400));
			_forceApplied = true;
		}


		if (_rb.velocity.y < -0.5f)
		{
			TransitionStates(new FallState(_rb, animator, _stateChangedCallback));
		}
	}

	protected override void OnEnterState()
	{
		base.OnEnterState();

		animator.SetTrigger("AttackDown");
		animator.SetBool("Grounded", false);
	}
}