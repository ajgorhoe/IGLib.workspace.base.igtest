// Copyright (c) Igor Grešovnik (2009), IGLib license; http://www2.arnes.si/~ljc3m2/igor/ioptlib/

/****************************/
/*                          */
/*  3D ANALYTICAL GEOMETRY  */
/*                          */
/****************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IG.Lib;

using F = IG.Num.M;

namespace IG.Num
{

    ///// <summary>Auxiliary vector 3D structure, for porting from C code.</summary>
    //public struct vec3
    //{

    //    public double x, y, z;

    //    public vec3(vec3 v)
    //    { this.x = v.x; this.y = v.y; this.z = v.z; }

    //    vec3 Base
    //    { 
    //        get{ return new vec3(x,y,z);  } 
    //        set {
    //            x = value.x;
    //            y = value.y;
    //            z = value.z;
    //        }
    //    }
        
    //}

    ///// <summary>Auxiliary matrix 3d structure, for porting from C code.</summary>
    //public struct mat3
    //{
    //    public double xx, xy, xz, yx, yy, yz, zx, zy, zz;


    //    public mat3(mat3 mat3)

    //}

    ///// <summary>Copy of vec3.</summary>
    //public struct vec3
    //{
    //    public double x, y, z;
    //}

    ///// <summary>Copy of mat3.</summary>
    //public struct mat3
    //{
    //    public double xx, xy, xz, yx, yy, yz, zx, zy, zz;
    //}




    /// <summary>Analytical Geometry in 3D.</summary>
    /// $A Igor Jul08;
    /// <remarks>This class was developed by transcribing the C library IOptLib's geometry
    /// related functions and rearranging and packing them into a C# class.</remarks>
    public static class Geometry
    {

        #region Operations

        /// <summary>Square of double argument.</summary>
        private static double m_sqr(double x)
        { return x*x; }

        /// <summary>Square root of double argument.</summary>
        private static double sqrt(double x)
        { return Math.Sqrt(x); }

        /// <summary>Vector norm.</summary>
        private static double vecnorm3d(vec3 v)
        { return v.Norm; }

        private static double fabs(double x)
        { return Math.Abs(x); }

        /// <summary>Sets all components of the vector to 0.</summary>
        private static void zerovec3d(ref vec3 v)
        { v.Zero(); }

        /// <summary>Scalar product of two 3D vectors.</summary>
        private static double scalprod3d(vec3 v1, vec3 v2)
        { return v1.ScalarProduct(v1); }

        /// <summary>Vector product of two 3D vectors.</summary>
        private static void vecprod3d(vec3 v1, vec3 v2, ref vec3 res)
        { res = v1.VectorProduct(v2); }

        /// <summary>Difference of two 3D vectors.</summary>
        private static void vecdif3d(vec3 v1, vec3 v2, ref vec3 v3)
        { v3 = v1.Subtract(v1); }

        /// <summary>Sum of two 3D vectors.</summary>
        private static void vecsum3d(vec3 v1, vec3 v2, ref vec3 v3)
        { v3 = v1.Add(v1); }

        /// <summary>Product of vector with a scalar.</summary>
        private static void multscalvec3d(vec3 v1, double s, ref vec3 prod)
        { prod = v1.Multiply(s); }

        

        /// <summary>Sets all components of the matrix to 0.</summary>
        private static void zeromat3d(ref mat3 a)
        { a.Zero(); }

            /// <summary>V x zapise resitev sistema dveh enacb z dvema neznankama, kjer so
        // koeficienti enacbe v zgornjem levem kotu matrike a, desne strani pa v
        // zgornjem delu vektorja b. Funkcija vrne determinanto matrike sistema.</summary>
        private static void solve2d(mat3 A, vec3 b, out vec3 x)
        {
            // TODO: implement this!
            throw new NotImplementedException("Solution of a system of 2D equations not implemented.");
        }


        #endregion Operations


        public static double distpt3d(vec3 p1, vec3 p2)
        /* Vrne razdaljo med tockama p1 in p2. Ce je kateri od kazalcev p1 oz. p2
        NULL, vrne 0.
        $A Igor okt01; */
        {
                return sqrt(m_sqr(p2.x - p1.x) + m_sqr(p2.y - p1.y) + m_sqr(p2.z - p1.z));
        }


        public static double directioncos3d(vec3 s1, vec3 s2)
        /* Vrne kosinus kota med smerema s1 in s2 (smerni kosinus).
        $A Igor okt01; */
        {
            
            return scalprod3d(s1, s2) / (scalprod3d(s1, s1) * scalprod3d(s2, s2));
        }


        public static double ortprojptline3d(vec3 pt, vec3 r, vec3 s, vec3 proj)
        /* Izracuna ortogonalno projekcijo tocke pt na premico podano s tocko na
        premici r in smernim vektorjem s. Rezultat zapise v proj. Funkcija vrne
        se pozicijo tocke proj na premici izrazeno s koeficientom (c) pri smernem
        vektorju, tako da je proj=r+c*s.
        Formula: proj = r+((pt-r)*s/(s*s)) s
        $A Igor okt01; */
        {
            double k = 0;
                vecdif3d(pt, r, ref proj);  /* proj=pt-r */
                k = scalprod3d(proj, s) / scalprod3d(s, s); /* k=((pt-r)*s/(s*s)) */
                multscalvec3d(s, k, ref proj);  /* proj=((pt-r)*s/(s*s)) s */
                vecsum3d(r, proj, ref proj);
            return k;
        }




            public static double distptline3d(vec3 pt, vec3 r, vec3 s)
        /* Vrne oddaljenost tocke pt od premice podane s tocko na premici r in s
        smernim vektorjem s.
        Formula: d= || (pt-r) x s || / ||s||
        $A Igor okt01; */
        {
            vec3 ref1 = new vec3();
              vecdif3d(pt,r, ref ref1);  /* ref=pt-r */
              vecprod3d(ref1, s, ref ref1);  /* ref=(pt-r) x s */
              return vecnorm3d(ref1)/vecnorm3d(s);
            return 0;
        }


        public static double nearestptlines3d(vec3 r1, vec3 s1, vec3 r2, vec3 s2, ref vec3 pt1, ref vec3 pt2)
        /*  V pr1 zapise tocko na 1. premici, ki je najblizje 2. premici, v pt2
        pa tocko na 2, premici, ki je najblizje 1. premici, ter vrne razdaljo med
        obema tockama (t.j. razdaljo med premicama). 1. premica je podana s tocko
        na premici r1 in smefnim vektorjem s1, 2. premica pa s tocko na premici r2
        in smer. vektorjem s2.
         Resitev:
        Resujemo p1=r1+k1*s1, p2=r2+k2*s2, k1,k2:min || p1-p2 ||^2;
        k1 in k2 dobimo dako, da odvod ||...||^2  izenacimo z 0, dobimo
        k1=-(r1*s2)/(s1*s2) in k2=-(r2*s1)/(s1*s2), izracunamo p1 in p2.
         POZOR!
         Problemi so lahko pri vzporednih premicah, ker numericni algoritem premic
        ne prepozna za vzporedne zaradi numericnih napak in vrne neko veliko
        razdaljo med dvema premicama, saj sta ort. projekciji cisto narobe
        izracunani.
        $A Igor okt01; */
        {
            double ret = 0, k1, k2;
            mat3 A = new mat3();
            vec3 b = new vec3(), x = new vec3();
            if (true) //r1 != NULL && s1 != NULL && r2 != NULL && s2 != NULL && pt1 != NULL && pt2 != NULL)
            {
                if (fabs(directioncos3d(s1, s2)) - 1 == 0)
                {
                    /* Premici sta vzporedni, pt1 postane r1, pt2 pa prav. proj. r1 na 2.
                    premico, vrne se razdalja med premicama */
                    pt1 = r1;
                    ortprojptline3d(r1, r2, s2, pt2);
                    ret = distpt3d(pt1, pt2);
                }
                else
                {
                    /* Koeficienti enacbe za izracun k1 in kv, ki dolocata najbljizji tocki: */
                    zeromat3d(ref A); zerovec3d(ref b);
                    A.xx = -scalprod3d(s1, s1);
                    A.xy = scalprod3d(s1, s2);
                    A.yx = scalprod3d(s1, s2);
                    A.yy = -scalprod3d(s2, s2);
                    b.x = scalprod3d(r1, s1) - scalprod3d(s1, r2);
                    b.y = scalprod3d(r2, s2) - scalprod3d(r1, s2); ;
                    solve2d(A, b, out x);
                    k1 = x.x; k2 = x.y;
                    multscalvec3d(s1, k1, ref pt1); vecsum3d(r1, pt1, ref pt1);
                    multscalvec3d(s2, k2, ref pt2); vecsum3d(r2, pt2, ref pt2);
                    ret = distpt3d(pt1, pt2);
                    if (ret > distptline3d(r1, r2, s2))
                    {
                        /* Ce je izracunana razdalja med premicama manjsa od razdalje med r1 in 2.
                        premico, je nekaj narobe (najverjetneje vzporednost ali numericne napake),
                        v tem primeru pt1 postane r1, pt2 pravokot. projekcija r1 na 2. premico,
                        vrne pa se razdalja med r1 in pravokot. proj. r1 na r2: */
                        pt1 = r1;
                        ortprojptline3d(r1, r2, s2, pt2);
                        ret = distpt3d(pt1, pt2);
                    }
                }
            }
            return ret;
        }















        public static double nearestptlinescoef3d(vec3 r1, vec3 s1, vec3 r2, vec3 s2,
               ref vec3 pt1, ref double c1, ref vec3 pt2, ref double c2)
        /*  V pr1 zapise tocko na 1. premici, ki je najblizje 2. premici, v pr2
        pa tocko na 2, premici, ki je najblizje 1. premici, ter vrne razdaljo med
        obema tockama (t.j. razdaljo med premicama). 1. premica je podana s tocko
        na premici r1 in smefnim vektorjem s1, 2. premica pa s tocko na premici r2
        in smer. vektorjem s2. Funkcija vrne v *c1 in *c2 tudi koeficienta, ki na
        obeh premicah dolocata najbljizji tocki, tako da je pt1=r1+c1*s1 in
        pt2=r2+c2*s2.
         Resitev:
        Resujemo p1=r1+k1*s1, p2=r2+k2*s2, k1,k2:min || p1-p2 ||^2;
        k1 in k2 dobimo dako, da odvod ||...||^2  izenacimo z 0, dobimo
        k1=-(r1*s2)/(s1*s2) in k2=-(r2*s1)/(s1*s2), izracunamo p1 in p2.
         POZOR!
         Problemi so lahko pri vzporednih premicah, ker numericni algoritem premic
        ne prepozna za vzporedne zaradi numericnih napak in vrne neko veliko
        razdaljo med dvema premicama, saj sta ort. projekciji cisto narobe
        izracunani.
        $A Igor okt01; */
        {
            double ret = 0, k1, k2;
            mat3 A = new mat3();
            vec3 b = new vec3(), x;
            if (true) // r1 != NULL && s1 != NULL && r2 != NULL && s2 != NULL && pt1 != NULL && pt2 != NULL)
            {
                if (fabs(directioncos3d(s1, s2)) - 1 == 0)
                {
                    /* Premici sta vzporedni, pt1 postane r1, pt2 pa prav. proj. r1 na 2.
                    premico, vrne se razdalja med premicama */
                    pt1 = r1; c1 = 0;
                    c2 = ortprojptline3d(r1, r2, s2, pt2);
                    ret = distpt3d(pt1, pt2);
                }
                else
                {
                    /* Koeficienti enacbe za izracun k1 in kv, ki dolocata najbljizji tocki: */
                    zeromat3d(ref A); zerovec3d(ref b);
                    A.xx = -scalprod3d(s1, s1);
                    A.xy = scalprod3d(s1, s2);
                    A.yx = scalprod3d(s1, s2);
                    A.yy = -scalprod3d(s2, s2);
                    b.x = scalprod3d(r1, s1) - scalprod3d(s1, r2);
                    b.y = scalprod3d(r2, s2) - scalprod3d(r1, s2); ;
                    solve2d(A, b, out x);
                    k1 = x.x; k2 = x.y;
                    multscalvec3d(s1, k1, ref pt1); vecsum3d(r1, pt1, ref pt1);
                    multscalvec3d(s2, k2, ref pt2); vecsum3d(r2, pt2, ref pt2);
                    c1 = k1; c2 = k2;
                    ret = distpt3d(pt1, pt2);
                    if (ret > distptline3d(r1, r2, s2))
                    {
                        /* Ce je izracunana razdalja med premicama manjsa od razdalje med r1 in 2.
                        premico, je nekaj narobe (najverjetneje vzporednost ali numericne napake),
                        v tem primeru pt1 postane r1, pt2 pravokot. projekcija r1 na 2. premico,
                        vrne pa se razdalja med r1 in pravokot. proj. r1 na r2: */
                        pt1 = r1; c1 = 0;
                        c2 = ortprojptline3d(r1, r2, s2, pt2);
                        ret = distpt3d(pt1, pt2);
                    }
                }
            }
            return ret;
        }


        public static void ortprojptplane3d(vec3 pt, vec3 r, vec3 n, ref vec3 proj)
        /* Izracuna ortogonal. projekcijo tocke pt na ravnino, ki je podana s tocko
        na ravnini r in normalo ravnine n. Rezultat zapise v proj.
        Formula: proj = pt+((r-pt)*n) n / n*n
        $A Igor okt01; */
        {
            double k;
            if (true) // pt != NULL && r != NULL && n != NULL)
            {
                vecdif3d(r, pt, ref proj);  /* proj = r-pt */
                k = scalprod3d(proj, n) / scalprod3d(n, n);  /* k = ((r-pt)*n) / n*n */
                multscalvec3d(n, k, ref proj);   /* proj =  ((r-pt)*n) n / n*n */
                vecsum3d(pt, proj, ref proj);
            }
        }


        public static void ortprojlineplane3d(vec3 r1, vec3 s1, vec3 r2, vec3 n2, ref vec3 rpr, ref vec3 spr)
        /* Izracuna premico, ki je pravokotna projekcija premice podane s tocko na
        premici r1 in smernim vektorjem s1, na ravnino, ki je podana s tocko na
        ravnini r2 in normalo n2. V vektor rpr zapise tocko na projekciji, v vektor
        spr pa smerni vektor projekcije. Projekcijo se dobi enostavno tako, da se
        na ravnino posebej projicirata tocka na premici in njen smerni vektor.
        Funkcija ne obravnava posebej primera, ko je premica pravokotna na ravnino
        - to se lahko preveri tako, da se pogleda, ce so vse tri komponente vektorja
        spr enake 0.
        $A Igor okt01; */
        {
            ortprojptplane3d(r1, r2, n2, ref rpr);
            ortprojptplane3d(s1, r2, n2, ref rpr);
        }


        public static double signdistptplane3d(vec3 pt, vec3 r, vec3 n)
        /* Vrne razdaljo tocke pt od ravnine, ki je podana s tocko na ravnini r in
        normalo ravnine n. Razdalja je negativna, ce je pt na nasprotni strani
        ravnine, kot je tista, v katero kaze normala n, in pozitivno, ce je tocka na
        isti strani.
        Formula: d= (pt-r)*n / ||n||
        $A Igor okt01; */
        {
        vec3 ref1 = new vec3();
        vecdif3d(pt,r, ref ref1);  /* ref=pt-r */
        return (scalprod3d(ref1, n) / vecnorm3d(n));
        }


        public static double distptplane3d(vec3 pt, vec3 r, vec3 n)
        /* Vrne absolutno razdaljo  tocke pt od ravnine, ki je podana s tocko na
        ravnini r in normalo ravnine n, ki je vedno pozitivno stevilo.
        $A Igor okt01; */
        {
            return fabs(signdistptplane3d(pt, r, n));
        }


        public static double intsectlineplane3d(vec3 r1, vec3 s1, vec3 r2, vec3 n2, ref vec3 sect)
        /* V sect zapise presecisce med premico, podano s tocko na premici r1 in
        smernim vektorjem s1, in ravnino podano s tocko na ravnini r2 in normalo
        n2. Ce presecisce obstaja, vrne funkcija 0, drugace vrne predznaceno
        razdaljo med premico in ravnino (to je v primeru, ko sta ravnina in premica
        vzporedni).
        Formula: sect=r1+((r2*n2-r1*n2)/(s1*n2)) s1
        $A Igor okt01; */
        {
            double ret = 0, k;
            if (true)  // r1 != NULL && s1 != NULL && r2 != NULL && n2 != NULL)
            {
                if ((k = scalprod3d(s1, n2)) != 0)
                {
                    multscalvec3d(s1,
                     (scalprod3d(r2, n2) - scalprod3d(r1, n2)) / k /* s1*n2 */ ,
                     ref sect);  /* sect = ((r2*n2-r1*n2)/(s1*n2)) s1 */
                    vecsum3d(r1, sect, ref sect);  /* sect := sect+r1 */
                }
                else
                {
                    ret = signdistptplane3d(r1, r2, n2);
                    sect = r1;
                }
            }
            return ret;
        }


        public static double intsectplanes3d(vec3 r1, vec3 n1, vec3 r2, vec3 n2, ref vec3 r, ref vec3 s)
        /* Najde premico, ki je PRESECISCE RAVNIN, od katerih je 1. dolocena z
        vektorjem r1 in normalo n1, 2. pa z vektorjem r2 in normalo n2. V r zapise
        tocko na tej premici, v s pa njen smerni vektor. Ce sta ravnini vzporedni,
        vrne funkcija razdaljo med ravninama (v r pa se zapise r1), drugace pa vrne
        0.
         POSTOPEK:
        Smerni vektor premice je n1xn2 (vekt. produkt normal). Rabimo se tocko na
        premici, to dobimo tako, da vzamemo presecisce premice, ki gre skozi r1
        ter je pravokotna na n1*n2 in na n1 (torej lezi v 1. ravnini) z 2. ravnino.
        $A Igor okt01; */
        {
            double ret = 0;
            if (true) // r1 != NULL && n1 != NULL && r2 != NULL && n2 != NULL && r != NULL && s != NULL)
            {
                vecprod3d(n1, n2, ref s); /* Smer. vektor premice, ki predstavlja presecisce */
                if (fabs(directioncos3d(n1, n2)) - 1 == 0)
                {
                    /* Ravnini sta vzporedni, zato r postane r1, vrne se razdaljo med r1 in 2.
                    ravnino: */
                    r = r1;
                    ret = distptplane3d(r1, r2, n2);
                }
                else
                {
                    vecprod3d(n1, s, ref r);  /* r postane smer. vektor, pravokoten na n1 in n1xn2 */
                    if (intsectlineplane3d(r1, r, r2, n2, ref r)!=0.0)
                    {
                        /* Presecisca med premico na 1. ravnini in med 2. ravnino ni (kljub temu,
                        da se je abs. vred. smer. kosinusa med normalama prej izkazala za
                        razlicne od 1, a je ocitno slo za numer. napake), zato r postane r1, vrne
                        se razdaljo med r1 in 2. ravnino: */
                        r = r1;
                        ret = distptplane3d(r1, r2, n2);
                    }
                }
            }
            return ret;
        }



        /* TRANSFORMACIJE KOORDINAT. SIST. IN KOORDINAT (VRTENJA, TRANSLAC.) */


        /* TRANSFORMACIJE KOORDINAT V DESNOSUC. ENOTSKIH KARTEZ. SISTEMIH */



        /*  DOGOVORI IN OPOMBE:
          TRANSF. KOORD. SIST.:
          Vektor r naj ima v desnosucnem ortonormir. sist. (enotski in ortogonalni enot.
        vektorji, e1xe2=e3) koordinate r=(x, y, z). V koordinatnem sistemu z baznimi
        vektorji e1', e2' in e3' ima isti vektor koordinate r'=(x', y' in z'). Direktna
        transformacijska matrika potem recemo matriki Q, ki pretvori koordinate r' v r,
        torej r=Qr'. Inverzna transformacijska matrika pretvori koordinate v starem
        koordinatnem sistemu v koordinate v novem, dorej r'=Q-1 r.
          Transformacijaka matrika Q tudi pretvori bazne vektorje starega koordinatnega
        sistema v bazne vektorje novega sist., kar pomeni, da je e1'=Qe1, e2'=Qe2 in
        e3'=Qe3.
        Stolpci matrie Q so koordinate baznih vektorjev novega sistema (e1', e2' in
        e3') v starem koordinatnem sistemu z ortonorm. baz. vektorji e1, e2 in e3.
          Ce so bazni vektorji e1', e2' in e3' paroma ortogonalni in normirani, je
        Q-1=QT (Inverz Q je kar njena transponirana matrika). V tem primeru je
        determinanta Q enaka 1, ce tvorijo e1', e2' in e3' desnosucni koordinatni
        sistem, in -1, ce tvorijo levosucnega.
          Transformacija linearnih operatorjev:
        Ce je A matrika linearnega operatorja v starem sistemu, potem dobimo matriko
        linearnega operatorja v novem koordinatnem sistemu A' kot A'=Q-1 A Q . Iz
        zapisa operatorja v novem sistemu dobimo zapis v starem sistemu na naslednji
        nacin: A=Q A Q-1.
        */



        //void invtransfmat3dortnormbas(vec3 xnew, vec3 ynew, mat3 transfinv)
        ///* Naredi inverzno transformacijsko matriko iz desnosucnega ortonormiranega
        //koordinatnega sistema v novi desnosucni ortonormiran sistem, katerega 1.
        //bazni vektor je normiran xnew, drugi pa je ortonormiran glede na prvega in
        //lezi v podprostoru, ki ga razpenjata xnew in ynew. Matriko shrani v
        //transfinv; ce s to matriko pomnozimo koordinate vektorja v starem
        //koordinatnem sistemu, dobimo koordinate vektorja v novem sistemu. xnew mora
        //biti razlicen od NULL in nenicelen vektor, medtem ko je lahko ynew tudi
        //neustrezen vektor - lahko je NULL ali kolinearen oz. skoraj kolinearen z
        //xnew. Ce je ynew neustrezen, funkcija sama izbere drugi bazni vektor;
        //tretji bazni vektor je vektorski produkt prvih dveh.
        //$A Igor okt01; */
        //{
        //    vec3 x, y, z;
        //    double sp, norm1, norm2, tol = 1e-10;
        //    if (ynew == NULL)
        //    {
        //        y.x = y.y = y.z = 0;
        //        ynew = &y;
        //    }
        //    if (xnew != NULL && transfinv != NULL)
        //    {
        //        if ((norm1 = vecnorm3d(xnew)) > 0)
        //        {
        //            /* x postane normiran xnew: */
        //            multscalvec3d(xnew, 1 / norm1, &x);
        //            /* Ortogonalizacija ynew glede na x in normiranje */
        //            if (sp = scalprod3d(&x, ynew) != 0)
        //                lincombvec3d(1, ynew, -sp, &x, &y);
        //            else (y = *ynew);
        //            if ((norm2 = vecnorm3d(&y)) <= tol * norm1)
        //            {
        //                /* Ce je bila norma ynew veliko manjsa od norme xnew oz. ce je bil ynew
        //                skoraj kolinearen z xnew, se za ynew izbere drug vektor, ki je bolj
        //                ustrezen: */
        //                setvec3d(ynew, 1, 0, 0);
        //                if (1 - fabs(sp = scalprod3d(&x, ynew)) < 0.1)
        //                {
        //                    setvec3d(ynew, 0, 1, 0);
        //                    sp = scalprod3d(&x, ynew);
        //                }
        //                lincombvec3d(1, ynew, -sp, &x, &y);
        //            }
        //            normvec3d(&y);
        //            vecprod3d(&x, &y, &z);
        //            transfinv.x = x;
        //            transfinv.y = y;
        //            transfinv.z = z;
        //        }
        //        else
        //        {
        //            errfunc0("(inv)transfmat3dortnormbas");
        //            fprintf(erf(), "Norm of the first vector of the new coordinate basis is 0.\n");
        //            errfunc2();
        //        }
        //    }
        //    else
        //    {
        //        errfunc0("(inv)transfmat3dortnormbas");
        //        fprintf(erf(), "One or more of the function arguments are NULL.\n");
        //        errfunc2();
        //    }
        //}




