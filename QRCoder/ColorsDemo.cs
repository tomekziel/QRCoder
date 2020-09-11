using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QRCoder.PayloadGenerator;
using static QRCoder.QRCodeGenerator;

namespace qrexperiment
{
    class ColorsDemo
    {


        static void Main(string[] args)
        {

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            int i = 0;
            foreach (var color in new ColorProvider[] {
                new ColorProviders.ColorProviderBW(),
                new ColorProviders.ColorProviderDebug(),
                new ColorProviders.ColorProviderRysowankiFinder(),
                new ColorProviders.ColorProviderRysowankiWyrownanie(),
                new ColorProviders.ColorProviderRysowankiTimer(),
                new ColorProviders.ColorProviderRysowankiFormat(),
                new ColorProviders.ColorProviderKolejnosc(),
                new ColorProviders.ColorProviderRysowankiAllReserved(),
                new ColorProviders.ColorProviderRysowankiAnim()
            })
            {

                QRCodeData qrCodeData = qrGenerator.CreateQrCode(
                    "This is a demo of different color providers",
                    QRCodeGenerator.ECCLevel.L
                    ); ;

                QRCode qrc = new QRCode(qrCodeData);

                var b = qrc.GetGraphic(
                    20,
                    color,
                    drawQuietZones: false, drawThinLines: true);
                b.Save(String.Format("colorexample_{0:00}.png", i++));

            }


            //loop();
            //perform('-', 0);

            //LitwoOjczyzno1do40();
            //RysowankiPozycja();
            //RysowankiWyrownanie();
            //RysowankiTiming();
            //RysowankiFormat();
            //RysowankiWersja();
            //RysowankiMaska();
            //RysowankiKolorek();
            //MaskiDemo();
            // CheckSizes();
            //DataWordAnim();
            //Kolejnosc();

            //NullBytes();


        }


        private static void NullBytes()
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            int i = 0;

            QRCodeData qrCodeData = qrGenerator.CreateQrCode(
                new String('\0', 106),
                QRCodeGenerator.ECCLevel.L

                );

            QRCode qrc = new QRCode(qrCodeData);

            var b = qrc.GetGraphic(
                new int[] { 20, 20, 20, 10 }[i++],
                new ColorProviders.ColorProviderBW(),
                drawQuietZones: false, drawThinLines: false);
            b.Save("nullbytes.png");


            QRCodeData qrCodeData2 = qrGenerator.CreateQrCode(
                new String((char)(255), 106),
                QRCodeGenerator.ECCLevel.L
            );

            QRCode qrc2 = new QRCode(qrCodeData2);

            var b2 = qrc2.GetGraphic(
                new int[] { 20, 20, 20, 10 }[i++],
                new ColorProviders.ColorProviderBW(),
                drawQuietZones: false, drawThinLines: false);
            b2.Save("nullbytes2.png");

        }


