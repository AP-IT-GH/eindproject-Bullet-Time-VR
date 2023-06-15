# Trainings verslag
#### In het volgende document bespreken we hoe de training is verlopen. We behandelen de volgende onderwerpen: Hyperparameters, beloningen/straffen en we bespreken de grafieken.

##### Inleiding
De parameters die we gebruiken zijn voor een specifieke gedragscategorie genaamd "Cowboy" in Unity's ML-Agents, die wordt getraind met behulp van het Proximal Policy Optimization (PPO) algoritme. Hier is een korte uitleg van de belangrijkste hyperparameters die in de gegeven configuratie worden gebruikt:

- `batch_size`: Het aantal ervaringen (samples) dat wordt gebruikt om een enkele optimalisatiestap uit te voeren.
- `buffer_size`: De grootte van de ervaringsbuffer die wordt gebruikt voor het opslaan van ervaringen tijdens het leren.
- `learning_rate`: De snelheid waarmee het algoritme leert. Het bepaalt hoeveel de modelparameters worden aangepast op basis van de leervergelijking.
- `beta`: Een coëfficiënt die de sterkte van de entropieregularisatie bepaalt, waardoor de verkenning van het beleid wordt bevorderd.
- `epsilon`: Een parameter die de clipwaarde bepaalt bij het berekenen van de PPO-objectieve functie.
- `lambd`: De lambda-waarde die wordt gebruikt bij de berekening van de Generalized Advantage Estimation (GAE), die de voordelen van acties schat.
- `num_epoch`: Het aantal optimalisatiestappen per leercyclus.
- `learning_rate_schedule`: Het schema waarmee de leersnelheid lineair wordt verminderd gedurende de training.
- `hidden_units`: Het aantal eenheden (neuronen) in elke verborgen laag van het neurale netwerk.
- `num_layers`: Het aantal verborgen lagen in het neurale netwerk.
- `vis_encode_type`: Het type visual encoding dat wordt gebruikt om visuele invoergegevens te verwerken.
- `user_recurrent`: Een vlag die aangeeft of recurrente neurale netwerken worden gebruikt voor de agent.
- `memory`: Een configuratie voor het geheugen van recurrente neurale netwerken, inclusief de sequentielengte en geheugengrootte.
- `reward_signals`: Configuratie van beloningssignalen voor het trainen van het gedrag van de agent.
- `keep_checkpoints`: Het aantal trainingscheckpoints dat wordt behouden tijdens het trainingsproces.
- `max_steps`: Het maximale aantal stappen dat de agent mag nemen tijdens het trainen.
- `time_horizon`: Het aantal tijdstappen dat wordt gebruikt voor het berekenen van voordeelsschattingen en het vormen van de leerdatabatch.
- `summary_freq`: De frequentie waarmee samenvattingsgegevens worden vastgelegd tijdens het trainingsproces.
- `threaded`: Een vlag die aangeeft of het trainingsproces multithreaded wordt uitgevoerd.

Dit zijn slechts enkele van de hyperparameters die kunnen worden aangepast in het trainingsproces van een ML-Agent in Unity. Door deze waarden aan te passen, kun je het leergedrag van de agent beïnvloeden en optimaliseren voor het specifieke probleem of de taak die je wilt oplossen.

## Hyper parameters:
```
behaviors:
  Cowboy:
    trainer_type: ppo
    hyperparameters:
      batch_size: 128
      buffer_size: 1024
      learning_rate: 0.0003
      beta: 0.03
      epsilon: 0.2
      lambd: 0.95
      num_epoch: 3
      learning_rate_schedule: linear
    network_settings:
      normalize: false
      hidden_units: 128
      num_layers: 2
      vis_encode_type: simple
      memory:
        sequence_length: 64
        memory_size: 128
    reward_signals:
      extrinsic:
        gamma: 0.99
        strength: 1.0
    keep_checkpoints: 5
    max_steps: 10000000
    time_horizon: 64
    summary_freq: 10000
    threaded: true
```
## Trainingen

### Kleuren herkennen aan de hand van een camera
Als eerste opstelling hebben we ervoor gekozen om enkel de camera te gebruiken om kleueren te herkennen.

- Werking
	- We maken een map waarin er 1 agent staat met daarbij 2 verschillende kleuren. Deze 2 verschillende kleur objecten kunnen random van positie veranderen.
	- De agent kan 2 acties uitvoeren. Deze acties zijn beide discrete actions.
		- Links/rechts draaien. Hierbij kan de agent kiezen of hij naar rood of groen kijkt.
		- Schieten. Hiermet kan de agent bevestigen dat hij een bepaalde kleur selecteert.
	- We geven 2 observaties mee aan de agent
		- Zijn eigen rotatie.
		- Of er geschoten is.
- Beloningen/straffen:
	- Correcte kleur aangeduid = AddReward(1)
	- Niet de juiste kleur en/of muur = AddReward(-1)
- Grafieken:
	- Trainings veld om klueren te herkennen.
	![image](https://github.com/AP-IT-GH/eindproject-Bullet-Time-VR/blob/main/Images/Training/Camera_1.jpg)
	- Grafiek tensor bord i.v.m. kleuren herkennen.
	![image](https://github.com/AP-IT-GH/eindproject-Bullet-Time-VR/blob/main/Images/Training/Camera_2t.jpg)
- Besluit:
	- De agent leerde snel dat hij een beloning kreeg door op de kleur groen te schieten. Op het tensor bord is er te zien dat reward in een lineare lijn ligt. Ook heeft de agent na 80k stappen al een reward van bijna 1. Als we het uiteindelijke brein gebruiken is er te zien dat deze correct werkt.

### Kleuren herkennen aan de hand van een camera met continuous actions
Bij deze training gingen we de agent leren om zichzelf rond de daaien en nogsteeds de juiste kleur te schieten.

- Werking:
	- We herbruiken de vorige instellingen.
	- Hierbij gebruijken we een continuous actions waarbij de agent zelf kan rond draaien.
- Grafieken:
	-Grafiek tensor bord i.v.m. kleuren herkennen en zelf rond draaien.
	![image](https://github.com/AP-IT-GH/eindproject-Bullet-Time-VR/blob/main/Images/Training/Camera_3t.jpg)
- Besluit:
	- Aan de grafiek te zien is het duidelijk dat de extra toevoeging geen grote invloed heeft op de resultaten

### Kleur onthouden aan de hand van een camera


////////////

   - Vervolgens hebben wij hier het geheugen aan toegevoegd, dit zorgde voor wat tegenslag.
   ![image](https://github.com/AP-IT-GH/eindproject-Bullet-Time-VR/blob/main/Images/Training/CamMem_2.jpg)

   - Hier de grafiek.
   ![image](https://github.com/AP-IT-GH/eindproject-Bullet-Time-VR/blob/main/Images/Training/CamMem_2t.jpg)
	
   - Zelf hebben we ook logboeken bijgehouden. Hierbij wordt het aantal keren dat we hebben getest vastgelegd. Rood staat voor de juiste target, groen staat voor de vriendelijke target, en FOUT betekent dat       er geen van beide is geraakt. Hieruit kunnen we direct zien dat de agent een 50/50 kans neemt, omdat hij na verloop van tijd alleen naar rechts kijkt, waar hij een kans van 50% heeft.
   ![image](https://github.com/AP-IT-GH/eindproject-Bullet-Time-VR/blob/main/Images/Training/CamMem_1.jpg)
   
