# PRJ4

4. Semesterprojekt - Software Teknologi

## Commit Message - Syntax

---

### Summary

`Console (Linux, Git Bash etc.): Summary er de første 50 chars.`

Information skrevet i summary er de største ændringer foretaget i det kommende commit.

Eksempel:

```
Modificerede void x(int y, int z)'s retur-værdi. 
```

### Description

```
Mere detaljeret beskrivelse baseret ud fra Summary.

Files:

	+ Tilføjede Filer.

	~ Modificerede Filer.

	- Slettede Filer.

Classes:

	+ Tilføjede Metoder: retur-værdi navn(paramtre)
		Metodens funktionalitet

	~ Modificerede Metoder
		Metodens opdaterede funktionalitet

	- Slettede Metoder
		Begrundelse

```

## Commits, Merge and General Workflow

---

For vores generalle workflow, er vores git-repository sat op i følgende branches:
* WIP-(Iteration)
* master

For hver iteration der bliver udviklet, bliver der dannet en branch ud fra master.
Denne nye branch er WIP-(Iteration), som indeholder vores Work-In-Progress implementering.
Master-branch vil altid indeholde vores Release-version af vores produkt, og derfor skal der ikke foretages commits til master.
Når der bliver dannet en ny iteration-branch, skal der dannes en ny branch for hvert modul (unit, klasse etc.), som bliver udviklet under iterationen.
Efter færddiggørelse af dette modul, kan modul-branchen merges med WIP-(Iteration).
Når alle moduler i iterationen er færddiggjort, testet og merged med WIP, bliver der foretaget en endelig test på vores WIP-(Iteration) implementering.
Hvis denne test er succesfuld, bliver vores WIP-(Iteration) merged med master.
Efter dette merge, bliver der branched ud til en ny WIP-(Iteration)-branch, hvorefter udviklingen på næste iteration påbegynnes.

Step-by-Step:
1. Branch WIP-(Iteration) ud fra master
2. Branch moduler (klasser, units etc.) ud fra WIP-(Iteration)
3. Merge moduler med WIP-(Iteration) efter de er færdiggjort
4. Merge WIP-(Iteration) med master efter alle moduler er færdiggjort, og merged med WIP-(Iteration).
5. Returner til punkt 1.