        private static void Kolejnosc()
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            int i = 0;
            foreach (int version in new int[] { 1, 2, 7, 14 })
            {

                QRCodeData qrCodeData = qrGenerator.CreateQrCode(
                    "Kolejnosc",
                    QRCodeGenerator.ECCLevel.L,
                    requestedVersion: version
                    ); ;

                QRCode qrc = new QRCode(qrCodeData);

                var b = qrc.GetGraphic(
                    new int[] { 20, 20, 20, 10 }[i++],
                    new ColorProviders.ColorProviderKolejnosc(),
                    drawQuietZones: false, drawThinLines: false, drawDirections: true);
                b.Save(String.Format("kolejnosc_{0:00}.png", version));

            }
        }

        private static void CheckSizes()
        {
            for (int i = 1; i<=40; i++)
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(
                    "a",
                    QRCodeGenerator.ECCLevel.Q,
                    true,
                    requestedVersion: i,
                    forcedpattern:1
                );

                int databits = 0;
                foreach (var row in qrCodeData.SourceMatrix)
                {
                    foreach (var col in row)
                    {
                        if (col.Equals(SourceType.DATA))
                        {
                            databits++;
                        }

                    }
                }

                Console.WriteLine(i+"\t"+ databits+"\t"+(databits/ 8));


            }
        }

        private static void DataWordAnim()
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            int frame = 240;
            while (true)
            {
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(
                    "👨‍🏭",
                    QRCodeGenerator.ECCLevel.H,
                    true,
                    //requestedVersion: 2,
                    forcedpattern: -1,
                    maxBits: frame,
                    doMasking: true
                );


                QRCode qrc = new QRCode(qrCodeData);

                var b = qrc.GetGraphic(
                        20,
                        //new ColorProviders.ColorProviderRysowankiAnim()
                        //new ColorProviderBW()
                        new ColorProviders.ColorProviderDebug2()
                        , drawQuietZones: true
                        );
                    b.Save(String.Format("anim_x{0:000}.png", frame));


                if (frame++ > 240)
                    //if (frame++ > qrCodeData.DataWordsLen)
                {
                    Console.WriteLine(frame);
                    break;
                }


            }


        }


        private static void MaskiDemo()
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            int i = 0;
            for (int maska = 1; maska <= 8; maska++)
            {
                foreach (int version in new int[] { 2, 7, 14 })
                {

                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(
                        " ",
                        QRCodeGenerator.ECCLevel.L,
                        requestedVersion: version,
                        forcedpattern: -maska);

                    QRCode qrc = new QRCode(qrCodeData);

                    var b = qrc.GetGraphic(
                        20,
                        new ColorProviders.ColorProviderRysowankiMaska()
                        , drawQuietZones: false
                        , drawThinLines: true
                        );
                    b.Save(String.Format("maska_{0:0}_{1:00}.png", maska, version));

                }
            }
        }


        private static void RysowankiPozycja()
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            int i = 0;
            foreach (int version in new int[] { 2, 7, 14 })
            {

                QRCodeData qrCodeData = qrGenerator.CreateQrCode(
                    "Finder pattern",
                    QRCodeGenerator.ECCLevel.L,
                    requestedVersion: version); ;

                QRCode qrc = new QRCode(qrCodeData);

                var b = qrc.GetGraphic(
                    new int[] { 20, 20, 10 }[i++],
                    new ColorProviders.ColorProviderRysowankiFinder(),
                    drawQuietZones: false, drawThinLines: true);
                b.Save(String.Format("RysowankiFinder{0:00}.png", version));

            }
        }


        private static void RysowankiTiming()
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            int i = 0;
            foreach (int version in new int[] { 2, 7, 14 })
            {

                QRCodeData qrCodeData = qrGenerator.CreateQrCode(
                    "Timer pattern",
                    QRCodeGenerator.ECCLevel.L,
                    requestedVersion: version); ;

                QRCode qrc = new QRCode(qrCodeData);

                var b = qrc.GetGraphic(
                    new int[] { 20, 20, 10 }[i++],
                    new ColorProviders.ColorProviderRysowankiTimer(),
                    drawQuietZones: false, drawThinLines: true);
                b.Save(String.Format("RysowankiTiming{0:00}.png", version));

            }
        }

        private static void RysowankiFormat()
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            int i = 0;
            foreach (int version in new int[] { 2, 7, 14 })
            {

                QRCodeData qrCodeData = qrGenerator.CreateQrCode(
                    "Timer pattern",
                    QRCodeGenerator.ECCLevel.L,
                    requestedVersion: version); ;

                QRCode qrc = new QRCode(qrCodeData);

                var b = qrc.GetGraphic(
                    new int[] { 20, 20, 10 }[i++],
                    new ColorProviders.ColorProviderRysowankiFormat(),
                    drawQuietZones: false, drawThinLines: true);
                b.Save(String.Format("RysowankiFormat{0:00}.png", version));

            }
        }


        private static void RysowankiWersja()
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            int i = 0;
            foreach (int version in new int[] { 7, 14, 25 })
            {

                QRCodeData qrCodeData = qrGenerator.CreateQrCode(
                    "Timer pattern",
                    QRCodeGenerator.ECCLevel.L,
                    requestedVersion: version); ;

                QRCode qrc = new QRCode(qrCodeData);

                var b = qrc.GetGraphic(
                    new int[] { 20, 20, 10 }[i++],
                    new ColorProviders.ColorProviderRysowankiWersja(),
                    drawQuietZones: false, drawThinLines: true);
                b.Save(String.Format("RysowankiWersja{0:00}.png", version));

            }
        }


        private static void RysowankiMaska()
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            int i = 0;
            foreach (int version in new int[] { 2, 7, 14 })
            {

                QRCodeData qrCodeData = qrGenerator.CreateQrCode(
                    "Maska pattern",
                    QRCodeGenerator.ECCLevel.L,
                    requestedVersion: version); ;

                QRCode qrc = new QRCode(qrCodeData);

                var b = qrc.GetGraphic(
                    new int[] { 20, 20, 10 }[i++],
                    new ColorProviders.ColorProviderRysowankiAllReserved(),
                    drawQuietZones: false, drawThinLines: true);
                b.Save(String.Format("RysowankiMaska{0:00}.png", version));

            }
        }


        private static void RysowankiWyrownanie()
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            int i = 0;
            foreach (int version in new int[] { 2, 7, 14, 21, 28, 35 })
            {

                QRCodeData qrCodeData = qrGenerator.CreateQrCode(
                    "Takie sobie kolorki",
                    QRCodeGenerator.ECCLevel.L,
                    requestedVersion: version); ;

                QRCode qrc = new QRCode(qrCodeData);


                var b = qrc.GetGraphic(
                    new int[] { 20, 20, 7, 7, 5, 5 }[i++],
                    new ColorProviders.ColorProviderRysowankiWyrownanie(),
                    drawQuietZones: false, drawThinLines: true);
                b.Save(String.Format("RysowankiWyrownanie{0:00}.png", version));

            }
        }


        private static void RysowankiKolorek()
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            int i = 0;
            foreach (int version in new int[] { 7 })
            {

                QRCodeData qrCodeData = qrGenerator.CreateQrCode(
                    "Taki kawałek wystarcza 👨‍🏭",
                    QRCodeGenerator.ECCLevel.L,
                    requestedVersion: version); ;

                QRCode qrc = new QRCode(qrCodeData);


                var b = qrc.GetGraphic(
                    new int[] { 20, 20, 7, 7, 5, 5 }[i++],
                    //new ColorProviderDebug(),
                    new ColorProviders.ColorProviderBW(),
                    drawQuietZones: false, drawThinLines: true);
                b.Save(String.Format("RysowankiKolorek{0:00}.png", version));

            }
        }

        private static void LitwoOjczyzno1do40()
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();

            int version = 1;

            for (int i=1; i<2955; i++)
            {
                int verCalc = 0;
                try {
                    verCalc = QRCodeGenerator.GetVersion(i, EncodingMode.Byte, ECCLevel.L);
                }
                catch (Exception)
                {
                    verCalc = 41;
                }

                if (version < verCalc )
                {
                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(
                        Litwo.Substring(0, i - 1),
                        QRCodeGenerator.ECCLevel.L,
                        requestedVersion: version); ;

                    QRCode qrc = new QRCode(qrCodeData);

                    int px = 370 / (21 + (4 * version));

                    Console.WriteLine(version + "  ,  " + (i-1));

                    var b = qrc.GetGraphic(px, new ColorProviders.ColorProviderBW());
                    b.Save( String.Format("litwo{0:00}.png", version));


                    version++;
                }

            }




        }

        static void loop()
        {
            String chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            String txt = "INFORMATYK ZAKLADOWY";
            for (int i=0; i < txt.Length*chars.Length; i++)
            //for (int i = 0; i < 2; i++)
            {
                var idx = i / chars.Length;
                var toprint = txt.ToCharArray();
                toprint[idx] = chars[i % chars.Length];
                perform(new string(toprint), i);
            }
        }

        static void perform(string s, int i)
        {

            
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(s, QRCodeGenerator.ECCLevel.M, 
                //requestedVersion:8, 
                forcedpattern:1);
            
            AsciiQRCode qra = new AsciiQRCode(qrCodeData);

            //foreach (string s in qra.GetLineByLineGraphic(1))
            //{
            //    Console.WriteLine(s);
            //}


            //foreach (string s in qra.GetLineByLineSource())
            //{
            //    Console.WriteLine(s);
            //}


            QRCode qrc = new QRCode(qrCodeData);

            //var b = qrc.GetGraphic(10, new ColorProviders.ColorProviderDebug());
            //b.Save("out" + v + ".png");

            //return;

            Image glowka = Bitmap.FromFile("glowka.png");

            var c = qrc.GetGraphic(12, new ColorProviders.ColorProviderBW());

            Bitmap newImage = new Bitmap(c.Width, c.Height+40, c.PixelFormat);
            //Graphics g = Graphics.FromImage(c);
            Graphics g = Graphics.FromImage(newImage);
            g.FillRectangle(Brushes.White, 0, 0, newImage.Width, newImage.Height);
            g.DrawImage(c,0,0);
            //g.DrawImage(glowka, newImage.Width/2-glowka.Width/2, glowka.Width );

            RectangleF rectf = new RectangleF(15, c.Height - 10, c.Width, 100);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.DrawString(s, new Font("Consolas", 20, FontStyle.Bold), Brushes.Black, rectf);
            g.Flush();

            string fname = String.Format("out_{0:000}.png", i);
            newImage.Save(fname);

        }


        public const string Litwo =
