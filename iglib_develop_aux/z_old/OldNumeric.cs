// Copyright (c) Igor Grešovnik (2009), IGLib license; http://www2.arnes.si/~ljc3m2/igor/ioptlib/


     // OUTDATED DEFINITIONS OF THE Ig.Num NAMESPACE (sorted in internal namespaces)


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using MathNet.Numerics.Properties;
using MathNet.Numerics.Distributions;
//using MathNet.Numerics.RandomSources;

namespace IG.Old
{

    /// <summary>Generic Matrix interface.</summary>
    public interface IMatrix_OldNumerics<T>
    {

        /// <summary>Gets the number of rows.</summary>
        int RowCount { get; }

        /// <summary>Gets the number of columns.</summary>
        int ColumnCount { get; }

        /// <summary>Gets or set the element indexed by <c>(i, j)</c> in the <c>Matrix</c>.</summary>
        /// <param name="i">Row index.</param>
        /// <param name="j">Column index.</param>
        T this[int i, int j] { get; set; }

        /// <summary>Copy all elements of this matrix to a rectangular 2D array.</summary>
        T[,] CopyToArray();

        /// <summary>Copy all elements of this matrix to a jagged array.</summary>
        T[][] CopyToJaggedArray();
    }  // interface IMatrixOld<T>


    public interface IMatrixOld : IMatrix_OldNumerics<double>
    {

        IMatrixOld GetCopy();

    }  //  interface IMatrixOld


    /// <summary>Generic Vector interface</summary>
    public interface IVector_OldNumerics<T>
    {

        /// <summary>Gets the number of rows.</summary>
        int Length { get; }

        /// <summary>Gets or set the element indexed by <c>i</c> in the <c>Vector</c>.</summary>
        /// <param name="i">Element index (zero-based by agreement).</param>
        T this[int i] { get; set; }

        /// <summary>Copy all elements of this vector to an array.</summary>
        T[] CopyToArray();
    }  // interface IVectorOld<T>

    /// <summary>Generic vector with components of type double.</summary>
    public interface IVector_OldNumerics : IVector_OldNumerics<double>
    {

        /// <summary>Returns a deep copy of the current object.</summary>
        IVector_OldNumerics GetCopy();

    }




    namespace ComplexOldNamespace
    {
    }  // namespace IG.Num.Complex  // DO NOT USE THIS NAMESPACE!



    /* Spodaj je kot primer definiran razred kompleksnih števil.
    Vsak objekt tega razreda predstavlja eno kompleksno število. V razredu definiramo vse podatke, result katerimi
    opišemo kompleksno število, ter različne operacije na kompleksnih številih in operacije, ki definirajo
    možne interakcije kompleksnih števil z drugimi tipi. 
      Spremenljivke in metode (funkcije) razreda lahko definiramo z različnimi določili dostopa, npr. private
    (dostopno le znotraj razreda), protected (dostopno le znotraj razreda in znotraj izpeljanih razredov, ki 
    dedujejo po danem razredu) ali public (dostopno povsod, kjer je dostopen sam razred)  */