//void transfmat3dortnormbas(vec3 xnew,vec3 ynew,mat3 transf)
//    /* Naredi transformacijsko matriko iz desnosucnega ortonormiranega
//    koordinatnega sistema v novi desnosucni ortonormiran sistem, katerega 1.
//    bazni vektor je normiran xnew, drugi pa je ortonormiran glede na prvega in
//    lezi v podprostoru, ki ga razpenjata xnew in ynew. Matriko shrani v transf;
//    ce s to matriko pomnozimo koordinate vektorja v novem koordinatnem sistemu,
//    dobimo koordinate vektorja v starem sistemu. xnew mora biti razlicen od NULL
//    in nenicelen vektor, medtem ko je lahko ynew tudi neustrezen vektor - lahko
//    je NULL ali kolinearen oz. skoraj kolinearen z xnew. Ce je ynew neustrezen,
//    funkcija sama izbere drugi bazni vektor; tretji bazni vektor je vektorski
//    produkt prvih dveh.
//    $A Igor okt01; */
//{
//invtransfmat3dortnormbas(xnew,ynew,transf);
//transpmat3d(transf,transf);
//}



//void transfmat3d(vec3 xnew,vec3 ynew,vec3 znew,mat3 transf)
//    /* Naredi direktno transformacijsko matriko iz desnosucnega ortonormiranega
//    sistema v novi koordinatni sistem z baznimi vektorji xnew, ynew in znew.
//    Pri tem ni potrebno, da so novi bazni vektorji ortogonalni ali normirani.
//    Transformacijsko matriko zapise v transf. Kordinate vektorja v starem
//    koordinatnem sistemu dobimo tako, da mnozimo zapis vektorja v novem sistemu
//    s to matriko.
//    $A Igor okt01; */
//{
//if (xnew!=NULL && ynew!=NULL && znew!=NULL && transf!=NULL)
//{
//  transf.xx=xnew.x;
//  transf.yx=xnew.y;
//  transf.z.x=xnew.z;
//  transf.xy=ynew.x;
//  transf.yy=ynew.y;
//  transf.z.y=ynew.z;
//  transf.x.z=znew.x;
//  transf.y.z=znew.y;
//  transf.z.z=znew.z;
//}
//}

