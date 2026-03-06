using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class buildBurger : MonoBehaviour
{

    bool BottomBread = false;
    bool patty = false;
    bool Cheese = false;
    bool Tomato = false;
    bool Onion = false;
    bool Letuce = false;
    bool TopBread = false;

    public GameObject BottomBreadGameobject;
    public GameObject pattyGameobject;
    public GameObject CheeseGameobject;
    public GameObject TomatoGameobject;
    public GameObject OnionGameobject;
    public GameObject LetuceGameobject;

    public GameObject fullBurger;

    private void OnTriggerEnter(Collider other)
    {

        

        if(other.name == "Bread" && !BottomBread)
        {
            BottomBread = true;
            BottomBreadGameobject.SetActive(true);
            
        }
        else if (other.name == "Patty" && !patty && BottomBread)
        {
            patty = true;
            pattyGameobject.SetActive(true);
        }
        else if (other.name == "Cheese" && !Cheese && patty)
        {
            Cheese = true;
            CheeseGameobject.SetActive(true);
        }
        else if (other.name == "Tomato" && !Tomato && Cheese)
        {
            Tomato = true;
            TomatoGameobject.SetActive(true);
        }
        else if (other.name == "Onion" && !Onion && Tomato)
        {
            Onion = true;
            OnionGameobject.SetActive(true);
        }
        else if (other.name == "Letuce" && !Letuce && Onion)
        {
            Letuce = true;
            LetuceGameobject.SetActive(true);
        }
        else if (other.name == "Bread" && !TopBread && Letuce)
        {
            TopBread = true;
            fullBurger.SetActive(true);
            gameObject.SetActive(false);
        }
        else
        {
            return;
        }
        other.gameObject.SetActive(false);
        other.gameObject.SetActive(true);
    }
}