    /// <summary>Class representing general complex numbers.</summary>
    public partial class Complex_OldNumerics : IComplex_OldNumerics  // Ključna beseda partial pomeni, da lahko imamo del definicije drugje.
    // : Inumber pomeni, da razred deduje od INumber, ki je vmesnik (angleško interface). Več o tem je napisanega 
    // pri definiciji INumber.
    {
        #region Constructors
        // prevajalniška direktiva #region nima vpliva na to, kako se prevede program, in je samo v pomoč pri
        // preglednosti izvorne kode. vsak #region mora biti zaključen z #endregion. Kjer vrinemo direktivo
        // #region, se na levi strani urejevalnikovega okna prikaže majhen kvadratak; ko z miško kliknemo na
        // ta kvadratek, se del kode skrije ali ponovno razširi. Območja med #region in #endregion lahko 
        // gnezdimo. 


        /* Konstruktorji so metode, ki se izvedejo, ko naredimo objekt danega razreda. Če ne definirmo nobenega
        konstruktorja, prevajalnik avtomatično tvori konstruktor brez argumentov.
          Konstruktorjem ne definiramo tipa metode (torej tipa, ki ga metoda vrne). Imajo isto ime, kot je ime
        razreda. */

        /// <summary>Default (parameter-less) constructor, creates 0+0*i.</summary>
        public Complex_OldNumerics()
        {
            this.Re = 0.0;  // tu uporabljamo lastnosti, da priredimo ralni in imaginarni del; ključna beseda
            // this se nanaša na objekt tega razreda, na katerem kličemo dano metodo. V tem primeru lahko
            // besedo this izpustimo, ker je nedvoumno, da gre za podatek, ki je del tega objekta.
            this.Im = 0.0;
        }

        /// <summary>Initializes a complex number with specified real and imaginary part.</summary>
        /// <param name="real">Value assigned to the real part of the created complex number.</param>
        /// <param name="imaginary">Value assigned to the imaginary part of the created complex number.</param>
        public Complex_OldNumerics(double real, double imaginary)
        {
            this.Re = real;
            Im = imaginary;  // tu smo izpustili besedo this, zgoraj pa jo uporabljamo
        }

        /// <summary>Initializes a complex number with another complex number.</summary>
        /// <param name="a">Complex number whose copy is created in this constructor.</param>
        public Complex_OldNumerics(Complex_OldNumerics a)
        {
            if (a == null)
                throw new ArgumentNullException("Complex(complex): Attempt was made to initialize a complex number with a null reference.");
            this.Re = a.Re;
            this.Im = a.Im;
        }

        #endregion  // Constructors


        #region Data

        // Spremenljivki _real in _imaginary vsebujeta realni in imaginarni del kompleksnega števila. Ker ju
        // definiramo kot private, do njiju nimamo neposrednega dostopa izven definicijskega območja razreda:

        private double _real = 0, _imaginary = 0;

        // Za dostop do realnega in imaginarnega dela definiramo lastnosti (an. "property") Re in Im.
        // Vsako lastnost prevajalnik tretira kot dve funkciji - set() (result katero nastavimo vrednost) in get()
        // (result katero pridobimo vrednost). Lastnost se navzven kaže kot spremenljivka. Prevajalnik vsako
        // prirejanje vrednosti tej 'spremenljivki' prevede v klic funkcije set(), vsako ovrednotenje 'spremenljivke'
        // v izrazu pa v klic funkcije get().
        // Lastnosti med drugim omogočajo, da ločeno definiramo dostopne pravice za prirajanje in za ovrednotenje
        // spremenljivke, da pred prirejanjem testiramo morebitne omejitve za vrednosti spremenljivk, ali da pri 
        // vsakem prirejanju poskrbimo za stvari, ki so odvisne od danih spremenljivk.


        /// <summary>Gets (public access) or sets (protected access) the real part of the complex number.</summary>
        public virtual double Re   // virtual pomeni, da lahko v izpeljanih razredih definicijo povozimo
        {
            get   // ker ni posebej definiran, velja za vrednost dostop public, tako kot za celotno lastnost
            {
                return _real;
            }
            protected set  // dostop za prirejanje je omejen na notranjost razreda in izpeljanih razredov
            {
                _real = value;  // ključna beseda value nadomesti vrednost, ki jo priredimo lastnosti
            }
        }

        /// <summary>Gets (public access) or sets (protected access) the imaginary part of the complex number.</summary>
        public virtual double Im
        {
            get
            {
                return _imaginary;
            }
            protected set
            {
                _imaginary = value;
            }
        }

        // Za parametra trigonometrične (polarne) oblike uporabimo lastnosti, v tem primeru vrednotenje lastnosti
        // vključuje enostaven izračun, ne le prepisovanje vrednosti interne spremenljivke:

        /// <summary>Modulus of the complex number</summary>
        public virtual double r
        {
            get { return Math.Sqrt(this.Re * this.Re + this.Im * this.Im); }
        }

        /// <summary>Argument of the complex number.</summary>
        public virtual double fi
        {
            get
            {
                if (Re > 0)
                    return Math.Atan(Im / Re);
                else if (Re == 0)
                {
                    if (Im > 0)
                        return Math.PI / 2;
                    else if (Im < 0)
                        return -Math.PI / 2;
                    else
                        return 0.0;
                }
                else if (Re < 0)
                {
                    if (Im >= 0)
                        return Math.Atan(Im / Re) + Math.PI;
                    else if (Im < 0)
                        return Math.Atan(Im / Re) - Math.PI;
                }
                return 0.0;
            }
        }

        #endregion  // Data


        #region Constants

        /* Ker lahko result "const" definiramo samo vrednostne (torej enostavne) tipe ter nize, moramo za konstante
        uporabiti lastnosti, ki nimajo definiranega set(). */

        private static Complex_OldNumerics _zero = null;
        /// <summary>Complex constant 0 + 0*i (summation unit)</summary>
        public static Complex_OldNumerics Zero
        {
            get
            {
                if (_zero == null)
                    _zero = new Complex_OldNumerics(0.0, 0.0);
                _zero.Re = _zero.Im = 0.0;  // to je za vsak primer, ker bi lahko kdo spreminjal vrednosti
                return _zero;
            }
        }

        private static Complex_OldNumerics _one = null;
        /// <summary>Complex constant 1 + 0*i (summation unit)</summary>
        public static Complex_OldNumerics One
        {
            get
            {
                if (_one == null)
                    _one = new Complex_OldNumerics(0.0, 0.0);
                _one.Re = 1.0; _one.Im = 0.0;
                return _one;
            }
        }

        private static Complex_OldNumerics _i = null;
        /// <summary>Complex constant 1 + 0*i (summation unit)</summary>
        public static Complex_OldNumerics i
        {
            get
            {
                if (_i == null)
                    _i = new Complex_OldNumerics(0.0, 0.0);
                _i.Re = 0.0; _i.Im = 1.0;  // to je za vsak primer, ker bi lahko kdo spreminjal vrednosti
                return _i;
            }
        }

        #endregion


        #region Operators

        /* V C# lahko definiramo običajne operatorje kot so +, - ali *, tudi za tipe, ki jih sami definiramo.
        To lastnost programskega jezika v angleščini imenujemo "operator overloading".
        Definiramo lahko tako unarne kot binarne operatorje.
        Deklaracije operatorjev na novih tipih ne morejo spremeniti sintakse, precedence ali asociativnosti operatorja.
          Nov operator definiramo kot metodo, ki ima namesto imena ključno besedo "operator" in operator, ki ji sledi
        operator, ki ga definiramo. Kot običajno navedemo tip objekta, ki ga metoda vrne, in tipe ter imena parametrov.
          Funkcijo, result katero definiramo operator na novem tipu, moramo deklarirati kot public in static. Za smiselnost
        definicije moramo poskrbeti samo, saj je prevajalnik v nobenem primeru ne more preveriti (tako bi lahko npr. 
        definirali, da operator * seđšteje dve kompleksni števili). */

        /// <summary>Defines the binary operator + for summation of two complex numbers.</summary>
        public static Complex_OldNumerics operator +(Complex_OldNumerics a, Complex_OldNumerics b)
        {
            // Dogovorimo se, da nam neinicializirani operandi predstavljajo vrednost 0+0*i:
            if (a == null)
                a = Complex_OldNumerics.Zero;
            if (b == null)
                b = Complex_OldNumerics.Zero;
            Complex_OldNumerics ret = new Complex_OldNumerics();  // objekt, ki ga funkcija vrne, moramo najprej narediti
            // Nato implementiramo pravilo za seštevanje dveh kompleksnih števil - enostavno seštejemo
            // realna in imaginarna dela obeh števil:
            ret.Re = a.Re + b.Re;
            ret.Im = a.Im + b.Im;
            return ret;  // Na koncu moramo obvezno vrniti rezultat.
        }

        /// <summary>Defines the binary operator - for subtraction of two complex numbers.</summary>
        public static Complex_OldNumerics operator -(Complex_OldNumerics a, Complex_OldNumerics b)
        {
            if (a == null)
                a = Complex_OldNumerics.Zero;
            if (b == null)
                b = Complex_OldNumerics.Zero;
            Complex_OldNumerics ret = new Complex_OldNumerics();
            ret.Re = a.Re - b.Re;
            ret.Im = a.Im - b.Im;
            return ret;
        }

        /// <summary>Defines the unary operator - for changing sign of a complex number.</summary>
        public static Complex_OldNumerics operator -(Complex_OldNumerics a)
        {
            if (a == null)
                a = Complex_OldNumerics.Zero;
            Complex_OldNumerics ret = new Complex_OldNumerics();
            ret.Re = -a.Re;
            ret.Im = -a.Im;
            return ret;
        }

        /// <summary>Left multiplication of a complex number by a real number.</summary>
        public static Complex_OldNumerics operator *(double a, Complex_OldNumerics c)
        {
            if (c == null)
                c = Complex_OldNumerics.Zero;
            Complex_OldNumerics ret = new Complex_OldNumerics();
            ret.Re = a * c.Re;
            ret.Im = a * c.Im;
            return ret;
        }

        /// <summary>Right multiplication of a complex number by a real number.</summary>
        public static Complex_OldNumerics operator *(Complex_OldNumerics c, double a)
        {
            if (c == null)
                c = Complex_OldNumerics.Zero;
            Complex_OldNumerics ret = new Complex_OldNumerics();
            ret.Re = a * c.Re;
            ret.Im = a * c.Im;
            return ret;
        }

        /// <summary>Complex multiplication.</summary>
        public static Complex_OldNumerics operator *(Complex_OldNumerics a, Complex_OldNumerics b)
        {
            if (a == null)
                a = Complex_OldNumerics.Zero;
            if (b == null)
                b = Complex_OldNumerics.Zero;
            Complex_OldNumerics ret = new Complex_OldNumerics();
            ret.Re = a.Re * b.Re - a.Im * b.Im;
            ret.Im = a.Re * b.Im + b.Re * a.Im;
            return ret;
        }

        public static Complex_OldNumerics operator /(Complex_OldNumerics a, Complex_OldNumerics b)
        {
            if (a == null)
                a = Complex_OldNumerics.Zero;
            if (b == null)
                b = Complex_OldNumerics.Zero;
            Complex_OldNumerics ret = new Complex_OldNumerics();
            ret.Re = (a.Re * b.Re + a.Im * b.Im) / (b.Re * b.Re + b.Im * b.Im);
            ret.Im = (b.Re * a.Im - a.Re * b.Im) / (b.Re * b.Re + b.Im * b.Im);
            return ret;
        }

        /* V C# lahko razredu definiramo indeksne operatorje. Indeksi so lahko poljubnega tipa in ni nujno, da so
        naravna števila. Spodaj kompleksnemu številu definiramo enoparametrični indeks, ki lahko ima le vrednosti
        0 ali 1. Definicija je nekoliko podobna kot pri lastnostih, le da imamo lahko še poljubno število argumentov
        in definicijo uvedemo result ključno besedo this.
          Opomba: V C# po dogovoru indeksi tabel tečejo od nič naprej. Pri uporabniško definiranih razredih lahko ta 
        dogovor kršimo z definicijo lastnih indeksnih operatorjev. Tako lahko na primer definiramo razreda za vektorje
        in matrike, kjer komponente naslavljamo z indeksi, ki tečejo od 1 naprej (in ne od 0) */

        public double this[int index]
        {
            get
            {
                if (index == 0)
                    return Re;
                else if (index == 1)
                    return Im;
                else
                {
                    // indeksi, ki niso 0 ali ena, niso dovoljeni:
                    throw new OverflowException("Index " + index.ToString() + "is out of range, should be 0 or 1.");
                }
            }
            set
            {
                if (index == 0)
                    Re = value;
                else if (index == 1)
                    Im = value;
                else
                    throw new OverflowException("Index " + index.ToString() + "is out of range, should be 0 or 1.");
            }
        }

        /* Spodaj je demonstracija, kako lahko indeksne operatorje res definiramo na poljubne načine. 
        Definiramo operator, ki kot indekse sprejme vnaprej določen nabor nizov in vrne vrednosti glede 
        na dogovorjeni pomen nizov. Indeksni operator definiramo tako, da so lahko nizi podani v velikih 
        ali malih črkah, lahko tudi mešano. */

        public double this[string index]
        {
            get
            {
                string ind = index.ToLower();
                if (ind == "real")
                    return Re;
                else if (ind == "imaginary")
                    return Im;
                else if (ind == "re")
                    return Re;
                else if (ind == "im")
                    return Im;
                else if (ind == "r")
                    return r;
                else if (ind == "fi")
                    return fi;
                else
                {
                    // indeksi, ki niso 0 ali ena, niso dovoljeni:
                    throw new OverflowException("Index " + index.ToString() + "is out of allowed values.");
                }
            }
            set
            {
                string ind = index.ToLower();
                if (ind == "real")
                    Re = value;
                else if (ind == "imaginary")
                    Re = value;
                else if (ind == "re")
                    Re = value;
                else if (ind == "im")
                    Im = value;
                else
                {
                    // indeksi, ki niso 0 ali ena, niso dovoljeni:
                    throw new OverflowException("Index " + index.ToString() + "is out of allowed values.");
                }
            }
        }


        #endregion  // Operators


        #region Methods

        // Implementacija vmesnika IField:



        public Complex_OldNumerics Add(Complex_OldNumerics x) { return this + x; }
        public Complex_OldNumerics Subtract(Complex_OldNumerics x) { return this - x; }
        public Complex_OldNumerics Multiply(Complex_OldNumerics x) { return this * x; }
        public Complex_OldNumerics Divide(Complex_OldNumerics x) { return this / x; }
        public Complex_OldNumerics Negative() { return -this; }
        public Complex_OldNumerics Inverse() { return One / this; }





        /* Statično metodo lahko uporabimo za inicializacijo kompleksnega števila result parametri v polarni obliki, ker
        smo konstruktor z dvema argumentoma tipa double že uporabili za inicializacijo, kjer podamo realni in 
        imaginarni del: */

        public static Complex_OldNumerics Polar(double r, double fi)
        {
            return new Complex_OldNumerics(r * Math.Cos(fi), r * Math.Sin(fi));
        }

        /* Spodaj definiramo nekaj pogosto uporabljanih operacij na tipu Complex, od računalniških (npr. pretvorba v
        tekstovno reprezentacijo) do matmatičnih (izračun absolutne vrednosti).
        Včasih je koristno definirati tudi statične verzije funkcij, ki jih ne kličemo preko narejenega objekta, ampak
        z navedbo razreda, v katerem so funkcije definirane. */

        /// <summary>Returns a string representation of the complec number in the form "2.3+4.5*i".</summary>
        public override string ToString()  // beseda override je potrebna, ker tu povozimo funkcijo ToString, ki je definirana v
        // razretu object, od tega razreda pa implicitno dedujejo vsi razredi v C#.
        {
            if (this == null)
                return ("null");
            else if (Im < 0)
                return "(" + this.Re + this.Im + "*i)";
            else
                return "(" + this.Re + "+" + this.Im + "*i)";
        }


        /// <summary>Absolute value of the complex number.</summary>
        public double Abs()
        {
            return Math.Sqrt(this.Re * this.Re + this.Im * this.Im);
        }

        /// <summary>Returns an absolute value of a complex number.</summary>
        static public double Abs(Complex_OldNumerics a)
        {
            return Math.Sqrt(a.Re * a.Re + a.Im * a.Im);
        }

        /// <summary>Complex conjugate.</summary>
        public Complex_OldNumerics Conjugate()
        {
            return new Complex_OldNumerics(this.Re, -this.Im);
        }

        /// <summary>Returns a complex conjugate of the argument.</summary>
        static public Complex_OldNumerics Conjugate(Complex_OldNumerics a)
        {
            return new Complex_OldNumerics(a.Re, -a.Im);
        }


        #endregion  // Methods


        #region Demonstration_Testing

        public static void Example()
        {
            Complex_OldNumerics a, b, c;
            Real_OldNumerics r;
            // Naredimo nekaj kompleksnih števil in jih priredimo spremenljivkam:
            a = new Complex_OldNumerics(1, 2);  // tu naredimo novo kompleksno število result konstruktorjem z dvema elementoma 
            b = Complex_OldNumerics.Conjugate(a);  // uporaba statične funkcije, ki naredi konjugirano koompleksno št. argumenta
            c = new Real_OldNumerics(5);  // Spremenljivki tipa Complex priredimo na novo ustvarjeni objekt tipa Real.
            // To je dopustno, ker je razred Real izpeljan iz Complex, kar pomeni, da lahko vsak objekt tipa Real 
            // uporabljamo tudi kot objekt tipa Complex (obratno pa ne drži). 
            // Procesu, kjer spremenljivki danega tipa priredimo objekt izpeljanega tipa, pravimo downcasting.
            // Angleška beseda casting se na splošno uporablja za prirejanje vrednosti danega tipa spremenljivki drugega tipa.
            // Spremenljivki tipa double lahko na primer brez izgube natančnosti priredimo vrednost tipa int.
            r = c as Real_OldNumerics;  // Tu je pretvorba tipov obratna: spremenljivki izpeljanega tipa priredimo vrednost spremenljivke
            // osnovnega tipa. Procesu pravimo downcasting. Ta stavek je možen le, ker spremenljivka c (ki je sicer 
            // osnovnega tipa, to je Complex) vsebuje objekt, ki je tipa Real (ki je izpeljan iz Complex in je enak 
            // spremenljivke r, ki ji prirejamo vrednost). Pri tem uporabimo operator "as", ki ugotvi, ali c dejansko vsebuje
            // objekt navedenega tipa, torej Real. Če to ne bi bilo res in bi spremenljivka c vsebovala objekt, ki je le
            // tipa Complex, ne pa tudi tipa Real, bi operator "as" vrnil null. Pomembno je vedeti, da se ujemanje tipa
            // objekta, ki ga vsebuje spremenljivka c, lahko preveri šele med izvajanjem programa in ne že v fazi 
            // prevajanja. Zaradi tega takšno prirejanje vzame nekaj dodatnega računskega časa. Lastnost jezika C#, da 
            // lahko med izvajanjem programa preverjamo tipe spremenljivk in druge podatke o spremenljivkah, metodah, 
            // razredih itd., imenujemo refleksija. Ta lastnost ni lastna vsem sodobnim programkim jezikom.
            // Izpis spremenljivk:
            Console.WriteLine("a = " + a.ToString());
            Console.WriteLine("a[0] = " + a[0].ToString());  // demonstracija uporabniško definiranega indeksnega operatorja
            Console.WriteLine("a[1] = " + a[1].ToString());  // demonstracija uporabniško definiranega indeksnega operatorja
            Console.WriteLine("b = " + b.ToString());
            Console.WriteLine("c = " + c.ToString());  // pozor, c ima drugačen izpis kot a in s; to je zato, ker c vsebuje objekt
            // tipa Real, v tem tipu pa smo predefinirali metodo ToString().
            Console.WriteLine("r = " + r.ToString());
            Console.WriteLine("a + b = " + (a + b).ToString());
            Console.WriteLine("a * b = " + (a * b).ToString());
            Console.WriteLine("a * b + c = " + (a * b + c).ToString());
            Console.WriteLine("a + c = " + (a + c).ToString());

            // Prirejanje referenc in kopiranje vsebine objektov:
            // Zelo pomembno je ločiti med prirejanjem referenc objektov in kopiranjem objektov. Spremenljivke referenčnih
            // tipov v C#, kamor spadajo vsi uporabniško definirani tazredi, v resnici vsebujejo le naslove lokacij v spominu,
            // kjer se nahaja vsebina objektov. Pri prirejanju z operatorjem = se kopirajo le naslovi in ne dejanska vsebina.
            // Če hočemo kopirati celo vsebino objektov, moramo posebej implementirati metode za to. En način je definicija
            // konstruktorja, ki kot argument vzame objekt danega razreda in skopira njegovo vsebino v na novo narejeni objekt.
            // Primer za to je konstruktor Complex(Complex).
            // Ko naredimo now objekt danega razreda z uporabo operatorja new, se v dinamičnem delu spomina najprej alocira
            // prostor za vsebino objekta, potem se kliče konstruktor, ki sledi operatorju new in ustrezno inicializira
            // vsebino objekta (kako to naredi, defiiramo v telesu konstruktorja; tudi, če za nek razred ne definiramo
            // nobenega konstruktorja, C# implicitno definira konstruktor brez argumentov, ki inicializira vsa polja na
            // izhodiščne vrednosti - 0 za vrednostne in null za referenčne tipe). Na koncu operator null vrne naslov tako
            // alociranega območja v spominu in tega navadno priredimo kakšni spremenljivki.
            // Za sproščanje dinamično alociranega spomina v C# programerju ni potrebno skrbeti. Za to avtomatsko poskrbi
            // poseben proces, ki ga v angleščini imenujemo Garbage collector.

            // Spodnji primer nazorno pokaže razliko med kopiranjem referenc (naslovov) in vrednosti.
            // Kopiranje referenc z uporabo operatorja =:
            Complex_OldNumerics x, y;
            x = new Complex_OldNumerics(1, 2);
            y = x;   // prirejanje kopira samo naslov; zdaj imamo dve spremenljivki, ki kažeta na isti objekt.
            Console.WriteLine();
            Console.WriteLine("Values of x and y after y = x:");
            Console.WriteLine("x = " + x.ToString());
            Console.WriteLine("y = " + y.ToString());
            // Sedaj spremenimo vsebino x. S tem se spremeni tudi vsebina y, saj x in y naslavljata isti objekt:
            x[1] = 3.33;
            Console.WriteLine("Values of x and y after x has changed - y has also changed because y addresses the same object:");
            Console.WriteLine("x = " + x.ToString());
            Console.WriteLine("y = " + y.ToString());
            // Potem naredimo stvari drugače - za prirejanje y uporabimo konstruktor, ki skopira vsebino objekta y:
            // Najprej inicializiramo x:
            x = new Complex_OldNumerics(1, 2);
            y = new Complex_OldNumerics(x);  // y sedaj kaže na svoj objekt, ki pa vsebuje kopijo objekta, na katerega kaže x
            Console.WriteLine();
            Console.WriteLine("Values of x and y after y = new Complex(x):");
            Console.WriteLine("x = " + x.ToString());
            Console.WriteLine("y = " + y.ToString());
            // Sedaj spremenimo vsebino x. S tem vsebina y ostane nedotaknjena, saj x in y naslavljata različna objekta:
            x[1] = 3.33;
            Console.WriteLine("Values of x and y after x has changed - change has no effect on y:");
            Console.WriteLine("x = " + x.ToString());
            Console.WriteLine("y = " + y.ToString());

            // V C# poznamo poleg referenčnih tipov tudi vrednostne tipe. To so poleg osnovnih preddefiniranih tipov
            // kot so double, int, long itd. še strukture, ki jih definiramo result struct. Strukture so zelo potobne razredom,
            // le da so to vrednostni tipi, spremenljivke teh tipov torej dejansko vsebujejo vsebino in ne samo naslovov
            // na spominske lokacije, kjer se vsebina nahaja.
            // Vrednost null pri spremenljivkah referenčnih tipov enostavno pomeni, da spremanljivka ne kaže na noben
            // objekt tega tipa. Z drugimi besedami - vrednost spremenljivke ni definirana. Spremenljivkam vrednostnih tipov
            // ne moremo prirediti null, ker ne vsebujejo spominskih naslovov. Za vsako spremenljivko vrednostnega tipa
            // je ves čas njenega obstoja rezerviranega natančno toliko spomina, kolikor je potrebnega za vsebino danega
            // tipa. 
            // Razliko glede na referenčne tipe ponazarja naslednji primer, ki je analogen demonstraciji prirejanja pri
            // referenčnih tipih:
            double xd, yd;
            xd = 10;
            yd = xd;  // prirejanje kopira vrednost (vsebino spremenljivke), spremenljivki pa imata ločen
            // prostor v spominu
            Console.WriteLine();
            Console.WriteLine("Values of x and y after yd = xd:");
            Console.WriteLine("xd = " + xd.ToString());
            Console.WriteLine("yd = " + yd.ToString());
            // Sedaj spremenimo vsebino x. S tem vsebina y ostane nedotaknjena:
            xd = Math.PI;
            Console.WriteLine("Values of x and y after x has changed - change has no effect on y:");
            Console.WriteLine("xd = " + xd.ToString());
            Console.WriteLine("yd = " + yd.ToString());

            // Uporaba vmesnikov pri deklaracij spremenljivk:
            // Posebnost C# je, da lahko vmesnike uporabimo pri deklaraciji spremenljivk, čeprav ne vsebujejo
            // konkretnih definicij.
            // Ker razred Complex implementira vmesnik IComplex, lahko za definicijo tipa Complex uporabimo tudi
            // vmesnik IXomplex:
            IComplex_OldNumerics aa;
            aa = new Complex_OldNumerics(5, 7);
            Console.WriteLine();
            Console.WriteLine("Variable declared through an interface:");
            Console.WriteLine("aa = " + aa.ToString());
            // Opomba: pri aa ne moremo uporabiti recimo aa.Conjugate(), ker ta metoda ni deklarirana v vmesniku.

        }


        #endregion  // Demonstration_Testing

        #region Testing


        #endregion

    }  // class Complex




