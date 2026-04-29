using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player Instance;

    public int health = 5;
    public Image healthRedImage;
    public UnityEvent onDeath;
    void Start()
    {
        Player.Instance = this;
    }

    public void takeDamage(int damage = 1)
    {
        health -= damage;
        Debug.Log(health);

        if (healthRedImage != null)
        {
            switch (health)
            {
                case 5:
                    healthRedImage.color = new Color(96, 0, 0, 0f);
                    break;
                case 4:
                    healthRedImage.color = new Color(96, 0, 0, .02f);
                    break;
                case 3:
                    healthRedImage.color = new Color(96, 0, 0, .04f);
                    break;
                case 2:
                    healthRedImage.color = new Color(96, 0, 0, .06f);
                    break;
                case 1:
                    healthRedImage.color = new Color(96, 0, 0, .08f);
                    break;
                case 0:
                    healthRedImage.color = new Color(96, 0, 0, .10f);
                    GetComponent<Animator>().Play("Death_anim");
                    break;
                default:
                    healthRedImage.color = new Color(96, 0, 0, 0f);
                    Debug.Log("error");
                    break;
            }
        }
    }

    public void quit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu_scene");
    }

    public void restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void die()
    {
        onDeath.Invoke();
        Time.timeScale = 0f;
    }
}
