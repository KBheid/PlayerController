using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public State currentState;

	Animator _animator;
	Rigidbody2D _rb;
	SpriteRenderer _renderer;

	// Start is called before the first frame update
	void Start()
	{
		_animator = GetComponent<Animator>();
		_rb = GetComponent<Rigidbody2D>();
		_renderer = GetComponent<SpriteRenderer>();

		currentState = new WalkState(_rb, _animator, OnStateChange);
	}

	void OnStateChange(State state)
	{
		currentState = state;
	}

	// Update is called once per frame
	void Update()
	{
		float xMovement = Input.GetAxisRaw("Horizontal");
		_renderer.flipX = xMovement != 1 && (xMovement == -1 || _renderer.flipX);

		currentState.Update(Time.deltaTime);
	}

	private void OnGUI()
	{
		// Send input to current state
		Event e = Event.current;

		switch (e.type)
		{
			case EventType.KeyDown:
				currentState.Input(e.keyCode, true);
				break;

			case EventType.KeyUp:
				currentState.Input(e.keyCode, false);
				break;
		}
	}

}
