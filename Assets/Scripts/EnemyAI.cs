using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float _speed = 3.0f;
    [SerializeField] private GameObject _enemyExplosionPrefab;
    [SerializeField] private AudioClip _clip;

    private UIManager _uiManager;

    // Start is called before the first frame update
    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if(transform.position.y < -7)
        {
            transform.position = new Vector3(Random.Range(-8.0f, 8.0f), 7, 0);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Laser")
        {
            Destroy(other.gameObject);
            Instantiate(_enemyExplosionPrefab, transform.position, Quaternion.identity);
            _uiManager.UpdateScore();
            //AudioSource.PlayClipAtPoint(_clip, Camera.main.transform.position);
            AudioSource.PlayClipAtPoint(_clip, new Vector3(0, 0, -10));
            Destroy(this.gameObject);
        }
        else if(other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            Instantiate(_enemyExplosionPrefab, transform.position, Quaternion.identity);
            _uiManager.UpdateScore();
            //AudioSource.PlayClipAtPoint(_clip, Camera.main.transform.position);

            AudioSource.PlayClipAtPoint(_clip, new Vector3(0, 0, -10));
            Destroy(this.gameObject);
        }
    }
}
