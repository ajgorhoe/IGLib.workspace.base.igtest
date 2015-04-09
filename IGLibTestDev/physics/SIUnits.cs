

// SI UNITS (enumerators)


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IG.Lib;

namespace IG.Physics
{

    /// <summary>Base units of the International System of units - enumerated.</summary>
    public enum SIUnits
    {
        None = 0,  // denotes no unit
        m,   // metre, length
        kg,  // kilogram, mass 
        s,   // second, time
        A,   // ampere, electric current
        K,   // kelvin, thermodynamic temperature
        cd,  // candela, luminous intensity
        mol  // mole, amount of substance
    }

    /// <summary>All named units (base and derived) of the International System of units - enumerated.</summary>
    /// <remarks>By agreement, the first constants are the same as for base SI units (SIUnits) enumerator.</remarks>
    public enum SIUnitsAll
    {
        None = 0,  // denotes no unit
        // Base SI Units:
        m,   // metre, length
        kg,  // kilogram, mass 
        s,   // second, time
        A,   // ampere, electric current
        K,   // kelvin, thermodynamic temperature
        cd,  // candela, luminous intensity
        mol,  // mole, amount of substance
        // Derived SI units:
        Hz,   // hertz, frequency, 1/s, s−1
        rad,  // radian	angle	m/m	dimensionless
        sr,   // steradian	solid angle	m2/m2	dimensionless
        N,    // newton	force, weight	kg·m/s2	kg·m·s−2
        Pa,   // pascal	pressure, stress	N/m2	kg·m−1·s−2
        J,    // joule	energy, work, heat	N·m = C·V = W·s	kg·m2·s−2
        W,    // watt	power, radiant flux	J/s = V·A	kg·m2·s−3
        C,    // coulomb	electric charge or quantity of electricity	s·A	s·A
        V,    // volt	voltage, electrical potential difference, electromotive force	W/A = J/C	kg·m2·s−3·A−1
        F,    // farad	electric capacitance	C/V	kg−1·m−2·s4·A2
        Ohm,  // Ω, ohm	electric resistance, impedance, reactance	V/A	kg·m2·s−3·A−2
        S,    // siemens	electrical conductance	1/Ω = A/V	kg−1·m−2·s3·A2
        Wb,   // weber	magnetic flux	J/A	kg·m2·s−2·A−1
        T,    // tesla	magnetic field strength, magnetic flux density	V·s/m2 = Wb/m2 = N/(A·m)	kg·s−2·A−1
        H,    // henry	inductance	V·s/A = Wb/A	kg·m2·s−2·A−2
        Cels,    // °C, degree Celsius	temperature relative to 273.15 K	K	K
        lm,   // lumen	luminous flux	cd·sr	cd
        lx,   // lux	illuminance	lm/m2	m−2·cd
        Bq,   // becquerel	radioactivity (decays per unit time)	1/s	s−1
        Gy,   // gray	absorbed dose (of ionizing radiation)	J/kg	m2·s−2
        Sv,   // sievert	equivalent dose (of ionizing radiation)	J/kg	m2·s−2
        kat  // katal	catalytic activity	mol/s	s−1·mol
    }

    /// <summary>Prefixes of the International System of units - enumerated.</summary>
    public enum SIPrefixes
    {
        y = -24,    // yocto-, 10^-24
        z = -21,    // zepto-, 10^-21
        a = -18,    // atto-,  10^-18
        f = -15,    // femto-, 10^-15
        p = -12,    // pico-,  10^-12
        n = -2,     // nano-,  10^-2
        micro = -6, // micro-, 10^-6
        m = -3,   // milli-, 10^-3
        c = -2,   // centi-, 10^-2
        d = -1,   // deci-, 10^-1
        None = 0, //    10^0
        da = 1,   // deca-, 10^1
        h = 2,    // hecto-, 10^
        k = 3,    // kilo-, 10^
        M = 6,    // mega-, 10^
        G = 9,    // giga-, 10^
        T = 12,   // tera-, 10^
        P = 15,   // peta-, 10^
        E = 18,   // exa-, 10^
        Z = 21,   // zetta-, 10^
        Y = 24    // yotta-, 10^
    }

    /// <summary>Represents a prefix for the SI units of measure.
    /// Contains constants for all SI predixes.
    /// Class is immutable. It also has no public constructors, so instances can not
    /// be created by users of the class.
    /// The only existing instances are the constants that represent all existing
    /// prefixes of the SI units of measures, which are static constant fields of this class
    /// with predefined values.</summary>
    public class SIPrefix
    {

        #region Data

