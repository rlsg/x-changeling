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
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using DS.X2Core;

namespace Changeling.Lib.X3TC
{
    /// <summary>
    /// 
    /// </summary>
    internal class Configuration : IConfig
    {
        internal const Int32 maxWeaponIdx = 1000;
        internal const String fileNameEES = "8387";
        internal const Int32 pageEESConfig = 60000;
        internal const Int32 pageEESFirstRace = 60001;
        internal const String fileNameMARS = "7047";
        internal const Int32 pageMARSConfig = 7047;
        internal readonly static String[] raceStrings={
            "Argon",
            "Boron",
            "Split",
            "Paranid",
            "Teladi",
            "Xenon",
            "Kha'ak",
            "Pirate",
            "Goner",
            "Player",
            "Enemy",
            "Neutral",
            "Friendly",
            "Unknown",
            "Race 1",
            "Race 2",
            "ATF",
            "Terran",
            "Yaki"
        };

        bool dirty = false;
        SpecLaserList lasers= null;
        SpecMissileList missiles = null;
        SpecBulletList bullets = null;
        SpecCockpitList cockpits = null;
        SpecShipList ships = null;
        SpecDockList docks = null;
        SpecFactoryList factories = null;
        SpecWareBioList waresBio = null;
        SpecWareEnergyList waresEnergy = null;
        SpecWareFoodList waresFood = null;
        SpecWareMineralList waresMineral = null;
        SpecWareNaturalList waresNatural = null;
        SpecWareTechList waresTech = null;
        CriteriaRaceList races=null;
        GlobalMARS configMARS = null;
        TextDictionary textDatabase = null;
        ChangePackageList changePackages = new ChangePackageList();
        ModificationList modPackages = null;
        FileDetailsList scriptFiles = null;
        FileDetailsList directorFiles = null;
        VFSInterface sessionVfs = null;
        int locale = 44;
        SortedDictionary<String, String> laserTypeAliases = new SortedDictionary<string, string>();
        SortedDictionary<String, String> missileTypeAliases = new SortedDictionary<string, string>();

