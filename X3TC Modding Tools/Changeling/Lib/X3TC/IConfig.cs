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
using System.Windows.Forms;
using DS.X2Core;
using System.IO;

namespace Changeling.Lib.X3TC
{
    internal interface IConfig : Changeling.Lib.IConfig
    {
        ListViewItem add(SpecLaser spec, int p, string raceName, int targetIdx);
        ListViewItem add(SpecMissile spec, int p, string raceName, int targetIdx);

        void insert(CriteriaMissile crit, string raceName, int targetIdx);
        void insert(CriteriaLaser crit, string raceName, int targetIdx);

        void delete(CriteriaLaser crit, string raceName, int p);
        void delete(CriteriaMissile crit, string raceName, int p);

        bool validate(CriteriaMissile criteriaMissile);
        bool validate(SpecLaser specLaser);
        bool validate(CriteriaLaser criteriaLaser);
        bool validate(ChangePackage changePackage);
        bool validate(SpecMissile specMissile);
        bool validate(SpecBullet specBullet);
        bool validate(SpecCockpit specCockpit);
        bool validate(SpecDock specDock);
        bool validate(SpecFactory specFactory);
        bool validate(SpecWareBio specWareBio);
        bool validate(SpecShip specShip);
        bool validate(SpecWareEnergy specWareEnergy);
        bool validate(SpecWareFood specWareFood);
        bool validate(SpecWareMineral specWareMineral);
        bool validate(SpecWareNatural specWareNatural);
        bool validate(SpecWareTech specWareTech);
        bool validate(Start start);
        bool validate(GlobalMARS globalMARS);

        SpecMissile getMissile(int missileIndex);
        SpecBullet getBullet(int bulletIndex);
        SpecLaser getLaser(int laserIndex);
        SpecCockpit getCockpit(int cockpitIndex);
        SpecWareTech getWareTech(int ammoIndex);

        int getIndexOf(SpecBullet spec);
        int getIndexOf(SpecLaser spec);
        int getIndexOf(SpecMissile spec);
        int getIndexOf(SpecCockpit spec);
        int getIndexOf(SpecWareTech value);

        string getPath(ChangePackage changePackage);
    }
}