//void invtransfmat3d(vec3 xnew,vec3 ynew,vec3 znew,mat3 invtransf)
//    /* Naredi inverzno transformacijsko matriko iz desnosucnega ortonormiranega
//    sistema v novi koordinatni sistem z baznimi vektorji xnew, ynew in znew.
//    Pri tem ni potrebno, da so novi bazni vektorji ortogonalni ali normirani.
//    Inv. transf.matriko zapise v invtransf. Kordinate vektorja v novem
//    koordinatnem sistemu dobimo tako, da mnozimo zapis vektorja v starem
//    sistemu s to matriko.
//    $A Igor okt01; */
//{
//if (xnew!=NULL && ynew!=NULL && znew!=NULL && invtransf!=NULL)
//{
//  invtransf.xx=xnew.x;
//  invtransf.yx=xnew.y;
//  invtransf.z.x=xnew.z;
//  invtransf.xy=ynew.x;
//  invtransf.yy=ynew.y;
//  invtransf.z.y=ynew.z;
//  invtransf.x.z=znew.x;
//  invtransf.y.z=znew.y;
//  invtransf.z.z=znew.z;
//  invmat3d(invtransf,invtransf);
//}
//}


//void eulertransfmat3d(double theta,double psi,double fi,mat3 transf)
//    /* Naredi direktno transformacijsko matriko za zasuk koordinatnega sistema
//    v nov koordinatni sistem z Eulerjevimi koti fi, theta in psi.
//    Ref: Mat. prir. str. 178; S. Pahor: Uvod v analit. meh., str. 32
//    $A Igor okt01; */
//{
//double c1,c2,c3,s1,s2,s3;
//if (transf!=NULL)
//{
//  c1=cos(theta);  c2=cos(psi);  c3=cos(fi);
//  s1=sin(theta);  s2=sin(psi);  s3=sin(fi);
//  transf.xx= c2*c3-c1*s2*s3 ;
//  transf.xy= -c2*s3-c1*s2*c3 ;
//  transf.x.z= s1*s2 ;
//  transf.yx= s2*c3+c1*c2*s3 ;
//  transf.yy= -s2*s3+c1*c2*c3 ;
//  transf.y.z= -s1*c2 ;
//  transf.z.x= s1*s3 ;
//  transf.z.y= s1*c3 ;
//  transf.z.z= c1 ;
//}
//}


