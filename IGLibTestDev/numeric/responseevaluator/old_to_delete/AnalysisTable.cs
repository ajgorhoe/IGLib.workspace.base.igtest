// Copyright (c) Igor Grešovnik (2009), IGLib license; http://www2.arnes.si/~ljc3m2/igor/ioptlib/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IG.Lib;
using IG.Num;


using GridGenerator = IG.Num.GridGenerator1dBase;

namespace IG.NumExperimental
{
    
    /// <summary>Generation of tables of analysis pooints.</summary>
    /// $A Igor Apr10;
    [Obsolete("Other classes should be used instead. Delete this class when others are fully developed!")]
    public class AnalysisTable
    {

        // TODO: rearrange this! include in hte system of evaluators!

        /// <summary>Calculates factors for a table of values between two points, and stores
        /// them to factors.</summary>
        /// <param name="numElements">Number of sampling points (elements). It should be greater than 1.</param>
        /// <param name="centered">if 0 then factors run from 0 to 1 (for a table is from the
        /// specified starting till hte end point), otherwise factors run from -1 to 1
        /// (for tables centered in the starting point, ending in the end point and 
        /// starting in the reflected end point across the start point).</param>
        /// <param name="growthFactor">Factor by which length of successive intervals is increases
        /// to obtain table with intervals growing in geometric order. If the specified
        /// factor is 0 then it is set to 1.</param>
        /// <param name="scalingfactor">additional factor by which each factor is mutiplied.
        ///  If centered!=0 and growthfactor>1 then intervals fall from -1 to 0 and 
        /// grow from 0 to 1.</param>
        /// <param name="factors">Ouptput - list where factors are stored.</param>
        /// $A Igor Apr10;
        /// TODO: replace all references of this method by GridGenerator1dBase.CalculateGridUnitFactors()!
        public static void CalculateGridUnitFactors(int numElements, bool centered, double growthFactor,
             double scalingFactor, ref List<double> factors)
        {
            GridGenerator1dBase.CalculateGridUnitFactors(numElements, centered, growthFactor,
                scalingFactor, ref factors);
        }  // CalculateTableUnitFactors



        /// <summary>Calculates lengths of subsequent intervals defined by the first list, 
        /// and stores it to the second list.</summary>
        /// <param name="nodes">List containing nodes that define intervals.</param>
        /// <param name="lengths">List where interval lengths are stored.</param>
        /// TODO: replace all references of this method by GridGenerator1dBase.CalculateIntervalLenghts()!
        public static  void CalculateIntervalLenghts(List<double> nodes, ref List<double> lengths)
        {
            GridGenerator1dBase.CalculateIntervalLenghts(nodes, ref lengths);
        }


        /// <summary>Calculates length ratios between subsequent intervals defined by the first list, 
        /// ans stores it to the second list.</summary>
        /// <param name="nodes">List containing nodes that define intervals.</param>
        /// <param name="lengthRatios">List where interval length ratios are stored.
        /// This list has two elements less than the original list.</param>
        /// TODO: replace all references of this method by GridGenerator1dBase.CalculateIntervalLenghRatios()!
        public static void CalculateIntervalLenghRatios(List<double> nodes, ref List<double> lengthRatios)
        {
            GridGenerator1dBase.CalculateIntervalLenghRatios(nodes, ref lengthRatios);
        }


        /// <summary>Examples for calculating table factors.</summary>
        /// TODO: replace all references of this method by GridGenerator1dBase.ExampleTableFactors()!
        public static void ExampleTableFactors()
        {
            GridGenerator1dBase.ExampleTableFactors();
        }


    }
}