    /* Dedovanje:
      Iz razreda a lahko izpeljemo razred s, ki ima lastnosti razreda a (spremenljivke in metode) in
    še nekaj dodatnih lastnosti. Pravimo, da razred s deduje po razredu a, je izpeljan iz razreda a, 
    oziroma je podrazred razreda a.
      V C# lahko razred deduje samo po enem razredu (to je drugače kot v C++, kjer je dovoljeno večkratno
    dedovanje). Lahko pa razred deduje po poljubnem številu vmesnikov (interface-ov).
      Spodaj definiramo razred realnih števil, ki deduje od razreda kompleksnih števil. Pri tem se oziramo
    na dejstvo, da so realna števila  kompleksna števila z imaginarnim delom nič in temu priredimo implementacijo
    izpeljanega tipa.
      V praksi je seveda nesmiselno za reprezentacijo enostavnega tipa, kot so realna števila, uporabljati
     razred izpeljan iz tipa, ki predstavlja kompleksna števila. Zato ta razred uporabimo samo za demonstracijo
     koncepta dedovanja, za predstavitev realnih števil pa bomo še naprej uporabljali preddefinirani tip double. */

    public class Real_OldNumerics : Complex_OldNumerics // dvopičje označuje, da razred Real deduje od razreda Complex (ali
    // da je Real podrazred razreda Complex). Razred Real tudi implementira vmesnik IComplex, ker 
    // razred Complex (po katerem Real deduje) implementira ta vmesnik. To bi lahko še eksplicitno
    // navedli tako, da bi IComplex navedli za Complex in ga ločili z vejico.
    {

        #region Constructors

        /* Konstruktorje definiramo na novo in tudi niso vsi enaki, kot pri razredu complex */

        public Real_OldNumerics()
        {
            Re = 0.0;
        }

        public Real_OldNumerics(double x)
        {
            Re = x;
        }

        public Real_OldNumerics(Complex_OldNumerics a)
        {
            if (a == null)
                Re = 0.0;
            else
            {
                Re = a.Re;
                Im = a.Im;  // ta stavek vrže izjemo (v lastnosti Im), če a.Im ni 0.
            }
        }


        #endregion


        #region Data


        // Lastnost Re vzamemo kar iz dedovanega razreda Complex, Im pa vedno vrne 0, pri prirejanju pa javimo
        // napako, če je prirejena vrednost različna od 0.

        /// <summary>Gets (public access) or sets (protected access) the imaginary part of the complex number.</summary>
        public override double Im  // beseda override pomeni, da povozimo (nadomestimo) definicijo iz dedovanega razreda.
        {
            get
            {
                return 0;  // ker je pri realnih številih imaginarni del vedno enak 0.
            }
            protected set
            {
                // Realno število nima imaginarnega dela, zato je prireditev imaginarnega dela napaka in
                // pri takšnem pokusu sprožimo napako:
                if (value != 0.0)
                    throw new Exception("Attempt to assign imaginary part to a real number.");
            }
        }

        // Lastnosti r in fi, ki predstavljata parametra polarnega zapisa kompleksnega števila, definiramo 
        // na novo. Definicija v dedovanem dipu Complex je sicr pravilna tudi za realna števila, vendar 
        // lahko pri realnih številih ti dve lastnosti bolj enostavno izračunamo in result ponovno definicijo 
        // nekaj prdobimo pri računski učinkovitosti. S podobnim argumentom bi lahko predefinirali še druge
        // lastnosti, metode in operatorje.

        public override double r { get { return Math.Abs(Re); } }

        public override double fi { get { return 0.0; } }

        #endregion Data

        // V izpeljanem tipu predefiniramotudi metodo ToString(), ker nima smisla, da bi prikazovali tudi 
        // imaginarni del, ki je pri realnih številih vedno enak nič:

        /// <summary>Vrne tekstovno repreyentacijo realnega števila.</summary>
        public override string ToString()
        {
            return Re.ToString();
        }


    }