@"Litwo! Ojczyzno moja! ty jestes jak zdrowie.
Ile cie trzeba cenic, ten tylko sie dowie,
Kto cie stracil. Dzis pieknosc twa w calej ozdobie
Widze i opisuje, bo tesknie po tobie.
 
Panno Swieta, co jasnej bronisz Czestochowy
I w Ostrej swiecisz Bramie! Ty, co grod zamkowy
Nowogrodzki ochraniasz z jego wiernym ludem!
Jak mnie dziecko do zdrowia powrocilas cudem
(Gdy od placzacej matki pod Twoja opieke
Ofiarowany, martwa podnioslem powieke
I zaraz moglem pieszo do Twych swiatyn progu
Isc za wrocone zycie podziekowac Bogu),
Tak nas powrocisz cudem na Ojczyzny lono.
Tymczasem przenos moje dusze uteskniona
Do tych pagorkow lesnych, do tych lak zielonych,
Szeroko nad blekitnym Niemnem rozciagnionych;
Do tych pol malowanych zbozem rozmaitem,
Wyzlacanych pszenica, posrebrzanych zytem;
Gdzie bursztynowy swierzop, gryka jak snieg biala,
Gdzie panienskim rumiencem dziecielina pala,
A wszystko przepasane, jakby wstega, miedza
Zielona, na niej z rzadka ciche grusze siedza.
 
