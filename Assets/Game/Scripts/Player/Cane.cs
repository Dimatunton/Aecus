using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Cane : MonoBehaviour
{
    public float hugeSonarInterval = 2f;
    public enemy_damage enemy_Damage;
    public AudioClip sonarSFX;


    private AudioSource audioSource;
    private Rigidbody rb;
    private bool isGrabbed = false;
    private XRGrabInteractable grabComponent;
    GameObject sonarScanPrefab;

    Coroutine onTapCoroutine = null;


    private void Awake()
    {
        sonarScanPrefab = Resources.Load<GameObject>("Prefabs/SonarScan_prefab");
    }
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        grabComponent = GetComponent<XRGrabInteractable>();
    }

    private void FixedUpdate()
    {
        if (!isGrabbed)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(Vector3.zero);
        }
    }
    public void selected()
    {
        enemy_Damage.enabled = true;
        isGrabbed = true;
    }

    public void unselected()
    {
        isGrabbed = false;
        enemy_Damage.enabled = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(grabComponent.interactorsSelecting.Count > 0) // add && isgrabbed? so it will only sonar when selected like damage only enemy on grabbed in selected() and unselected()
        {
            if (collision.gameObject.GetComponent<SimpleEnemyAI>())
            {
                collision.gameObject.GetComponent<SimpleEnemyAI>().hit(1);
            }
            if (onTapCoroutine == null)
            {
                onTapCoroutine = StartCoroutine(onTap(collision.contacts[0].point));
            }
            else
            {
                sonarScanSpawn(collision.contacts[0].point, 5f);
            }
        }
    }
    
    IEnumerator onTap(Vector3 contactPoint)
    {
        audioSource.PlayOneShot(sonarSFX);
        sonarScanSpawn(contactPoint, 15f);
        yield return new WaitForSeconds(hugeSonarInterval);
        onTapCoroutine = null;
    }

    void sonarScanSpawn(Vector3 position,float range)
    {
        GameObject SC = Instantiate(sonarScanPrefab, position, Quaternion.identity);
        SC.GetComponent<SonarScan>().scanRange = range;
        SC.SetActive(true);
    }
}
