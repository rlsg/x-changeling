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
    class SpecShip : SpecWare
    {
        internal class Cockpit
        {
            internal const String xmlName = "Cockpit";

            protected IConfig config;

            internal class Weapon
            {
                internal const String xmlName = "Weapon";

                internal NumericI gunCount = 0;
                internal String[] modelScene = { "0", "0" };
                internal NumericI[] modelNode = { -1, -1 };
            }

            internal class WeaponList : List<Weapon>
            {
                internal NumericI gunCount
                {
                    get
                    {
                        Int32 totalGuns = 0;

                        foreach (Weapon entry in this)
                        {
                            totalGuns += entry.gunCount;
                        }

                        return totalGuns;
                    }
                }
            }

            internal NumericI gunCount
            {
                get
                {
                    return cockpitWeapons.gunCount;
                }
            }

            internal SpecCockpit cockpit
            {
                get
                {
                    return config.getCockpit(cockpitIndex);
                }
                set
                {
                    cockpitIndex = config.getIndexOf(value);
                }
            }

            internal NumericI cockpitIndex = 0;
            internal NumericI cockpitPlacement = 0;
            internal String cockpitScene = "0";
            internal NumericI cockpitSceneNode = 0; 
            internal WeaponList cockpitWeapons = new WeaponList();

            internal Cockpit(IConfig config)
            {
                this.config = config;
            }
        }

        internal class CockpitList : List<Cockpit>
        {
            internal NumericI gunCount
            {
                get
                {
                    Int32 totalGuns = 0;

                    foreach (Cockpit entry in this)
                    {
                        totalGuns += entry.gunCount;
                    }

                    return totalGuns;
                }
            }
        }

        internal int maskLaserCompatable
        {
            get
            {
                int mask=maskLaser;

                foreach (Cockpit cockpit in turretCockpits)
                {
                    SpecCockpit specCockpit=cockpit.cockpit;

                    if (specCockpit!=null)
                    {
                        mask |= specCockpit.maskLaser;
                    }
                }

                return mask;
            }
        }

        internal List<String> mountLaserList
        {
            get
            {
                return SpecLaser.getTypes(maskLaser);
            }
            set
            {
                maskLaser = SpecLaser.getBitMask(value);
            }
        }

        internal List<String> mountLaserCompatableList
        {
            get
            {
                return SpecLaser.getTypes(maskLaserCompatable);
            }
        }

        internal List<String> mountMissileList
        {
            get
            {
                return SpecMissile.getTypes(maskMissile);
            }
            set
            {
                maskMissile = SpecMissile.getBitMask(value);
            }
        }

        internal Numeric gunCount
        {
            get
            {
                return mainCockpit.gunCount+turretCockpits.gunCount;
            }
        }

        internal const String xmlName = "SpecShip";

        protected override Numeric priceAverageScale { get { return 80.7212M; } }

        internal Cockpit mainCockpit = null;
        internal CockpitList turretCockpits = new CockpitList();
        internal NumericI speed = 0;
        internal NumericI accel = 0;
        internal NumericI engineSoundId = -1;
        internal NumericI averageDelay = 0;
        internal NumericI engineEffectColour = 0;
        internal NumericI engineGlowEffectId = -1;
        internal NumericI powerShield=0;
        internal NumericI engineVolumeMin = 0;
        internal NumericI engineVolumeMax = 0;
        internal String sceneModel = "0";
        internal String sceneCockpit = "0";
        internal NumericI maskLaser = 0;
        internal NumericI batteryLaser = 0;
        internal NumericFP batteryLaserRecharge = 0.0;
        internal NumericI maxShieldId = 0;
        internal NumericI maxShieldCount = 0;
        internal NumericI maskMissile = 0;
        internal NumericI npcMissileCount = 0;
        internal NumericI maxUpgradesSpeed = 0;
        internal NumericI maxUpgradesSteer = 0;
        internal NumericI cargoMin = 0;
        internal NumericI cargoMax = 0;
        internal NumericI builtinWareListId = -1;
        internal NumericI maxDockedShips = 0;
        internal NumericI maxWareClass = 0;
        internal NumericI raceId = 0;
        internal NumericI hullStrength = 0;
        internal NumericI explosionEffectId = -1;
        internal NumericI explosionDebrisId = -1;
        internal NumericI engineTrailId = -1;
        internal NumericI variationIdx = 0;
        internal NumericI accelRot=1;
        internal String shipObjectClass="OBJ";

        internal int cockpitCount
        {
            get
            {
                return turretCockpits.Count + 1;
            }
        }

        internal Cockpit getCockpit(Int32 idx)
        {
            if (idx>turretCockpits.Count)
            {
                throw new ArgumentOutOfRangeException();
            }
            else if (idx > 0)
            {
                return turretCockpits[idx - 1];
            }
            else
            {
                return mainCockpit;
            }
        }

        SpecShip(IConfig config) :
            base(config)
        {
            mainCockpit = new Cockpit(config);
        }

        public SpecShip(IConfig config, List<string> datLine) :
            base(config, datLine)
        {
            int idx = 7;

            try
            {
                speed = datLine[idx++];
                accel = datLine[idx++];
                engineSoundId = datLine[idx++];
                averageDelay = datLine[idx++];
                engineEffectColour = datLine[idx++];
                engineGlowEffectId = datLine[idx++];
                powerShield = datLine[idx++];
                engineVolumeMin = datLine[idx++];
                engineVolumeMax = datLine[idx++];
                sceneModel = datLine[idx++];
                sceneCockpit = datLine[idx++];
                maskLaser = datLine[idx++];
                NumericI shipLaserCount = datLine[idx++];
                batteryLaser = datLine[idx++];
                batteryLaserRecharge = datLine[idx++];
                maxShieldId = datLine[idx++];
                maxShieldCount = datLine[idx++];
                maskMissile = datLine[idx++];
                npcMissileCount = datLine[idx++];
                maxUpgradesSpeed = datLine[idx++];
                maxUpgradesSteer = datLine[idx++];
                cargoMin = datLine[idx++];
                cargoMax = datLine[idx++];
                builtinWareListId = datLine[idx++];

                mainCockpit = new Cockpit(config);
                bool undefinedTurret = false;
                for (int i = 0; i < 6; i++)
                {
                    NumericI cockpitIndex = datLine[idx++];
                    NumericI cockpitPlacement = datLine[idx++];

                    if (cockpitPlacement > 0)
                    {
                        if (undefinedTurret)
                        {
                            throw new Exception("Gap in turret definitions for " + name);
                        }

                        Cockpit newCockpit = new Cockpit(config);

                        newCockpit.cockpitIndex = cockpitIndex;
                        newCockpit.cockpitPlacement = cockpitPlacement;

                        turretCockpits.Add(newCockpit);
                    }
                    else
                    {
                        undefinedTurret = true;
                    }
                }

                maxDockedShips = datLine[idx++];
                maxWareClass = datLine[idx++];
                raceId = datLine[idx++];
                hullStrength = datLine[idx++];
                explosionEffectId = datLine[idx++];
                explosionDebrisId = datLine[idx++];
                engineTrailId = datLine[idx++];
                variationIdx = datLine[idx++];
                accelRot = datLine[idx++];
                shipObjectClass = datLine[idx++];

                NumericI cockpitCount = datLine[idx++];

                if (cockpitCount > (turretCockpits.Count + 1))
                {
                    throw new Exception("More cockpit than turret definitions for " + name);
                }

                for (int i = 0; i < cockpitCount; i++)
                {
                    NumericI turretNum = datLine[idx++];
                    NumericI turretIndex = datLine[idx++];

                    Cockpit cockpit = getCockpit(turretIndex);

                    cockpit.cockpitScene = datLine[idx++];
                    cockpit.cockpitSceneNode = datLine[idx++];
                }

                NumericI gunGroupCount = int.Parse(datLine[idx++]);

                if (gunGroupCount > (turretCockpits.Count + 1))
                {
                    throw new Exception("More gun group than turret definitions for " + name);
                }

                for (int i = 0; i < gunGroupCount; i++)
                {
                    NumericI initGunIndex = datLine[idx++];
                    NumericI groupLaserCount = datLine[idx++];
                    NumericI index = datLine[idx++];
                    NumericI weaponCount = datLine[idx++];

                    Cockpit cockpit = getCockpit(index-1);

                    for (int j = 0; j < weaponCount; j++)
                    {
                        NumericI gunIndex = datLine[idx++];

                        Cockpit.Weapon weapon = new Cockpit.Weapon();

                        weapon.gunCount = datLine[idx++];
                        weapon.modelScene[0] = datLine[idx++];
                        weapon.modelNode[0] = datLine[idx++];
                        weapon.modelScene[1] = datLine[idx++];
                        weapon.modelNode[1] = datLine[idx++];

                        cockpit.cockpitWeapons.Add(weapon);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal override List<String> getSpec()
        {
            List<String> rval = base.getSpec();

            List<String> subList = new List<String>();

            subList.AddRange(new String[] {
                speed,
                accel,
                engineSoundId,
                averageDelay,
                engineEffectColour,
                engineGlowEffectId,
                powerShield,
                engineVolumeMin,
                engineVolumeMax,
                sceneModel,
                sceneCockpit,
                maskLaser,
                gunCount,
                batteryLaser,
                batteryLaserRecharge,
                maxShieldId,
                maxShieldCount,
                maskMissile,
                npcMissileCount,
                maxUpgradesSpeed,
                maxUpgradesSteer,
                cargoMin,
                cargoMax,
                builtinWareListId
            });

            int undefinedTurretCount = 6 - turretCockpits.Count;
            if (undefinedTurretCount < 0)
            {
                throw new Exception("Too many turrets defined for " + name);
            }

            foreach (Cockpit cockpit in turretCockpits)
            {
                subList.AddRange(new String[] {
                    cockpit.cockpitIndex,
                    cockpit.cockpitPlacement
                });
            }

            for (int idx = 0; idx < undefinedTurretCount; idx++)
            {
                subList.AddRange(new String[] { "0", "0" });
            }

            subList.AddRange(new String[] {
                maxDockedShips,
                maxWareClass,
                raceId,
                hullStrength,
                explosionEffectId,
                explosionDebrisId,
                engineTrailId,
                variationIdx,
                accelRot,
                shipObjectClass
            });

            int cockpitCount = turretCockpits.Count + 1;

            subList.Add(cockpitCount.ToString());

            for (NumericI idx = 0; idx < cockpitCount; idx++)
            {
                Cockpit cockpit = getCockpit(idx);

                subList.AddRange(new String[] {
                    idx+1,
                    idx,
                    cockpit.cockpitScene,
                    cockpit.cockpitSceneNode
                });
            }

            subList.Add(cockpitCount.ToString());

            Numeric initGunIndex = 1;
            for (NumericI idx = 0; idx < cockpitCount; idx++)
            {
                Cockpit cockpit = getCockpit(idx);
                NumericI laserCount = cockpit.gunCount;
                NumericI weaponCount = cockpit.cockpitWeapons.Count;

                subList.AddRange(new String[] {
                    initGunIndex,
                    laserCount,
                    idx+1,
                    weaponCount
                });

                NumericI weaponIdx = 0;
                while (weaponIdx < weaponCount)
                {
                    Cockpit.Weapon weapon = cockpit.cockpitWeapons[weaponIdx++];

                    subList.AddRange(new String[] {
                        weaponIdx,
                        weapon.gunCount,
                        weapon.modelScene[0],
                        weapon.modelNode[0],
                        weapon.modelScene[1],
                        weapon.modelNode[1]
                    });
                }

                initGunIndex += laserCount;
            }

            rval.InsertRange(7, subList);

            return rval;
        }

        public SpecShip(IConfig config, XmlElement xmlNode) :
            base(config, xmlNode)
        {
            speed = getAttr(xmlNode, "speed", speed);
            accel = getAttr(xmlNode, "accel", accel);
            engineSoundId = getAttr(xmlNode, "engineSoundId", engineSoundId);
            averageDelay = getAttr(xmlNode, "averageDelay", averageDelay);
            engineEffectColour = getAttr(xmlNode, "engineEffectColour", engineEffectColour);
            engineGlowEffectId = getAttr(xmlNode, "engineGlowEffectId", engineGlowEffectId);
            powerShield = getAttr(xmlNode, "powerShield", powerShield);
            engineVolumeMin = getAttr(xmlNode, "engineVolumeMin", engineVolumeMin);
            engineVolumeMax = getAttr(xmlNode, "engineVolumeMax", engineVolumeMax);
            sceneModel = getAttr(xmlNode, "sceneModel", sceneModel);
            sceneCockpit = getAttr(xmlNode, "sceneCockpit", sceneCockpit);
            maskLaser = getAttr(xmlNode, "maskLaser", maskLaser);
            batteryLaser = getAttr(xmlNode, "batteryLaser", batteryLaser);
            batteryLaserRecharge = getAttr(xmlNode, "batteryLaserRecharge", batteryLaserRecharge);
            maxShieldId = getAttr(xmlNode, "maxShieldId", maxShieldId);
            maxShieldCount = getAttr(xmlNode, "maxShieldCount", maxShieldCount);
            maskMissile = getAttr(xmlNode, "maskMissile", maskMissile);
            npcMissileCount = getAttr(xmlNode, "npcMissileCount", npcMissileCount);
            maxUpgradesSpeed = getAttr(xmlNode, "maxUpgradesSpeed", maxUpgradesSpeed);
            maxUpgradesSteer = getAttr(xmlNode, "maxUpgradesSteer", maxUpgradesSteer);
            cargoMin = getAttr(xmlNode, "cargoMin", cargoMin);
            cargoMax = getAttr(xmlNode, "cargoMax", cargoMax);
            builtinWareListId = getAttr(xmlNode, "builtinWareListId", builtinWareListId);
            maxDockedShips = getAttr(xmlNode, "maxDockedShips", maxDockedShips);
            maxWareClass = getAttr(xmlNode, "maxWareClass", maxWareClass);
            raceId = getAttr(xmlNode, "raceId", raceId);
            hullStrength = getAttr(xmlNode, "hullStrength", hullStrength);
            explosionEffectId = getAttr(xmlNode, "explosionEffectId", explosionEffectId);
            explosionDebrisId = getAttr(xmlNode, "explosionDebrisId", explosionDebrisId);
            engineTrailId = getAttr(xmlNode, "engineTrailId", engineTrailId);
            variationIdx = getAttr(xmlNode, "variationIdx", variationIdx);
            accelRot = getAttr(xmlNode, "accelRot", accelRot);
            shipObjectClass = getAttr(xmlNode, "shipObjectClass", shipObjectClass);

            loadCockpits(xmlNode);
        }

        internal override XmlElement save(XmlDocument xmlDoc)
        {
            XmlElement specNode = xmlDoc.CreateElement(xmlName);

            base.save(specNode);

            specNode.SetAttribute("speed", speed);
            specNode.SetAttribute("accel", accel);
            specNode.SetAttribute("engineSoundId", engineSoundId);
            specNode.SetAttribute("averageDelay", averageDelay);
            specNode.SetAttribute("engineEffectColour", engineEffectColour);
            specNode.SetAttribute("engineGlowEffectId", engineGlowEffectId);
            specNode.SetAttribute("powerShield", powerShield);
            specNode.SetAttribute("engineVolumeMin", engineVolumeMin);
            specNode.SetAttribute("engineVolumeMax", engineVolumeMax);
            specNode.SetAttribute("sceneModel", sceneModel);
            specNode.SetAttribute("sceneCockpit", sceneCockpit);
            specNode.SetAttribute("maskLaser", maskLaser);
            specNode.SetAttribute("batteryLaser", batteryLaser);
            specNode.SetAttribute("batteryLaserRecharge", batteryLaserRecharge);
            specNode.SetAttribute("maxShieldId", maxShieldId);
            specNode.SetAttribute("maxShieldCount", maxShieldCount);
            specNode.SetAttribute("maskMissile", maskMissile);
            specNode.SetAttribute("npcMissileCount", npcMissileCount);
            specNode.SetAttribute("maxUpgradesSpeed", maxUpgradesSpeed);
            specNode.SetAttribute("maxUpgradesSteer", maxUpgradesSteer);
            specNode.SetAttribute("cargoMin", cargoMin);
            specNode.SetAttribute("cargoMax", cargoMax);
            specNode.SetAttribute("builtinWareListId", builtinWareListId);
            specNode.SetAttribute("maxDockedShips", maxDockedShips);
            specNode.SetAttribute("maxWareClass", maxWareClass);
            specNode.SetAttribute("raceId", raceId);
            specNode.SetAttribute("hullStrength", hullStrength);
            specNode.SetAttribute("explosionEffectId", explosionEffectId);
            specNode.SetAttribute("explosionDebrisId", explosionDebrisId);
            specNode.SetAttribute("engineTrailId", engineTrailId);
            specNode.SetAttribute("variationIdx", variationIdx);
            specNode.SetAttribute("accelRot", accelRot);
            specNode.SetAttribute("shipObjectClass", shipObjectClass);

            specNode.AppendChild(save(xmlDoc, mainCockpit));

            foreach (Cockpit cockpit in turretCockpits)
            {
                specNode.AppendChild(save(xmlDoc, cockpit));
            }

            return specNode;
        }

        private void loadCockpits(XmlElement xmlNode)
        {
            foreach (XmlNode cockpitNode in xmlNode.ChildNodes)
            {
                switch (cockpitNode.Name)
                {
                case Cockpit.xmlName:
                    {
                        Cockpit cockpit = new Cockpit(config);
                        if (mainCockpit == null)
                        {
                            mainCockpit = cockpit;
                        }
                        else
                        {
                            turretCockpits.Add(cockpit);

                            cockpit.cockpitIndex = getAttr(cockpitNode, "cockpitIndex", cockpit.cockpitIndex);
                            cockpit.cockpitPlacement = getAttr(cockpitNode, "cockpitPlacement", cockpit.cockpitPlacement);
                        }
                        cockpit.cockpitScene = getAttr(cockpitNode, "cockpitScene", cockpit.cockpitScene);
                        cockpit.cockpitSceneNode = getAttr(cockpitNode, "cockpitSceneNode", cockpit.cockpitSceneNode);

                        loadWeapons(cockpitNode, cockpit);
                    }
                    break;
                }
            }

            if (mainCockpit != null)
            {
                mainCockpit = new Cockpit(config);
            }
        }

        private XmlElement save(XmlDocument xmlDoc, Cockpit cockpit)
        {
            XmlElement cockpitNode = xmlDoc.CreateElement(Cockpit.xmlName);

            if (cockpit != mainCockpit)
            {
                cockpitNode.SetAttribute("cockpitIndex", cockpit.cockpitIndex);
                cockpitNode.SetAttribute("cockpitPlacement", cockpit.cockpitPlacement);
            }
            cockpitNode.SetAttribute("cockpitScene", cockpit.cockpitScene);
            cockpitNode.SetAttribute("cockpitSceneNode", cockpit.cockpitSceneNode);

            foreach(Cockpit.Weapon weapon in cockpit.cockpitWeapons)
            {
                cockpitNode.AppendChild(save(xmlDoc, weapon));
            }

            return cockpitNode;
        }

        private void loadWeapons(XmlNode cockpitNode, Cockpit cockpit)
        {
            foreach (XmlNode weaponNode in cockpitNode.ChildNodes)
            {
                switch (weaponNode.Name)
                {
                case Cockpit.Weapon.xmlName:
                    {
                        Cockpit.Weapon weapon = new Cockpit.Weapon();

                        weapon.gunCount = getAttr(weaponNode, "gunCount", weapon.gunCount);
                        weapon.modelScene[0] = getAttr(weaponNode, "modelScene0", weapon.modelScene[0]);
                        weapon.modelNode[0] = getAttr(weaponNode, "modelNode0", weapon.modelNode[0]);
                        weapon.modelScene[1] = getAttr(weaponNode, "modelScene1", weapon.modelScene[1]);
                        weapon.modelNode[1] = getAttr(weaponNode, "modelNode1", weapon.modelNode[1]);

                        cockpit.cockpitWeapons.Add(weapon);
                    }
                    break;
                }
            }
        }

        private XmlNode save(XmlDocument xmlDoc, Cockpit.Weapon weapon)
        {
            XmlElement weaponNode = xmlDoc.CreateElement(Cockpit.Weapon.xmlName);

            weaponNode.SetAttribute("gunCount", weapon.gunCount);
            weaponNode.SetAttribute("modelScene0", weapon.modelScene[0]);
            weaponNode.SetAttribute("modelNode0", weapon.modelNode[0]);
            weaponNode.SetAttribute("modelScene1", weapon.modelScene[1]);
            weaponNode.SetAttribute("modelNode1", weapon.modelNode[1]);

            return weaponNode;
        }

        public override bool isValid()
        {
            return config.validate(this);
        }

        public override string generateTooltip()
        {
            String tooltip =
                "Docking Capacity: " + maxDockedShips.ToString() +
                "\nBase Cargo Capacity: " + cargoMin.ToString() +
                "\nMaximum Cargo Capacity: " + cargoMax.ToString() +
                "\nMaximum Cargo Size: " + config.getWareClassText(maxWareClass);

            List<String> laserTypes = mountLaserCompatableList;
            tooltip += "\nCompatable Lasers:-";

            if (laserTypes.Count > 0)
            {
                foreach (String laserMount in laserTypes)
                {
                    tooltip += "\n    " + laserMount;
                }
            }
            else
            {
                tooltip += "\n    None";
            }

            List<String> missileTypes = mountMissileList;
            tooltip += "\nCompatable Missiles:-";

            if (missileTypes.Count > 0)
            {
                foreach (String missileMount in missileTypes)
                {
                    tooltip += "\n    " + missileMount;
                }
            }
            else
            {
                tooltip += "\n    None";
            }

            String descr = base.generateTooltip();

            if (descr != null)
            {
                tooltip += "\n" + descr;
            }

            return tooltip;
        }

        public override string generateLabel()
        {
            String label=base.generateLabel();

            if (label != null)
            {
                if (variationIdx > 0)
                {
                    String suffix=getText(10000 + variationIdx);

                    if (suffix != null)
                    {
                        label += " " + suffix;
                    }
                }
            }

            return label;
        }

        public override void generateSubItems(System.Windows.Forms.ListViewItem.ListViewSubItemCollection subItems)
        {
            base.generateSubItems(subItems);

            if (raceId > 0)
            {
                subItems.Add(config.getText(1266, raceId));
            }
            else
            {
                subItems.Add("");
            }

            subItems.Add(skinIndex.ToString());
            subItems.Add(sceneModel);
            subItems.Add(type);
            subItems.Add(shipObjectClass);
        }
    }
}
