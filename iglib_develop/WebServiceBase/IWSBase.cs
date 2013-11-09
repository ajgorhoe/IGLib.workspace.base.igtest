using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

using IG.Lib;

namespace IG.Web
{

    /// <summary>Interface for basic web service.</summary>
    [ServiceContract(Namespace = WSBaseClass.DefaultNamespace)]
    public interface IWSBase
    {

        /// <summary>URL of the web service.</summary>
        /// <remarks>This is put into Web service such that base service classes can be used instead of proxy classes,
        /// which may be useful when the appropriate service references are not available to generate the proxy classes.</remarks>
        string Url
        {
            get;
            set;
        }


        /// <summary>Cookie container, for compatibility with proxy classes that are derived 
        /// from the <see cref="System.Web.Services.Protocols.HttpWebClientProtocol"/> class.</summary>
        System.Net.CookieContainer CookieContainer
        {
            get; set;
        }
        
        ///// <summary>Implementation of interface function <see cref="IWSBase.Close"/>, for compatibility 
        ///// with proxy classes that are derived  from the <see cref="System.Web.Services.Protocols.HttpWebClientProtocol"/> class.</summary>
        //void Close();

        /// <summary>Returns the name of the web service.
        /// <para>Usually, address (URL) of the web service will consists of some base address and service name.</para></summary>
        [OperationContract]
        string GetServiceName();

        /// <summary>Sets the name of the web service.
        /// <para>Usually, address (URL) of the web service will consists of some base address and service name.</para></summary>
        [OperationContract]
        void SetServiceName(string name);

        /// <summary>Tests whether the web service is alive.
        /// <para>Returns a string identifying web service' class and object ID.</para>
        /// <para>To test, right-click the Web Service's .asmx file and select View in a browser.</para></summary>
        /// <returns></returns>
        [OperationContract]
        string TestService();

        /// <summary>Tests whether the web service is alive.
        /// <para>Returns a string identifying web service' class and object ID.</para>
        /// <para>To test, right-click the Web Service's .asmx file and select View in a browser.</para></summary>
        /// <returns></returns>
        [OperationContract]
        string TestServiceCmd(string commandlineArguments);

        /// <summary>Tests whether the web service is alive.
        /// <para>Returns a string identifying web service' class and object ID.</para>
        /// <para>To test, right-click the Web Service's .asmx file and select View in a browser.</para></summary>
        /// <returns></returns>
        [OperationContract]
        string TestServiceArgs(string[] commandlineArguments);

        /// <summary>Web service method example. Increments a static counter.
        /// <para>To test, right-click the Web Service's .asmx file and select View in Browser</para></summary>
        /// <returns></returns>
        [OperationContract]
        string TestCount();


    }
}
