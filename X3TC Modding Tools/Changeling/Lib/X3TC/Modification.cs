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
using Ionic.Zip;
using System.Windows.Forms;
using System.IO;

namespace Changeling.Lib.X3TC
{
    class Modification : ConfigDatum, IConfig
    {
        internal const String xmlName = "Modification";
        String dataSource;
        VFSInterface modVfs;


        public Modification(IConfig config, String fileName) :
            base(config)
        {
            dataSource = fileName;
            modVfs = new VFSInterface(this, config);
            modVfs.importModifications(new ZipFile(fileName));
        }

        public Modification(IConfig config, System.Xml.XmlElement xmlNode) :
            base(config)
        {
        }

        public override bool isValid()
        {
            throw new NotImplementedException();
        }

        public override string generateName()
        {
            return dataSource;
        }

        public override string generateLabel()
        {
            return dataSource;
        }

        public override string generateTooltip()
        {
            return null;
        }

        internal override void refresh(TreeNodeCollection nodeList)
        {
        }

        internal override System.Xml.XmlElement save(XmlDocument xmlDoc)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<FileDetails> findFiles(string vfsLocation, string filter = @".*", bool recurse = true)
        {
            throw new NotImplementedException();
        }

        public bool isModified(string fileName)
        {
            throw new NotImplementedException();
        }

        public System.IO.Stream readFile(string fileName)
        {
            throw new NotImplementedException();
        }

        public string writeFile(string fileName, Stream dataStream)
        {
            throw new NotImplementedException();
        }

        public string revertFile(string fileName)
        {
            throw new NotImplementedException();
        }

        public System.Windows.Forms.ListViewItem add(SpecLaser spec, int p, string raceName, int targetIdx)
        {
            throw new NotImplementedException();
        }

        public System.Windows.Forms.ListViewItem add(SpecMissile spec, int p, string raceName, int targetIdx)
        {
            throw new NotImplementedException();
        }

        public void insert(CriteriaMissile crit, string raceName, int targetIdx)
        {
            throw new NotImplementedException();
        }

        public void insert(CriteriaLaser crit, string raceName, int targetIdx)
        {
            throw new NotImplementedException();
        }

        public void delete(CriteriaLaser crit, string raceName, int p)
        {
            throw new NotImplementedException();
        }

        public void delete(CriteriaMissile crit, string raceName, int p)
        {
            throw new NotImplementedException();
        }

        public bool validate(CriteriaMissile criteriaMissile)
        {
            throw new NotImplementedException();
        }

        public bool validate(SpecLaser specLaser)
        {
            throw new NotImplementedException();
        }

        public bool validate(CriteriaLaser criteriaLaser)
        {
            throw new NotImplementedException();
        }

        public bool validate(ChangePackage changePackage)
        {
            throw new NotImplementedException();
        }

        public bool validate(SpecMissile specMissile)
        {
            throw new NotImplementedException();
        }

        public bool validate(SpecBullet specBullet)
        {
            throw new NotImplementedException();
        }

        public bool validate(SpecCockpit specCockpit)
        {
            throw new NotImplementedException();
        }

        public bool validate(SpecDock specDock)
        {
            throw new NotImplementedException();
        }

        public bool validate(SpecFactory specFactory)
        {
            throw new NotImplementedException();
        }

        public bool validate(SpecWareBio specWareBio)
        {
            throw new NotImplementedException();
        }

        public bool validate(SpecShip specShip)
        {
            throw new NotImplementedException();
        }

        public bool validate(SpecWareEnergy specWareEnergy)
        {
            throw new NotImplementedException();
        }

        public bool validate(SpecWareFood specWareFood)
        {
            throw new NotImplementedException();
        }

        public bool validate(SpecWareMineral specWareMineral)
        {
            throw new NotImplementedException();
        }

        public bool validate(SpecWareNatural specWareNatural)
        {
            throw new NotImplementedException();
        }

        public bool validate(SpecWareTech specWareTech)
        {
            throw new NotImplementedException();
        }

        public bool validate(Start start)
        {
            throw new NotImplementedException();
        }

        public bool validate(GlobalMARS globalMARS)
        {
            throw new NotImplementedException();
        }

        public SpecMissile getMissile(int missileIndex)
        {
            throw new NotImplementedException();
        }

        public SpecBullet getBullet(int bulletIndex)
        {
            throw new NotImplementedException();
        }

        public SpecLaser getLaser(int laserIndex)
        {
            throw new NotImplementedException();
        }

        public SpecCockpit getCockpit(int cockpitIndex)
        {
            throw new NotImplementedException();
        }

        public SpecWareTech getWareTech(int ammoIndex)
        {
            throw new NotImplementedException();
        }

        public int getIndexOf(SpecBullet spec)
        {
            throw new NotImplementedException();
        }

        public int getIndexOf(SpecLaser spec)
        {
            throw new NotImplementedException();
        }

        public int getIndexOf(SpecMissile spec)
        {
            throw new NotImplementedException();
        }

        public int getIndexOf(SpecCockpit spec)
        {
            throw new NotImplementedException();
        }

        public int getIndexOf(SpecWareTech value)
        {
            throw new NotImplementedException();
        }

        public DS.X2Core.X2FD.Catalog getCatalog(string destinationPath)
        {
            throw new NotImplementedException();
        }

        public string getPath(ChangePackage changePackage)
        {
            throw new NotImplementedException();
        }

        public string getWareClassText(int wareClass)
        {
            throw new NotImplementedException();
        }

        public string getText(int pageId, int textId)
        {
            throw new NotImplementedException();
        }

        public int getLocale()
        {
            return config.getLocale();
        }
    }
}
