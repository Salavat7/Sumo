using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    [SerializeField]private float _speed = 10.0f;
    private float _bottomBound = -10.0f;
    private GameObject _player;
    private Rigidbody _enemyRb;

    private void Start()
    {
        _enemyRb = GetComponent<Rigidbody>();
        _player = GameObject.Find("Player");
    }

    private void Update()
    {
        //Moove to player
        Vector3 lookDirection = (_player.transform.position - _enemyRb.transform.position).normalized;
        _enemyRb.AddForce(lookDirection * _speed);

        if(transform.position.y < _bottomBound)
        {
            Destroy(gameObject);
        }
    }
}
