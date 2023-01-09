# hangman-game
Jest to gra, która wykonuje się w konsoli. 

Na początku ładowany jest plik z listą kilkudziesięciu możliwych haseł (w j. angielskim) i wybierana jest losowo jedna pozycja. 

Gracz wpisuje kolejne litery.

Gdy litera nie znajduje się w zasłoniętym tekście, wtedy rysowana jest kolejna część wisielca. Gdy
wisielec zostanie uzupełniony do końca, gra się kończy z napisem „PRZEGRAŁEŚ”. Jeżeli gracz zdoła wygrać,
wtedy jego wynik (liczba błędów w stosunku do wszystkich znaków i czas który potrzebował na
wypełnienie słowa) zapisywany jest do pliku typu TXT.