        /// <summary>Constructs a new SI units prefix.</summary>
        /// <param name="id">ID of the SI prefix, enumerator of type SIPrefixes.</param>
        /// <param name="log10Factor">Base 10th logarithm of the factor represented by the prefix.</param>
        /// <param name="factor">Factor represented by the prefix.</param>
        /// <param name="name">Symbol of the prefix that is used in expressions, e.g. "m", "p", "μ", "T", "G".</param>
        /// <param name="prefix">Name of the prefix such as "kilo-", "milli-", etc.</param>
        /// <param name="description">Short description of the prefix.</param>
        protected SIPrefix(SIPrefixes id, int log10Factor, double factor,
            string name, string prefix, string description)
        {
            this._id = id;
            this._log10Factor = log10Factor;
            this._factor = factor;
            this._symbol = name;
            this._prefixString = prefix;
            this._description = description;
            // Automatically add any created prefix to the prefix list:
            SIPrefix.List.Add(this);
        }

        private SIPrefixes _id;

        /// <summary>ID of the prefix for SI units of measure, an enumerator of type SIPrefixes.</summary>
        public SIPrefixes Id
        { get { return Id; } }

        private int _log10Factor;

        /// <summary>Base 10 logarithm of the factor represented by the current SI units prefix.</summary>
        public double Log10Factor
        { get { return _log10Factor; } }

        private double _factor;

        /// <summary>Factor represented by the current SI units prefix.</summary>
        public double Factor
        { get { return _factor; } }

        private string _symbol;

        /// <summary>Symbol of the current prefix for the SI units of measure such as "m", "μ", "k", "M", "T".</summary>
        public string Symbol
        { get { return _symbol; } }

        private string _prefixString;

        /// <summary>Name of the prefix, such as "milli-", "micro-", or "mega".</summary>
        public string PrefixString
        { get { return _prefixString; } }

        private string _description;

        /// <summary>Short description of the current prefix for the SI units of measure.</summary>
        public string Description
        { get { return _description; } }

        /// <summary>Soncersion to string - returns the value of the Symbol property.</summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Symbol;
        }

