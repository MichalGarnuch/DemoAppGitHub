# DemoAppGitHub: mały program z przewidywalnym interfejsem CLI

Branch: `wersja-inzynierska-2026`. Stan bazowy: `master` / `9fdaa097f7f93f32dfab00dfe68fddd31a01adae`. Data wykonania: 29.10.2026.

To autorski wariant rozwojowy konkretnego fragmentu projektu. Branch zachowuje pełne drzewo kodu z wersji bazowej i zawiera opisane poniżej zmiany. [Wcześniejszy audyt](https://github.com/MichalGarnuch/DemoAppGitHub/tree/audyt-edukacyjny-2026) jest na osobnym branchu. Nie wykonywano merge ani wdrożenia.

## Kontrakt procesu

Nawet prosty program konsolowy powinien dać się uruchomić z terminala, CI i procesu nadrzędnego. Oczekiwanie na `Console.ReadLine` bez potrzeby może zawiesić automatyczny krok. Wyświetlanie daty zależne od kultury utrudnia parsowanie i porównywanie wyników.

`Program.Main` zwraca teraz kod procesu. Domyślne uruchomienie wypisuje powitanie i czas z przesunięciem strefy w formacie ISO 8601 (`O`, kultura invariant). `--utc` wybiera UTC, `--help` wyświetla użycie i kończy pracę, a nieznany argument trafia na stderr i daje kod 2. Zwykłe wykonanie kończy się kodem 0 bez oczekiwania na wejście.

## Dlaczego tak

`DateTimeOffset` zawiera offset i dzięki temu zapis nie udaje uniwersalnego czasu pozbawionego strefy. Rozdzielenie stdout i stderr pozwala przekierować wynik niezależnie od diagnostyki. Kod zakończenia jest kontraktem procesu, więc nadrzędny skrypt nie musi odgadywać błędu z tekstu.

Program jest mały, dlatego nie dodaję frameworka do parsowania dwóch przełączników ani kontenera zależności. Przy dalszym wzroście CLI można wprowadzić parser i wstrzykiwany zegar. Uzasadnieniem powinna być liczba opcji i stabilność formatu, nie sam fakt istnienia projektu .NET.

## Użycie i rozwój

Na zwykłym SDK: `dotnet run --project DemoAppGitHub -- --utc`, analogicznie `--help`. `dotnet run --project DemoAppGitHub -- --unknown` ma zakończyć się kodem 2. Jeśli program zacznie eksportować dane, dodałbym jawny `--format json` i osobny kontrakt wersji wyniku. Logi diagnostyczne nie powinny mieszać się z JSON na stdout.

W sesji skompilowano rzeczywisty plik Program.cs Roslynem względem .NET 8 i uruchomiono cztery scenariusze. To realne wykonanie programu, ale nie pełny test infrastruktury budowania projektu przez MSBuild.

## Weryfikacja wykonana w tej sesji

| Sprawdzenie | Wynik i znaczenie |
|---|---|
| Kompilacja Program.cs Roslynem/.NET 8 | PASS |
| Uruchomienie bez argumentów, --utc, --help | PASS: trzy poprawne wykonania, kod 0 |
| --unknown | PASS: stderr i kod 2; proces nie oczekuje na wejście |

### Granice weryfikacji

Nie uruchomiono pełnego builda przez MSBuild ze względu na ograniczenie inicjalizacji procesu w środowisku. Nie testowano wszystkich kultur i systemów operacyjnych; invariant i format O zapewniają jawny kontrakt na poziomie API.

## Mapa zmian

- [`DemoAppGitHub/Program.cs`](DemoAppGitHub/Program.cs)