//void eulerinvtransfmat3d(double theta,double psi,double fi,mat3 invtransf)
//    /* Naredi inverzno transformacijsko matriko za zasuk koordinatnega sistema
//    v nov koordinatni sistem z Eulerjevimi koti fi, theta in psi.
//    Ref: Mat. prir. str. 178; S. Pahor: Uvod v analit. meh., str. 32
//    $A Igor okt01; */
//{
//double c1,c2,c3,s1,s2,s3;
//if (invtransf!=NULL)
//{
//  c1=cos(theta);  c2=cos(psi);  c3=cos(fi);
//  s1=sin(theta);  s2=sin(psi);  s3=sin(fi);
//  invtransf.xx= c2*c3-c1*s2*s3 ;
//  invtransf.yx= -c2*s3-c1*s2*c3 ;
//  invtransf.z.x= s1*s2 ;
//  invtransf.xy= s2*c3+c1*c2*s3 ;
//  invtransf.yy= -s2*s3+c1*c2*c3 ;
//  invtransf.z.y= -s1*c2 ;
//  invtransf.x.z= s1*s3 ;
//  invtransf.y.z= s1*c3 ;
//  invtransf.z.z= c1 ;
//}
//}


//void rotvectransfmat3d(vec3 c,double fi,mat3 transf)
//    /* Naredi direktno transformacijsko matriko za zasuk koordinatnega sistema
//    v nov koordinatni sisem okrog vektorja c za kot fi in jo zapise v transf.
//    Zasuk je v pozitivni smeri, ce se gleda z vrha vektorja v.
//     POSTOPEK:
//    Matriko najprej tvorimo v sistemu, v katerem je os x vzporedna z vektorjem
//    c, nato pa to matriko pretransformiramo v zacetni sistem. Ce je R rotacijska
//    matrika v sistemu, kjer je os x vzporedna s c, Q pa direktna
//    transformacijska matrika iz zacetnega sistema v ta sistem, potem je matrika,
//    ki jo iscemo, enaka transf = Q R QT .
//    $A Igor nov01; */
//{
//mat3 R,Q;
//double ds,dc;
//if (c!=NULL && transf!=NULL)
//{
//  ds=sin(fi);
//  dc=cos(fi);
//  R.xx= 1 ;
//  R.xy= 0 ;
//  R.x.z= 0 ;
//  R.yx= 0 ;
//  R.yy= dc ;
//  R.y.z= -ds ;
//  R.z.x= 0 ;
//  R.z.y= ds ;
//  R.z.z= dc ;
//  transfmat3dortnormbas(c,NULL,&Q);
//  matprod3d(&Q,&R,transf);
//  transpmat3d(&Q,&Q);
//  matprod3d(transf,&Q,transf);
//}
//}


