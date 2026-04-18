# Security Policy

## Unterstützte Versionen

Sicherheitsupdates werden ausschließlich für die jeweils aktuelle Version der App bereitgestellt.

| Version | Unterstützt |
|---------|-------------|
| aktuell (main) | ✅ |
| ältere Releases | ❌ |

## Sicherheitslücke melden

**Bitte öffne für Sicherheitslücken kein öffentliches Issue.**

Melde Sicherheitsprobleme vertraulich über die [GitHub Security Advisories](https://github.com/Sanifant/App-LuebeckRegatta/security/advisories/new) oder per E-Mail an den Projektverantwortlichen (siehe GitHub-Profil).

Bitte füge deinem Bericht folgende Informationen bei:

- Beschreibung der Schwachstelle
- Betroffene Version / Komponente
- Schritte zur Reproduktion
- Mögliche Auswirkungen (z. B. Datenverlust, unbefugter Zugriff)

## Umgang mit Berichten

- Eingang wird innerhalb von **5 Werktagen** bestätigt.
- Eine erste Einschätzung erfolgt innerhalb von **14 Tagen**.
- Nach Behebung wird ein Security Advisory veröffentlicht.

## Sicherheitsrelevante Hinweise zur App

- Die App kommuniziert ausschließlich mit dem konfigurierten frgle-Server im lokalen Netzwerk.
- Es werden **keine personenbezogenen Daten** an externe Dienste übertragen.
- Zugangsdaten (Keystore-Passwörter) dürfen **nicht** im Quellcode hinterlegt werden – ausschließlich über CI/CD-Secrets oder Umgebungsvariablen.
- HTTP-Verbindungen zum frgle-Server sollten über ein abgesichertes, internes Netzwerk erfolgen.

## Bekannte Einschränkungen

- Die App validiert TLS-Zertifikate des frgle-Servers gemäß Standard-.NET-Verhalten. Bei selbstsignierten Zertifikaten in internen Netzen liegt die Absicherung beim Betreiber.
