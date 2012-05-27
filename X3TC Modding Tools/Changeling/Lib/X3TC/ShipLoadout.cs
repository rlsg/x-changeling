/* 
 * Copyright (c) 2011-2012 Roger L.S. Griffiths
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Changeling.Lib.X3TC
{
    /// <summary>
    /// 
    /// </summary>
    class ShipLoadout : ConfigDatum
    {
        public TextEntryTranslated shipName=null;
        public NumericI shipTypeIndex=-1;
        public NumericI spawnCount=1;
        public Location location=null;
        public NumericFP percEngine = 0;
        public NumericFP percRudder = 0;
        public NumericFP percCargo = 0;
        public ShipLoadoutLaserList lasers;
        public ShipLoadoutMissileList missiles;
        public ShipLoadoutCargoList cargo;
        public ShipLoadoutEquipmentList equipment;
        public ShipLoadoutList dockedShips;
        public ShipLoadoutList convoyShips;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public ShipLoadout(IConfig config) :
            base(config)
        {
            lasers = new ShipLoadoutLaserList(config);
            missiles = new ShipLoadoutMissileList(config);
            cargo = new ShipLoadoutCargoList(config);
            equipment = new ShipLoadoutEquipmentList(config);
            dockedShips = new ShipLoadoutList(config, "DockedShips");
            convoyShips = new ShipLoadoutList(config, "ConvoyShips");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <param name="node"></param>
        public ShipLoadout(IConfig config, XmlElement node) :
            this(config)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool isValid()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string generateName()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string generateLabel()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string generateTooltip()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        internal override System.Xml.XmlElement save(System.Xml.XmlDocument xmlDoc)
        {
            throw new NotImplementedException();
        }
    }
}
