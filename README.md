# MacroLink — plugin Macro Deck

Łączy się z modem MacroLink (Fabric, Minecraft 26.2) przez WebSocket
(`ws://127.0.0.1:25599` domyślnie) i wystawia dane gracza jako **zmienne
Macro Deck**, które możesz przypiąć do dowolnego przycisku (tekst, ikona,
warunki itp. — standardowy mechanizm zmiennych w Macro Decku).

## Zmienne, które ustawia plugin

| Zmienna          | Opis                                              |
|------------------|----------------------------------------------------|
| `mc_health`      | aktualne HP                                        |
| `mc_max_health`  | maksymalne HP                                      |
| `mc_armor`       | poziom pancerza                                    |
| `mc_hunger`      | poziom głodu                                       |
| `mc_x`           | koordynata X                                       |
| `mc_y`           | koordynata Y                                       |
| `mc_z`           | koordynata Z                                       |
| `mc_dimension`   | wymiar (np. `minecraft:overworld`)                 |
| `mc_biome`       | biom (np. `minecraft:plains`)                      |
| `mc_game_time`   | TYMCZASOWO surowy licznik ticków (patrz README moda) |

## Konfiguracja

W konfiguratorze pluginu (ikonka koła zębatego przy pluginie w Macro Decku)
możesz zmienić host/port, jeśli mod nasłuchuje gdzie indziej niż domyślnie.
Zmiana automatycznie restartuje połączenie.

## Zachowanie przy braku gry

Jeśli Minecraft nie jest uruchomiony (albo mod jeszcze nie wystartował
serwera), plugin próbuje połączyć się ponownie co 5 sekund w tle — nie
trzeba nic klikać, samo się złapie jak odpalisz grę.

## Build

Otwórz `.csproj` w Visual Studio / JetBrains Rider (albo `dotnet build`
z SDK .NET 8 na Windows) i zbuduj — DLL wyląduje w `bin/Debug/net8.0-windows.../`
albo `bin/Release/...` w zależności od konfiguracji. Skopiuj go tam, gdzie
Macro Deck szuka pluginów (tak jak przy Twoim poprzednim projekcie).
