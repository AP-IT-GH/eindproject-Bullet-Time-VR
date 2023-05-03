using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shoot : MonoBehaviour
{

    //Bullet & shoot
    public GameObject projectile;
    public AudioSource GunShot;
    public AudioSource ReloadClip;

    // Reload variables
    public int maxAmmo = 15;
    private int currentAmmo;

    //public Text AmmoText;
    public float reloadTime = 1f;
    private bool isReloading = false;

    //public Animator animator;

    //ADS
    public GameObject Gun;

    //Shooting
    public float damage = 10f;
    public float range = 100f;

    public Camera fpsCam;

    //Muzzle Flare
    public GameObject MuzzleFlash;

    // Use this for initialization
    void Start()
    {
        //reload
        currentAmmo = maxAmmo;
        //AmmoText.text = currentAmmo.ToString("Ammo: " + currentAmmo);
    }

    void OnEnable()
    {
        isReloading = false;
        //animator.SetBool("Reloading", false);
    }

    // Update is called once per frame
    float timer = 10f;
    bool start = false;
    public float shootRate = 1f;

    void Update()
    {
        if (isReloading)
            return;

        //ADS
        if (Input.GetMouseButtonDown(1))
        {
            Gun.GetComponent<Animator>().Play("ADS");
        }

        if (Input.GetMouseButtonUp(1))
        {
            Gun.GetComponent<Animator>().Play("New State");
        }

        float shoot = Input.GetAxis("Fire1");

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        //shoot
        if (shoot == 1 && timer >= shootRate)
        {
            Shoot();
            //Ammunition
            currentAmmo--;
            //AmmoText.text = currentAmmo.ToString("Ammo: " + currentAmmo);

            //Muzzle flash
            //MuzzleFlash.SetActive(true);

            //Recoil
            //GetComponent<ProceduralRecoil>().recoil();

            //gameObject.GetComponent<Animator>().Play("shoot");
            GameObject newProjectile = Instantiate(projectile, transform.position + transform.forward, transform.rotation);
            newProjectile.GetComponent<Rigidbody>().AddForce(transform.forward * 100, ForceMode.VelocityChange);
            start = true;
            timer = 0f;
            //GunShot.Play();
        }

        if (start)
        {
            if (timer < shootRate)
                timer += Time.deltaTime;
            else
            {
                timer = shootRate;
                start = false;
            }
        }
    }

    IEnumerator Reload()
    {
        Debug.Log("reloading");
        isReloading = true;
        ReloadClip.Play();

        //animator.SetBool("Reloading", true);
        yield return new WaitForSeconds(reloadTime - .25f);
        //animator.SetBool("Reloading", false);
        yield return new WaitForSeconds(.25f);

        currentAmmo = maxAmmo;

        isReloading = false;
    }

    void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }
    }
}