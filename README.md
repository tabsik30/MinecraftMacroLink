# MacroLink — plugin Macro Deck

connects my minecraft mod  thrue WebSocket default ws://127.0.0.1:25599 
with Macro Deck 2 and shows statistic from MC on MD2 as variables

first instal my mc 26.2 mod https://github.com/tabsik30/Minecraft-Macro-link-Mod/tree/main


Łączy się z modem MacroLink (Fabric, Minecraft 26.2) przez WebSocket
(`ws://127.0.0.1:25599` domyślnie) i wystawia dane gracza jako **zmienne
Macro Deck**, które możesz przypiąć do dowolnego przycisku (tekst, ikona,
warunki itp. — standardowy mechanizm zmiennych w Macro Decku).

najpierw zainstaluj mojego moda do minecraft 26.2 https://github.com/tabsik30/Minecraft-Macro-link-Mod/tree/main
## Zmienne, które ustawia plugin / variables that this plugin adds

| Zmienna/Variabl  | Opis                                               |
|------------------|----------------------------------------------------|
| {mc_health}      | aktualne HP    actual HP                              |
| {mc_max_health}  | maksymalne HP  mx HP                                |
| {mc_armor}       | poziom pancerzaArmor Status                       |
| {mc_hunger}      | poziom głodu   Hunger Status                        |
| {mc_x}           | koordynata X   coordinate X                        |
| {mc_y}           | koordynata Y   coordinate Y                        |
| {mc_z}           | koordynata Z   coordinate Z                        |
| {mc_dimension}   | wymiar (np. `minecraft:overworld`)  Dimension      |
| {mc_biome}       | biom (np. `minecraft:plains`)       Biome          |
| {mc_game_time}   | narazie zignorować jest dla testu Ignore this one is only for test |
| {mc_air}         | pokazuje poziom powietrza  show air level           |
| {mc_max_air}     | maksymalny poziom powietrza shows max air level    |
| {mc_xp_level}    | poziom gracza  shows player level 


## Zachowanie przy braku gry/ if game is not launched

Jeśli Minecraft nie jest uruchomiony (albo mod jeszcze nie wystartował
serwera), plugin próbuje połączyć się ponownie co 5 sekund w tle — nie
trzeba nic klikać, samo się złapie jak odpalisz grę.

if game is not lauched or server not started plugin will try to coonect evry 5sec in background
it will connetc automaticly when game is launched 