    // VMESNIKI (angleško interface):
    // Vmesniki so na videz podobni razredom, vendar ne vsevbujejo definicij spremenljivk, metod  ali lastnosti,
    // temveč le DEKLARACIJE metod in lastnosti. 
    // Deklaracija metode je le predpis, ki pove, kako kličemo metodo, ne vsebuje pa implementacije konkretne metode,
    // ki jo lahko dejansko kličemo in ki nekaj naredi. Deklaracija vsebuje tip vrnjene vrednosti ter ime metode, ki
    // ji sledijo v oklepajih našteti parametri metode.
    // Razredi lahko dedujejo po vmesnikih. V nasprotju z dedovanjem po drugih razredih lahko razred deduje po poljubnem
    // številu vmesnikov.
    // Vmesniki vsebujejo le predpise vzorcev, ki jim mora zadoščati razred, ki deduje po danem vmesniku. Metode, ki so
    // deklarirane v danem vmesniku, mora razred, ki po njem deduje, sam definirati. Zato navadno rečemo, da razred
    // implementira določeni vmesnik in ne, da po njem deduje.
    // Vmesniki lahko vsebujejo le deklaracije metod in lastnosti, ki imajo dostop 'public' in niso statične. Ne morejo
    // vsebovati definicij operatorjev.
    // Vmesniki imajo lahko podobno vlogo kot deklaracije funkcij v C in C++, vendar imajo v C# še druge vloge. Vmesnike
    // lahko tako kot razrede uporabimo v deklaracijah spremenljivk, čeprav ne definirajo konkretnih podatkovnih struktur.


