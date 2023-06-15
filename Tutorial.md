# Bullet Time VR

Studenten:
1. Niels Aarts
2. Kevin Lahey 
3. Gil Struyf 
4. Quinten Moons

## Een tutorial om ons project te reproduceren
In het onderstaande document wordt onze werkwijze, de gemaakte keuzes en de gebruikte materialen en de vooruitgang besproken.

### Inleiding
Bij het betreden van ons meeslepende spel wordt de spelers kort geïnformeerd over het doel en de uitdagingen die hen te wachten staan. Zodra de "Start" knop wordt ingedrukt, begint een zinderende countdown en wordt de tegenstander, de AI, geactiveerd. Zodra de tijd begint te tikken, is het aan de spelers en de AI om met precisie en snelheid het juiste doelwit te raken, want falen betekent verlies. Het is essentieel om het juiste doelwit te raken, foutieve doelwitten te vermijden, geen enkel schot te missen en op tijd te schieten. Alleen door sneller en trefzekerder te zijn dan de AI, kan de speler de ultieme overwinning behalen.

### Samenvatting

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

3. Scripten
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
        		this.transform.Rotate(0.0f, rotationMultiplier * actionBuffers.ContinuousActions[0], 0.0f);
        		if (shoot) // if jump button is pressed and is on the ground
        		{
            		print("Pang");

            		if (shootScript != null)
            		{
                		string tag = shootScript.Shoot();
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

Onze trainings file kan u terug vinden op volgende (link)[https://github.com/AP-IT-GH/eindproject-Bullet-Time-VR/blob/main/Bullet-Time-VR/Assets/ML%20Agent/config/Cowboy.yaml].

### Kleuren herkennen a.d.h.v. een camera

### Kleuren onthouden a.d.h.v. een camera en geheugen


## Conclusie
Ons westernspel is ontworpen om spelers uit te dagen om zo snel mogelijk een doelwit te raken, terwijl ze het opnemen tegen een AI-tegenstander. We hebben ons best gedaan om ervoor te zorgen dat het spel zowel speelbaar als eerlijk is, zodat spelers een leuke en uitdagende ervaring hebben.

Onze huidige AI-agent presteert over het algemeen goed en kan effectief reageren op de acties van spelers. Echter, toen we probeerden om een getrainde agent met geheugen te implementeren in onze VR-omgeving, ondervonden we enkele problemen. Om die reden raden we het gebruik van geheugen in onze toepassing af.

Het gebruik van geheugen in een AI-agent kan verschillende voordelen bieden, zoals het vermogen om te leren van eerdere ervaringen en beslissingen. Het stelt de agent in staat om te anticiperen op bepaalde situaties en beter te presteren na verloop van tijd. Echter, in ons specifieke geval bleek het geheugen niet goed te werken binnen onze VR-omgeving.

We hebben vastgesteld dat de implementatie van het geheugen in onze agent leidde tot technische complicaties en ongewenste resultaten. Het had een negatieve invloed op de algehele prestaties en speelbaarheid van het spel. We hebben de moeilijkheid ervaren om het geheugen effectief te integreren in de VR-omgeving en het spelveld, waardoor de ervaring voor spelers minder bevredigend werd.

Hoewel we begrijpen dat het gebruik van geheugen in sommige gevallen voordelig kan zijn, hebben we besloten om deze functie niet te gebruiken in onze huidige toepassing. We zijn nog steeds tevreden met hoe onze AI-agent functioneert zonder het geheugen en geloven dat het spel een aantrekkelijke uitdaging biedt aan spelers.

## Bronvermelding
1. Marwan Mattar, (2018, Maart 14). Memory-enhanced Agents using Recurrent Neural Networks. https://github.com/miyamotok0105/unity-ml-agents/blob/master/docs/Feature-Memory.md
2. Unity Technologies, (2022). Training Configuration File. https://unity-technologies.github.io/ml-agents/Training-Configuration-File/#memory-enhanced-agents-using-recurrent-neural-networks
3. Matt Cone, (2023). Markdown Cheat sheet. https://www.markdownguide.org/cheat-sheet/
4. 
