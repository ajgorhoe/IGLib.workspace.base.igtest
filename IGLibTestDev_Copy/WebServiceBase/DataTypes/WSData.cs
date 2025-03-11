// Copyright (c) Igor Grešovnik (2008 - present), IGLib license; http://www2.arnes.si/~ljc3m2/igor/iglib/

// DATA CLASSES FOR WEB SERVICES.


using System;
using System.Collections;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Services;
using System.ServiceModel;


namespace IG.Web
{

    /// <summary>Class used for transferring client data between services.</summary>
    //[Serializable]
    //[DataContract(Namespace = WSBaseClass.DefaultNamespace)]
    public class ClientDataBase
    {
        public ClientDataBase(): this(0)
        {  }

        public ClientDataBase(int id)
        {
            this.Id = id;
        }

        protected string _name;

        /// <summary>Returns name of the client.</summary>
        [DataMember]
        public String Name
        {
            get {
                if (string.IsNullOrEmpty(_name))
                {
                    _name = "Client_" + Id.ToString();
                }
                return _name; }
            set { _name = value; }
        }


        protected int _id;

        /// <summary>Returns ID of the client.</summary>
        [DataMember]
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }



    } // class ClientDataBase


}