//void rotvecinvtransfmat3d(vec3 c,double fi,mat3 invtransf)
//    /* Naredi inverzno transformacijsko matriko za zasuk koordinatnega sistema
//    v nov koordinatni sisem okrog vektorja c za kot fi in jo zapise v invtransf.
//    Zasuk je v pozitivni smeri, ce se gleda z vrha vektorja v.
//    $A Igor nov01; */
//{
//if (c!=NULL && invtransf!=NULL)
//{
//  rotvectransfmat3d(c,fi,invtransf);
//  transpmat3d(invtransf,invtransf);
//}
//}




//    /* PODAJANJE PREMIC IN RAVNIN NA RAZLICNE NACINE: */


//void line3dp1p2(vec3 p1,vec3 p2,vec3 r,vec3 s)
//    /* Izracuna tocko na premici r in smer. vektor premice s iz dveh tock na
//    premici p1 in p2.
//    $A Igor okt01; */
//{
//if (p1!=NULL && p2!=NULL && r!=NULL && s!=NULL)
//{
//  *r=*p1;
//  vecdif3d(p2,p1,s);
//}
//}

//void line3p1dn1n2(vec3 p1,vec3 n1,vec3 n2,vec3 r,vec3 s)
//    /* Izracuna tocko na premici r in smer. vektor premice s iz tocke na
//    premici p1 in in normal na premico n1 in n2.
//    $A Igor okt01; */
//{
//if (p1!=NULL && n1!=NULL && n2!=NULL && r!=NULL && s!=NULL)
//{
//  *r=*p1;
//  vecprod3d(n1,n2,s);
//}
//}


