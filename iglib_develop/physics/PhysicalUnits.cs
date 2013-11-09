

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IG.Lib;

namespace IG.Physics
{


    public interface IPhysicalUnit
    {

        string Id
        { get; set; }

        string Name
        { get; set; }

        string Description
        { get; set; }

        SIUnits SIUnit
        { get; }

    }

    public abstract class PhysicalUnitBase : IPhysicalUnit
    {


        /// <summary>Name that uniquely identifies a physical unit.
        /// Composed units do not have a name.</summary>
        public virtual string Id
        {
            get { return null; }
            set { throw new InvalidOperationException("Can not set an ID to this kind of unit."); }
        }

        protected string _name;

        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        protected string _description;

        public virtual string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public virtual SIUnits SIUnit
        { get { return SIUnits.None; } }

    }


    public class PhysicalUnitConst
    {

    }



}