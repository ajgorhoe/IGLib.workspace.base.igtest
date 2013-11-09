using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IG.Lib;

namespace IG.Web
{

    interface IWsDevelop: IWSBase,
        IIdentifiable, ILockable
    {


    }


    interface IWsDevelop1 : IWsDevelop, IWSBase
    {


    }

}