//void plane3dn1d(vec3 n1,double d1,vec3 r,vec3 n)
//    /* Izracuna tocko na ravnini r in normalo ravnine n iz normale ravnine n1
//    in oddaljenostjo ravnine od izhodisca d1.
//    $A Igor okt01; */
//{
//if (n1!=NULL && r!=NULL && n!=NULL)
//{
//  multscalvec3d(n1,d1/scalprod3d(n1,n1),r);
//  *n=*n1;
//}
//}


//void plane3dp1p2p3(vec3 p1,vec3 p2,vec3 p3,vec3 r,vec3 n)
//    /* Izracuna tocko na ravnini r in normalo ravnine n iz treh tock na ravnini
//    p1, p2 in p3.
//    $A Igor okt01; */
//{
//if (p1!=NULL && p2!=NULL && p3!=NULL && r!=NULL && n!=NULL)
//{
//  vecdif3d(p2,p1,r);
//  vecdif3d(p3,p1,n);
//  vecprod3d(r,n,n);
//  *r=*p1;
//}
//}


//void plane3dp1s1s2(vec3 p,vec3 s1,vec3 s2,vec3 r,vec3 n)
//    /* Izracuna tocko na ravnini r in normalo ravnine n iz tocke na ravnini p
//    ter iz dveh smernih vektorjev premic na ravnini s1 in s2.
//    $A Igor okt01; */
//{
//if (p!=NULL && s1!=NULL && s2!=NULL && r!=NULL && n!=NULL)
//{
//  vecprod3d(s1,s2,n);
//  *r=*p;
//}
//}



