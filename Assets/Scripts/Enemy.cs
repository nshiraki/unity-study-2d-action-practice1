using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
	public int life;
	public bool canHitAttack;
	private Animator animator;

	// Start is called before the first frame update
	void Start()
	{
		animator = GetComponent<Animator>();
	}

	public void OnDamage(int damage)
	{
		animator.SetTrigger("IsHurt");
		life -= damage;
		if (life <= 0)
		{
			Dead();
		}
	}

	private void Dead()
	{
		canHitAttack = false;
		life = 0;
		animator.SetTrigger("IsDeath");
		Invoke("Recover", 3);
	}

	private void Recover()
	{
		animator.SetTrigger("Recover");
	}

	// AnimationEventから呼ばれる関数
	public void RecoverFinishEvent()
	{
		Debug.Log("RecoverFinishEvent() called");
		life = 3;
		canHitAttack = true;
	}

	public bool IsHitAttackActive()
	{
		return canHitAttack;
	}
}