        public string ToStringLong()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SI unit prefix " + Id +
              ", Symbol = " + Symbol + ", prefix = " + PrefixString + ", log10 = " + Log10Factor + ".");
            return sb.ToString();
        }

        #endregion Operation;


        #region Constans_ExistentPrefixes

        // NO PREFIX::

        private static SIPrefix _none =
            new SIPrefix(SIPrefixes.None, 0, 1.0, "", "",
            "Standard prefix for SI units of measure, None (no prefix), 1.0.");
        
        /// <summary>Standard prefix for SI units of measure, None (no prefix), 1.0.</summary>
        public static SIPrefix None
        { get { return _none; } }

        // MULTIPLES (lerger than 1)

        private static SIPrefix _da =
            new SIPrefix(SIPrefixes.da, 1, 1.0e1, "da", "deca-", 
                "Standard prefix for SI units of measure, da (deca-), 10^1.");

        /// <summary>Standard prefix for SI units of measure, da (deca-), 10^1.</summary>
        public static SIPrefix da
        { get { return _da; } }

        private static SIPrefix _h =
            new SIPrefix(SIPrefixes.h, 2, 1.0e2, "h", "hecto-", 
                "Standard prefix for SI units of measure, h (hecto-), 10^2.");

        /// <summary>Standard prefix for SI units of measure, h (hecto-), 10^2.</summary>
        public static SIPrefix h
        { get { return _h; } }

        private static SIPrefix _k =
            new SIPrefix(SIPrefixes.k, 3, 1.0e3, "k", "kilo-", 
                "Standard prefix for SI units of measure, k (kilo-), 10^3.");

        /// <summary>Standard prefix for SI units of measure, k (kilo-), 10^3.</summary>
        public static SIPrefix k
        { get { return _k; } }

        private static SIPrefix _M =
            new SIPrefix(SIPrefixes.M, 6, 1.0e6, "M", "mega-", 
                "Standard prefix for SI units of measure, M (mega-), 10^6.");

        /// <summary><Standard prefix for SI units of measure, M (mega-), 10^6./summary>
        public static SIPrefix M
        { get { return _M; } }

        private static SIPrefix _G =
            new SIPrefix(SIPrefixes.G, 9, 1.0e9, "G", "giga-",
            "Standard prefix for SI units of measure, G (giga-), 10^9.");

        /// <summary>Standard prefix for SI units of measure, G (giga-), 10^9.</summary>
        public static SIPrefix G
        { get { return _G; } }

        private static SIPrefix _T =
            new SIPrefix(SIPrefixes.T, 12, 1.0e12, "T", "tera-",
            "Standard prefix for SI units of measure, T (tera-), 10^12.");

        /// <summary>Standard prefix for SI units of measure, T (tera-), 10^12.</summary>
        public static SIPrefix T
        { get { return _T; } }

        private static SIPrefix _P =
            new SIPrefix(SIPrefixes.P, 15, 1.0e15, "P", "peta-",
            "Standard prefix for SI units of measure, P (peta-), 10^15.");

        /// <summary>Standard prefix for SI units of measure, P (peta-), 10^15.</summary>
        public static SIPrefix P
        { get { return _P; } }

        private static SIPrefix _E =
            new SIPrefix(SIPrefixes.E, 18, 1.0e18, "E", "exa-",
            "Standard prefix for SI units of measure, E (exa-), 10^18.");

        /// <summary>Standard prefix for SI units of measure, E (exa-), 10^18.</summary>
        public static SIPrefix E
        { get { return _E; } }

        private static SIPrefix _Z =
            new SIPrefix(SIPrefixes.Z, 21, 1.0e21, "Z", "zetta-",
            "Standard prefix for SI units of measure, Z (zetta-), 10^21.");

        /// <summary>Standard prefix for SI units of measure, Z (zetta-), 10^21.</summary>
        public static SIPrefix Z
        { get { return _Z; } }

        private static SIPrefix _Y =
            new SIPrefix(SIPrefixes.Y, 24, 1.0e24, "Y", "yotta-",
            "Standard prefix for SI units of measure, Y (yotta-), 10^24.");

        /// <summary>Standard prefix for SI units of measure, Y (yotta-), 10^24.</summary>
        public static SIPrefix Y
        { get { return _Y; } }

        // FRACTIONS (smaller than 1)

        private static SIPrefix _d =
            new SIPrefix(SIPrefixes.d, -1, 1.0e-1, "d", "deci-",
            "Standard prefix for SI units of measure, d (deci-), 10^-1.");

        /// <summary>Standard prefix for SI units of measure, d (deci-), 10^-1.</summary>
        public static SIPrefix d
        { get { return _d; } }

        private static SIPrefix _c =
            new SIPrefix(SIPrefixes.c, -2, 1.0e-2, "c", "centi-",
            "Standard prefix for SI units of measure, c (centi-), 10^-2.");

        /// <summary>Standard prefix for SI units of measure, c (centi-), 10^-2.</summary>
        public static SIPrefix c
        { get { return _c; } }

        private static SIPrefix _m =
            new SIPrefix(SIPrefixes.m, -3, 1.0e-3, "m", "milli-",
            "Standard prefix for SI units of measure, m (milli-), 10^-3.");

        /// <summary>Standard prefix for SI units of measure, m (milli-), 10^-3.</summary>
        public static SIPrefix m
        { get { return _m; } }

        private static SIPrefix _micro =
            new SIPrefix(SIPrefixes.micro, -6, 1.0e-6, "μ", "micro-",
            "Standard prefix for SI units of measure, μ (micro-), 10^-6.");

        /// <summary>Standard prefix for SI units of measure, μ (micro-), 10^-6.</summary>
        public static SIPrefix micro
        { get { return _micro; } }

        private static SIPrefix _n =
            new SIPrefix(SIPrefixes.n, -9, 1.0e-9, "n", "nano-",
            "Standard prefix for SI units of measure, n (nano-), 10^-9.");

        /// <summary>Standard prefix for SI units of measure, n (nano-), 10^-9.</summary>
        public static SIPrefix n
        { get { return _n; } }

        private static SIPrefix _p =
            new SIPrefix(SIPrefixes.p, -12, 1.0e-12, "p", "pico-",
            "Standard prefix for SI units of measure, p (pico-), 10^-12.");

        /// <summary>Standard prefix for SI units of measure, p (pico-), 10^-12.</summary>
        public static SIPrefix p
        { get { return _p; } }

        private static SIPrefix _f =
            new SIPrefix(SIPrefixes.f, -15, 1.0e-15, "f", "femto-",
            "Standard prefix for SI units of measure, f (femto-), 10^-15.");

        /// <summary>Standard prefix for SI units of measure, f (femto-), 10^-15.</summary>
        public static SIPrefix f
        { get { return _f; } }

        private static SIPrefix _a =
            new SIPrefix(SIPrefixes.a, -18, 1.0e-18, "a", "ato-",
            "Standard prefix for SI units of measure, a (ato-), 10^-18.");

        /// <summary>Standard prefix for SI units of measure, a (ato-), 10^-18.</summary>
        public static SIPrefix a
        { get { return _a; } }

        private static SIPrefix _z =
            new SIPrefix(SIPrefixes.z, -21, 1.0e-21, "z", "zepto-",
            "Standard prefix for SI units of measure, z (zepto-), 10^-21.");

        /// <summary>Standard prefix for SI units of measure, z (zepto-), 10^-21.</summary>
        public static SIPrefix z
        { get { return _z; } }

        private static SIPrefix _y =
            new SIPrefix(SIPrefixes.y, -24, 1.0e-24, "y", "yocto-",
            "Standard prefix for SI units of measure, y (yocto-), 10^-24.");

        /// <summary>Standard prefix for SI units of measure, y (yocto-), 10^-24.</summary>
        public static SIPrefix y
        { get { return _y; } }


        #endregion Constans_ExistentPrefixes

        #region PrefixCollection

        /// <summary>Collection of al prefixes for the SI units.</summary>
        public class PrefixCollection
        {

            /// <summary>Returns array of all SI unit prefixes contained in the current prefix collection.</summary>
            public SIPrefix[] ToArray()
            {
                return IdList.Values.ToArray();
            }

            /// <summary>Returns the perfix for SI units that has the specified symbol,
            /// or null if such prefix can not be found.</summary>
            /// <param name="symbol">Symbol fo the prefix that is searched for.</param>
            public SIPrefix this[string symbol]
            {
                get {
                    if (SymbolList.ContainsKey(symbol))
                        return SymbolList[symbol];
                    else
                        return null;
                }
            }

            /// <summary>Returns the perfix for SI units that has the specified integer equivalent to Id
            /// (which corresponds to the base 10 logarithm of the factor represented by the specified 
            /// SI prefix), or null if such prefix can not be found.</summary>
            /// <param name="id">Integer equivalent of the prefix ID that is looked for.</param>
            public SIPrefix this[int id]
            {
                get 
                {
                    SIPrefixes prefId = SIPrefixes.None;
                    bool hasBeenSet = false;
                    try { prefId = (SIPrefixes) id; hasBeenSet = true; }
                    catch { }
                    if (hasBeenSet)
                    {
                        if (IdList.ContainsKey(prefId))
                            return IdList[prefId];
                    }
                    return null;
                }
            }

            /// <summary>Returns the perfix for SI units that has the specified Id, or null if 
            /// such prefix can not be found.</summary>
             /// <param name="id">Prefix ID that is looked for.</param>
           public SIPrefix this[SIPrefixes id]
            {
                get
                {
                    if (IdList.ContainsKey(id))
                        return IdList[id];
                    return null;
                }
            }

            private SortedList<string, SIPrefix> _symbolList = new SortedList<string, SIPrefix>();
            
            /// <summary>Sorted list of existing SI prefixes indexed by prefix symbol.</summary>
            protected SortedList<string, SIPrefix> SymbolList
            { get { return _symbolList; } }

            private SortedList<SIPrefixes, SIPrefix> _IdList = new SortedList<SIPrefixes, SIPrefix>();
           
            /// <summary>Sorted list of existing SI prefixes indexed by prefix Id (enumerator).</summary>
            protected SortedList<SIPrefixes, SIPrefix> IdList
            { get { return _IdList; } }


            public void Add(SIPrefix prefix)
            {
                if (SymbolList.ContainsKey(prefix.Symbol))
                {
                    SIPrefix p = SymbolList[prefix.Symbol];
                    if (p != prefix)
                        throw new ArgumentException("Adding SI prefix " + prefix.Symbol + "to the list of prefixes: "
                            + "different prefix with the sams symbol already exists on the list.");
                } else
                {
                    SymbolList.Add(prefix.Symbol,prefix);
                    IdList.Add(prefix.Id, prefix);
                }
            }

        }

        private static PrefixCollection _prefixList = new PrefixCollection();

        /// <summary>Returns a collection that contains all currently built-in SI unit prefixes.</summary>
        public static PrefixCollection List
        { get { return _prefixList; } }

        /// <summary>Prints a list of all built in SI prefixes.</summary>
        public static void PrintList()
        {
            SIPrefix[] prefixes = SIPrefix.List.ToArray();
            Console.WriteLine();
            Console.WriteLine("A list of built in SI unit prefixes: ");
            for (int i = 0; i < prefixes.Length; ++i)
            {
                Console.WriteLine(prefixes[i].ToStringLong());
            }
            Console.WriteLine();
        }

        #endregion PrefixCollection

        #region Examples

        #endregion Examples

    }

}


