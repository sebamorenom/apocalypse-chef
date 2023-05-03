using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall : MonoBehaviour
{
    [SerializeField] private float hitThreshold;
    private Collider _mainColl;
    private Rigidbody _rb;
    private Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _mainColl = GetComponent<Collider>();
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Cube"))
            Debug.Log(collision.impulse.magnitude);
        {
            if (collision.impulse.magnitude > hitThreshold)
            {
                for (int i = 0; i < collision.contacts.Length; i++)
                {
                    _rb.velocity = Vector3.zero;
                    _animator.enabled = false;
                    var contact = collision.GetContact(i);
                    var contactPosition = contact.point;
                    collision.GetContact(i).thisCollider.attachedRigidbody.AddExplosionForce(4f, contactPosition, 80);
                }

            }
        }
    }
}
