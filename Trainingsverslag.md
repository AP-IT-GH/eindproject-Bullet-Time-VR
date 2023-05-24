##Trainings verslag
#### In het volgende document bespreken we hoe de training is verlopen. We behandelen de volgende onderwerpen: Hyperparameters, beloningen/straffen en we bespreken de grafieken.

#####Inleiding
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

1. Hyper parameters:
```
behaviors:
  Cowboy:
    trainer_type: ppo
    hyperparameters:
      batch_size: 128
      buffer_size: 2048
      learning_rate: 0.0003
      beta: 0.005
      epsilon: 0.2
      lambd: 0.95
      num_epoch: 3
      learning_rate_schedule: linear
    network_settings:
      normalize: false
      hidden_units: 256
      num_layers: 2
      vis_encode_type: simple
      user_recurrent: true
      memory:
        sequence_length: 64
        memory_size: 256
    reward_signals:
      extrinsic:
        gamma: 0.99
        strength: 1.0
    keep_checkpoints: 5
    max_steps: 1000000
    time_horizon: 64
    summary_freq: 10000
    threaded: true
```


2. Beloningen/straffen:
   - Correcte target (de target die wij laten zien voor het geheugen) = +1
   - Foute target (de friendly die de agent moet vermijden) = -1
   - Schieten vanuit de startpositie (op geen van beide, willekeurig) = -0.2


3. Grafieken:
   - We zijn begonnen met het herkennen van kleuren aan de hand van een camera.
   ![image](https://github.com/AP-IT-GH/eindproject-Bullet-Time-VR/blob/main/Images/Training/Camera_1.jpg)

   - Dit gaf redelijk snel positieve resultaten.
   ![image](https://github.com/AP-IT-GH/eindproject-Bullet-Time-VR/blob/main/Images/Training/Camera_2t.jpg)


   - Vervolgens hebben wij hier het geheugen aan toegevoegd, dit zorgde voor wat tegenslag.
   ![image](https://github.com/AP-IT-GH/eindproject-Bullet-Time-VR/blob/main/Images/Training/CamMem_2.jpg)

   - Hier de grafiek.
   ![image](https://github.com/AP-IT-GH/eindproject-Bullet-Time-VR/blob/main/Images/Training/CamMem_2t.jpg)
	
   - Zelf hebben we ook logboeken bijgehouden. Hierbij wordt het aantal keren dat we hebben getest vastgelegd. Rood staat voor de juiste target, groen staat voor de vriendelijke target, en FOUT betekent dat     er geen van beide is geraakt. Hieruit kunnen we direct zien dat de agent een 50/50 kans neemt, omdat hij na verloop van tijd alleen naar rechts kijkt, waar hij een kans van 50% heeft.
   ![image](https://github.com/AP-IT-GH/eindproject-Bullet-Time-VR/blob/main/Images/Training/CamMem_1.jpg)
   