Srod takich pol przed laty, nad brzegiem ruczaju,
Na pagorku niewielkim, we brzozowym gaju,
Stal dwor szlachecki, z drzewa, lecz podmurowany;
Swiecily sie z daleka pobielane sciany,
Tym bielsze, ze odbite od ciemnej zieleni
Topoli, co go bronia od wiatrow jesieni.
Dom mieszkalny niewielki, lecz zewszad chedogi,
I stodole mial wielka, i przy niej trzy stogi
Uzatku, co pod strzecha zmiescic sie nie moze;
Widac, ze okolica obfita we zboze,
I widac z liczby kopic, co wzdluz i wszerz smugow
Swieca gesto jak gwiazdy, widac z liczby plugow
Orzacych wczesnie lany ogromne ugoru,
Czarnoziemne, zapewne nalezne do dworu,
Uprawne dobrze na ksztalt ogrodowych grzadek:
Ze w tym domu dostatek mieszka i porzadek.
Brama na wciaz otwarta przechodniom oglasza,
Ze goscinna i wszystkich w goscine zaprasza.
 
Wlasnie dwokonna bryka wjechal mlody panek
I obieglszy dziedziniec zawrocil przed ganek,
Wysiadl z powozu; konie porzucone same,
Szczypiac trawe ciagnely powoli pod brame.
We dworze pusto, bo drzwi od ganku zamknieto
Zaszczepkami i kolkiem zaszczepki przetknieto.
Podrozny do folwarku nie biegl slug zapytac;
Odemknal, wbiegl do domu, pragnal go powitac.
Dawno domu nie widzial, bo w dalekim miescie
Konczyl nauki, konca doczekal nareszcie.
Wbiega i okiem chciwie sciany starodawne
Oglada czule, jako swe znajome dawne.
Tez same widzi sprzety, tez same obicia,
Z ktoremi sie zabawiac lubil od powicia;
Lecz mniej wielkie, mniej piekne, niz sie dawniej zdaly.
I tez same portrety na scianach wisialy.
Tu Kosciuszko w czamarce krakowskiej, z oczyma
Podniesionymi w niebo, miecz oburacz trzyma;
Takim byl, gdy przysiegal na stopniach oltarzow,
Ze tym mieczem wypedzi z Polski trzech mocarzow
Albo sam na nim padnie. Dalej w polskiej szacie
Siedzi Rejtan zalosny po wolnosci stracie,
W reku trzymna noz, ostrzem zwrocony do lona,
A przed nim lezy Fedon i zywot Katona.
Dalej Jasinski, mlodzian piekny i posepny,
Obok Korsak, towarzysz jego nieodstepny,
Stoja na szancach Pragi, na stosach Moskali,
Siekac wrogow, a Praga juz sie wkolo pali.
Nawet stary stojacy zegar kurantowy
W drewnianej szafie poznal u wniscia alkowy
I z dziecinna radoscia pociagnal za sznurek,
By stary Dabrowskiego uslyszec mazurek.
 