//    /* TESTIRANJE FUNKCIJ ZA ANALITICNO GEOMETRIJO V 3D: */


//void testgeom3d(void)
//    /* Test funkcij za analiticno 3D geometrijo.
//    $A Igor okt01; */
//{
//vec3 p1=NULL,p2=NULL,p3=NULL,p4=NULL,
//      r1=NULL,r2=NULL,r3=NULL,r4=NULL,r5=NULL,
//      s1=NULL,s2=NULL,s3=NULL,s4=NULL,s5=NULL,
//      n1=NULL,n2=NULL,n3=NULL,n4=NULL;
//vec3 x1,x2;
//double d1=0,d2=0,d3=0,d4=0;
///* 1. premica: */
//r1=getvec3dval(3.5,1.5,0);  s1=getvec3dval(3,1,0);
///* 2. premica: */
//r2=getvec3dval(0,5,0);  s2=getvec3dval(1,-2,0);
///* 1. tocka: */
//p1=getvec3dval(5,7,0);
///* 2. tocka: */
//p2=getvec3dval(3,6,0);
///* Ort. proj. tocke na premico in razdalja med tocko in premico: */
//d1=distptline3d(p1,r1,s1);
//ortprojptline3d(p1,r1,s1,&x1);
//printf("\nDistance between line 1 and point p1: %g\n",d1);
//printf("Orthogonal projection of p1 to line 1:\n");
//printvec3d(&x1);
//printf("\n");
//d2=distptline3d(p2,r1,s1);
//ortprojptline3d(p2,r1,s1,&x2);
//printf("\nDistance between line 1 and point p2: %g\n",d2);
//printf("Orthogonal projection of p2 to line 1:\n");
//printvec3d(&x2);
//printf("\n");
///* Izracun secisca premice in ravnine: */
//n1=getvec3dval(-1,3,0);  /* Pravokotnica na s1 - 1. ravnino izpeljemo iz 1. premice */
//intsectlineplane3d(r2,s2,r1,n1,&x1);
//printf("\nIntersection between line 2 and plane 1:\n");
//printvec3d(&x1);
///* Ort. projekcija premice na ravnino: */
//ortprojlineplane3d(r2,s2,r1,n1,&x1,&x2);
//printf("\nOrt. projection of line 2 on plane 1:\nPoint on projection:\n");
//printvec3d(&x1);
//printf("Direction vector:\n");
//printvec3d(&x2);
///* Ort. projekcija tocke na ravnino: */
//p3=getvec3d(); *p3=*p1; p3.z=5;
//ortprojptplane3d(p3,r1,n1,&x1);
//printf("\nOrt. proj. of point p3 to a plane:\n");
//printvec3d(&x1);
///* Razdalja med tocko in ravnino: */
//d1=distptplane3d(p3,r1,n1);
//printf("Distance between point p3 and plane 1: %g\n",d1);
///* Naredimo ravnino iz 2. premice komp. z r2 premaknemo, norm. je prav. na s1): */
//r3=getvec3d(); *r3=*r2; r3.z=7;
//n3=getvec3dval(2,1,0);
///* Secisce dveh ravnin: */
//d1=intsectplanes3d(r1,n1,r3,n3,&x1,&x2);
//printf("\nIntersection between plane 3 and plane 1:\nPoint on intersection:\n");
//printvec3d(&x1);
//printf("Direction vector of intersecting line:\n");
//printvec3d(&x2);
//printf("Distance between planes: %g\n",d1);
///* Razdalja in najbljizje tocke pri mimobecnicah: */
///* Naredimo premico, ki je 2. premica zamaknjena za 2 navzgor: */
//r4=getvec3d(); *r4=*r2; r4.z=2;
//s4=getvec3d(); *s4=*s2;
//d1=nearestptlines3d(r1,s1,r4,s4,&x1,&x2);
//printf("\nLine 1:\nr:\n");
//printvec3d(r1);
//printf("s:\n");
//printvec3d(s1);
//printf("Line4:\nr:\n");
//printvec3d(r4);
//printf("s:\n");
//printvec3d(s4);
//printf("\nClosest points on two non-intersecting lines:\nOn line 1:\n");
//printvec3d(&x1);
//printf("On line 4:\n");
//printvec3d(&x2);
//printf("Distance between lines: %g\n",d1);
///* Razdalja in najbljizje tocke pri premicah, ki se seceta: */
//d1=nearestptlines3d(r1,s1,r2,s2,&x1,&x2);
//printf("\nLine 1:\nr:\n");
//printvec3d(r1);
//printf("s:\n");
//printvec3d(s1);
//printf("Line2:\nr:\n");
//printvec3d(r2);
//printf("s:\n");
//printvec3d(s2);
//printf("\nClosest points on two intersecting lines:\nOn line 1:\n");
//printvec3d(&x1);
//printf("On line 2:\n");
//printvec3d(&x2);
//printf("Distance between lines: %g\n",d1);
///* Premici, katerih smerna vektorja sta pravokotna: */
//s5=getvec3dval(0,0,10);
//r5=getvec3d(); *r5=*p1; r5.z=123;
//d1=nearestptlines3d(r1,s1,r5,s5,&x1,&x2);
//printf("\nLine 1:\nr:\n");
//printvec3d(r1);
//printf("s:\n");
//printvec3d(s1);
//printf("Line5:\nr:\n");
//printvec3d(r5);
//printf("s:\n");
//printvec3d(s5);
//printf("\nClosest points on two nonintersecting orthog. lines:\nOn line 1:\n");
//printvec3d(&x1);
//printf("On line 2:\n");
//printvec3d(&x2);
//printf("Distance between lines: %g\n",d1);


