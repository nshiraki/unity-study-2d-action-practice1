using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	Rigidbody2D rb2d;
	Animator animator;
	public float moveSpeed;
	public Transform attackPoint;
	public float attackRadius;
	public LayerMask enemyLayerMask;
	public int attackPower;
	public float jumpForce;
	private bool jumping = false;

	// Start is called before the first frame update
	void Start()
	{
		rb2d = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			animator.SetTrigger("IsAttack");
		}

		Move();
	}

	private void Move()
	{
		if (Input.GetKeyDown(KeyCode.UpArrow) && !jumping)
		{
			Jump();
			jumping = true;
		}
		animator.SetBool("IsJump", jumping);

		float horizontalKey = Input.GetAxisRaw("Horizontal");

		if (horizontalKey > 0)
		{
			transform.localScale = new Vector3(-1, 1, 1);
		}
		if (horizontalKey < 0)
		{
			transform.localScale = new Vector3(1, 1, 1);
		}

		animator.SetFloat("speed", Mathf.Abs(horizontalKey));
		rb2d.velocity = new Vector2(horizontalKey * moveSpeed, rb2d.velocity.y);

		// 移動範囲を補正
		Vector3 pos = transform.position;
		pos = new Vector3(
			Mathf.Clamp(pos.x, -7.5f, 7.5f),
			pos.y,
			pos.z);
		transform.position = pos;
	}

	private void Jump()
	{
		rb2d.AddForce(Vector2.up * jumpForce);
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("Ground"))
		{
			jumping = false;
		}
	}

	// AnimationEventから呼ばれる関数
	public void Attack()
	{
		Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, enemyLayerMask);

		foreach (Collider2D collider in colliders)
		{
			Enemy ene = collider.GetComponent<Enemy>();
			if (ene.IsHitAttackActive())
			{
				ene.OnDamage(attackPower);
			}
		}
	}

	private void OnDrawGizmos()
	{
		// AttackPointをGizmoで可視化
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
	}
}
