## Een tutorial om ons project te reproduceren
1. Niels Aarts
2. Kevin Lahey
3. Gil Struyf
4. Quinten Moons

###
1. Begin met je omgeving te creeren dit bevat volgende zaken
	- Uw targets, ML Agent en friendly.
	- Importeer alle packeges (ML Agents en XR rig).
	- Importeer de nodige assets die je wilt gebruiken.

2. Design een map
	![image](https://github.com/AP-IT-GH/eindproject-Bullet-Time-VR/blob/main/Images/MAP.JPG)
	- We hebben een pack geimporteerd van verschillende huizen en hiermee onze eigen omgeving gecreerd.
	https://himmelfar.itch.io/low-poly-western-town 

	
3. Scripten
	- Een shoot script met raycast: Deze is om te bepalen wie de target of ander object heeft geraakt.
	  Dit is van belang omdat de snelste het spel zal winnen.
	- Een target script die de schade opneemt en een functie kan activeren (dood gaan of disable).
	- ML agent script met geheugen. We moeten de agent 2 stappen laten doen: de eerste is is schieten en de tweede is omdraaien dit doen we aan de hand van deze functie 

```
		public override void OnActionReceived(ActionBuffers actionBuffers)
    		{
        		this.transform.Rotate(0.0f, rotationMultiplier * actionBuffers.ContinuousActions[0], 0.0f);'

```

```
		public override void OnActionReceived(ActionBuffers actionBuffers)
    		{    // Acties, size = 2;


        		bool shoot = actionBuffers.DiscreteActions[0] == 1;
        		this.transform.Rotate(0.0f, rotationMultiplier * actionBuffers.ContinuousActions[0], 0.0f);
        		/*        print(this.transform.rotation);
                		print(actionBuffers.DiscreteActions[0]);
        		*/
        		if (shoot) // if jump button is pressed and is on the ground
        		{
            		print("Pang");
            		/*            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
                        		if (Physics.Raycast(ray, out hit, 100f))
                        		{
                            		// Check if the raycast hit an object with the specified tag
                            		if (hit.collider.CompareTag("Target"))
                            		{
                                		// Do something if the raycast hit the target object
                                		Debug.Log("Raycast hit target object!");
                                		SetReward(1);

                            		}
                        		}*/

            		if (shootScript != null)
            		{
                		string tag = shootScript.ShootGun();
                		if (tag == "Target")
                		{
                    		print("Target hit");
                    		SetReward(1);
                		} else
                		{
                    		  SetReward(-0.1f);
                		}
            		}

            		EndEpisode();
        		}

    		}
```

4. Trainen
	- Nu onze agent kan draaien en schieten kunnen we het geheugen gaan trainen.
	  Dit door eerst onze target te laten zien, nu moet hij zich omdraaien de target
	  opnieuw zoeken en zo snel mogelijk schieten op de target.

5. Belonen
	- Als de agent de target raakt krijgt die een positieve beloning.
	- Als de agent de target niet raakt krijgt hij een kleine negatieve beloning.
	- Als de agent de friendly raakt krijgt hij een negatieve beloning.

6. Afwerking
	- UI: Een start scherm en reset scherm.
	![image](https://github.com/AP-IT-GH/eindproject-Bullet-Time-VR/blob/main/Images/UI_Example.JPG)