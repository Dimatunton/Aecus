using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TempButton : Interactable
{

    public Sprite buttonPressedSprite;
    public UnityEvent onButtonPressedEvent;

    public override void onInteract(Transform player)
    {
        StartCoroutine(onButtonPressed());
    }

    IEnumerator onButtonPressed()
    {
        GetComponent<Image>().color = Color.red;
        yield return new WaitForSeconds(.25f);
        SceneManager.LoadScene("1_FirstLevel");
        onButtonPressedEvent.Invoke();
    }
}
