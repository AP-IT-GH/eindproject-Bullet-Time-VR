# ML-Agent Gun-Dual Game

Studenten:

1. Niels Aarts
2. Kevin Lahey
3. Gil Struyf
4. Quinten Moons

**INHOUD**

[Tutorial](https://github.com/AP-IT-GH/eindproject-Bullet-Time-VR/blob/main/Tutorial.md#een-tutorial-om-ons-project-te-reproduceren)

[Informatie one-pager](https://github.com/AP-IT-GH/eindproject-Bullet-Time-VR/blob/main/Tutorial.md#informatie-one-pager)

[Trainingsverslag](https://github.com/AP-IT-GH/eindproject-Bullet-Time-VR/blob/main/Tutorial.md#trainingsverslag)

[Conclusie](https://github.com/AP-IT-GH/eindproject-Bullet-Time-VR/blob/main/Tutorial.md#conclusie)

[Bronnen](https://github.com/AP-IT-GH/eindproject-Bullet-Time-VR/blob/main/Tutorial.md#bronvermelding)

## Een tutorial om ons project te reproduceren

In het onderstaande document wordt onze werkwijze, de gemaakte keuzes en de gebruikte materialen en de vooruitgang besproken.

### Inleiding

Bij het betreden van ons meeslepende spel wordt de spelers kort geïnformeerd over het doel en de uitdagingen die hen te wachten staan. Zodra de "Start" knop wordt ingedrukt, begint een zinderende countdown en wordt de tegenstander, de AI, geactiveerd. Zodra de tijd begint te tikken, is het aan de spelers en de AI om met precisie en snelheid het juiste doelwit te raken, want falen betekent verlies. Het is essentieel om het juiste doelwit te raken, foutieve doelwitten te vermijden, geen enkel schot te missen en op tijd te schieten. Alleen door sneller en trefzekerder te zijn dan de AI, kan de speler de ultieme overwinning behalen.

### Werkwijze

1. Begin met je omgeving te creeren dit bevat volgende zaken

   - Uw targets, ML Agent en friendly.
   - Importeer alle packages (ML Agents en XR rig).
     ![image](https://github.com/AP-IT-GH/eindproject-Bullet-Time-VR/blob/main/Images/Versies.png)
   - Importeer de nodige assets die je wilt gebruiken.

2. Design een map
   ![image](https://github.com/AP-IT-GH/eindproject-Bullet-Time-VR/blob/main/Images/MAP.JPG)

   - We hebben een pack geïmporteerd van verschillende huizen en hiermee onze eigen omgeving gecreëerd.
     https://himmelfar.itch.io/low-poly-western-town

   Assets:
   Voor de map gebruiken we:

   - eretichable Technologies
   - Low-poly Western Town

   Voor audio gebruiken we:

   - MaGuns

3. Objecten

   - Pistool: Je kunt dit object oppakken en hiermee schieten.
   - ML-Agent: Dit is de agent die een pistool vast heeft en a.d.h.v. een camera de juiste target zal raken.
   - 2 kleuren spheres: Er is een rode en een groene sphere die op random plaatsen spawnen zodat het niet altijd opdezelfde plek is.

4. Verloop van het spel
   Als je het spel start dan krijg je een startscherm, hier wordt het spel uitgelegd. Als je dan op de Begin knop drukt, dan krijg je een wanted poster te zien met de persoon waar jij op moet schieten. Druk je dan op de start knop dan krijg je een countdown te zien die van 5 tot 0 telt. Als deze op 0 is gekomen dan moet je zo snel mogelijk omdraaien, het geweer oppakken en de juiste persoon neerschieten. Heb je de juiste persoon neergeschoten dan krijg je een scherm te zien dat zegt dat je gewonnen hebt en vraagt of je opnieuw wilt spelen. Ben je tragen dan de ML-Agent dan krijg je een scherm te zien dat zegt dat je verloren hebt en vraagt of je opnieuw wilt spelen. Hebben zowel jij als je de ML-Agent niemand neergeschoten dan krijg je een scherm te zien dat zegt dat jullie allebei verloren hebben en vraagt of je opnieuw wilt spelen. Je kunt ook een scherm openen dat aan je linker hand is verbonden waarmee je het spel kunt resetten.

5. Scripten
   - Een shoot script met raycast: Deze is om te bepalen wie de target of ander object heeft geraakt.
     Dit is van belang omdat de snelste het spel zal winnen.
   - Een target script die de schade opneemt en een functie kan activeren (dood gaan of disable).
   - ML agent script met geheugen. We moeten de agent 2 acties laten doen: het eerste is omdraaien dit doen we aan de hand van deze rotate() functie. Waarbij de MLAgent een continuou action kan gebruiken die we dus koppelen aan een rotatie. Samen met een rotationMultiplier om af te stellen hoe snel de MLAgent kan draaien.

```
		public override void OnActionReceived(ActionBuffers actionBuffers)
    		{
        		this.transform.Rotate(0.0f, rotationMultiplier * actionBuffers.ContinuousActions[0], 0.0f);'

```

De tweede is het schieten, Het is natuurlijk ook handig dat onze agent kan schieten. Dit gebeurt door een discrete action te gebruiken. Aangezien schieten geen continuou iets is. Het is schieten (value = 1) of niet schieten (alle andere values), hiervoor dat we een "actionBuffers.DiscreteActions[0] == 1" checken dan geeft hij een bool terug en is het gemakkelijker om met de werken doorheen onze code. Als deze bool true wordt zullen we een functie van een extern script (shootScript.Shoot()) aanspreken om dus te schieten. Dit shoot script zal een tag string terug geven van het object dat hij heeft geraakt, hiervan kunnen we bepalen of hij goed geschoten heeft of niet om al dan niet te belonen.

```
public override void OnActionReceived(ActionBuffers actionBuffers)

    {
        bool shoot = actionBuffers.DiscreteActions[0] == 1;
        transform.Rotate(0.0f, 2.0f * actionBuffers.ContinuousActions[0], 0.0f);

        if (shoot && !shot)
        {
            shot = true;
            print("Shoot");
            string tag = shootScript.Shoot();

            if (tag == "Friendly")
            {
                AddReward(1f);
            }
            else if(tag == "Target")
            {
                AddReward(-1f);
            }
            else
            {
                AddReward(-0.5f);
            }

            print("cummulative: " + GetCumulativeReward());
            EndEpisode();
        }

    }
```

6. Trainen

   - Nu onze agent kan draaien en schieten kunnen we het geheugen gaan trainen.
     Dit door eerst onze target te laten zien, nu moet hij zich omdraaien de target
     opnieuw zoeken en zo snel mogelijk schieten op de target. Verdere informatie over het trainen van de agent (zoals grafieken). Vindt u onderaan bij het trainingsverslag.

7. Beloningen

   - Als de agent de target raakt krijgt die een positieve beloning.
   - Als de agent de target niet raakt krijgt hij een kleine negatieve beloning.
   - Als de agent de friendly raakt krijgt hij een grotere negatieve beloning.

8. Afwerking
   - UI: Een start scherm en reset scherm.
     ![image](https://github.com/AP-IT-GH/eindproject-Bullet-Time-VR/blob/main/Images/UI_Example.JPG)

## Informatie one-pager

### Wat is het doel van de VR toepassing

Een leuke ervaring met als focus op entertainment.

### Hoe zal de VR wereld er ongeveer uitzien

Western met dessert thema. Er zullen 2 objecten tevoorschijn komen 1 als target en 1 friendly.

De agent zal moeten onthouden wie de target is en wie friendly om later te bepalen op wie die moet schieten.

Jij (de speler) en de ML agent worden naast elkaar geplaatst en zullen om ter snelst de target raken. Wie deze als eerst raakt wint de game.

We kunnen uiteindelijk verschillende agents trainen voor verschillende moeilijkheden. (1 minder trainen en 1 harder trainen).

### Wat is de functie van de ML-Agent

Uw tegenspeler met geheugen. We moeten beide op een target schieten wie het snelste is, wint.

### Hoe wordt de agnt beloond tijdens de training

We gaan dit in verschillende stadia doen

- De agent moet onthouden wat hij gezien heeft. Om hierna een positieve beloning te geven als hij de correcte item aanduid. En een negatieve als hij het foute item aanduid.

- Als de agent het verschil heeft geleerd tussen de target en de friendly zal hij een wapen krijgen waarmee hij kan schieten op deze items. Als hij het juiste target aanschiet krijgt hij een positieve beloning. Anders krijgt hij een negatieve beloning.

### Afwijkingen van de one-pager

We maken voor de eind toepassing niet gebruik van het geheugen. Dit doen we omdat het geheugen getraind is om enkel de optie links of rechts te kiezen. We konden de agent niet opnieuw trainen waarbij deze zelf kon rond draaien.

## Trainingsverslag

Hierond vindt u terug hoe wij onze agent hebben getraind. In ons eindresultaat maken we gebruik van de agent die de correcte kleur kan aanduiden, waarom we dit doen zal u hieronder kunnen lezen.

### Parameters

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

**Hyper parameters**

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

Onze trainings file kan u terug vinden op volgende [link](https://github.com/AP-IT-GH/eindproject-Bullet-Time-VR/blob/main/Bullet-Time-VR/Assets/ML%20Agent/config/Cowboy.yaml). Deze trainings file werd gebruikt voor alle trainingen die we hebben uitgevoerd in dit project. Om het gedeelte memory aan of uit te zetten moesten we dit gedeelte toevoegen/verwijderen.

### Trainingen

We hebben de ML-Agent in stappen geprogrameerd zodat we altijd iets hadden om te presenteren indien een volgende stap niet zou werken.

#### Kleuren herkennen a.d.h.v. een camera

We zijn begonnen om de agent te gebruiken om kleuren te herkennen.

- Werking
  1.  Als eerste hebben we een agent gemaakt die 2 actie's kan uitvoeren. Deze kan schieten en heeft de keuze om link/rechts te draaien. Bij het draaien zal de hoek naarwaar de agent kijkt worden aangepast naar 1 van de 2 kleuren. (2 discrete actions) Als de agent naar de kleur groen kijkt en schiet heeft hij gewonnen. De kleur groen en rood veranderen van positie zodat de agent de kleur moet herkennen en niet zijn rotatie kan onthouden. De observaties dat de agent mee krijgt zijn de rotatie en of hij heeft geschoten of niet. Ook heeft de agent de camera sensor met een widh en height van 48x48.
  2.  Als tweede hebben we de agent de mogelijkheid gegeven om deze vrij te laten ronddraaien. (1 discrete action en 1 continuous action)
- Beloningen/straffen
  - Correcte kleur beschoten = AddReward(1)
  - Niet de juiste kleur beschoten = AddReward(-1)
  - De muur geraakt = AddReward(-0.5)
- Test opstelling
  - Als trainings veld gebruiken we 2 spheres met 2 verchillende kleuren. De kleur groen is in het script ingesteld dat deze de correcte kleur is en waarvoor hij een reward zal ontvangen.
  - ![image](https://github.com/AP-IT-GH/eindproject-Bullet-Time-VR/blob/main/Images/Training/Camera_1.jpg)
- Grafieken
  1.  ![image](https://github.com/AP-IT-GH/eindproject-Bullet-Time-VR/blob/main/Images/Training/Camera_2t.jpg)
      - In de grafiek is te zien dat de agent na 80k stappen doorhad welke sphere hij een hoge reward opleverden. De grafiek heeft een linear verband.
  2.  ![image](https://github.com/AP-IT-GH/eindproject-Bullet-Time-VR/blob/main/Images/Training/Camera_3t.jpg)
      - Bij deze grafiek is er de mogelijkheid toegevoed dat de agent zelf rond draait. Het verschil met de vorige grafiek is dat er een kleinere stijging is van de reward. Dit komt waarschijnlijk doordat de agent ook moest leren rond draaien. De grafiek heeft ook een linear verband.
- Conclusie
  - Het trainen van de agent met de camera verliep zeer vlot. De agent had snel door welke acties hij moest uitvoeren om een goede beloning te ontvangen.

#### Kleuren onthouden a.d.h.v. een camera en geheugen

In dit gedeelte hebben we memory toegevoegd aan de agent zodat deze de eerste kleur dat hij zag kan onthouden en hierna op de juiste kan schieten. Om gebruik te maken van het memory gedeelte in een ML agent moeten we dit in de config.yaml file aanpassen.

- Werking
  1.  De agent wordt geplaats met de camera richting een rode of groene kleur. Hierna kan de agent kiezen voor links of rechts en zal dan naar een andere kleur draaienen. Hierbij kan hij ook de actie uitoveren om te schieten en zal er gecontroleerd worden of dit dezelfde kleur was als de agent als eerste zag. (2 discrete actions) De rode en groene kleur veranderen van positie en zullen ook van kleur veranderen op de startpositie.
- Beloningen/straffen
  - Correcte kleur beschoten = AddReward(1)
  - Niet de juiste kleur en/of de muur beschoten = AddReward(-1)
- Test opstelling
  - Als trainings veld gebruiken we 2 spheres met 2 verschillende kleuren. Deze speheres veranderen van positie als er een nieuwe episode start. Ook bevindt er zich aan 1 zijde van het veld een extra sphere die de kleur rood of groen zal bevatten. Dit is de kleur die de agent moet onthouden.
  - ![image](https://github.com/AP-IT-GH/eindproject-Bullet-Time-VR/blob/main/Images/Training/CamMem_2.jpg)
- Grafieken
  1.  ![image](https://github.com/AP-IT-GH/eindproject-Bullet-Time-VR/blob/main/Images/Training/CamMem_3t.jpg)
      - In de bovestaande grafiek is te zien dat er veel verschil in de grafiek zit. Het gedeelte van 0.5M tot 4M zit de reward tussen 0.4 en 0.5 te schommelen. De reden hiervoor kan zijn dat de agent 1/2 kans heeft dat het, het rode element of het groene element moet raken. Na 4M daalt de reward weer tot tussen 0.2 en -0.2 waarna hij linear stijgt totdat hij stabiel is bij 9M.
  2.  ![image](https://github.com/AP-IT-GH/eindproject-Bullet-Time-VR/blob/main/Images/Training/CamMem_4.jpg)
      - Deze resultaten zijn van het brein dat we 5 minuten hebben laten runnen. Hieruit kunnen we afleiden dat hij van de 362 maar 2 keer fout heeft geschoten. En kunnen concluderen dat het brein werkt.
- Conclusie
  - Uit de resultaten kan ik afleiden dat het werken met geheugen een grote invloed heeft op deze resultaten. Ook is er een groot verschil in het aantal stappen dat we deze agent moesten laten trainen tegenover als we enkel de camera gebruiken.

## Conclusie

Ons westernspel is ontworpen om spelers uit te dagen om zo snel mogelijk een doelwit te raken, terwijl ze het opnemen tegen een AI-tegenstander. We hebben ons best gedaan om ervoor te zorgen dat het spel zowel speelbaar als eerlijk is, zodat spelers een leuke en uitdagende ervaring hebben.

Onze huidige AI-agent presteert over het algemeen goed en kan effectief reageren op de acties van spelers. Echter, toen we probeerden om een getrainde agent met geheugen te implementeren in onze VR-omgeving, ondervonden we enkele problemen. Om die reden raden we het gebruik van geheugen in onze toepassing af. Het had een negatieve invloed op de algehele prestaties en speelbaarheid van het spel. We hebben de moeilijkheid ervaren om het geheugen effectief te integreren in de VR-omgeving en het spelveld, waardoor de ervaring voor spelers minder bevredigend werd.

Hoewel we begrijpen dat het gebruik van geheugen in sommige gevallen voordelig kan zijn, hebben we besloten om deze functie niet te gebruiken in onze huidige toepassing. We zijn nog steeds tevreden met hoe onze AI-agent functioneert zonder het geheugen en geloven dat het spel een aantrekkelijke uitdaging biedt aan spelers.

In de toekomst zouden er verschillende moeilijkheids graden gekozen kunnen worden. Dit zou dan bv gedaan kunnen worden door een agent maar een x aantal stappen te laten trainen waardoor deze beter/slechter wordt.

## Bronvermelding

1. Marwan Mattar, (2018, Maart 14). Memory-enhanced Agents using Recurrent Neural Networks. https://github.com/miyamotok0105/unity-ml-agents/blob/master/docs/Feature-Memory.md
2. Unity Technologies, (2022). Training Configuration File. https://unity-technologies.github.io/ml-agents/Training-Configuration-File/#memory-enhanced-agents-using-recurrent-neural-networks
3. Matt Cone, (2023). Markdown Cheat sheet. https://www.markdownguide.org/cheat-sheet/
4. Unity Technologies, (2023). Create with VR. https://learn.unity.com/course/create-with-vr?uv=2021.3
