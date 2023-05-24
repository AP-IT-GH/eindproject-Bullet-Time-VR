##Trainings verslag
#### In het volgende document bespreken we hoe de training is verlopen. We behandelen de volgende onderwerpen: Hyperparameters, beloningen/straffen en we bespreken de grafieken.

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
   ![image](https://github.com/AP-IT-GH/eindproject-Bullet-Time-VR/blob/main/Images/Training/Camera_2.jpg)

   - Dit gaf redelijk snel positieve resultaten.
   ![image](https://github.com/AP-IT-GH/eindproject-Bullet-Time-VR/blob/main/Images/Training/Camera_2t.jpg)


   - Vervolgens hebben wij hier het geheugen aan toegevoegd, dit zorgde voor wat tegenslag.
   ![image](https://github.com/AP-IT-GH/eindproject-Bullet-Time-VR/blob/main/Images/Training/CamMem_2.jpg)

   - Hier de grafiek.
   ![image](https://github.com/AP-IT-GH/eindproject-Bullet-Time-VR/blob/main/Images/Training/CamMem_2t.jpg)
	
   - Zelf hadden wij ook nog eens gelogged. Waarbij shoot het aantal keren da we testen, rood is de juiste target, groen is de friendly target en FOUT is op geen van beide.
     Hier zien we meteen dat de agent 50/50 kans pakt omdat hij na een tijd enkel naar rechts kijkt omdat hij daar 50% kans heeft.
   ![image](https://github.com/AP-IT-GH/eindproject-Bullet-Time-VR/blob/main/Images/Training/CamMem_1.jpg)
   