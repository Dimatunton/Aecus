using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Shotgun : MonoBehaviour
{
    public Transform gunpont = null;
    public TextMeshProUGUI ammoText = null;

    public float ShotgunSpread = .2f;
    public int bulletCount = 2;


    public AudioClip fireSFX;
    public AudioClip reloadSFX;
    private AudioSource audioSource;

    public GameObject fireParticles;

    private Rigidbody rb;
    private bool isGrabbed = false;
    private XRGrabInteractable grabComponent;

    GameObject sonarScanPrefab;

    private void Awake()
    {
        sonarScanPrefab = Resources.Load<GameObject>("Prefabs/SonarScan_prefab");
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        grabComponent = GetComponent<XRGrabInteractable>();
        audioSource = GetComponent<AudioSource>();
        updateBulletCount();
    }

    private void FixedUpdate()
    {
        if (!isGrabbed)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(new Vector3(90, 0, 0));
        }
    }

    public void selected()
    {
        isGrabbed = true;
    }
    public void unselected()
    {
        if(grabComponent.interactorsSelecting.Count < 1)
        {
            isGrabbed = false;
        }
    }

    public void activated()
    {
        if(bulletCount > 0)
        {
            print(bulletCount);
            fireParticles.SetActive(false);
            fireParticles.SetActive(true);
            audioSource.PlayOneShot(fireSFX,.5f);
            for (int i = 0; i < 5; i++)
            {
                float tempShotgunSpread = ShotgunSpread;
                if (grabComponent.interactorsSelecting.Count < 2)
                {
                    tempShotgunSpread = ShotgunSpread + .3f;
                }

                float xrandom = Random.Range(-tempShotgunSpread, tempShotgunSpread);
                float yrandom = Random.Range(-tempShotgunSpread, tempShotgunSpread);
                float zrandom = Random.Range(-tempShotgunSpread, tempShotgunSpread);
                RaycastHit hit;

                Physics.Raycast(new Vector3(gunpont.position.x + xrandom, gunpont.position.y + yrandom, gunpont.position.z + zrandom), gunpont.forward, out hit, 25f);

                if(hit.collider != null)
                {
                    float sonarScale = Random.Range(2f, 4f);
                    GameObject SC = Instantiate(sonarScanPrefab, hit.point, Quaternion.identity);
                    SC.GetComponent<SonarScan>().scanRange = sonarScale;
                    SC.SetActive(true);

                    SimpleEnemyAI ai;
                    if (hit.collider.TryGetComponent<SimpleEnemyAI>(out ai))
                    {
                        ai.hit(1);
                    }
                }
            }
            bulletCount--;
            updateBulletCount();
        }
    }

    public bool reload()
    {
        if (bulletCount > 1)
        {
            updateBulletCount();
            return false;
        }
        else
        {
            bulletCount++;
            updateBulletCount();
            return true;
        }
    }

    void updateBulletCount()
    {
        audioSource.PlayOneShot(reloadSFX);
        ammoText.text = bulletCount.ToString();
        print("Bullet Count is :" + bulletCount);
    }
}
