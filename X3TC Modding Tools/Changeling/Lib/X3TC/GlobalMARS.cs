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
    class GlobalMARS : ConfigDatum
    {
        internal const string xmlNodeName = "GlobalMARS";

        public Int32 safetyRangeAoE = 3800;             // AoE Laser Safety Range

        public Boolean captureMode = true;              // Capture Mode: Enable
        public Int32 capturePause = 7;                  // Capture Mode: Pause Duration (s)
        public Boolean captureSmooth = true;            // Capture Mode: Smooth Laser

        public Int32 turretBurst = 2;                   // Turret Burst Time (s)
        public Int32 turretInterval = 0;                // Salvo Interval (ms)
        public Int32 turretCeaseFire = 120;             // Cease Fire (s)

        public Int32 maxHitEnvelope = 300;              // Maximum Hit Envelope

        public Boolean weaponSwitchLittle = true;       // Enable Weapon Switching (Small Ships)
        public Boolean weaponSwitchBig = true;          // Enable Weapon Switching (Big Ships)

        public Boolean targetLeadPrediction = false;    // Target Lead Prediction (experimental)

        public Int32 priorityAttack = 20;               // Attack Target Priority Bonus
        public Int32 priorityLeader = 25;               // Wing Leader Target Priority Bonus
        public Int32 priorityPlayer = 30;               // Player Target Priority Bonus
        public Int32 priorityHull = 50;                 // Hull Damage Priority Bonus

        public Int32 thresholdDefence = 4;              // Defence - Missile Damage Threshold Divisor
        public Int32 thresholdOffence = 4;              // Offense - Missile Damage Threshold Divisor

        public Boolean selfServicePopup = true;         // Self-Service

        public Int32 repairDronePercent = 66;           // Drone Repair Factor (%)

        public Int32 goblinZigZag = 300;                // Goblin Zig-Zag m per km
        public Int32 goblinCountAll = 4;                // Maximum Goblins for all ships
        public Int32 goblinCountHuge = 6;               // Maximum Goblins for Huge Ship
        public Int32 goblinDistance = 2000;             // Goblin distance (m)
        public Int32 goblinPauseRTB = 600;              // Goblin pause on RTB (s)
        public Boolean goblinAttackLittle = true;       // Goblin attack Little ships
        public Boolean goblinAttackBig = true;          // Goblin attack Big Ships
        public Boolean goblinAttackHuge = true;         // Goblin attack Huge Ships
        public Boolean goblinReceiveTransport = true;   // Receive Goblin cargo (TL/TP/TS)
        public Boolean goblinReceiveFighter = true;     // Receive Goblin cargo (M3/M4/M5)
        public Boolean goblinReceiveCapital = true;     // Receive Goblin cargo (M1/M2/M6/M7)
        public Int32 goblinFlightTime = 60;             // Goblin Flight Time (s)
        public Int32 goblinRange = 20000;               // Goblin Range (m)

        internal GlobalMARS(IConfig config) :
            base(config)
        {
        }

        public void export(XmlNode configPage)
        {
            XmlDocument doc = configPage.OwnerDocument;

            configPage.AppendChild(TextDictionary.createTextEntry(doc, 20131, safetyRangeAoE));
            configPage.AppendChild(TextDictionary.createTextEntry(doc, 20141, capturePause));
            configPage.AppendChild(TextDictionary.createTextEntry(doc, 20151, turretBurst));
            configPage.AppendChild(TextDictionary.createTextEntry(doc, 20161, captureMode));
            configPage.AppendChild(TextDictionary.createTextEntry(doc, 20191, priorityAttack));
            configPage.AppendChild(TextDictionary.createTextEntry(doc, 20201, priorityLeader));
            configPage.AppendChild(TextDictionary.createTextEntry(doc, 20211, priorityPlayer));
            configPage.AppendChild(TextDictionary.createTextEntry(doc, 20221, maxHitEnvelope));
            configPage.AppendChild(TextDictionary.createTextEntry(doc, 20231, priorityHull));
            configPage.AppendChild(TextDictionary.createTextEntry(doc, 20241, goblinZigZag));
            configPage.AppendChild(TextDictionary.createTextEntry(doc, 20251, goblinCountAll));
            configPage.AppendChild(TextDictionary.createTextEntry(doc, 20261, goblinCountHuge));
            configPage.AppendChild(TextDictionary.createTextEntry(doc, 20271, goblinDistance));
            configPage.AppendChild(TextDictionary.createTextEntry(doc, 20291, goblinPauseRTB));
            configPage.AppendChild(TextDictionary.createTextEntry(doc, 20301, goblinAttackLittle));
            configPage.AppendChild(TextDictionary.createTextEntry(doc, 20311, goblinAttackBig));
            configPage.AppendChild(TextDictionary.createTextEntry(doc, 20321, goblinAttackHuge));
            configPage.AppendChild(TextDictionary.createTextEntry(doc, 20331, goblinReceiveTransport));
            configPage.AppendChild(TextDictionary.createTextEntry(doc, 20341, goblinReceiveFighter));
            configPage.AppendChild(TextDictionary.createTextEntry(doc, 20351, goblinReceiveCapital));
            configPage.AppendChild(TextDictionary.createTextEntry(doc, 20361, targetLeadPrediction));
            configPage.AppendChild(TextDictionary.createTextEntry(doc, 20371, turretCeaseFire));
            configPage.AppendChild(TextDictionary.createTextEntry(doc, 20381, weaponSwitchLittle));
            configPage.AppendChild(TextDictionary.createTextEntry(doc, 20391, thresholdOffence));
            configPage.AppendChild(TextDictionary.createTextEntry(doc, 20401, thresholdDefence));
            configPage.AppendChild(TextDictionary.createTextEntry(doc, 20461, captureSmooth));
            configPage.AppendChild(TextDictionary.createTextEntry(doc, 20471, turretInterval));
            configPage.AppendChild(TextDictionary.createTextEntry(doc, 20481, goblinFlightTime));
            configPage.AppendChild(TextDictionary.createTextEntry(doc, 20491, selfServicePopup));
            configPage.AppendChild(TextDictionary.createTextEntry(doc, 20801, goblinRange));
            configPage.AppendChild(TextDictionary.createTextEntry(doc, 20811, weaponSwitchBig));
            configPage.AppendChild(TextDictionary.createTextEntry(doc, 21091, repairDronePercent));
        }

        public void import(XmlNode configPage)
        {
            foreach(XmlNode node in configPage.ChildNodes)
            {
                Int32 id = TextDictionary.getTextEntryId(node);

                switch (id)
                {
                case 20131:
                    TextDictionary.getTextEntryValue(node, ref safetyRangeAoE);
                    break;

                case 20141:
                    TextDictionary.getTextEntryValue(node, ref capturePause);
                    break;

                case 20151:
                    TextDictionary.getTextEntryValue(node, ref turretBurst);
                    break;

                case 20161:
                    TextDictionary.getTextEntryValue(node, ref captureMode);
                    break;
                case 20191:
                    TextDictionary.getTextEntryValue(node, ref priorityAttack);
                    break;

                case 20201:
                    TextDictionary.getTextEntryValue(node, ref priorityLeader);
                    break;

                case 20211:
                    TextDictionary.getTextEntryValue(node, ref priorityPlayer);
                    break;

                case 20221:
                    TextDictionary.getTextEntryValue(node, ref maxHitEnvelope);
                    break;

                case 20231:
                    TextDictionary.getTextEntryValue(node, ref priorityHull);
                    break;

                case 20241:
                    TextDictionary.getTextEntryValue(node, ref goblinZigZag);
                    break;

                case 20251:
                    TextDictionary.getTextEntryValue(node, ref goblinCountAll);
                    break;

                case 20261:
                    TextDictionary.getTextEntryValue(node, ref goblinCountHuge);
                    break;

                case 20271:
                    TextDictionary.getTextEntryValue(node, ref goblinDistance);
                    break;

                case 20291:
                    TextDictionary.getTextEntryValue(node, ref goblinPauseRTB);
                    break;

                case 20301:
                    TextDictionary.getTextEntryValue(node, ref goblinAttackLittle);
                    break;

                case 20311:
                    TextDictionary.getTextEntryValue(node, ref goblinAttackBig);
                    break;

                case 20321:
                    TextDictionary.getTextEntryValue(node, ref goblinAttackHuge);
                    break;

                case 20331:
                    TextDictionary.getTextEntryValue(node, ref goblinReceiveTransport);
                    break;

                case 20341:
                    TextDictionary.getTextEntryValue(node, ref goblinReceiveFighter);
                    break;

                case 20351:
                    TextDictionary.getTextEntryValue(node, ref goblinReceiveCapital);
                    break;

                case 20361:
                    TextDictionary.getTextEntryValue(node, ref targetLeadPrediction);
                    break;

                case 20371:
                    TextDictionary.getTextEntryValue(node, ref turretCeaseFire);
                    break;

                case 20381:
                    TextDictionary.getTextEntryValue(node, ref weaponSwitchLittle);
                    break;

                case 20391:
                    TextDictionary.getTextEntryValue(node, ref thresholdOffence);
                    break;

                case 20401:
                    TextDictionary.getTextEntryValue(node, ref thresholdDefence);
                    break;

                case 20461:
                    TextDictionary.getTextEntryValue(node, ref captureSmooth);
                    break;

                case 20471:
                    TextDictionary.getTextEntryValue(node, ref turretInterval);
                    break;

                case 20481:
                    TextDictionary.getTextEntryValue(node, ref goblinFlightTime);
                    break;

                case 20491:
                    TextDictionary.getTextEntryValue(node, ref selfServicePopup);
                    break;

                case 20801:
                    TextDictionary.getTextEntryValue(node, ref goblinRange);
                    break;

                case 20811:
                    TextDictionary.getTextEntryValue(node, ref weaponSwitchBig);
                    break;

                case 21091:
                    TextDictionary.getTextEntryValue(node, ref repairDronePercent);
                    break;
                }
            }
        }

        public override bool isValid()
        {
            return config.validate(this);
        }

        public override string generateName()
        {
            return "MARS";
        }

        public override string generateLabel()
        {
            return "MARS";
        }

        public override string generateTooltip()
        {
            return null;
        }

        internal override XmlElement save(XmlDocument xmlDoc)
        {
            XmlElement xmlNode=xmlDoc.CreateElement(xmlNodeName);

            export(xmlNode);

            return xmlNode;
        }

        internal void save(XmlElement configRoot)
        {
            configRoot.AppendChild(save(configRoot.OwnerDocument));
        }

        internal void load(XmlNode docRoot)
        {
            foreach (XmlNode xmlNode in docRoot.ChildNodes)
            {
                if (xmlNode.Name.Equals(xmlNodeName))
                {
                    import(xmlNode);
                }
            }
        }
    }
}
