using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class startButton : Interactable
{

   public Sprite buttonPressedSprite;

    public override void onInteract(Transform player)
    {
        StartCoroutine(onButtonPressed());
    }

    IEnumerator onButtonPressed()
    {
        GetComponent<Image>().color = Color.red;
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("1_FirstLevel");
    }
}
