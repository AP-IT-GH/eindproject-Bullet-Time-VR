## Een tutorial om ons project te reproduceren
1. Niels Aarts
2. Kevin Lahey
3. Gil Struyf
4. Quinten Moons

###
1. Begin met je omgeving te creeren dit bevat volgende zaken
	a. Uw targets, ML Agent en friendly.
	b. Importeer alle packeges (ML Agents en XR rig).
	c. Importeer de nodige assets die je wilt gebruiken.

2. Design een map

3. Scripten
	a. Een shoot script met raycast.
	b. Een target script die de schade opneemt en een functie kan activeren (dood gaan of disable).
	c. ML agent script met geheugen.

4. Trainen
	a. Nu onze agent kan draaien en schieten kunnen we het geheugen gaan trainen.
	   Dit door eerst onze target te laten zien, nu moet hij zich omdraaien de target
	   opnieuw zoeken en zo snel mogelijk schieten op de target.

5. Belonen
	a. Als de agent de target raakt krijgt die een positieve beloning.
	b. Als de agent de target niet raakt krijgt hij een kleine negatieve beloning.
	c. Als de agent de friendly raakt krijgt hij een negatieve beloning.