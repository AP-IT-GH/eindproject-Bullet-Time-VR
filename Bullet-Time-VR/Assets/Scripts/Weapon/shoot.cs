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
    }

    // Update is called once per frame
    float timer = 10f;
    bool start = false;
    public float shootRate = 1f;

    void Update()
    {
        if (isReloading)
            return;


        float shoot = Input.GetAxis("Fire1");

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        //shoot
        if (shoot == 1 && timer >= shootRate)
        {
            //Shoot();


            //Ammunition
            currentAmmo--;
            //AmmoText.text = currentAmmo.ToString("Ammo: " + currentAmmo);

            //Muzzle flash
            //MuzzleFlash.SetActive(true);
            
            start = true;
            timer = 0f;
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

        yield return new WaitForSeconds(reloadTime - .25f);
        yield return new WaitForSeconds(.25f);

        currentAmmo = maxAmmo;

        isReloading = false;
    }

    public string Shoot()
    {
        // Gunshot sounds
        GunShot.Play();

        // Bullet aanmaken
        GameObject newProjectile = Instantiate(projectile, transform.position + transform.forward, transform.rotation);
        newProjectile.GetComponent<Rigidbody>().AddForce(transform.forward * 100, ForceMode.VelocityChange);

        // Perform raycast
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            // Draw raycast in scene view
            Debug.DrawRay(fpsCam.transform.position, fpsCam.transform.forward * hit.distance, Color.green, 5f);

            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
            return hit.collider.gameObject.tag;
        }

        // Draw raycast in scene view
        Debug.DrawRay(fpsCam.transform.position, fpsCam.transform.forward * range, Color.red);

        return null;
    }


    public void ShootNoReturn()
    {
        //Gunshot sounds
        GunShot.Play();

        //Bullet aanmaken
        GameObject newProjectile = Instantiate(projectile, transform.position + transform.forward, transform.rotation);
        newProjectile.GetComponent<Rigidbody>().AddForce(transform.forward * 100, ForceMode.VelocityChange);

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