Biegal po calym domu i szukal komnaty,
Gdzie mieszkal, dzieckiem bedac, przed dziesieciu laty.
Wchodzi, cofnal sie, toczyl zdumione zrenice
Po scianach: w tej komnacie mieszkanie kobiece?
Ktoz by tu mieszkal? Stary stryj nie byl zonaty,
A ciotka w Petersburgu mieszkala przed laty.
To nie byl ochmistrzyni pokoj! Fortepiano?
Na niem noty i ksiazki; wszystko porzucano
Niedbale i bezladnie; nieporzadek mily!
Niestare byly raczki, co je tak rzucily.
Tuz i sukienka biala, swiezo z kolka zdjeta
Do ubrania, na krzesla poreczu rozpieta.
A na oknach donice z pachnacymi ziolki,
Geranium, lewkonija, astry i fijolki.
 
Podrozny stanal w jednym z okien - nowe dziwo:
W sadzie, na brzegu niegdys zaroslym pokrzywa,
Byl malenki ogrodek, sciezkami porzniety,
Pelen bukietow trawy angielskiej i miety.
Drewniany, drobny, w cyfre powiazany plotek
Polyskal sie wstazkami jaskrawych stokrotek.
Grzadki widac, ze byly swiezo polewane;
Tuz stalo wody pelne naczynie blaszane,
Ale nigdzie nie widac bylo ogrodniczki;
Tylko co wyszla; jeszcze kolysza sie drzwiczki
Swiezo tracone; blisko drzwi slad widac nozki
Na piasku, bez trzewika byla i ponczoszki;
Na piasku drobnym, suchym, bialym na ksztalt sniegu,
Slad wyrazny, lecz lekki; odgadniesz, ze w biegu
Chybkim byl zostawiony nozkami drobnemi
Od kogos, co zaledwie dotykal sie ziemi.
 
