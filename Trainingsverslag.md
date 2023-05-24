##Trainings verslag
#### In het volgende document bespreken we hoe de training is verlopen. We behandelen de volgende onderwerpen: Hyperparameters, beloningen/straffen en grafieken.

1. Hyper parameters
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


2. Beloningen/straffen
   - Correcte target = +1
   - Foute target = -1
   - Schieten vanuit de startpositie = -0.2


3. Grafieken