    /// <summary>Interface that must be implemented by all complex numbers.</summary>
    interface IComplex_OldNumerics
    {

        // Deklaracije lastnosti:
        // V na[em primeru imajo samo get deli teh lastnosti dostop "public". Specifikatorja dostopa v vmesnikih 
        // ne navajamo, ker mora biti public. 

        double Re { get; }

        double Im { get; }

        double r { get; }

        double fi { get; }

        // Deklaracije lastnosti:
        double Abs();

    }



    /// <summary>A generic interface.</summary>
    public interface IField_OldNumerics<T>
    {
        T Add(T a, T b);
        T Subtract(T a, T b);
        T Multiply(T a, T b);
        T Divide(T a, T b);
        T Negative(T a);
        T Inverse(T a);
    }

    public interface IFieldComplex_OldNumerics<T> : IField_OldNumerics<T>
    {
        T Conjugate(T a);
    }





    /// <summary>XVector whose vec are real numbers.</summary>
    public class Vector_OldNumerics : Vector_OldNumerics<double>
    {

        #region Initialization

        protected Vector_OldNumerics() { }

        public Vector_OldNumerics(int dimension)
        {
            Init(dimension);
        }

        public Vector_OldNumerics(int dimension, double comp)
        {
            Init(dimension, comp);
        }

        public Vector_OldNumerics(double[] array)
        {
            Init(array);
        }

        public Vector_OldNumerics(Vector_OldNumerics cv)
        {
            Init(cv);
        }

        #endregion Initialization


        #region Operations


        // Implementacija podedovanih abstraktnih metod:

        protected override double Zero
        { get { return 0.0; } }

        protected override double Add(double a, double b)
        { return a + b; }

        protected override double Subtract(double a, double b)
        { return a - b; }

        protected override double Multiply(double a, double b)
        { return a * b; }

        protected override double Divide(double a, double b)
        { return a / b; }

        protected override double Negative(double a)
        { return -a; }

        protected override double Inverse(double a)
        { return 1.0 / a; }

        protected override double Conjugate(double a)
        { return a; }


        // Overloaded operators:

        public static Vector_OldNumerics operator +(Vector_OldNumerics a, Vector_OldNumerics b)
        {
            Vector_OldNumerics ret = new Vector_OldNumerics(a);
            ret.Add(b);
            return ret;
        }

        public static Vector_OldNumerics operator -(Vector_OldNumerics a, Vector_OldNumerics b)
        {
            Vector_OldNumerics ret = new Vector_OldNumerics(a);
            ret.Subtract(b);
            return ret;
        }

        public static Vector_OldNumerics operator *(Vector_OldNumerics a, Vector_OldNumerics b)
        {
            Vector_OldNumerics ret = new Vector_OldNumerics(a);
            ret.RightScalarProduct(b);
            return ret;
        }


        public static Vector_OldNumerics operator *(Vector_OldNumerics a, double b)
        {
            Vector_OldNumerics ret = new Vector_OldNumerics(a);
            ret.Multiply(b);
            return ret;
        }

        public static Vector_OldNumerics operator *(double b, Vector_OldNumerics a)
        {
            Vector_OldNumerics ret = new Vector_OldNumerics(a);
            ret.Multiply(b);
            return ret;
        }

        #endregion  // Operations

    }  // class XVector


    /// <summary>Complex vector.</summary>
    public class ComplexVector_OldNumerics : Vector_OldNumerics<Complex_OldNumerics>
    {

        #region Initialization

        // Constructors:

        protected ComplexVector_OldNumerics() { }

        public ComplexVector_OldNumerics(int dimension)
        {
            Init(dimension);
        }

        public ComplexVector_OldNumerics(int dimension, Complex_OldNumerics comp)
        {
            Init(dimension, comp);
        }

        public ComplexVector_OldNumerics(Complex_OldNumerics[] array)
        {
            Init(array);
        }

        public ComplexVector_OldNumerics(ComplexVector_OldNumerics cv)
        {
            Init(cv);
        }

        #endregion   // Initialization

        #region Operations


        // Implementacija podedovanih abstraktnih metod:

        protected override Complex_OldNumerics Zero
        { get { return new Complex_OldNumerics(0.0, 0.0); } }

        protected override Complex_OldNumerics Add(Complex_OldNumerics a, Complex_OldNumerics b)
        { return a + b; }

        protected override Complex_OldNumerics Subtract(Complex_OldNumerics a, Complex_OldNumerics b)
        { return a - b; }

        protected override Complex_OldNumerics Multiply(Complex_OldNumerics a, Complex_OldNumerics b)
        { return a * b; }

        protected override Complex_OldNumerics Divide(Complex_OldNumerics a, Complex_OldNumerics b)
        { return a / b; }

        protected override Complex_OldNumerics Negative(Complex_OldNumerics a)
        { return Complex_OldNumerics.Zero - a; }

        protected override Complex_OldNumerics Inverse(Complex_OldNumerics a)
        { return Complex_OldNumerics.One / a; }

        protected override Complex_OldNumerics Conjugate(Complex_OldNumerics a)
        {
            return Complex_OldNumerics.Conjugate(a);
        }


        // Overloaded operators:

        public static ComplexVector_OldNumerics operator +(ComplexVector_OldNumerics a, ComplexVector_OldNumerics b)
        {
            ComplexVector_OldNumerics ret = new ComplexVector_OldNumerics(a);
            ret.Add(b);
            return ret;
        }

        public static ComplexVector_OldNumerics operator -(ComplexVector_OldNumerics a, ComplexVector_OldNumerics b)
        {
            ComplexVector_OldNumerics ret = new ComplexVector_OldNumerics(a);
            ret.Subtract(b);
            return ret;
        }

        public static ComplexVector_OldNumerics operator *(ComplexVector_OldNumerics a, ComplexVector_OldNumerics b)
        {
            ComplexVector_OldNumerics ret = new ComplexVector_OldNumerics(a);
            ret.RightScalarProduct(b);
            return ret;
        }


        public static ComplexVector_OldNumerics operator *(ComplexVector_OldNumerics a, Complex_OldNumerics b)
        {
            ComplexVector_OldNumerics ret = new ComplexVector_OldNumerics(a);
            ret.Multiply(b);
            return ret;
        }

        public static ComplexVector_OldNumerics operator *(Complex_OldNumerics b, ComplexVector_OldNumerics a)
        {
            ComplexVector_OldNumerics ret = new ComplexVector_OldNumerics(a);
            ret.Multiply(b);
            return ret;
        }

