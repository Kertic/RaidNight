using System;
using UnityEngine;

namespace Code.Player
{
    public class PlayerPhysics : MonoBehaviour
    {
        public BoxCollider2D _HitboxCollider2D { get; private set; }
        public Rigidbody2D _Rigidbody2D { get; private set; }
        public Vector2 velocity;


        private void Awake()
        {
            _HitboxCollider2D = GetComponent<BoxCollider2D>();
            _Rigidbody2D = GetComponent<Rigidbody2D>();
            velocity = Vector2.zero;
        }

        private void FixedUpdate()
        {
            _Rigidbody2D.MovePosition(_Rigidbody2D.position + velocity);
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            for (int i = 0; i < col.contacts.Length; i++)
            {
                Debug.Log(col.contacts[i]);
            }
        }
    }
}