//if (0)
//{
//  vec3 a,b,c;
//  mat3 A,B;
//  vec3 pr1;
//  double d1,d2;
//  printf("\n\n\n\n");
//  printf("Test of mixed product:\n");
//  a=getvec3drand(); b=getvec3drand(); c=getvec3drand();
//  printf("Vectors a, b and c:\n");
//  printvec3d(a); printvec3d(b); printvec3d(c);
//  printf("\n");
//  vecprod3d(a,b,&pr1);
//  d1=scalprod3d(&pr1,c);
//  d2=mixprod3d(a,b,c);
//  printf("(a x b)*c = %g\n",d1);
//  printf("(a,b,c)   = %g\n\n",d2);
  
//  printf("\n\n");
//  s3=getvec3dval(-2,2,0);
//  s4=getvec3dval(2,2,0);
//  vecprod3d(s3,s4,s4);
//  printf("\nVector product between (-2,2,0) and (2,2,0):\n");
//  printvec3d(s4);

//  s3=getvec3dval(-2,2,0);
//  s4=getvec3dval(0,2,0);
//  vecprod3d(s3,s4,s4);
//  printf("\nVector product between (-2,2,0) and (2,0,0):\n");
//  printvec3d(s4);

//  s3=getvec3dval(-2,7,11);
//  printf("\nNorm of vector (-2,7,11) is %g.\n",
//   vecnorm3d(s3));
   
//  /* Testiranje resevanja sistema 2 enacb z 2 neznankama: */
//  A=getmat3dval(5,2,222,  3,1,432,  643,6436,654);
//  b=getvec3dval(16,9,64365);
//  solve2d(A,b,&pr1);
//  printf("System of equations:\nSystem matrix:\n");
//  printmat3d(A);
//  printf("Right-hand vector:\n");
//  printvec3d(b);
//  printf("Solution:\n");
//  printvec3d(&pr1);
//  printf("2D Determinant: %g.\n",detmat2d(A));
//  /* Testiranje inverz. matrik v 2d: */
//  A=getmat3drand();
//  B=getmat3d();
//  invmat2d(A,B);
//  printf("\n2D Inverse:\n");
//  printmat3d(B);
//  matprod3d(A,B,B);
//  printf("\nOriginal matrix:\n");
//  printmat3d(A);
//  printf("\nProduct of orig. matrix by its 2D inverse:\n");
//  printmat3d(B);
//  /* Testiranje inverz. matrik v 3d: */
//  A=getmat3drand();
//  invmat3d(A,B);
//  matprod3d(A,B,B);
//  printf("\nOriginal matrix:\n");
//  printmat3d(A);
//  printf("\nProduct of orig. matrix by its 3D inverse:\n");
//  printmat3d(B);
//}
//}


//            /****************************/
//            /*                          */
//            /*  CO-ORDINATE TRANSFORMS  */
//            /*                          */
//            /****************************/



//void polarcoord(double x,double y,double *r,double *fi)
//    /* Converts cartesian co-ordinates (x,y) to polar co-ordinates (*r,*fi).
//    $A Igor feb03; */
//{
//*r=sqrt(x*x+y*y);
//if (x>0)
//  *fi=arctg(y/x);
//else if (x<0)
//  *fi=ConstPi+arctg(y/x);
//else if (y==0)
//  *fi=0;
//else if (y>0)
//  *fi=ConstPi/2;
//else if (y<0)
//  *fi=3*ConstPi/2;
//}

//void polartocartescoord(double r,double fi,double *x,double *y)
//    /* Converts polar co-ordinates (r,fi) to cartesian co-ordinates (*x,*y).
//    $A Igor feb03; */
//{
//*x=r*cos(fi);
//*y=r*sin(fi);
//}






    }   // class Geometry

}