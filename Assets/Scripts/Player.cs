using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
public class Player : MonoBehaviour
{
    private FixedJoystick Joystick;

    public bool canTripleShoot = false;
    public bool canSpeedBoost = false;
    public bool shieldsActive = false;
    public bool isPlayerOne = false;
    public bool isPlayerTwo = false;

    public int lives = 3;
    [SerializeField] private GameObject _explosion;
    [SerializeField] private float _speed = 5.0f;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private float _fireRate = 0.25f;
    private float _canFire = 0.0f;

    [SerializeField] private GameObject _shieldGameObject;

    [SerializeField] private GameObject[] _engines;

    private UIManager _uiManager;
    private GameManager _gameManager;
    private SpawnManager _spawnManager;
    private AudioSource _audioSource;

    private int hitCount = 0;


    // Start is called before the first frame update
    void Start()
    {
        Joystick = GameObject.Find("Fixed Joystick").GetComponent<FixedJoystick>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager != null)
        {
            _uiManager.UpdateLives(lives);
        }
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        
        /*if(_spawnManager != null)
        {
            _spawnManager.StartSpawnRoutines();
        }*/

        _audioSource = GetComponent<AudioSource>();

        hitCount = 0;

        if (_gameManager.isCoopMode == false)
        {
            transform.position = new Vector3(0, 0, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerOne == true)
        {
            Movement();

            if ((Input.GetKeyDown(KeyCode.Space) || CrossPlatformInputManager.GetButton("Fire")) && isPlayerOne == true)
            {
                Shoot();
            }

#if UNITY_ANDROID

            if ((Input.GetKeyDown(KeyCode.Space) || CrossPlatformInputManager.GetButton("Fire")) && isPlayerOne == true)
            {
                Shoot();
            }

#else

            if ((Input.GetKey(KeyCode.Space) || Input.GetMouseButtonDown(0)) && isPlayerOne == true)
            {
                Shoot();
            }

#endif
        }
        if(isPlayerTwo== true)
        {
            PlayerTwoMovement();
            if (Input.GetKeyDown(KeyCode.O))
            {
                Shoot();
            }
        }
    }

    public void TripleShotPowerupOn()
    {
        canTripleShoot = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }
    public void SpeedBoost()
    {
        canSpeedBoost = true;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }
    public void EnableShields()
    {
        shieldsActive = true;
        _shieldGameObject.SetActive(true);
    }
    public IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        canTripleShoot = false;
    }
    public IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        canSpeedBoost = false;
    }

    private void Shoot()
    {
        if (Time.time > _canFire)
        {
            _audioSource.Play();
            if (canTripleShoot == true)
            {
                Instantiate(_laserPrefab, transform.position + new Vector3(-0.55f, 0, 0), Quaternion.identity); //left laser
                Instantiate(_laserPrefab, transform.position + new Vector3(0.55f, 0, 0), Quaternion.identity); //right laser
            }

            Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.88f, 0), Quaternion.identity);
            _canFire = Time.time + _fireRate;
        }
    }
    public void Damage()
    {

        if(shieldsActive == true)
        {
            shieldsActive = false;
            _shieldGameObject.SetActive(false);
            return;
        }

        hitCount++;

        if (hitCount == 1)
        {
            _engines[0].SetActive(true);
        }
        else if (hitCount == 2)
        {
            _engines[1].SetActive(true);
        }

        lives--;
        _uiManager.UpdateLives(lives);
        
        if (lives < 1)
        {
            Destroy(this.gameObject);
            _gameManager.gameOver = true;
            _uiManager.ShowTitleScreen();
            _uiManager.CheckForBestScore();
            Instantiate(_explosion, transform.position, Quaternion.identity);
        }
    }
    private void Movement()
    {


        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput =  Input.GetAxis("Vertical");

        if (canSpeedBoost == true)
        {
            _speed = 10.0f;
        }
        else
            _speed = 5.0f;
        transform.Translate(Vector3.right * Time.deltaTime * _speed * horizontalInput);
        transform.Translate(Vector3.up * Time.deltaTime * _speed * verticalInput);

        if (transform.position.y > 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y < -4.4f)
        {

            transform.position = new Vector3(transform.position.x, -4.4f, 0);
        }
        /*else if (transform.position.x < -8.3f)
        {

            transform.position = new Vector3(-8.3f, transform.position.y, 0);
        }
        else if (transform.position.x > 8.3f)
        {

            transform.position = new Vector3(8.3f, transform.position.y, 0);
        }*/

        if (transform.position.x > 9.5)
        {
            transform.position = new Vector3(-9.5f, transform.position.y, 0);
        }
        else if (transform.position.x < -9.5)
        {
            transform.position = new Vector3(9.5f, transform.position.y, 0);
        }
    }

    private void PlayerTwoMovement()
    {

        if (canSpeedBoost == true)
        {
            _speed = 10.0f;
        }
        else
        {
            _speed = 5.0f;

        }
        
        if (Input.GetKey(KeyCode.I))
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.L))
        {
            transform.Translate(Vector3.right * _speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.K))
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.J))
        {
            transform.Translate(Vector3.left * _speed * Time.deltaTime);
        }

        if (transform.position.y > 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y < -4.4f)
        {

            transform.position = new Vector3(transform.position.x, -4.4f, 0);
        }
        /*else if (transform.position.x < -8.3f)
        {

            transform.position = new Vector3(-8.3f, transform.position.y, 0);
        }
        else if (transform.position.x > 8.3f)
        {

            transform.position = new Vector3(8.3f, transform.position.y, 0);
        }*/

        if (transform.position.x > 9.5)
        {
            transform.position = new Vector3(-9.5f, transform.position.y, 0);
        }
        else if (transform.position.x < -9.5)
        {
            transform.position = new Vector3(9.5f, transform.position.y, 0);
        }
    }
}
