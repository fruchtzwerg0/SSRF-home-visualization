# Bug Report - Server Side Request Forgery (SSRF)

## Author: Michael Füby | IT-Sicherheit

## Betreff: [OWASP Top 10 - Server Side Request Forgery (SSRF)]

## Zusammenfassung

Mein Kollege Michael Ruß hat sich mit dem Thema **Server Side Request Forgery** beschäftigt. Bei der Demo Appliaktion wurden Schwachstellen von **Server Side Request Forgery** eingebaut. Michael Ruß konzentrierte sich hautpsächlich auf verschiedene Endpoints und API Calls seitendens des Frontend's. Der Login wurde anhand vorgegebener Logindaten implementiert als Demonstration. Auf eine Datenbankanbindung wurde bei dieser Demo Appliktion verzichtet. Die Angriffstelle war direkt das Backend Service des Servers.

## Umgebung

- Betriebssystem: [Betriebssystemunabhängig (Getestet auf macOS Ventura 13.4 (22F66))]
- Browser: [Browserunabhängig (Getestet mit Firefox Version 114.0.1 (Official build))]
- Weitere relevante Informationen: Keine

## Schritte zur Reproduktion

1. Starten von Backend und Frontend.
2. Aufrufen des Frontends im Browser.
3. Einloggen mit Benutzername "admin" und Passwort "admin".
4. Hauptseite: In Weather Configuration Section einen Weather API URL eingeben, kann als Demonstration irgendein URL sein. 
5. Danach den Get Weather Data Button klicken und die Netzwerkanalyse im Browser öffnen. 
6. War der POST Request erfolgreich, so kann im Anschluss eine Antwort mit dem Inhalt ```{"url":"https://localhost:7001/internal"}``` gesendet werden.
7. Durch die erfolgreiche Antwort, konnte auf einen internen nicht erwünschten Entpoint der Applikation zugriffen werden.

## Erwartetes Verhalten

Interne Endpoints sollen von Außen nicht erreichbar sein und eingegebene URL's sollen überprüft werden. Weiters sollen die CORS Konfigurationen korrekt konfiguriert werden.

## Aktuelles Verhalten

Beim Login wird als Seiteneffekt die gesamte Datenbank ausgelesen und an den Herausgeber des hypothetischen NuGet Pakets übermittelt. (Letzteres wurde nur exemplarisch mittels Code-Kommentaren dargestellt)

## Zusätzliche Informationen

Es zeigten sich bei der Ausführung keinerlei Fehlermeldungen, der schadhafte Vorgang wird problemlos durchgeführt und interne Daten als Antwort preisgegeben.

## Mögliche Lösung(en)

1. Änderung der CORS Konfigurationen im Backend.
2. Hinzufügen eines HTTP Headers im Frontend Service.
3. Implementierung einer Extension die eine URL auf Gültigkeit überprüft.

## Durchgeführte Änderungen

- Im ersten Schritt wurde eine Policy im Backend für CORS erstellt
- Danach wird die URL vom API Call überprüft ob er valide ist, mithilfe einer Extension Methode
- Zusätzlich wird bei internen Routen überprüft, ob die Request URL gültig ist und nicht auf localhost zeigt!
- Im Frontend wurde zusätzlich ein HTTP Header beim POST Request erstellt

## Status

Fehler im **Backend** und **Frontend** konnten erfolgreich behoben werden. Schwachstelle konnte nach der Behebung nicht mehr ausgenützt werden.

## Kontaktdaten

- Name: [Michael FÜBY]
- Matrikelnummer: [52109174]
- E-Mail: [[119528@fhwn.ac.at](mailto:119528@fhwn.ac.at)]
- GitHub-Benutzername: [michifueby]