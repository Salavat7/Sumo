using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject _powerupIndicator;
    [SerializeField] private GameObject rocketPrefab;
    [SerializeField] private Button _restartButton;
    private GameManager _gameManager;
    private Vector3 _powerupOffset = new Vector3 (0, -0.5f, 0);
    private Rigidbody _playerRb;
    private GameObject _focalPoint;
    private PowerUpType _currentPowerUp = PowerUpType.None;
    private GameObject _rocket;
    private Coroutine _powerupCountdown;
    private float _powerupStrenght = 100.0f;
    private float _speed = 40.0f;
    private bool _hasPowerup;
    private float _hangTime = 0.1f;
    private float _smashSpeed = 25;
    private float _explosionForce = 200;
    private float _explosionRadius = 9;
    private float _floorY = 0.1f;
    private bool _isSmashing = false;
    private float _bottomBound = -10f;
    public string PlayerName {  get; private set; }

    private void Start()
    {
        _playerRb = GetComponent<Rigidbody>();
        _focalPoint = GameObject.Find("Focal Point");
        _gameManager = GameManager.Instance;
        gameObject.GetComponent<MeshRenderer>().material = _gameManager.CurrentSkin;
    }

    private void Update()
    {
        float forwardInput = Input.GetAxis("Vertical");
        _playerRb.AddForce(_focalPoint.transform.forward * _speed * forwardInput);
        _powerupIndicator.gameObject.transform.position = transform.position + _powerupOffset;

        if (_currentPowerUp == PowerUpType.Rockets && Input.GetKeyDown(KeyCode.F))
        {
            LaunchRockets();
        }

        if (_currentPowerUp == PowerUpType.Smash && Input.GetKeyDown(KeyCode.Space) && !_isSmashing)
        {
            _isSmashing = true;
            StartCoroutine(Smash());
        }

        if (transform.position.y < _bottomBound)
        {
            Death();
        }
    }

    private IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        _hasPowerup = false;
        _currentPowerUp = PowerUpType.None;
        _powerupIndicator.gameObject.SetActive(false);
    }

    private void LaunchRockets()
    {
        foreach (var enemy in FindObjectsOfType<Enemy>())
        {
            _rocket = Instantiate(rocketPrefab, transform.position + Vector3.up, Quaternion.identity);
            _rocket.GetComponent<RocketBehaviour>().Fire(enemy.transform);
        }
    }

    private IEnumerator Smash()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        float jumpTime = Time.time + _hangTime; //Calculate amount of time we will go up
        while (Time.time < jumpTime)
        {
            //move the player up while still keeping their x and z velocity.
            _playerRb.velocity = new Vector3(_playerRb.velocity.x, _smashSpeed, _playerRb.velocity.z);
            yield return null;
        }
        //Now move the player down
        while (transform.position.y > _floorY)
        {
            _playerRb.velocity = new Vector3(_playerRb.velocity.x, -_smashSpeed * 2, _playerRb.velocity.z);
            yield return null;
        }

        for (int i = 0; i < enemies.Length; i++)
        {
            //Apply an explosion force that originates from our position.
            if (enemies[i] != null)
            {
                Rigidbody enemyRb = enemies[i].GetComponent<Rigidbody>();
                enemyRb.AddExplosionForce(_explosionForce, transform.position, _explosionRadius, 0.0f, ForceMode.Impulse);
            }
        }
        _isSmashing = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            _hasPowerup = true;
            _currentPowerUp = other.gameObject.GetComponent<PowerUp>().PowerUpType;
            _powerupIndicator.gameObject.SetActive(true);
            Destroy(other.gameObject);
            if(_powerupCountdown != null)
            {
                StopCoroutine(_powerupCountdown);
            }
            _powerupCountdown = StartCoroutine(PowerupCountdownRoutine());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy") && _currentPowerUp == PowerUpType.Pushback)
        {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;
            enemyRb.AddForce(awayFromPlayer * _powerupStrenght, ForceMode.Impulse);
        }
    }

    private void Death()
    {
        _gameManager.SavePlayer();
        _restartButton.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Menu");
    }
}
