## Een tutorial om ons project te reproduceren
Studenten:
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
	- ML agent script met geheugen. We moeten de agent 2 stappen laten doen: het eerste is omdraaien dit doen we aan de hand van deze functie:

```
		public override void OnActionReceived(ActionBuffers actionBuffers)
    		{
        		this.transform.Rotate(0.0f, rotationMultiplier * actionBuffers.ContinuousActions[0], 0.0f);'

```
De tweede is het schieten:

```
		public override void OnActionReceived(ActionBuffers actionBuffers)
    		{
        		bool shoot = actionBuffers.DiscreteActions[0] == 1;
        		this.transform.Rotate(0.0f, rotationMultiplier * actionBuffers.ContinuousActions[0], 0.0f);
        		if (shoot) // if jump button is pressed and is on the ground
        		{
            		print("Pang");

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
	  opnieuw zoeken en zo snel mogelijk schieten op de target. Verdere informatie over het trainen van de agent (zoals grafieken).
	  Vindt u in het trainingsverslag: https://github.com/AP-IT-GH/eindproject-Bullet-Time-VR/blob/main/Trainingsverslag.md

5. Belonen
	- Als de agent de target raakt krijgt die een positieve beloning.
	- Als de agent de target niet raakt krijgt hij een kleine negatieve beloning.
	- Als de agent de friendly raakt krijgt hij een grotere negatieve beloning.

6. Afwerking
	- UI: Een start scherm en reset scherm.
	![image](https://github.com/AP-IT-GH/eindproject-Bullet-Time-VR/blob/main/Images/UI_Example.JPG)