        #endregion  // Operations


    }  // class ComplexVector


    public abstract class Vector_OldNumerics<ComponentType>
    {

        #region Contructors_Initialization

        protected Vector_OldNumerics()  // hide default constructor
        {
        }


        public Vector_OldNumerics(int dim1, ComponentType comp)
        {
            Init(dim1, comp);
        }

        public Vector_OldNumerics(ComponentType[] array)
        {
            Init(array);
        }

        /// <summary>Initializes a matrix with dimendions dim1 and dim2.</summary>
        /// <param name="dim1">Number of rows.</param>
        /// <param name="dim2">Number of columns.</param>
        public void Init(int dim)
        {
            _dim = dim;
            if (_dim < 1)
                throw new IndexOutOfRangeException("Can not create a vector with dimension less than 1. Specified dimension: " + dim);
            _tab = new ComponentType[_dim];
            // TODO: check this! dim1;
            for (int i = 0; i < _dim; ++i)
                _tab[i] = default(ComponentType);
        }

        /// <summary>Initializes a vector with dimendion length and sets all vec to comp.</summary>
        /// <param name="length">XVector dimension.</param>
        public void Init(int dim, ComponentType comp)
        {
            _dim = dim;
            if (_dim < 1)
                throw new IndexOutOfRangeException("Can not create a vector of dimension less than 1. Specified dimension: " + dim);
            _tab = new ComponentType[_dim];
            _dim = dim;
            for (int i = 0; i < _dim; ++i)
                _tab[i] = comp;
        }

        /// <summary>Initializes a vector with dimendion length and sets all vec to comp.</summary>
        /// <param name="length">XVector dimension.</param>
        public void Init(ComponentType[] array)
        {
            _dim = array.Count<ComponentType>();
            if (_dim < 1)
                throw new IndexOutOfRangeException("Can not create a vector of dimension less than 1. Specified dimension: " + _dim);
            _tab = new ComponentType[_dim];
            for (int i = 0; i < _dim; ++i)
                _tab[i] = array[i];
        }

        /// <summary>Initializes a vector with dimendion length and sets all vec to comp.</summary>
        /// <param name="length">XVector dimension.</param>
        public void Init(Vector_OldNumerics<ComponentType> v)
        {
            _dim = v.Dimension;
            if (_dim < 1)
                throw new IndexOutOfRangeException("Can not create a vector of dimension less than 1. Specified dimension: " + _dim);
            _tab = new ComponentType[_dim];
            for (int i = 0; i < _dim; ++i)
                _tab[i] = v[i];
        }


        public static ComponentType[] Copy(Vector_OldNumerics<ComponentType> v)
        {
            ComponentType[] ret;
            if (v == null)
                ret = null;
            else
            {
                int dim = v.Dimension;
                ret = new ComponentType[dim];
                for (int i = 0; i < dim; ++i)
                    ret[i] = v[i];
            }
            return ret;
        }


        #endregion  // Contructors_Initialization

        #region Data

        protected int _dim;
        protected ComponentType[] _tab;

        /// <summary>Returns vector dimension.</summary>
        public int Dimension { get { return _dim; } }

        /// <summary>Returns vector dimension.</summary>
        public int Dimension2 { get { return _dim; } }

        /// <summary>Synonyme for Dimension, returns vector dimension.</summary>
        public int d { get { return _dim; } }

        /// <summary>Gets or sets a specific vector componene.</summary>
        /// <param name="ind">Component index running from 0 to l-1.</param>
        /// <returns>a referance to a component for a reference type ComponentType or its value for value types.</returns>
        public virtual ComponentType this[int ind]
        {

            get
            {
                if (ind < 0 || ind >= _dim)
                    throw new IndexOutOfRangeException("Index " + ind.ToString() + " is out of range, should be between 0 and "
                        + (_dim - 1).ToString() + ".");
                return _tab[ind];
            }
            set
            {
                if (ind < 0 || ind >= _dim)
                    throw new IndexOutOfRangeException("Index " + ind.ToString() + " is out of range, should be between 0 and "
                        + (_dim - 1).ToString() + ".");
                _tab[ind] = value;
            }
        }


        #endregion  // Data


        #region Operations


        /// <summary>Redefinition of Tostring(), converts a vector to string representation.</summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            int d1 = _dim - 1;
            sb.Append('{');
            for (int i = 0; i < d1; ++i)
            {
                sb.Append(this[i].ToString());
                sb.Append(", ");
                sb.Append(this[d1]);
                sb.Append("}");
            }
            return sb.ToString();
        }

        // Spodaj deklariramo osnovne operacije na polju, nad katerim je definiran vektor. Te operacije so
        // potrebne, da lahko definiramo seštevanje vektorjev, množenje result skalarjem, skalarni produkt itd.
        // Vendar na tem mestu ne moremo operacij tudi definirati, ker so lahko argumenti poljubnih tipov in
        // zato ni znano vnaprej, kako se dejansko izvede recimo seštevanje ali množenje dveh objektov tega tipa.
        // Da gre samo za deklaracije in ne za definicije, povemo z besedo "abstract". Razred, ki vsebue vsaj 
        // eno abstraktno metodo, mora biti tudi sam deklariran kot abstrakten. Abstraktnega razreda ne moremo
        // instanciirati, lahko pa po njem dedujejo drugi razredi. Razred, ki implementira vse metode, ki so 
        // deklarirane kot abstraktne v abstraktnem razredu, po katerem deduje, je konkreten in ga lahko 
        // instanciiramo (to pomeni, da lahko z operatorjem new() naredimo objekt danega tipa).

        protected abstract ComponentType Zero { get; }
        protected abstract ComponentType Add(ComponentType a, ComponentType b);
        protected abstract ComponentType Subtract(ComponentType a, ComponentType b);
        protected abstract ComponentType Multiply(ComponentType a, ComponentType b);
        protected abstract ComponentType Divide(ComponentType a, ComponentType b);
        protected abstract ComponentType Negative(ComponentType a);
        protected abstract ComponentType Inverse(ComponentType a);
        protected abstract ComponentType Conjugate(ComponentType a);

        /// <summary>XVector addition.</summary>
        public void Add(Vector_OldNumerics<ComponentType> a)
        {
            if (a == null)
                throw new ArgumentNullException("Vector addition: first operand is null.");
            if (a.Dimension <= 0)
                throw new ArgumentException("Vector addition: dimension of operand is not greater thatn 0.");
            if (this.Dimension != a.Dimension)
                throw new ArgumentException("Vector addition: dimensions of operands are not consistent.");
            for (int i = 0; i < a.Dimension; ++i)
            {
                this[i] = Add(this[i], a[i]);
            }
        }

        /// <summary>XVector subtraction.</summary>
        public void Subtract(Vector_OldNumerics<ComponentType> a)
        {
            if (a == null)
                throw new ArgumentNullException("Vector subtraction: first operand is null.");
            if (a.Dimension <= 0)
                throw new ArgumentException("Vector subtraction: dimension of operand is not greater thatn 0.");
            if (this.Dimension != a.Dimension)
                throw new ArgumentException("Vector subtraction: dimensions of operands are not consistent.");
            for (int i = 0; i < a.Dimension; ++i)
            {
                this[i] = Subtract(this[i], a[i]);
            }
        }

        /// <summary>Multiplication of a vector by a scalar.</summary>
        public void Multiply(ComponentType scalar)
        {
            if (this.Dimension < 1)
                throw new ArgumentException("Vector subtraction: vector dimension is less than 1.");
            for (int i = 0; i < this.Dimension; ++i)
            {
                this[i] = Multiply(scalar, this[i]);
            }
        }


        /// <summary>Right scalar product.</summary>
        public ComponentType RightScalarProduct(Vector_OldNumerics<ComponentType> a)
        {
            if (a == null)
                throw new ArgumentNullException("Vector subtraction: first operand is null.");
            if (a.Dimension <= 0)
                throw new ArgumentException("Vector subtraction: dimension of operand is not greater thatn 0.");
            if (this.Dimension != a.Dimension)
                throw new ArgumentException("Vector subtraction: dimensions of operands are not consistent.");
            ComponentType ret = Zero;
            for (int i = 0; i < a.Dimension; ++i)
            {
                ret = Add(ret, Multiply(this[i], Conjugate(a[i])));
            }
            return ret;
        }

