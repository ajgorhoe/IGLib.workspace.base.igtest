// Copyright (c) Igor Grešovnik (2008 - present), IGLib license; http://www2.arnes.si/~ljc3m2/igor/iglib/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace IG.Web
{



    /// <summary>Class for referencing a web service based on the <see cref="WSDevelopClass"/>. 
    /// <para>In the conditions where web service proxy can not be obtained, this clas' base will inherit 
    /// from the service' base clase instead of it's generated proxy class.</para></summary>
    public class WSDevelopRef : WSDevelopRefBase, IWsDevelop
    {

        #region ComplexTypes.Conversion

        // Aim of this section is to provide interface methods in their original form by 
        // explicit conversion of what web method return to original types. 
        // Web service proxy classes make their own class types to transfer data of composite types.


        /// <summary>"Returns an array of Clients (maximum 10!)."</summary>
        /// <param name="Number">Number of client data objects retuerned, maximum 10.</param>
        public new ClientDataBase[] GetClientData(int Number)
        {
            var baseRet = base.GetClientData(Number);
            ClientDataBase[] ret = null;
            if (baseRet != null)
            {
                int numElements = baseRet.Length;
                ret = new ClientDataBase[numElements];
                for (int i = 0; i < numElements; ++i)
                {
                    var elementBase = baseRet[i];
                    ClientDataBase element = new ClientDataBase();
                    element.Id = elementBase.Id;
                    element.Name = elementBase.Name;
                }
            }
            //ClientDataBase[] ret = baseRet as ClientDataBase[];
            //return baseRet;
            return ret;
        }


        #endregion ComplexTypes.Conversion

    }


}
