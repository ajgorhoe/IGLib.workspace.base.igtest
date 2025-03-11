using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

using IG.Lib;


namespace IG.Web
{

    /// <summary>Interface for development web services.</summary>
    [ServiceContract(Namespace = WSBaseClass.DefaultNamespace)]
    public interface IWsDevelop : IWSBase
    {

        /// <summary>Returns an array of client data objects.</summary>
        string ReturnClientTable();

        /// <summary>"Returns an array of Clients (maximum 10!)."</summary>
        /// <param name="Number">Number of client data objects retuerned, maximum 10.</param>
        ClientDataBase[] GetClientData(int Number);

    } // Interface IWsDevelop


    /// <summary>Interface for higher level development web services.</summary>
    [ServiceContract(Namespace = WSBaseClass.DefaultNamespace)]
    public interface IWsDevelop1 : IWsDevelop, IWSBase
    {
        
        /// <summary>Another test method from higher level development service.</summary>
        string TestMethodWS2();

    }  // Interface IWsDevelop1

}