        /// <summary>Right scalar product.</summary>
        public ComponentType LeftScalarProduct(Vector_OldNumerics<ComponentType> a)
        {
            if (a == null)
                throw new ArgumentNullException("Vector subtraction: first operand is null.");
            if (a.Dimension <= 0)
                throw new ArgumentException("Vector subtraction: dimension of operand is not greater thatn 0.");
            if (this.Dimension != a.Dimension)
                throw new ArgumentException("Vector subtraction: dimensions of operands are not consistent.");
            ComponentType ret = Zero;
            for (int i = 0; i < a.Dimension; ++i)
            {
                ret = Add(ret, Multiply(a[i], Conjugate(this[i])));
            }
            return ret;
        }

        #endregion // Operations

        public static void Example()
        {
            Console.WriteLine(); Console.WriteLine();
            Console.WriteLine("Example: complex vectors:");
            ComplexVector_OldNumerics a = null, b = null;
            a = new ComplexVector_OldNumerics(3);
            a[0] = new Complex_OldNumerics(1, 2);
            a[1] = new Complex_OldNumerics(4, 1);
            a[2] = new Complex_OldNumerics(0.5, 0.6);
            b = new ComplexVector_OldNumerics(3);
            b[0] = new Complex_OldNumerics(5, 2);
            b[1] = new Complex_OldNumerics(0.2, 0.4);
            b[2] = new Complex_OldNumerics(9, 0);
            Console.WriteLine("a = " + a.ToString());
            Console.WriteLine("b = " + b.ToString());
            Console.WriteLine("a + b = " + (a + b).ToString());
        }


    }  // class XVector


    /// <summary>Matrix whose vec are real numbers.</summary>
    public class Matrix_OldNumerics : Matrix_OldNumerics<double>
    {

        #region Initialization

        protected Matrix_OldNumerics() { }

        public Matrix_OldNumerics(int dim1, int dim2)
        {
            Init(dim1, dim2);
        }

        public Matrix_OldNumerics(int dim1, int dim2, double comp)
        {
            Init(dim1, dim2, comp);
        }

        public Matrix_OldNumerics(double[,] array)
        {
            Init(array);
        }

        public Matrix_OldNumerics(Matrix_OldNumerics cv)
        {
            Init(cv);
        }

        #endregion Initialization


        #region Operations


        // Implementacija podedovanih abstraktnih metod:

        public override double Zero
        { get { return 0.0; } }

        public override double Add(double a, double b)
        { return a + b; }

        public override double Subtract(double a, double b)
        { return a - b; }

        public override double Multiply(double a, double b)
        { return a * b; }

        public override double Divide(double a, double b)
        { return a / b; }

        public override double Negative(double a)
        { return -a; }

        public override double Inverse(double a)
        { return 1.0 / a; }

        public override double Conjugate(double a)
        { return a; }


        // Overloaded operators:

        public static Matrix_OldNumerics operator +(Matrix_OldNumerics a, Matrix_OldNumerics b)
        {
            Matrix_OldNumerics ret = new Matrix_OldNumerics(a);
            ret.Add(b);
            return ret;
        }

        public static Matrix_OldNumerics operator -(Matrix_OldNumerics a, Matrix_OldNumerics b)
        {
            Matrix_OldNumerics ret = new Matrix_OldNumerics(a);
            ret.Subtract(b);
            return ret;
        }

        //public static Matrix operator *(Matrix a, Matrix s)
        //{
        //    return a.Multiply(s);
        //}


        public static Matrix_OldNumerics operator *(Matrix_OldNumerics a, double b)
        {
            Matrix_OldNumerics ret = new Matrix_OldNumerics(a);
            ret.Multiply(b);
            return ret;
        }

        public static Matrix_OldNumerics operator *(double b, Matrix_OldNumerics a)
        {
            Matrix_OldNumerics ret = new Matrix_OldNumerics(a);
            ret.Multiply(b);
            return ret;
        }

        // Produkta dveh matrik nismo mogli definirati v osnovnem razredu, ker produkt nima nujno iste dimenzije kot
        // kateri od operandov. Zato produkt dveh matrik definiramo posebej.

        /// <summary>Multiplication of two matrices.</summary>
        public static Matrix_OldNumerics operator *(Matrix_OldNumerics a, Matrix_OldNumerics b)
        {
            if (a.d1 < 1)
                throw new ArgumentException("Matrix multiplication: first matrix dimension is less than 1.");
            if (a.d2 < 1)
                throw new ArgumentException("Matrix multiplication: second matrix dimension is less than 1.");
            if (a.d2 != b.d1)
                throw new ArgumentException("Matrix multiplication: incompatible dimensions.");
            Matrix_OldNumerics ret = new Matrix_OldNumerics(a.d1, b.d2);
            for (int i = 0; i < a.d1; ++i)
                for (int j = 0; j < b.d2; ++j)
                {
                    double sum = 0.0;
                    for (int k = 0; k < a.d2; ++k)
                        sum += a[i, k] * b[k, j];
                    ret[i, j] = sum;
                }
            return ret;
        }



        #endregion  // Operations

        public static void Example()
        {
            Console.WriteLine(); Console.WriteLine();
            Console.WriteLine("Example: matrix operations:");
            Matrix_OldNumerics A = null, B = null;
            double[,] taba = { { 1, 2, 0 }, { 0, 0, 1 } };
            A = new Matrix_OldNumerics(taba);
            double[,] tabb = { { 1, 2 }, { 2, 3 }, { 0, 2 } };
            B = new Matrix_OldNumerics(tabb);
            Console.WriteLine("Matrix A: " + Environment.NewLine + A.ToString(true));
            Console.WriteLine("Matrix B: " + Environment.NewLine + B.ToString(true));
            Console.WriteLine("A * B = " + (A * B).ToString());

        }

    }  // class Matrix


    /// <summary>Generic matrix class.</summary>
    /// <typeparam name="ComponentType">Type of matrix vec.</typeparam>
    public abstract class Matrix_OldNumerics<ComponentType>
    {

        #region Contructors_Initialization

        protected Matrix_OldNumerics()  // hide default constructor
        {
        }


        public Matrix_OldNumerics(int dim1, int dim2, ComponentType comp)
        {
            Init(dim1, dim2, comp);
        }

        public Matrix_OldNumerics(ComponentType[,] array)
        {
            Init(array);
        }

        /// <summary>Initializes a Matrix with dimendion length.</summary>
        /// <param name="length">Matrix dimension.</param>
        public void Init(int dim1, int dim2)
        {
            if (dim1 < 1)
                throw new IndexOutOfRangeException("Can not create a Matrix with first dimension less than 1. Specified dimension: " + dim1);
            if (dim2 < 1)
                throw new IndexOutOfRangeException("Can not create a Matrix with second dimension less than 1. Specified dimension: " + dim2);
            _dim1 = dim1;
            _dim2 = dim2;
            _tab = new ComponentType[dim1, dim2];
            for (int i = 0; i < _dim1; ++i)
                for (int j = 0; j < _dim2; ++j)
                    _tab[i, j] = default(ComponentType);
        }

        /// <summary>Initializes a Matrix with dimendion length and sets all vec to comp.</summary>
        /// <param name="length">Matrix dimension.</param>
        public void Init(int dim1, int dim2, ComponentType comp)
        {
            if (dim1 < 1)
                throw new IndexOutOfRangeException("Can not create a Matrix with first dimension less than 1. Specified dimension: " + dim1);
            if (dim2 < 1)
                throw new IndexOutOfRangeException("Can not create a Matrix with second dimension less than 1. Specified dimension: " + dim2);
            _dim1 = dim1;
            _dim2 = dim2;
            _tab = new ComponentType[dim1, dim2];
            for (int i = 0; i < _dim1; ++i)
                for (int j = 0; j < _dim2; ++j)
                    _tab[i, j] = comp;
        }

        /// <summary>Initializes a Matrix with dimendion length and sets all vec to comp.</summary>
        /// <param name="length">Matrix dimension.</param>
        public void Init(ComponentType[,] array)
        {
            _dim1 = array.GetUpperBound(0) + 1;
            _dim2 = array.GetUpperBound(1) + 1;
            if (_dim1 < 1)
                throw new IndexOutOfRangeException("Can not create a Matrix with the first dimension less than 1. Specified dimension: " + _dim1);
            if (_dim2 < 1)
                throw new IndexOutOfRangeException("Can not create a Matrix with the second dimension less than 1. Specified dimension: " + _dim2);
            _tab = new ComponentType[_dim1, _dim2];
            for (int i = 0; i < _dim1; ++i)
                for (int j = 0; j < _dim2; ++j)
                    _tab[i, j] = array[i, j];
        }

        /// <summary>Initializes a Matrix with dimendion length and sets all vec to comp.</summary>
        /// <param name="length">Matrix dimension.</param>
        public void Init(Matrix_OldNumerics<ComponentType> m)
        {
            _dim1 = m.d1;
            _dim2 = m.d2;
            if (_dim1 < 1)
                throw new IndexOutOfRangeException("Can not create a Matrix with the first dimension less than 1. Specified dimension: " + _dim1);
            if (_dim2 < 1)
                throw new IndexOutOfRangeException("Can not create a Matrix with the second dimension less than 1. Specified dimension: " + _dim2);
            _tab = new ComponentType[_dim1, _dim2];
            for (int i = 0; i < _dim1; ++i)
                for (int j = 0; j < _dim2; ++j)
                    _tab[i, j] = m[i, j];
        }


        public static ComponentType[,] Copy(Matrix_OldNumerics<ComponentType> m)
        {
            ComponentType[,] ret;
            if (m == null)
                ret = null;
            else
            {
                int
                    dim1 = m.d1,
                    dim2 = m.d2;
                ret = new ComponentType[dim1, dim2];
                for (int i = 0; i < dim1; ++i)
                    for (int j = 0; j < dim2; ++j)
                        ret[i, j] = m[i, j];
            }
            return ret;
        }


        #endregion  // Contructors_Initialization

        #region Data

        protected int _dim1, _dim2;
        protected ComponentType[,] _tab;

        /// <summary>Returns the first Matrix dimension (number of rows).</summary>
        public int Dimension1 { get { return _dim1; } }

        /// <summary>Returns the second Matrix dimension (number of columns).</summary>
        public int Dimension2 { get { return _dim2; } }

        /// <summary>Returns the first Matrix dimension (number of rows).</summary>
        public int d1 { get { return _dim1; } }

        /// <summary>Returns the second Matrix dimension (number of columns).</summary>
        public int d2 { get { return _dim2; } }


        /// <summary>Gets or sets a specific Matrix componene.</summary>
        /// <param name="ind">Component index running from 0 to l-1.</param>
        /// <returns>a referance to a component for a reference type ComponentType or its value for value types.</returns>
        public virtual ComponentType this[int ind1, int ind2]
        {
            get
            {
                if (ind1 < 0 || ind1 >= _dim1)
                    throw new IndexOutOfRangeException("First index " + ind1.ToString() + " is out of range, should be between 0 and "
                        + (_dim1 - 1).ToString() + ".");
                if (ind2 < 0 || ind2 >= _dim2)
                    throw new IndexOutOfRangeException("Secind index " + ind2.ToString() + " is out of range, should be between 0 and "
                        + (_dim2 - 1).ToString() + ".");
                return _tab[ind1, ind2];
            }
            set
            {
                if (ind1 < 0 || ind1 >= _dim1)
                    throw new IndexOutOfRangeException("First index " + ind1.ToString() + " is out of range, should be between 0 and "
                        + (_dim1 - 1).ToString() + ".");
                if (ind2 < 0 || ind2 >= _dim2)
                    throw new IndexOutOfRangeException("Second index " + ind2.ToString() + " is out of range, should be between 0 and "
                        + (_dim2 - 1).ToString() + ".");
                _tab[ind1, ind2] = value;
            }
        }



        #endregion  // Data


        #region Operations


        /// <summary>Redefinition of Tostring(), converts a Matrix to string representation.</summary>
        /// <returns>String representation of a matrix.</returns>
        public override string ToString()
        {
            return ToString(true);
        }

        /// <summary>Converts a matrix to string representation.</summary>
        /// <param name="multiline">If true then a multi-line representation is generated.</param>
        /// <returns></returns>
        public virtual string ToString(bool multiline)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append('{');
            for (int i = 0; i < _dim1; ++i)
            {
                if (multiline)
                    sb.Append(Environment.NewLine + "  ");
                sb.Append("{");
                for (int j = 0; j < _dim2; ++j)
                {
                    sb.Append(this[i, j].ToString());
                    if (j < _dim2 - 1)
                        sb.Append(", ");
                    else
                        sb.Append("}");
                }
                if (i < _dim1 - 1)
                    sb.Append(", ");
            }
            if (multiline)
                sb.Append(Environment.NewLine);
            sb.Append("}");
            if (multiline)
                sb.Append(Environment.NewLine);
            return sb.ToString();
        }

        // Spodaj deklariramo osnovne operacije na polju, nad katerim je definiran vektor. Te operacije so
        // potrebne, da lahko definiramo seštevanje vektorjev, množenje result skalarjem, skalarni produkt itd.
        // Vendar na tem mestu ne moremo operacij tudi definirati, ker so lahko argumenti poljubnih tipov in
        // zato ni znano vnaprej, kako se dejansko izvede recimo seštevanje ali množenje dveh objektov tega tipa.
        // Da gre samo za deklaracije in ne za definicije, povemo z besedo "abstract". Razred, ki vsebue vsaj 
        // eno abstraktno metodo, mora biti tudi sam deklariran kot abstrakten. Abstraktnega razreda ne moremo
        // instanciirati, lahko pa po njem dedujejo drugi razredi. Razred, ki implementira vse metode, ki so 
        // deklarirane kot abstraktne v abstraktnem razredu, po katerem deduje, je konkreten in ga lahko 
        // instanciiramo (to pomeni, da lahko z operatorjem new() naredimo objekt danega tipa).

        public abstract ComponentType Zero { get; }
        public abstract ComponentType Add(ComponentType a, ComponentType b);
        public abstract ComponentType Subtract(ComponentType a, ComponentType b);
        public abstract ComponentType Multiply(ComponentType a, ComponentType b);
        public abstract ComponentType Divide(ComponentType a, ComponentType b);
        public abstract ComponentType Negative(ComponentType a);
        public abstract ComponentType Inverse(ComponentType a);
        public abstract ComponentType Conjugate(ComponentType a);

        /// <summary>Matrix addition.</summary>
        public void Add(Matrix_OldNumerics<ComponentType> a)
        {
            if (a == null)
                throw new ArgumentNullException("Matrix addition: first operand is null.");
            if (a.d1 <= 0 || a.d1 <= 0)
                throw new ArgumentException("Matrix addition: dimension of operand is not greater thatn 0.");
            if (this.d1 != a.d1 || this.d2 != a.d2)
                throw new ArgumentException("Matrix addition: dimensions of operands are not consistent.");
            for (int i = 0; i < a.d1; ++i)
                for (int j = 0; i < a.d2; ++j)
                {
                    this[i, j] = Add(this[i, j], a[i, j]);
                }
        }

        /// <summary>Matrix subtraction.</summary>
        public void Subtract(Matrix_OldNumerics<ComponentType> a)
        {
            if (a == null)
                throw new ArgumentNullException("Matrix subtraction: first operand is null.");
            if (a.d1 <= 0 || a.d1 <= 0)
                throw new ArgumentException("Matrix subtraction: dimension of operand is not greater thatn 0.");
            if (this.d1 != a.d1 || this.d2 != a.d2)
                throw new ArgumentException("Matrix subtraction: dimensions of operands are not consistent.");
            for (int i = 0; i < a.d1; ++i)
                for (int j = 0; i < a.d2; ++j)
                {
                    this[i, j] = Subtract(this[i, j], a[i, j]);
                }
        }


        /// <summary>Multiplication of a matrix by a scalar.</summary>
        public void Multiply(ComponentType scalar)
        {
            if (this.d1 < 1)
                throw new ArgumentException("Multiplication of a matrix by a scalar: first matrix dimension is less than 1.");
            if (this.d2 < 1)
                throw new ArgumentException("Multiplication of a matrix by a scalar: second matrix dimension is less than 1.");
            for (int i = 0; i < this.d1; ++i)
                for (int j = 0; j < this.d2; ++j)
                {
                    this[i, j] = Multiply(scalar, this[i, j]);
                }
        }


        #endregion // Operations

    }  // class Matrix<>


}
