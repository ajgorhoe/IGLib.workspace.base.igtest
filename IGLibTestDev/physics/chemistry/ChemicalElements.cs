

// SI UNITS (enumerators)


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IG.Physics;

namespace IG.Physics
{


    /// <summary>Enum defining the particular chemical element.</summary>
    /// $A Igor Oct08;
    public enum ChemicalElements
    {
        None = 0, // this is not an element
        H = 1,    // hydrogen; at. weight = 1.00794 
        He = 2,   // helium; at. weight = 4.002602 
        Li = 3,   // lithium; at. weight = 6.941 
        Be = 4,   // beryllium; at. weight = 9.012182 
        B = 5,    // boron; at. weight = 10.811 
        C = 6,    // carbon; at. weight = 12.0107 
        N = 7,    // nitrogen; at. weight = 14.0067 
        O = 8,    // oxygen; at. weight = 15.9994 
        F = 9,    // fluorine; at. weight = 18.9984032 
        Ne = 10,  // neon; at. weight = 20.1797 
        Na = 11,  // sodium; at. weight = 22.989770 
        Mg = 12,  // magnesium; at. weight = 24.3050 
        Al = 13,  // aluminum; at. weight = 26.981538 
        Si = 14,  // silicon; at. weight = 28.0855 
        P = 15,   // phosphorus; at. weight = 30.973761 
        S = 16,   // sulfur; at. weight = 32.065 
        Cl = 17,  // chlorine; at. weight = 35.453 
        Ar = 18,  // argon; at. weight = 39.948 
        K = 19,   // potassium; at. weight = 39.0983 
        Ca = 20,  // calcium; at. weight = 40.078 
        Sc = 21,  // scandium; at. weight = 44.955910 
        Ti = 22,  // titanium; at. weight = 47.867 
        V = 23,   // vanadium; at. weight = 50.9415 
        Cr = 24,  // chromium; at. weight = 51.9961 
        Mn = 25,  // manganese; at. weight = 54.938049 
        Fe = 26,  // iron; at. weight = 55.845 
        Co = 27,  // cobalt; at. weight = 58.9332 
        Ni = 28,  // nickel; at. weight = 58.6934 
        Cu = 29,  // copper; at. weight = 63.546 
        Zn = 30,  // zinc; at. weight = 65.409 
        Ga = 31,  // gallium; at. weight = 69.723 
        Ge = 32,  // germanium; at. weight = 72.64 
        As = 33,  // arsenic; at. weight = 74.92160 
        Se = 34,  // selenium; at. weight = 78.96 
        Br = 35,  // bromine; at. weight = 79.904 
        Kr = 36,  // krypton; at. weight = 83.798 
        Rb = 37,  // rubidium; at. weight = 85.4678 
        Sr = 38,  // strontium; at. weight = 87.62 
        Y = 39,   // yttrium; at. weight = 88.90585 
        Zr = 40,  // zirconium; at. weight = 91.224 
        Nb = 41,  // niobium; at. weight = 92.90638 
        Mo = 42,  // molybdenum; at. weight = 95.94 
        Tc = 43,  // technetium; at. weight = 98 
        Ru = 44,  // ruthenium; at. weight = 101.07 
        Rh = 45,  // rhodium; at. weight = 102.90550 
        Pd = 46,  // palladium; at. weight = 106.42 
        Ag = 47,  // silver; at. weight = 107.8682 
        Cd = 48,  // cadmium; at. weight = 112.411 
        In = 49,  // indium; at. weight = 114.818 
        Sn = 50,  // tin; at. weight = 118.710 
        Sb = 51,  // antimony; at. weight = 121.760 
        Te = 52,  // tellurium; at. weight = 127.60 
        I = 53,   // iodine; at. weight = 126.90447 
        Xe = 54,  // xenon; at. weight = 131.293 
        Cs = 55,  // cesium; at. weight = 132.90545 
        Ba = 56,  // barium; at. weight = 137.327 
        La = 57,  // lanthanum; at. weight = 138.9055 
        Ce = 58,  // cerium; at. weight = 140.116 
        Pr = 59,  // praseodymium; at. weight = 140.90765 
        Nd = 60,  // neodymium; at. weight = 144.24 
        Pm = 61,  // promethium; at. weight = 145 
        Sm = 62,  // samarium; at. weight = 150.36 
        Eu = 63,  // europium; at. weight = 151.964 
        Gd = 64,  // gadolinium; at. weight = 157.25 
        Tb = 65,  // terbium; at. weight = 158.92534 
        Dy = 66,  // dysprosium; at. weight = 162.5 
        Ho = 67,  // holmium; at. weight = 164.93032 
        Er = 68,  // erbium; at. weight = 167.259 
        Tm = 69,  // thulium; at. weight = 168.93421 
        Yb = 70,  // ytterbium; at. weight = 173.04 
        Lu = 71,  // lutetium; at. weight = 174.967 
        Hf = 72,  // hafnium; at. weight = 178.49 
        Ta = 73,  // tantalum; at. weight = 180.9479 
        W = 74,   // tungsten; at. weight = 183.84 
        Re = 75,  // rhenium; at. weight = 186.207 
        Os = 76,  // osmium; at. weight = 190.23 
        Ir = 77,  // iridium; at. weight = 192.217 
        Pt = 78,  // platinum; at. weight = 195.078 
        Au = 79,  // gold; at. weight = 196.96655 
        Hg = 80,  // mercury; at. weight = 200.59 
        Tl = 81,  // thallium; at. weight = 204.3833 
        Pb = 82,  // lead; at. weight = 207.2 
        Bi = 83,  // bismuth; at. weight = 208.98038 
        Po = 84,  // polonium; at. weight = 209 
        At = 85,  // astatine; at. weight = 210 
        Rn = 86,  // radon; at. weight = 222 
        Fr = 87,  // francium; at. weight = 223 
        Ra = 88,  // radium; at. weight = 226 
        Ac = 89,  // actinium; at. weight = 227 
        Th = 90,  // thorium; at. weight = 232.0381 
        Pa = 91,  // protactinium; at. weight = 231.03588 
        U = 92,   // uranium; at. weight = 238.02891 
        Np = 93,  // neptunium; at. weight = 237 
        Pu = 94,  // plutonium; at. weight = 244 
        Am = 95,  // americium; at. weight = 243 
        Cm = 96,  // curium; at. weight = 247 
        Bk = 97,  // berkelium; at. weight = 247 
        Cf = 98,  // californium; at. weight = 251 
        Es = 99,  // einsteinium; at. weight = 252 
        Fm = 100,  // fermium; at. weight = 257 
        Md = 101,  // mendelevium; at. weight = 258 
        No = 102,  // nobelium; at. weight = 259 
        Lr = 103,  // lawrencium; at. weight = 262 
        Rf = 104,  // rutherfordium; at. weight = 261 
        Db = 105,  // dubnium; at. weight = 262 
        Sg = 106,  // seaborgium; at. weight = 266 
        Bh = 107,  // bohrium; at. weight = 264 
        Hs = 108,  // hassium; at. weight = 277 
        Mt = 109,  // meitnerium; at. weight = 268 
        Ds = 110,  // darmstadtium; at. weight = 281 
        Rg = 111,  // roentgenium; at. weight = 272 
        Uub = 112,  // ununbium; at. weight = 285 
        Uut = 113,  // ununtrium; at. weight = 284 
        Uuq = 114,  // ununquadium; at. weight = 289 
        Uup = 115,  // ununpentium; at. weight = 288 
        Uuh = 116,  // ununhexium; at. weight = 292 
        Uus = 117,  // ununseptium; at. weight = Missing[Unknown] 
        Uuo = 118,  // ununoctium; at. weight = 294 
        Unknown // Undefined chemical element
    }  // enum ChemicalElements


}