        /// <summary>
        /// 
        /// </summary>
        internal Configuration()
        {
            enumerateFiles();
            textDatabase = new TextDictionary(this);
            bullets = new SpecBulletList(this);
            lasers = new SpecLaserList(this);
            missiles = new SpecMissileList(this);
            cockpits = new SpecCockpitList(this);
            ships = new SpecShipList(this);
            docks = new SpecDockList(this);
            factories = new SpecFactoryList(this);
            waresBio = new SpecWareBioList(this);
            waresEnergy = new SpecWareEnergyList(this);
            waresFood = new SpecWareFoodList(this);
            waresMineral = new SpecWareMineralList(this);
            waresNatural = new SpecWareNaturalList(this);
            waresTech = new SpecWareTechList(this);
            races = new CriteriaRaceList(this);
            configMARS = new GlobalMARS(this);
            modPackages = new ModificationList(this);

            foreach (String laserType in SpecLaser.typeStrings)
            {
                laserTypeAliases[laserType] = laserType;
            }

            foreach (String missileType in SpecMissile.typeStrings)
            {
                missileTypeAliases[missileType] = missileType;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void enumerateFiles()
        {
            scriptFiles = new FileDetailsList(this, "Scripts", "scripts", false);
            directorFiles = new FileDetailsList(this, "Missions", "director", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        private XmlElement generateTextFileRoot(XmlDocument doc)
        {
            DateTime saveTime = DateTime.Now;

            doc.AppendChild(doc.CreateXmlDeclaration("1.0", "utf-8", null));
            doc.AppendChild(doc.CreateComment(
                "Automatically Generated by Changeling v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() +
                " on " + saveTime.ToLongDateString() + " at " + saveTime.ToLongTimeString()));

            XmlElement root = doc.CreateElement("language");
            root.SetAttribute("id", locale.ToString());

            return root;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        internal void exportEES(string fileName)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement root = generateTextFileRoot(doc);

            XmlElement configPage = doc.CreateElement("page");
            configPage.SetAttribute("id", pageEESConfig.ToString());
            configPage.SetAttribute("title", "Weapon Characteristics");
            configPage.SetAttribute("descr", "Weapon Characteristics");

            for (Int32 Idx = 0; Idx < lasers.Count; ++Idx)
            {
                SpecLaser spec = lasers[Idx];

                XmlElement entry = doc.CreateElement("t");
                entry.SetAttribute("id", ((0 * maxWeaponIdx) + Idx+1).ToString());
                entry.InnerText = spec.characteristics.ToString();

                configPage.AppendChild(entry);
            }

            for (Int32 Idx = 0; Idx < missiles.Count; ++Idx)
            {
                SpecMissile spec = missiles[Idx];

                XmlElement entry = doc.CreateElement("t");
                entry.SetAttribute("id", ((10 * maxWeaponIdx) + Idx+1).ToString());
                entry.InnerText = spec.characteristics.ToString();

                configPage.AppendChild(entry);
            }

            root.AppendChild(configPage);

            for (Int32 raceIdx = 0; raceIdx < raceStrings.Length; ++raceIdx)
            {
                String currentRace = raceStrings[raceIdx];
                CriteriaRace priorities = races[currentRace];
                XmlElement page = doc.CreateElement("page");
                page.SetAttribute("id", (pageEESFirstRace + raceIdx).ToString());
                page.SetAttribute("title", currentRace + " Weapon priorities");
                page.SetAttribute("descr", "Weapon Priorities");

                for (Int32 Idx = 0; Idx < priorities.lasers.Count; ++Idx)
                {
                    CriteriaLaser crit = priorities.lasers[Idx];
                    XmlElement entry = doc.CreateElement("t");
                    entry.SetAttribute("id", ((0 * maxWeaponIdx) + (Idx + 1)).ToString());
                    entry.InnerText = crit.laserIndex.ToString();

                    page.AppendChild(entry);
                }

                for (Int32 Idx = 0; Idx < priorities.lasers.Count; ++Idx)
                {
                    CriteriaLaser crit = priorities.lasers[Idx];
                    XmlElement entry = doc.CreateElement("t");
                    entry.SetAttribute("id", ((1 * maxWeaponIdx) + (Idx + 1)).ToString());
                    entry.InnerText = crit.bias.ToString();

                    page.AppendChild(entry);
                }

                for (Int32 Idx = 0; Idx < priorities.missiles.Count; ++Idx)
                {
                    CriteriaMissile crit = priorities.missiles[Idx];
                    XmlElement entry = doc.CreateElement("t");
                    entry.SetAttribute("id", ((10 * maxWeaponIdx) + (Idx + 1)).ToString());
                    entry.InnerText = crit.missileIndex.ToString();

                    page.AppendChild(entry);
                }

                root.AppendChild(page);
            }

            doc.AppendChild(root);

            doc.Save(fileName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        private SpecLaser getImportLaser(Int32 idx)
        {
            SpecLaser spec = null;

            for (Int32 laserIdx = lasers.Count; laserIdx <= idx; laserIdx++)
            {
                spec = new SpecLaser(this);
                spec.name = "SS_LASER_CHANGELING_UNKNOWN_" + laserIdx.ToString();
                lasers.Add(spec);
            }
            spec = lasers[idx];

            return spec;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        private SpecMissile getImportMissile(Int32 idx)
        {
            SpecMissile spec = null;

            for (Int32 missileIdx = missiles.Count; missileIdx <= idx; missileIdx++)
            {
                spec = new SpecMissile(this);
                spec.name = "SS_MISSILE_CHANGELING_UNKNOWN_" + missileIdx.ToString();
                missiles.Add(spec);
            }
            spec = missiles[idx];

            return spec;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        internal void importMARS(string fileName)
        {
            dirty = true;
            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);

            foreach (XmlNode root in doc.ChildNodes)
            {
                if (root.Name.Equals("language"))
                {
                    foreach (XmlElement page in root.ChildNodes)
                    {
                        if (page.Name.Equals("page"))
                        {
                            XmlAttribute pageId = page.Attributes["id"];
                            Int32 pageIdx = -1;

                            if (Int32.TryParse(pageId.Value, out pageIdx))
                            {
                                switch (pageIdx)
                                {
                                case pageMARSConfig:
                                    configMARS = new GlobalMARS(this);
                                    configMARS.import(page);
                                    
                                    foreach (XmlElement xmlEntry in page.ChildNodes)
                                    {
                                        if (xmlEntry.Name.Equals("t"))
                                        {
                                            string idAttr=xmlEntry.GetAttribute("id");
                                            string valueText = xmlEntry.InnerText;

                                            if (idAttr != null)
                                            {
                                                Int32 id = -1;
                                                Int32 value = 0;

                                                if (Int32.TryParse(idAttr, out id) &&
                                                    Int32.TryParse(valueText, out value))
                                                {
                                                    if (id < 10000)
                                                    {
                                                        Int32 idx = id / 10;
                                                        Int32 item = id % 10;

                                                        SpecLaser spec = getImportLaser(idx);

                                                        switch (item)
                                                        {
                                                        case 3:
                                                            spec.aoeClass = value;
                                                            break;

                                                        case 4:
                                                            spec.minShipSize = value;
                                                            break;

                                                        case 5:
                                                            spec.maxShipSize = value;
                                                            break;

                                                        default:
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    break;

                                default:
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        internal void exportMARS(string fileName)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement root = generateTextFileRoot(doc);

            XmlElement configPage = doc.CreateElement("page");
            configPage.SetAttribute("id", pageMARSConfig.ToString());
            configPage.SetAttribute("title", "Weapon Characteristics");
            configPage.SetAttribute("descr", "Weapon Characteristics");

            configPage.AppendChild(TextDictionary.createTextEntry(doc, 6422101, "MARS"));
            configPage.AppendChild(TextDictionary.createTextEntry(doc, 6422102, "     MARS v4.95 (Changeling v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + ")"));

            configMARS.export(configPage);

            for (Int32 Idx = 0; Idx < lasers.Count; ++Idx)
            {
                SpecLaser spec = lasers[Idx];

                configPage.AppendChild(TextDictionary.createTextEntry(doc, (10 * Idx) + 0, spec.displayName));
                configPage.AppendChild(TextDictionary.createTextEntry(doc, (10 * Idx) + 1, spec.shieldDmgPerSecond.ToString()));
                configPage.AppendChild(TextDictionary.createTextEntry(doc, (10 * Idx) + 2, spec.hullDmgPerSecond.ToString()));
                configPage.AppendChild(TextDictionary.createTextEntry(doc, (10 * Idx) + 3, spec.aoeClass.ToString()));
                configPage.AppendChild(TextDictionary.createTextEntry(doc, (10 * Idx) + 4, spec.minShipSize.ToString()));
                configPage.AppendChild(TextDictionary.createTextEntry(doc, (10 * Idx) + 5, spec.maxShipSize.ToString()));
            }

            root.AppendChild(configPage);
            doc.AppendChild(root);

            doc.Save(fileName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        internal void importEES(string fileName)
        {
            dirty = true;
            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);

            foreach (XmlNode root in doc.ChildNodes)
            {
                if (root.Name.Equals("language"))
                {
                    foreach (XmlNode page in root.ChildNodes)
                    {
                        if (page.Name.Equals("page"))
                        {
                            XmlAttribute pageId = page.Attributes["id"];
                            Int32 pageIdx = -1;

                            if (Int32.TryParse(pageId.Value, out pageIdx))
                            {
                                switch (pageIdx)
                                {
                                case pageEESConfig:
                                    importEESCfgPage(page.ChildNodes);
                                    break;

                                default:
                                    Int32 raceIdx = pageIdx - pageEESFirstRace;

                                    if (0 <= raceIdx && raceIdx < raceStrings.Length)
                                    {
                                        importEESRacePage(page.ChildNodes, raceStrings[raceIdx]);
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entryList"></param>
        private void importEESCfgPage(XmlNodeList entryList)
        {
            NumericI minIdx = null;
            foreach (XmlNode entry in entryList)
            {
                if (entry.Name.Equals("t"))
                {
                    XmlAttribute entryId = entry.Attributes["id"];
                    String entryText = entry.InnerText;
                    Int32 entryIdx = -1;
                    Int32 entryValue = -1;

                    if (Int32.TryParse(entryId.Value, out entryIdx) &&
                        Int32.TryParse(entryText, out entryValue))
                    {
                        if (minIdx == null)
                        {
                            minIdx = entryIdx;
                        }

                        Int32 idx = (entryIdx - minIdx) % maxWeaponIdx;
                        Int32 type = (entryIdx - minIdx) / maxWeaponIdx;

                        switch (type)
                        {
                        case 0: // 1-1000 : Laser Characteristics
                            {
                                SpecLaser spec = getImportLaser(idx);
                                spec.characteristics = entryValue;
                            }
                            break;

                        case 10: // 10001-11000 : Missile Characteristics
                            {
                                SpecMissile spec = getImportMissile(idx);
                                spec.characteristics = entryValue;
                            }
                            break;

                        default:
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entryList"></param>
        /// <param name="raceName"></param>
        private void importEESRacePage(XmlNodeList entryList, String raceName)
        {
            CriteriaRace priorities=races[raceName];
            SortedDictionary<Int32, Int32> laserMap = new SortedDictionary<Int32, Int32>();

            priorities.lasers.Clear();
            priorities.missiles.Clear();

            NumericI minIdx = null;
            foreach (XmlNode entry in entryList)
            {
                if (entry.Name.Equals("t"))
                {
                    XmlAttribute entryId = entry.Attributes["id"];
                    String entryText = entry.InnerText;
                    Int32 entryIdx = -1;
                    Int32 entryValue = -1;

                    if (Int32.TryParse(entryId.Value, out entryIdx) &&
                        Int32.TryParse(entryText, out entryValue))
                    {
                        if (minIdx == null)
                        {
                            minIdx = entryIdx;
                        }

                        Int32 idx = (entryIdx - minIdx) % maxWeaponIdx;
                        Int32 type = (entryIdx - minIdx) / maxWeaponIdx;

                        switch (type)
                        {
                            case 0: // 1-1000 : Laser Preference
                                {
                                    CriteriaLaser crit = new CriteriaLaser(this);
                                    crit.laserIndex = entryValue;
                                    crit.bias = 0;
                                    priorities.lasers.Add(crit);
                                    laserMap.Add(idx, priorities.lasers.Count);

                                    SpecLaser spec = null;
                                    for (Int32 laserIdx = lasers.Count; laserIdx <= entryValue; laserIdx++)
                                    {
                                        spec = new SpecLaser(this);
                                        spec.name = "SS_LASER_EES_UNKNOWN_" + laserIdx.ToString();
                                        lasers.Add(spec);
                                    }
                                    spec = lasers[entryValue];
                                }
                                break;

                            case 1: // 1001-2000 : Laser Biases
                                {
                                    Int32 prioIdx = -1;

                                    if (laserMap.TryGetValue(idx, out prioIdx))
                                    {
                                        priorities.lasers[prioIdx - 1].bias = entryValue;
                                    }
                                }
                                break;

                            case 10: // 10001-11000 : Missile Priorities
                                {
                                    CriteriaMissile crit = new CriteriaMissile(this);
                                    crit.missileIndex = entryValue;
                                    priorities.missiles.Add(crit);

                                    SpecMissile spec = null;
                                    for (Int32 missileIdx = missiles.Count; missileIdx <= entryValue; missileIdx++)
                                    {
                                        spec = new SpecMissile(this);
                                        spec.name = "SS_MISSILE_EES_UNKNOWN_" + missileIdx.ToString();
                                        missiles.Add(spec);
                                    }
                                    spec = missiles[entryValue];
                                }
                                break;

                            default:
                                break;
                        }
                    }
                }
            }

            laserMap.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configListView"></param>
        internal void refresh(ListView configListView)
        {
            if (configListView.Tag != null)
            {
                IConfigDatumList list = configListView.Tag as IConfigDatumList;

                if (list != null)
                {
                    list.refresh(configListView);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configTreeView"></param>
        internal void refresh(TreeView configTreeView)
        {
            try
            {
                TreeNode aliasesRoot = configTreeView.Nodes["Aliases"];
                if (aliasesRoot != null)
                {
                    TreeNode missileTypeAliasesRoot = aliasesRoot.Nodes["Missile Mounts"];

                    if (missileTypeAliasesRoot != null)
                    {
                        foreach (KeyValuePair<String, String> missileTypeAlias in missileTypeAliases)
                        {
                            TreeNode missileTypeNode = missileTypeAliasesRoot.Nodes[missileTypeAlias.Key];

                            if (missileTypeNode == null)
                            {
                                missileTypeNode = new TreeNode(missileTypeAlias.Key);

                                missileTypeNode.ToolTipText = missileTypeAlias.Key;

                                missileTypeAliasesRoot.Nodes.Add(missileTypeNode);
                            }

                            missileTypeNode.Text = missileTypeAlias.Value;
                        }
                    }

                    TreeNode laserTypeAliasesRoot = aliasesRoot.Nodes["Laser Mounts"];

                    if (laserTypeAliasesRoot != null)
                    {
                        foreach (KeyValuePair<String, String> laserTypeAlias in laserTypeAliases)
                        {
                            TreeNode laserTypeNode = laserTypeAliasesRoot.Nodes[laserTypeAlias.Key];

                            if (laserTypeNode == null)
                            {
                                laserTypeNode = new TreeNode(laserTypeAlias.Key);

                                laserTypeNode.ToolTipText = laserTypeAlias.Key;

                                laserTypeAliasesRoot.Nodes.Add(laserTypeNode);
                            }

                            laserTypeNode.Text = laserTypeAlias.Value;
                        }
                    }
                }

                modPackages.refresh(configTreeView.Nodes);
                ships.refresh(configTreeView.Nodes);
                lasers.refresh(configTreeView.Nodes);
                missiles.refresh(configTreeView.Nodes);

                TreeNode stationsRoot = configTreeView.Nodes["Stations"];
                if (stationsRoot != null)
                {
                    docks.refresh(stationsRoot.Nodes);
                    factories.refresh(stationsRoot.Nodes);
                }

                TreeNode waresRoot = configTreeView.Nodes["Wares"];
                if (waresRoot != null)
                {
                    waresBio.refresh(waresRoot.Nodes);
                    waresEnergy.refresh(waresRoot.Nodes);
                    waresFood.refresh(waresRoot.Nodes);
                    waresMineral.refresh(waresRoot.Nodes);
                    waresNatural.refresh(waresRoot.Nodes);
                    waresTech.refresh(waresRoot.Nodes);
                }

                TreeNode gameResourceRoot = configTreeView.Nodes["Game Resources"];
                if (gameResourceRoot != null)
                {
                    if (sessionVfs != null)
                    {
                        gameResourceRoot.ToolTipText = sessionVfs.vfsRoot;
                    }

                    textDatabase.fileDetailsList.refresh(gameResourceRoot.Nodes);
                    scriptFiles.refresh(gameResourceRoot.Nodes);
                    directorFiles.refresh(gameResourceRoot.Nodes);
                    bullets.refresh(gameResourceRoot.Nodes);
                    cockpits.refresh(gameResourceRoot.Nodes);
                }

                TreeNodeCollection changePackageNodes = configTreeView.Nodes["Change Packages"].Nodes;

                TreeNodeCollection raceNodes = configTreeView.Nodes["Races"].Nodes;
                foreach (String currentRace in raceStrings)
                {
                    TreeNode currentNode = raceNodes[currentRace];
                    CriteriaRace priorities = races[currentRace];

                    currentNode.Tag = priorities;
                    priorities.lasers.refresh(currentNode.Nodes);
                    priorities.missiles.refresh(currentNode.Nodes);
                }

            }
            catch (Exception err)
            {
                MainForm.displayError(err);
            }
            configTreeView.Refresh();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="treeNodeCollection"></param>
        internal void refresh(System.Windows.Forms.TreeNodeCollection treeNodeCollection)
        {
            LinkedList<System.Windows.Forms.TreeNode> delNodes= new LinkedList<System.Windows.Forms.TreeNode>();
            foreach (System.Windows.Forms.TreeNode node in treeNodeCollection)
            {
                if (!refresh(node))
                {
                    delNodes.AddLast(node);
                }
            }

            foreach (System.Windows.Forms.TreeNode node in delNodes)
            {
                node.Remove();
            }
            delNodes.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        internal bool refresh(System.Windows.Forms.TreeNode node)
        {
            bool valid=true;

            if (node.Tag==null)
            {
                refresh(node.Nodes);
            }
            else if (typeof(IConfigDatumList).IsInstanceOfType(node.Tag))
            {
                IConfigDatumList datumList = (IConfigDatumList)node.Tag;

                datumList.refresh(node);

                refresh(node.Nodes);
            }
            else if (typeof(ConfigDatum).IsInstanceOfType(node.Tag))
            {
                ConfigDatum datum = (ConfigDatum)node.Tag;

                if (datum.isValid())
                {
                    node.Name = datum.generateName();
                    node.Text = datum.generateLabel();
                    node.ToolTipText = datum.generateTooltip();
                }
                else
                {
                    valid = false;
                }
            }
            else
            {
                refresh(node.Nodes);
            }

            return valid;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        internal bool load(string fileName)
        {
            bool isFileValid = false;
            XmlDocument doc = new XmlDocument();

            Stream dataStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            if (fileName.EndsWith(".ses"))
            {
                Ionic.Zlib.GZipStream inStream = new Ionic.Zlib.GZipStream(dataStream, Ionic.Zlib.CompressionMode.Decompress);

                dataStream = inStream;
            }
            doc.Load(dataStream);

            foreach (XmlNode docRoot in doc.ChildNodes)
            {
                switch (docRoot.Name)
                {
                case "Changeling":
                    isFileValid = true;
                    dirty = false;
                    sessionVfs = new VFSInterface(this, docRoot.Attributes["resourcePath"].Value);
                    sessionVfs.load(docRoot);
                    enumerateFiles();
                    textDatabase.loadGameData();
                    bullets.load(docRoot);
                    lasers.load(docRoot);
                    missiles.load(docRoot);
                    cockpits.load(docRoot);
                    ships.load(docRoot);
                    docks.load(docRoot);
                    factories.load(docRoot);
                    waresBio.load(docRoot);
                    waresEnergy.load(docRoot);
                    waresFood.load(docRoot);
                    waresMineral.load(docRoot);
                    waresNatural.load(docRoot);
                    waresTech.load(docRoot);
                    races.load(docRoot);
                    configMARS.load(docRoot);
                    break;

                default:
                    break;
                }
            }

            dataStream.Close();

            return isFileValid;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        internal void save(string fileName)
        {
            dirty = false;
            XmlDocument doc = new XmlDocument();
            DateTime saveTime=DateTime.Now;

            doc.AppendChild(doc.CreateXmlDeclaration("1.0", "utf-8", null));
            doc.AppendChild(doc.CreateComment(
                "Created by Changeling v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() +
                " on " + saveTime.ToLongDateString() + " at " + saveTime.ToLongTimeString()));

            XmlElement configRoot = doc.CreateElement("Changeling");
            configRoot.SetAttribute("resourcePath", sessionVfs.vfsRoot);

            sessionVfs.save(configRoot);
            bullets.save(configRoot);
            lasers.save(configRoot);
            missiles.save(configRoot);
            cockpits.save(configRoot);
            ships.save(configRoot);
            docks.save(configRoot);
            factories.save(configRoot);
            waresBio.save(configRoot);
            waresEnergy.save(configRoot);
            waresFood.save(configRoot);
            waresMineral.save(configRoot);
            waresNatural.save(configRoot);
            waresTech.save(configRoot);
            races.save(configRoot);
            configMARS.save(configRoot);

            doc.AppendChild(configRoot);

            Stream dataStream=new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);
            if (fileName.EndsWith(".ses"))
            {
                dataStream = new Ionic.Zlib.GZipStream(dataStream,
                    Ionic.Zlib.CompressionMode.Compress,
                    Ionic.Zlib.CompressionLevel.BestCompression);
            }
            doc.Save(dataStream);
            dataStream.Flush();
            dataStream.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="subjectIdx"></param>
        /// <param name="targetIdx"></param>
        internal void move(SpecLaser subject, int subjectIdx, int targetIdx)
        {
            dirty = true;
            if (lasers[subjectIdx] == subject)
            {
                lasers.RemoveAt(subjectIdx);
                lasers.Insert(targetIdx, subject);

                foreach (String raceName in raceStrings)
                {
                    CriteriaRace priorities = races[raceName];

                    bool moveDown = subjectIdx < targetIdx;
                    int startIdx = (!moveDown ? targetIdx : subjectIdx);
                    int endIdx = (moveDown ? targetIdx : subjectIdx);
                    
                    foreach (CriteriaLaser crit in priorities.lasers)
                    {
                        if (crit.laserIndex == subjectIdx)
                        {
                            crit.laserIndex = targetIdx;
                        }
                        else if (startIdx <= crit.laserIndex && crit.laserIndex <= endIdx)
                        {
                            if (moveDown)
                            {
                                crit.laserIndex--;
                            }
                            else
                            {
                                crit.laserIndex++;
                            }

                        }
                    }
                }
            }
            else
            {
                throw new Exception("INTERNAL ERROR : Laser Specification Mismatch during Move");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="subjectIdx"></param>
        /// <param name="targetIdx"></param>
        internal void move(SpecMissile subject, int subjectIdx, int targetIdx)
        {
            dirty = true;
            if (missiles[subjectIdx] == subject)
            {
                missiles.RemoveAt(subjectIdx);
                missiles.Insert(targetIdx, subject);

                foreach (String raceName in raceStrings)
                {
                    CriteriaRace priorities = races[raceName];

                    bool moveDown = subjectIdx < targetIdx;
                    int startIdx = (!moveDown ? targetIdx : subjectIdx);
                    int endIdx = (moveDown ? targetIdx : subjectIdx);

                    foreach (CriteriaMissile crit in priorities.missiles)
                    {
                        if (crit.missileIndex == subjectIdx)
                        {
                            crit.missileIndex = targetIdx;
                        }
                        else if (startIdx <= crit.missileIndex && crit.missileIndex <= endIdx)
                        {
                            if (moveDown)
                            {
                                crit.missileIndex--;
                            }
                            else
                            {
                                crit.missileIndex++;
                            }

                        }
                    }
                }
            }
            else
            {
                throw new Exception("INTERNAL ERROR : Missile Specification Mismatch during Move");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="raceName"></param>
        /// <param name="subjectIdx"></param>
        /// <param name="targetIdx"></param>
        internal void move(CriteriaLaser subject, string raceName, int subjectIdx, int targetIdx)
        {
            dirty = true;
            CriteriaRace priorities = races[raceName];

            if (priorities.lasers[subjectIdx] == subject)
            {
                priorities.lasers.RemoveAt(subjectIdx);
                priorities.lasers.Insert(targetIdx, subject);
            }
            else
            {
                throw new Exception("INTERNAL ERROR : Laser Criteria Mismatch during Move");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="raceName"></param>
        /// <param name="subjectIdx"></param>
        /// <param name="targetIdx"></param>
        internal void move(CriteriaMissile subject, string raceName, int subjectIdx, int targetIdx)
        {
            dirty = true;
            CriteriaRace priorities = races[raceName];

            if (priorities.missiles[subjectIdx] == subject)
            {
                priorities.missiles.RemoveAt(subjectIdx);
                priorities.missiles.Insert(targetIdx, subject);
            }
            else
            {
                throw new Exception("INTERNAL ERROR : Missile Criteria Mismatch during Move");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="subjectIdx"></param>
        internal void delete(SpecLaser subject, int subjectIdx)
        {
            dirty = true;
            if (lasers[subjectIdx] == subject)
            {
                lasers.RemoveAt(subjectIdx);

                foreach (String raceName in raceStrings)
                {
                    CriteriaRace priorities = races[raceName];

                    LinkedList<CriteriaLaser> delEntries = new LinkedList<CriteriaLaser>();

                    foreach (CriteriaLaser crit in priorities.lasers)
                    {
                        if (crit.laserIndex == subjectIdx)
                        {
                            delEntries.AddLast(crit);
                        }
                        else if (subjectIdx < crit.laserIndex)
                        {
                            crit.laserIndex--;
                        }
                    }

                    foreach (CriteriaLaser crit in delEntries)
                    {
                        priorities.lasers.Remove(crit);
                    }
                    delEntries.Clear();
                }
            }
            else
            {
                throw new Exception("INTERNAL ERROR : Laser Specification Mismatch during Delete");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="subjectIdx"></param>
        internal void delete(SpecMissile subject, int subjectIdx)
        {
            dirty = true;
            if (missiles[subjectIdx] == subject)
            {
                missiles.RemoveAt(subjectIdx);

                foreach (String raceName in raceStrings)
                {
                    CriteriaRace priorities = races[raceName];

                    LinkedList<CriteriaMissile> delEntries = new LinkedList<CriteriaMissile>();

                    foreach (CriteriaMissile crit in priorities.missiles)
                    {
                        if (crit.missileIndex == subjectIdx)
                        {
                            delEntries.AddLast(crit);
                        }
                        else if (subjectIdx < crit.missileIndex)
                        {
                            crit.missileIndex--;
                        }
                    }

                    foreach (CriteriaMissile crit in delEntries)
                    {
                        priorities.missiles.Remove(crit);
                    }
                    delEntries.Clear();
                }
            }
            else
            {
                throw new Exception("INTERNAL ERROR : Missile Specification Mismatch during Delete");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="raceName"></param>
        /// <param name="subjectIdx"></param>
        public void delete(CriteriaLaser subject, string raceName, int subjectIdx)
        {
            dirty = true;
            CriteriaRace priorities = races[raceName];

            if (priorities.lasers[subjectIdx] == subject)
            {
                priorities.lasers.RemoveAt(subjectIdx);
            }
            else
            {
                throw new Exception("INTERNAL ERROR : Laser Criteria Mismatch during Delete");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="raceName"></param>
        /// <param name="subjectIdx"></param>
        public void delete(CriteriaMissile subject, string raceName, int subjectIdx)
        {
            dirty = true;
            CriteriaRace priorities = races[raceName];

            if (priorities.missiles[subjectIdx] == subject)
            {
                priorities.missiles.RemoveAt(subjectIdx);
            }
            else
            {
                throw new Exception("INTERNAL ERROR : Missile Criteria Mismatch during Delete");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="targetIdx"></param>
        /// <returns></returns>
        public ListViewItem add(SpecLaser subject, int targetIdx)
        {
            dirty = true;
            lasers.Insert(targetIdx, subject);

            foreach (String raceName in raceStrings)
            {
                CriteriaRace priorities = races[raceName];

                foreach (CriteriaLaser crit in priorities.lasers)
                {
                    if (targetIdx <= crit.laserIndex)
                    {
                        crit.laserIndex++;
                    }
                }
            }

            return subject.newListViewItem();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="targetIdx"></param>
        /// <returns></returns>
        public ListViewItem add(SpecMissile subject, int targetIdx)
        {
            dirty = true;
            missiles.Insert(targetIdx, subject);

            foreach (String raceName in raceStrings)
            {
                CriteriaRace priorities = races[raceName];

                foreach (CriteriaMissile crit in priorities.missiles)
                {
                    if (targetIdx <= crit.missileIndex)
                    {
                        crit.missileIndex++;
                    }
                }
            }

            return subject.newListViewItem();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="specLaser"></param>
        /// <param name="subjectIdx"></param>
        /// <param name="targetRace"></param>
        /// <param name="targetIdx"></param>
        /// <returns></returns>
        public ListViewItem add(SpecLaser specLaser, int subjectIdx, string targetRace, int targetIdx)
        {
            CriteriaRace priorities=races[targetRace];
            bool alreadyPresent = false;

            foreach (CriteriaLaser crit in priorities.lasers)
            {
                if (crit.laserIndex == subjectIdx)
                {
                    alreadyPresent = true;
                }
            }

            if (!alreadyPresent)
            {
                dirty = true;
                CriteriaLaser crit = new CriteriaLaser(this);

                crit.laserIndex = subjectIdx;
                crit.bias = specLaser.defaultBias;

                priorities.lasers.Insert(targetIdx, crit);

                return crit.newListViewItem();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="specMissile"></param>
        /// <param name="subjectIdx"></param>
        /// <param name="targetRace"></param>
        /// <param name="targetIdx"></param>
        /// <returns></returns>
        public ListViewItem add(SpecMissile specMissile, int subjectIdx, string targetRace, int targetIdx)
        {
            CriteriaRace priorities=races[targetRace];
            bool alreadyPresent = false;

            foreach (CriteriaMissile crit in priorities.missiles)
            {
                if (crit.missileIndex == subjectIdx)
                {
                    alreadyPresent = true;
                }
            }

            if (!alreadyPresent)
            {
                dirty = true;
                CriteriaMissile crit = new CriteriaMissile(this);

                crit.missileIndex = subjectIdx;

                races[targetRace].missiles.Insert(targetIdx, crit);

                return crit.newListViewItem();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="crit"></param>
        /// <param name="raceName"></param>
        /// <param name="targetIdx"></param>
        public void insert(CriteriaLaser crit, string raceName, int targetIdx)
        {
            CriteriaRace priorities = races[raceName];

            if (!priorities.lasers.Contains(crit))
            {
                dirty = true;

                priorities.lasers.Insert(targetIdx, crit);
            }
            else
            {
                throw new Exception("INTERNAL ERROR : Laser Criteria duplication during Insert");
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="crit"></param>
        /// <param name="raceName"></param>
        /// <param name="targetIdx"></param>
        public void insert(CriteriaMissile crit, string raceName, int targetIdx)
        {
            CriteriaRace priorities = races[raceName];

            if (!priorities.missiles.Contains(crit))
            {
                dirty = true;

                priorities.missiles.Insert(targetIdx, crit);
            }
            else
            {
                throw new Exception("INTERNAL ERROR : Laser Criteria duplication during Insert");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="srcRaceName"></param>
        /// <param name="dstRaceName"></param>
        internal void cloneCriteria(String srcRaceName, String dstRaceName)
        {
            cloneCriteriaLasers(srcRaceName, dstRaceName);
            cloneCriteriaMissiles(srcRaceName, dstRaceName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="srcRaceName"></param>
        /// <param name="dstRaceName"></param>
        internal void cloneCriteriaMissiles(string srcRaceName, string dstRaceName)
        {
            if (!srcRaceName.Equals(dstRaceName))
            {
                dirty = true;
                CriteriaRace src = races[srcRaceName];
                CriteriaRace dst = races[dstRaceName];

                dst.missiles.Clear();
                foreach (CriteriaMissile crit in src.missiles)
                {
                    CriteriaMissile newCrit = new CriteriaMissile(this);

                    newCrit.missileIndex = crit.missileIndex;

                    dst.missiles.Add(newCrit);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="srcRaceName"></param>
        /// <param name="dstRaceName"></param>
        internal void cloneCriteriaLasers(string srcRaceName, string dstRaceName)
        {
            if (!srcRaceName.Equals(dstRaceName))
            {
                dirty = true;
                CriteriaRace src = races[srcRaceName];
                CriteriaRace dst = races[dstRaceName];

                dst.lasers.Clear();
                foreach (CriteriaLaser crit in src.lasers)
                {
                    CriteriaLaser newCrit = new CriteriaLaser(this);

                    newCrit.laserIndex = crit.laserIndex;
                    newCrit.bias = crit.bias;

                    dst.lasers.Add(newCrit);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal void markDirty()
        {
            dirty = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal bool isDirty()
        {
            return dirty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="missileIndex"></param>
        /// <returns></returns>
        public SpecMissile getMissile(int missileIndex)
        {
            if (0 <= missileIndex && missileIndex < missiles.Count)
            {
                return missiles[missileIndex];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spec"></param>
        /// <returns></returns>
        public int getIndexOf(SpecMissile spec)
        {
            return missiles.IndexOf(spec);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="laserIndex"></param>
        /// <returns></returns>
        public SpecLaser getLaser(int laserIndex)
        {
            if (0 <= laserIndex && laserIndex < lasers.Count)
            {
                return lasers[laserIndex];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spec"></param>
        /// <returns></returns>
        public int getIndexOf(SpecLaser spec)
        {
            return lasers.IndexOf(spec);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cockpitIndex"></param>
        /// <returns></returns>
        public SpecCockpit getCockpit(int cockpitIndex)
        {
            if (0 <= cockpitIndex && cockpitIndex < cockpits.Count)
            {
                return cockpits[cockpitIndex];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spec"></param>
        /// <returns></returns>
        public int getIndexOf(SpecCockpit spec)
        {
            return cockpits.IndexOf(spec);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ammoIndex"></param>
        /// <returns></returns>
        public SpecWareTech getWareTech(int ammoIndex)
        {
            if (0 <= ammoIndex && ammoIndex < waresTech.Count)
            {
                return waresTech[ammoIndex];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int getIndexOf(SpecWareTech value)
        {
            return waresTech.IndexOf(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="criteriaMissile"></param>
        /// <returns></returns>
        public bool validate(CriteriaMissile criteriaMissile)
        {
            bool isValid = false;

            foreach (CriteriaRace priority in races.Values)
            {
                if (priority.missiles.Contains(criteriaMissile))
                {
                    isValid = true;
                }
            }

            return isValid;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="specLaser"></param>
        /// <returns></returns>
        public bool validate(SpecLaser specLaser)
        {
            return lasers.Contains(specLaser);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="criteriaLaser"></param>
        /// <returns></returns>
        public bool validate(CriteriaLaser criteriaLaser)
        {
            bool isValid = false;

            foreach (CriteriaRace priority in races.Values)
            {
                if (priority.lasers.Contains(criteriaLaser))
                {
                    isValid = true;
                }
            }

            return isValid;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="changePackage"></param>
        /// <returns></returns>
        public bool validate(ChangePackage changePackage)
        {
            bool isValid = false;

            if (changePackages.ContainsValue(changePackage))
            {
            }

            return isValid;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="specMissile"></param>
        /// <returns></returns>
        public bool validate(SpecMissile specMissile)
        {
            return missiles.Contains(specMissile);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="specBullet"></param>
        /// <returns></returns>
        public bool validate(SpecBullet specBullet)
        {
            return bullets.Contains(specBullet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="specCockpit"></param>
        /// <returns></returns>
        public bool validate(SpecCockpit specCockpit)
        {
            bool isValid = false;

            if (cockpits.Contains(specCockpit))
            {
                isValid = true;
            }

            return isValid;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="specDock"></param>
        /// <returns></returns>
        public bool validate(SpecDock specDock)
        {
            bool isValid = false;

            if (docks.Contains(specDock))
            {
                isValid = true;
            }

            return isValid;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="specFactory"></param>
        /// <returns></returns>
        public bool validate(SpecFactory specFactory)
        {
            bool isValid = false;

            if (factories.Contains(specFactory))
            {
                isValid = true;
            }

            return isValid;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="specWareBio"></param>
        /// <returns></returns>
        public bool validate(SpecWareBio specWareBio)
        {
            bool isValid = false;

            if (waresBio.Contains(specWareBio))
            {
                isValid = true;
            }

            return isValid;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="specShip"></param>
        /// <returns></returns>
        public bool validate(SpecShip specShip)
        {
            bool isValid = false;

            if (ships.Contains(specShip))
            {
                isValid = true;
            }

            return isValid;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="specWareEnergy"></param>
        /// <returns></returns>
        public bool validate(SpecWareEnergy specWareEnergy)
        {
            bool isValid = false;

            if (waresEnergy.Contains(specWareEnergy))
            {
                isValid = true;
            }

            return isValid;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="specWareFood"></param>
        /// <returns></returns>
        public bool validate(SpecWareFood specWareFood)
        {
            bool isValid = false;

            if (waresFood.Contains(specWareFood))
            {
                isValid = true;
            }

            return isValid;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="specWareMineral"></param>
        /// <returns></returns>
        public bool validate(SpecWareMineral specWareMineral)
        {
            bool isValid = false;

            if (waresMineral.Contains(specWareMineral))
            {
                isValid = true;
            }

            return isValid;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="specWareNatural"></param>
        /// <returns></returns>
        public bool validate(SpecWareNatural specWareNatural)
        {
            bool isValid = false;

            if (waresNatural.Contains(specWareNatural))
            {
                isValid = true;
            }

            return isValid;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="specWareTech"></param>
        /// <returns></returns>
        public bool validate(SpecWareTech specWareTech)
        {
            bool isValid = false;

            if (waresTech.Contains(specWareTech))
            {
                isValid = true;
            }

            return isValid;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        public bool validate(Start start)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="globalMARS"></param>
        /// <returns></returns>
        public bool validate(GlobalMARS globalMARS)
        {
            return this.configMARS == globalMARS;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="changePackage"></param>
        /// <returns></returns>
        public string getPath(ChangePackage changePackage)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        internal TextEntryTranslated getDefinitions(int page, int id)
        {
            return textDatabase.getEntry(page, id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public string getText(int page, int id)
        {
            return textDatabase.getText(page, id, locale);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        internal virtual void importGameData(string path)
        {
            sessionVfs = new VFSInterface(this, path);
            enumerateFiles();
            textDatabase.loadGameData();
            bullets.loadGameData();
            lasers.loadGameData();
            missiles.loadGameData();
            cockpits.loadGameData();
            ships.loadGameData();
            docks.loadGameData();
            factories.loadGameData();
            waresBio.loadGameData();
            waresEnergy.loadGameData();
            waresFood.loadGameData();
            waresMineral.loadGameData();
            waresNatural.loadGameData();
            waresTech.loadGameData();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        internal void exportGameData(string path)
        {
            textDatabase.saveGameData(path);
            bullets.saveGameData(path);
            lasers.saveGameData(path);
            missiles.saveGameData(path);
            cockpits.saveGameData(path);
            ships.saveGameData(path);
            docks.saveGameData(path);
            factories.saveGameData(path);
            waresBio.saveGameData(path);
            waresEnergy.saveGameData(path);
            waresFood.saveGameData(path);
            waresMineral.saveGameData(path);
            waresNatural.saveGameData(path);
            waresTech.saveGameData(path);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bulletIndex"></param>
        /// <returns></returns>
        public SpecBullet getBullet(int bulletIndex)
        {
            if (0 <= bulletIndex && bulletIndex < bullets.Count)
            {
                return bullets[bulletIndex];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int getIndexOf(SpecBullet value)
        {
            return bullets.IndexOf(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="locale"></param>
        internal void setLocale(int locale)
        {
            this.locale = locale;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int getLocale()
        {
            return locale;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        internal string getLocalisedFileName(string fileName)
        {
            int locale = getLocale();
            if (locale >= 0)
            {
                fileName += "-L" + locale.ToString("D3");
            }
            return fileName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="destinationPath"></param>
        /// <returns></returns>
        public X2FD.Catalog getCatalog(string destinationPath)
        {
            FileInfo destinationInfo = new FileInfo(destinationPath);

            if ((destinationInfo.Attributes & FileAttributes.Directory) == 0)
            {
                // Update Existing CAT
                return X2FD.Catalog.Open(destinationPath, DS.X2Core.X2FD.CreateDisposition.createNew);
            }
            else
            {
                // Create New CAT
                return X2FD.Catalog.Open(destinationPath + "\\Changeling.cat", DS.X2Core.X2FD.CreateDisposition.createNew);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wareClass"></param>
        /// <returns></returns>
        public string getWareClassText(int wareClass)
        {
            return getText(1999, wareClass);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public Stream readFile(string fileName)
        {
            if (sessionVfs != null)
            {
                return sessionVfs.readFile(fileName);
            }
            else
            {
                throw new Exception("VFS Is Not Initialised");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="dataStream"></param>
        /// <returns></returns>
        public String writeFile(string fileName, Stream dataStream)
        {
            if (sessionVfs != null)
            {
                return sessionVfs.writeFile(fileName, dataStream);
            }
            else
            {
                throw new Exception("VFS Is Not Initialised");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public String revertFile(string fileName)
        {
            if (sessionVfs != null)
            {
                return sessionVfs.revertFile(fileName);
            }
            else
            {
                throw new Exception("VFS Is Not Initialised");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool isModified(string fileName)
        {
            if (sessionVfs != null)
            {
                return sessionVfs.isModified(fileName);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vfsLocation"></param>
        /// <param name="filter"></param>
        /// <param name="recurse"></param>
        /// <returns></returns>
        public virtual IEnumerable<FileDetails> findFiles(string vfsLocation, string filter, bool recurse)
        {
            if (sessionVfs != null)
            {
                return sessionVfs.findAll(vfsLocation, filter, recurse);
            }
            else
            {
                return new List<FileDetails>();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        internal void importMod(string fileName)
        {
            modPackages.Add(new Modification(this, fileName));
        }
    }
}