Podrozny dlugo w oknie stal patrzac, dumajac,
Wonnymi powiewami kwiatow oddychajac,
Oblicze az na krzaki fijolkowe sklonil,
Oczyma ciekawymi po drozynach gonil
I znowu je na drobnych sladach zatrzymywal,
Myslal o nich i, czyje byly, odgadywal.
Przypadkiem oczy podniosl, i tuz na parkanie
Stala mloda dziewczyna. - Biale jej ubranie
Wysmukla postac tylko az do piersi kryje,
Odslaniajac ramiona i labedzia szyje.
W takim Litwinka tylko chodzic zwykla z rana,
W takim nigdy nie bywa od mezczyzn widziana:
Wiec choc swiadka nie miala, zalozyla rece
Na piersiach, przydawajac zaslony sukience.
Wlos w pukle nie rozwity, lecz w wezelki male
Pokrecony, schowany w drobne straczki biale,
Dziwnie ozdabial glowe, bo od slonca blasku
Swiecil sie, jak korona na swietych obrazku.
Twarzy nie bylo widac. Zwrocona na pole
Szukala kogos okiem, daleko, na dole;
Ujrzala, zasmiala sie i klasnela w dlonie,
Jak bialy ptak zleciala z parkanu na blonie
I wionela ogrodem przez plotki, przez kwiaty,
I po desce opartej o sciane komnaty,
Nim spostrzegl sie, wleciala przez okno, swiecaca,
Nagla, cicha i lekka jak swiatlosc miesiaca.
Nocac chwycila suknie, biegla do zwierciadla;
Wtem ujrzala mlodzienca i z rak jej wypadla
Suknia, a twarz od strachu i dziwu pobladla.
Twarz podroznego barwa splonela rumiana
Jak oblok, gdy z jutrzenka napotka sie ranna;
Skromny mlodzieniec oczy zmruzyl i przyslonil,
Chcial cos mowic, przepraszac, tylko sie uklonil
I cofnal sie; dziewica krzyknela bolesnie,
Niewyraznie, jak dziecko przestraszone we snie;
Podrozny zlakl sie, spojrzal, lecz juz jej nie bylo.
Wyszedl zmieszany i czul, ze serce mu bilo
Glosno, i sam nie wiedzial, czy go mialo smieszyc
To dziwaczne spotkanie, czy wstydzic, czy cieszyc.

Tymczasem na folwarku nie uszlo bacznosci,
Ze przed ganek zajechal ktorys z nowych gosci.
Juz konie w stajnie wzieto, juz im hojnie dano,
Jako w porzadnym domu, i obrok, i siano;
Bo Sedzia nigdy nie chcial, wedlug nowej mody,
Odsylac konie gosci Zydom do gospody.
Sludzy nie wyszli witac, ale nie mysl wcale,
Aby w domu Sedziego sluzono niedbale;
Sludzy czekaja, nim sie pan Wojski ubierze,
Ktory teraz za domem urzadzal wieczerze.
On Pana zastepuje i on w niebytnosci
Pana zwykl sam przyjmowac i zabawiac gosci
(Daleki krewny panski i przyjaciel domu).
Widzac goscia, na folwark dazyl po kryjomu
(Bo nie mogl wyjsc spotykac w tkackim pudermanie);
Wdzial wiec, jak mogl najpredzej, niedzielne ubranie
Nagotowane z rana, bo od rana wiedzial,
Ze u wieczerzy bedzie z mnostwem gosci siedzial.";

    }
}
