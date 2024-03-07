using UnityEngine;

public class RocketBehaviour : MonoBehaviour
{
    private Transform _target;
    private float _speed = 15.0f;
    private float _rocketStrenght = 15.0f;
    private float _aliveTimer = 5.0f;

    private void Update()
    {
        if (_target != null)
        {
            Vector3 moveDirection = (_target.transform.position - transform.position).normalized;
            transform.position += moveDirection * _speed * Time.deltaTime;
            transform.LookAt(_target);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(_target != null)
        {
            if (collision.gameObject.CompareTag(_target.tag))
            {
                Rigidbody targetRb = collision.gameObject.GetComponent<Rigidbody>();
                Vector3 away = -collision.contacts[0].normal;
                targetRb.AddForce(away * _rocketStrenght, ForceMode.Impulse);
                Destroy(gameObject);
            }
        }
    }

    public void Fire(Transform newTarget)
    {
        _target = newTarget;
        Destroy(gameObject, _aliveTimer);
    }
}
