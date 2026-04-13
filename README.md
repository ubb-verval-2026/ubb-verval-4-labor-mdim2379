# VerVal Labor 4

A DatesAndStuff.Web.Tests projektben kell dolgoznatok az alábbi feladatok esetén.

Segédeszközök:
[Katalon Recorder](https://chromewebstore.google.com/detail/katalon-recorder-selenium/ljdobmomdgdljniojadhoplhkpialdid)


1. (2 pont) A meglévő Person_SalaryIncrease_ShouldIncrease tesztet alakítsa át úgy, hogy több különböző százalékértékkel is tesztelje az algoritmust (emlékezzünk az előzőekben tanultakra).

2. (2 pont) Írjon unit tesztet annak ellenőrzésére, hogy -10-nél kisebb értékű növelési százalék megadása esetén a hibaüzenetek is megjelennek (az oldal tetején és a mező alatt is).

3. (3 pont) A frontend részletes tesztelése lehetőséget ad arra, hogy a backend és a frontend közötti funkcionális eltéréseket megtaláljuk. A jelenlegi kódban a frontend és a backend kissé eltérő feltételt ellenőriz. Az előző alpont feltételei esetén a Submit gomb megnyomására a fizetés értéke nem frissülhet -10 esetén. Találja meg az eltérést, majd frissítse úgy a kódot, hogy konzisztens legyen a két oldal. A frissítés után vagy a backend, vagy a frontend teszteknek el kell bukniuk (ehhez az állapothoz tartozzon commit hash), majd igazítsa a unit teszteket a változtatáshoz.

4. (4 pont) A [wizzair.com](https://www.wizzair.com/en-gb) weboldalhoz írjon olyan unit tesztet, amelynek célja annak ellenőrzése, hogy az aktuális dátumhoz viszonyított következő héten van-e Bucharest és Budapest között két járat.

(Bónusz 4 pont) Egészítse ki az előző tesztet úgy, hogy amennyiben van olyan Budapestre tartó járat, amelynek ára egy előre beállított érték alatt van, akkor készítsen róla egy képernyőképet, és helyezze el azt az Asztalon (vagy egy előre megadott helyen).
