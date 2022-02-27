using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] private float speed = 3.0f;
    [SerializeField] private int powerupId; // 0= tripleshot, 1= speed boost, 2=shield
    [SerializeField] private AudioClip _clip;

    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        if (transform.position.y < -7)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            /*Debug.Log(_clip.name);
            if (_clip != null)
            {
                AudioSource.PlayClipAtPoint(_clip, Camera.main.transform.position, 1f);
            }*/

            AudioSource.PlayClipAtPoint(_clip, new Vector3(0, 0, -10));
            if (player != null) 
            {
                // enable triple shot
                if (powerupId == 0)
                {
                    player.TripleShotPowerupOn();
                }
                else if(powerupId == 1)
                {
                    // enable speed boost
                    player.SpeedBoost();
                }
                else if(powerupId == 2)
                {
                    // enable shield
                    player.EnableShields();
                }
            }

            Destroy(this.gameObject);   
        }
    }
}
