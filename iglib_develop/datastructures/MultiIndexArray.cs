// Copyright (c) Igor Grešovnik (2008 - present), IGLib license; http://www2.arnes.si/~ljc3m2/igor/ioptlib/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IG.Lib
{


    /// <summary>Multidimensional arrays.</summary>
    /// $A Igor xx;
    public class MultiDimensionalArray<ElementType>
    {

        /// <summary>Delegate that is used to do work at specific point in various methods that iterate over
        /// multidimensional arrays.</summary>
        /// <typeparam name="ElementType">Type of array element.</typeparam>
        /// <param name="array">Array on which the method operates.</param>
        /// <param name="indices">Indices of the current element of sub-array.</param>
        /// <param name="currentLevel">Current level of iteration. Starts with 0 for the level where outermost sub-arrays are iterated,
        /// until the number of dimensions where individual element is worked on.</param>
        public delegate void MultidimensionalArrayIterator<ElementType>(MultiDimensionalArray<ElementType> array, int[] indices,
        int currentLevel);

        #region Construction


        /// <summary>Prevents calling argument-less constructor.</summary>
        private MultiDimensionalArray()
        {  }
        
        /// <summary>Constructs a new multidimensional array with specified dimensions.</summary>
        /// <param name="dimensions">Dimensions of the multidimensional array.
        /// <para>Can be null, meaning a zero rank array containing a sigle element.</para></param>
        public MultiDimensionalArray(params int[] dimensions)
        {
            this.Dimensions = dimensions;
        }

        #endregion Construction


        #region ILockable

        protected readonly object _lock = new object();

        /// <summary>Object used for thread locking.</summary>
        public object Lock
        {
            get { return _lock; }
        }

        #endregion ILockable


        #region Data

        protected ElementType[] _data;

        /// <summary>Array containing data of the multidimensional array.</summary>
        protected ElementType[] Data
        {
            get { lock (Lock) { return _data; } }
            set { lock (Lock) { _data = value; } }
        }

        /// <summary>Sets all elements to the default value for the specific element type.</summary>
        public void ClearElements()
        {
            lock (Lock)
            {
                if (_data != null)
                {
                    int count = _data.Length;
                    for (int i = 0; i < count; ++i)
                    {
                        _data[i] = default(ElementType);
                    }
                }
            }
        }

        protected int[] _dimensions;

        /// <summary>Gets or sets dimensions of the multidimensional array.</summary>
        /// <remarks><para>Getter creates and returns a copy of internal dimensions.</para></remarks>
        public int[] Dimensions
        {
            get { lock (Lock) { 
                if (_dimensions == null)
                    return null;
                int length = _dimensions.Length;
                int[] ret = new int[length];
                for (int i = 0; i < length; ++i)
                {
                    ret[i] = _dimensions[i];
                }
                return ret;
            } }
            set
            {
                lock (Lock)
                {
                    //if (value == null)
                    //    throw new ArgumentException("Array of dimensions is not specified (null argument).");
                    int length = 0;
                    if (value!=null)
                        length = value.Length;
                    bool dimensionsChanged = false;
                    if (_dimensions == null)
                    {
                        if (_numElements<1)
                            dimensionsChanged = true;
                    } else if (length != _dimensions.Length)
                    {
                        dimensionsChanged = true;
                    }
                    else
                    {
                        for (int i = 0; i < length; ++i)
                        {
                            if (value[i] != _dimensions[i])
                            {
                                dimensionsChanged = true;
                                break;
                            }
                        }
                    }
                    if (dimensionsChanged)
                    {
                        if (length == 0)
                        {
                            _dimensions = null;
                            _subTableLengths = null;
                            _numIndices = 0;
                            _data = new ElementType[1];
                            _numElements = 1;
                        } else
                        {
                            _dimensions = new int[length];
                            _subTableLengths = new int[length];
                            _numIndices = length;
                            int dataLength = 1; // number of all elements
                            for (int i = length-1; i >= 0; --i)
                            {
                                _subTableLengths[i] = dataLength;
                                int dim = value[i];
                                if (dim <= 0)
                                    throw new ArgumentException("The specified dimension No. " + i + " is less than or equal to 0.");
                                dataLength *= dim;
                                _dimensions[i] = dim;
                            }
                            _data = new ElementType[dataLength];
                            _numElements = dataLength;
                            for (int i = 0; i < dataLength; ++i)
                            {
                                _data[i] = default(ElementType);
                            }
                        }
                    }
                }
            }
        }

        protected int _numIndices;

        /// <summary>Gets numbet of indices of the multidimensional array.</summary>
        public int NumIndices
        {
            get { lock (Lock) { return _numIndices; } }
            // protected set { lock (Lock) { _numIndices = value; } }
        }

        public int _numElements;
        
        /// <summary>Gets total number of elements of the multidimensional array.</summary>
        public int NumElements
        {
            get { lock (Lock) { return _numElements; } }
        }

        /// <summary>Array of subtable lengths. Last element is always one.</summary>
        protected int[] _subTableLengths;

        /// <summary>Gets an array of lengths of the subtables with specified number of indices defined.</summary>
        /// <remarks><para>Getter creates and returns a copy of internal subtable lengths.</para></remarks>
        public int[] SubTableLengths
        {
            get
            {
                lock (Lock)
                {
                    if (_subTableLengths == null)
                        return null;
                    int length = _subTableLengths.Length;
                    int[] ret = new int[length];
                    for (int i = 0; i < length; ++i)
                    {
                        ret[i] = _subTableLengths[i];
                    }
                    return ret;
                }
            }
        }

        /// <summary>Sets new dimensions of the current multidimensional array.</summary>
        /// <param name="dimensions">New dimensions.</param>
        /// <remarks>Setter of the index operator is used.</remarks>
        public void SetDimensions(params int[] dimensions)
        {
            this.Dimensions = dimensions;
        }

        /// <summary>Returns the specified dimension of the multidimensional array.</summary>
        /// <param name="which">Specifies which dimension is returned.</param>
        public int Dimension(int which)
        {
            lock (Lock)
            {
                if (which < 0 || which >= _numIndices)
                    throw new IndexOutOfRangeException("Dimension No. " + which + " is out of range, should be between 0 and " + (_numIndices - 1) + ".");
                return _dimensions[which];
            }
        }

        /// <summary>Returns a linear index that corresponds to the specified table of indices,
        /// i.e. index of the element specified by these indices in the onedimensional data array.</summary>
        /// <param name="indices">Set of indices that specify the desired element.</param>
        /// <remarks>If the array has zero rank then <paramref name="indices"/> can be null, in this case 0 is 
        /// returned (pointing at teh only element).</remarks>
        public int GetLinearIndex(params int[] indices)
        {
            lock (Lock)
            {
                if (indices == null)
                {
                    if (_numIndices == 0 && _numElements > 0)
                        return 0;
                    else
                        throw new ArgumentException("Element indices not specified (null array).");
                } else
                {
                    int length = indices.Length;
                    if (length != _numIndices)
                    {
                        throw new ArgumentException("Wrong number of indices " + length + ", should be " + _numIndices + " or 1.");
                    }
                    int linearIndex = 0;
                    for (int i = 0; i < _numIndices; ++i)
                    {
                        int index = indices[i];
                        if (index < 0 || index >= _dimensions[i])
                            throw new ArgumentException("Index No. " + i + " out of range: " + index + ", should be between 0 and " + (_dimensions[i] - 1) + ".");
                        linearIndex += index * _subTableLengths[i];
                    }
                    return linearIndex;
                }
            }
        }

        /// <summary>Returns indices of the element whose linear index is specified (i.e. the index of element in the
        /// one dimensional data array that actually holds the data).</summary>
        /// <param name="linearIndex">Linear index of the element in the one dimensional array of elements, for which
        /// the multidimensional index is returned.</param>
        public int[] GetIndices(int linearIndex)
        {
            lock (Lock)
            {
                if (_numIndices == 0)
                {
                    if (linearIndex == 0 && _numElements > 0)
                        return null;
                    else if (linearIndex != 0)
                        throw new IndexOutOfRangeException("Linear index " + linearIndex + " out of range, can only be 0 for array of rank 0.");
                    else
                    {
                        throw new IndexOutOfRangeException("Linear index 0 out of range, multidimensional array contains no elements.");
                    }
                } else
                {
                    if (linearIndex < 0 || linearIndex >= _numElements)
                    {
                        throw new IndexOutOfRangeException("Linear index " + linearIndex + " out of range, should be between 0 and " + (_numElements - 1) + ".");
                    } else
                    {
                        int[] ret = new int[_numIndices];
                        int remainingElements = linearIndex;
                        for (int i = 0; i < _numIndices; ++i)
                        {
                            int index = remainingElements / _subTableLengths[i];
                            ret[i] = index;
                            remainingElements -= index * _subTableLengths[i];
                        }
                        return ret;
                    }
                }
            }
        }

        /// <summary>Index operator, gets or sets the specified element of the multidimensional array.
        /// <para>A single index can be specified, treated as linear index of the element in the data array.</para></summary>
        /// <param name="indices">Indices that specify the element of the multidimensional array.
        /// <para>Can either be a full set of indices, or only one linear index (or array with one element) 
        /// indicating the index of the element in the internal one dimensional data array.</para>
        /// <para>If the array is zero rank then indices can be null.</para></param>
        public ElementType this[params int[] indices]
        {
            get
            {
                lock (Lock)
                {
                    // Gets linear index of the element in the data array:
                    int linearIndex = -1;
                    if (indices != null)
                        if (indices.Length == 1)
                        {
                            linearIndex = indices[0];
                            if (linearIndex < 0 || linearIndex >= _numElements)
                                throw new ArgumentException("Linear index " + linearIndex + " out of range: should be between 0 and " + _numElements + ".");
                        }
                    if (linearIndex < 0)
                        linearIndex = GetLinearIndex(indices);
                    return _data[linearIndex];
                }
            }
            set
            {
                lock (Lock)
                {
                    // Gets linear index of the element in the data array:
                    int linearIndex = -1;
                    if (indices != null)
                        if (indices.Length == 1)
                        {
                            linearIndex = indices[0];
                            if (linearIndex < 0 || linearIndex >= _numElements)
                                throw new ArgumentException("Linear index " + linearIndex + " out of range: should be between 0 and " + _numElements + ".");
                        }
                    if (linearIndex < 0)
                        linearIndex = GetLinearIndex(indices);
                    _data[linearIndex] = value;
                }
            }
        }


        #endregion Data


        #region Operation

        /// <summary>Auxiliary method for <see cref="Iterate"/>, recursive.
        /// <para>This method is usually called by the <see cref="Iterate"/>  method to iterate recursively over elements.</para></summary>
        /// <param name="indices">Current array of indices that selects a specific element
        /// (if <paramref name="level"/> == <see cref="NumIndices"/>) or define the subtable over which to iterate in
        /// the next level. 
        /// <para>Initial call (outermost recursion level) to this method should be with all indices equal to 0.</para>
        /// <para>The first index is associated with the outer-most recursion level and varies most slowly durig recursion.</para></param>
        /// <param name="level">Current level of iteration; starts with 0. 
        /// <para>Initial call (outermost recursion level) to this method should be with level equal to 0.</para></param>
        /// <param name="beginLevel">Delegate that executes when a new level is begun. This is not executed for each single element, 
        /// but only for subarrays.</param>
        /// <param name="endLevel">delegate that executes when a new level ends. This is not executed for each single element, 
        /// but only for subarrays.</param>
        /// <param name="workElement">Delegate that executes in the final level, for each element of the multidimensional array.</param>
        /// <remarks>
        /// <para>The <paramref name="level"/> argument equals the number of indices that are fixed at the current recursion level.</para>
        /// <para></para>
        /// <para>The <paramref name="workElement"/> delegate is executed only in the deepest recursion level when 
        /// individual elements are worked. At this level, the <paramref name="level"/> argument equals the number of array dimensions
        /// (i.e. <see cref="NumIndices"/>).</para>
        /// <para>At the top-most iteration level (when the method is first called), the <paramref name="level"/> parameter
        /// equals 0, and no indices are specified at this level.</para>
        /// </remarks>
        protected void IterateAux(int[] indices, int level, 
            MultidimensionalArrayIterator<ElementType> beginLevel,
            MultidimensionalArrayIterator<ElementType> endLevel,
            MultidimensionalArrayIterator<ElementType> workElement)
        {
            
            if (level < _numIndices)
            {
                if (beginLevel != null)
                    beginLevel(this, indices, level);
                for (int i = 0; i < _dimensions[level]; ++i)
                {
                    indices[level] = i;
                    IterateAux(indices, level+1, beginLevel, endLevel, workElement);
                }
                indices[level] = 0;
                if (endLevel!=null)
                    endLevel(this, indices, level);
            } else if (level == _numIndices)
            {
                // Level of individual elements:
                if (workElement != null)
                    workElement(this, indices, level);
            }
        }


        /// <summary>Iterates over all elements of the multdimensional array, and executes the specified
        /// delegates at different events.
        /// <para>This is initiation method for iteration over elements, and the recursive method 
        /// <see cref="IterateAux"/> is called to actually do the job. See that method for more detailed description.</para></summary>
        /// <param name="beginLevel">Executed when a new level begins. This is not executed for each single element, 
        /// but only for subarrays.</param>
        /// <param name="endLevel">Execuded when a new level ends. This is not executed for each single element, 
        /// but only for subarrays.</param>
        /// <param name="workElement">Executed on each element (i.e. only on the last level).</param>
        public void Iterate(
            MultidimensionalArrayIterator<ElementType> beginLevel,
            MultidimensionalArrayIterator<ElementType> endLevel,
            MultidimensionalArrayIterator<ElementType> workElement)
        {
            lock (Lock)
            {
                if (_numElements < 1)
                    return;
                int[] indices = null;
                if (_numIndices!=0) 
                    indices = new int[_numIndices];
                for (int i = 0; i < _numIndices; ++i)
                {
                    indices[i] = 0;
                }
                IterateAux(indices, 0, beginLevel, endLevel, workElement);
            }
        }

        #endregion Operation


        /// <summary>Creates and returns a string representation of indices as a plain comma separated list of 
        /// integers in a single line (without brackets).</summary>
        /// <param name="indices">Indices whose string representation is returned.</param>
        public string IndicesToStringNoBrackets(int[] indices)
        {
            StringBuilder sb = new StringBuilder();
            if (indices != null)
            {
                int length = indices.Length;
                int last=length-1;
                for (int i = 0; i < length; ++i)
                {
                    sb.Append(indices[i].ToString());
                    if (i < last)
                        sb.Append(", ");
                }
            }
            return sb.ToString();
        }

        /// <summary>Creates and returns a string representation of indices as a comma separated list of 
        /// integers in a single line and tightly embedded in curly brackets.</summary>
        /// <param name="indices">Indices whose string representation is returned.</param>
        public string IndicesToString(int[] indices)
        {
            return "{" + IndicesToStringNoBrackets(indices) + "}";
        }


        /// <summary>Returns a simple string representation of a multidimensional array. Each element is written in its
        /// own line starting with element indices and a colon.</summary>
        /// <returns></returns>
        public string ToStringSimple()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Multidimensional array: ");
            sb.AppendLine("  Dimensions: " + IndicesToString(Dimensions));
            sb.AppendLine("  Values: "); 
            Iterate(
                null  /* beginLevel */, 
                null  /* endLevel */,
                (array, indices, currentLevel) =>
                {
                    sb.AppendLine(IndicesToString(indices) + ": " + array[indices].ToString());
                } /* workElement */
            );
            return sb.ToString();
        }


        /// <summary>Returns a simple string representation of a multidimensional array. Each element is written in its
        /// own line starting with element indices and a colon.</summary>
        /// <returns></returns>
        public string ToStringTest()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Multidimensional array: ");
            sb.AppendLine("  Dimensions: " + IndicesToString(Dimensions));
            sb.AppendLine("  Values: ");
            Iterate(
                (array, indices, currentLevel) =>
                {
                    for (int i = 0; i < currentLevel; ++i)
                        sb.Append("  ");
                    sb.AppendLine("<" + currentLevel.ToString() + "| ");
                } /* beginLevel */,
                (array, indices, currentLevel) =>
                {
                    for (int i = 0; i < currentLevel; ++i)
                        sb.Append("  ");
                    sb.AppendLine("|" + currentLevel.ToString() +  ">" +  " ");
                } /* endLevel */,
                (array, indices, currentLevel) =>
                {
                    for (int i = 0; i < currentLevel; ++i)
                        sb.Append("  ");
                    sb.AppendLine(array[indices].ToString());
                } /* workElement */
            );
            return sb.ToString();
        }

        /// <summary>Returns a string representation of a multidimensional array. Each element is written in its
        /// own line starting with element indices and a colon.</summary>
        /// <returns></returns>
        public string ToStringOld()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Multidimensional array: ");
            sb.AppendLine("  Dimensions: " + IndicesToString(Dimensions));
            sb.AppendLine("  Values: ");
            Iterate(
                (array, indices, currentLevel) =>
                {
                    if (currentLevel < array.NumIndices)
                    {
                        for (int i = 0; i < currentLevel; ++i)
                            sb.Append("  ");
                        sb.Append("{");
                        if (currentLevel < array.NumIndices -1)
                          sb.AppendLine();
                    }
                    //else if (currentLevel == array.NumIndices)
                    //{
                    //    for (int i = 0; i < currentLevel; ++i)
                    //        sb.Append("  ");
                    //    sb.Append("{");
                    //}
                } /* beginLevel */,

                (array, indices, currentLevel) =>
                {
                    if (currentLevel < array.NumIndices-1)
                    {
                        for (int i = 0; i < currentLevel; ++i)
                            sb.Append("  ");
                        sb.Append("}");
                        if (indices[currentLevel] < array.Dimension(currentLevel) - 1)
                            sb.Append(", ");
                        sb.AppendLine();
                    } else if (currentLevel == array.NumIndices -1)
                    {
                        sb.Append("} ");
                        if (indices[currentLevel] < array.Dimension(currentLevel) - 1)
                            sb.Append(", ");
                        sb.AppendLine();
                    } 
                    //else if (currentLevel == array._numIndices)
                    //{
                    //    if (indices[currentLevel-1] < array.Dimension(currentLevel-1) - 1)
                    //        sb.Append(",y ");
                    //}
                }  /* endLevel */,

                (array, indices, currentLevel) =>
                {
                    sb.Append(array[indices].ToString());
                    if (indices[_numIndices - 1] < Dimension(_numIndices - 1) - 1)
                        sb.Append(", ");
                } /* workElement */
            );
            return sb.ToString();
        }

        
        /// <summary>Returns a string representation of a multidimensional array in the Mathematica-like set notation. 
        /// <para>Elements are written in nested curly brackets.</para></summary>
        public override string ToString()
        {
            return ToString(true);
        }

        /// <summary>Returns a string representation of a multidimensional array in the Mathematica-like set notation. 
        /// <para>Elements are written in nested curly brackets.</para></summary>
        /// <param name="newLinesLast">If true then elements of the last level are written with newlines between.</param>
        public string ToString(bool newLinesLast)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Multidimensional array: ");
            sb.AppendLine("  Dimensions: " + IndicesToString(Dimensions));
            sb.AppendLine("  Values: ");
            Iterate(

                (array, indices, currentLevel) =>
                {
                    // beginLevel delegate:
                    if (_numIndices <= 0)
                    {  }
                    else
                    {
                        if (currentLevel == 0)
                        {
                            // Outer-most level, write the first open bracket if applicable:
                            sb.Append("{");
                            if (_numIndices > 1 || newLinesLast)
                                sb.AppendLine();
                        }
                        else if (currentLevel < _numIndices)
                        {
                            // Nested level but not the last one (i.e. contains subtables):
                            for (int i = 0; i < currentLevel; ++i)
                                sb.Append("  ");
                            sb.Append("{");
                            if (currentLevel < _numIndices - 1 || newLinesLast)
                                sb.AppendLine();
                        }
                    }
                }, // beginLevel


                (array, indices, currentLevel) =>
                {
                    // endLevel delegate:
                    if (_numIndices <= 0)
                    {  }
                    else
                    if (currentLevel == 0)
                    {
                        sb.AppendLine("}");
                    } else if (currentLevel < _numIndices - 1)
                    {
                        for (int i = 0; i < currentLevel; ++i)
                            sb.Append("  ");
                        sb.Append("}");
                        if (indices[currentLevel - 1] < _dimensions[currentLevel -1] - 1)
                            sb.Append(", ");
                        sb.AppendLine();
                    }
                    else if (currentLevel == _numIndices - 1)
                    {
                        if (newLinesLast)
                        {
                            for (int i = 0; i < currentLevel; ++i)
                            sb.Append("  ");
                        }
                        sb.Append("}");
                        if (indices[currentLevel - 1] < _dimensions[currentLevel - 1] - 1)
                            sb.Append(", ");
                        sb.AppendLine();
                    }
                    
                },  // endLevel


                (array, indices, currentLevel) =>
                {
                    // workElement delegate:
                    if (_numIndices <= 0)
                    { sb.AppendLine(array[indices].ToString()); }
                    else
                    {
                        if (currentLevel < _numIndices)  // check that this delegate is called correctly (only in the last recursion level):
                            throw new InvalidOperationException("Delegate for working an element is called in a wrong level of " + Environment.NewLine
                                + "  recursion (" + currentLevel + "), smaller than the number of array dimensions " + _numIndices + ".");
                        if (newLinesLast)
                        {
                            for (int i = 0; i < currentLevel; ++i)
                                sb.Append("  ");
                        }
                        sb.Append(array[indices].ToString());
                        if (indices[_numIndices - 1] < _dimensions[_numIndices - 1] - 1)
                            sb.Append(", ");
                        if (newLinesLast)
                            sb.AppendLine();
                    }
                } /* workElement */

            );
            return sb.ToString();
        }



        #region Auxiliary



        #endregion Auxiliary


        #region Examples


        /// <summary>Fills the specified multidimensional array of integers in such a way that
        /// element values are related to positions within the array in a simple and easily identifiable way.</summary>
        /// <param name="array"></param>
        public static void ExampleFillArray(MultiDimensionalArray<int> array)
        {
            if (array != null)
            {
                lock (array.Lock)
                {
                    int numElements = array.NumElements;
                    int numIndices = array.NumIndices;
                    int baseValue = 40000000;
                    for (int i = 0; i < numElements; ++i)
                    {
                        int value = baseValue;
                        int[] indices = array.GetIndices(i);
                        if (indices != null)
                        {
                            for (int which = 0; which < numIndices; ++which)
                            {
                                double power = numIndices-1-which;
                                value += indices[which] * (int)Math.Pow(10, power);
                            }
                        }
                        array[i] = value;
                    }
                }
            }
        }


        /// <summary>Example of use of multidimensional arrays.</summary>
        public static void Example(params int[] dimensions)
        {
            Console.WriteLine(Environment.NewLine + Environment.NewLine
                + "Multidimensional array example... " + Environment.NewLine);
            Console.WriteLine("Dimensions: ");
            if (dimensions == null)
                Console.WriteLine("null");
            else
            {
                for (int i = 0; i < dimensions.Length; ++i)
                {
                    Console.Write(dimensions[i] + " ");
                }
                Console.WriteLine();
            }
            MultiDimensionalArray<int> array = new MultiDimensionalArray<int>(dimensions);
            ExampleFillArray(array);

            //Console.WriteLine("Array in simple string form: " + Environment.NewLine + array.ToStringSimple() + Environment.NewLine);
            Console.WriteLine("Array in TEST form: " + Environment.NewLine + array.ToStringTest() + Environment.NewLine);
            Console.WriteLine("Array in set form, all newlines: " + Environment.NewLine + array.ToString(true /* newlinesLast */) 
                + "<end/>" + Environment.NewLine + Environment.NewLine);
            Console.WriteLine("Array in set form, last level in single line: " + Environment.NewLine + array.ToString(false /* newlinesLast */) 
                + "<end/>" + Environment.NewLine + Environment.NewLine);

            //Console.WriteLine(Environment.NewLine + Environment.NewLine + "Zero rank array: ");
            //MultiDimensionalArray<int> zeroRankArray = new MultiDimensionalArray<int>(null);
            //zeroRankArray[0] = 123456;
            //Console.WriteLine("Array in simple string form: " + Environment.NewLine + zeroRankArray.ToStringSimple() + Environment.NewLine);
            //Console.WriteLine("Array in set form: " + Environment.NewLine + zeroRankArray.ToString() + Environment.NewLine);
        }

        /// <summary>Example of use of multidimensional arrays.</summary>
        public static void Example()
        {
            int rank = 3;
            int[] dimensions;
            if (rank == 0)
            {
                dimensions = null;
            } else
            if (rank == 1)
            {
                dimensions = new int[] { 3 };
            }
            else if (rank == 2)
            {
                dimensions = new int[] { 3, 4 };
            }
            else // if (rank == 3)
            {
                dimensions = new int[] { 3, 2, 4 };
            }
            Example(dimensions);
        }


        #endregion Examples


    }
}
