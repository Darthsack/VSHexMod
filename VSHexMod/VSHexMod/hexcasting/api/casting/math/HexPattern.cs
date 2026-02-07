using Cairo;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.GameContent;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VSHexMod.hexcasting.api.casting.math
{
    public class HexPattern
    {
        private HexDir startD;
        public HexDir startDir
        {
            get { return new HexDir(startD.Direction);}
        }
        public HexAngle[] angles;
        public HexPattern(HexDir _startDir,HexAngle[] _angles = null) 
        {
            startD = _startDir;
            if (_angles == null) angles = new HexAngle[0];
            else angles = _angles;
        }

        public bool tryAppendDir(HexDir newDir)
        {
            Dictionary<string, string[]> linesSeen = new Dictionary<string, string[]>();

            HexDir compass = this.startDir;
            HexCoord cursor = HexCoord.Origin;
            
            foreach (HexAngle a in this.angles)
            {
                if (!linesSeen.ContainsKey(cursor.ToString()))
                    linesSeen.Add(cursor.ToString(), new string[] { compass.ToString() });
                else linesSeen[cursor.ToString()] = linesSeen[cursor.ToString()].AddToArray(compass.ToString());
                if (!linesSeen.ContainsKey((cursor + compass).ToString()))
                    linesSeen.Add((cursor + compass).ToString(), new string[] { (compass * HexAngle.BACK).ToString() });
                else linesSeen[(cursor + compass).ToString()] = linesSeen[(cursor + compass).ToString()].AddToArray((compass * HexAngle.BACK).ToString());



                cursor += compass;
                compass *= a;

               
            }
            cursor += compass;

            

            //potentialNewLine = (cursor, compass);


            if (linesSeen.ContainsKey(cursor.ToString()) && linesSeen[cursor.ToString()].Contains(newDir.ToString()))
                return false;

            HexAngle nextAngle = newDir - compass;
            if (nextAngle == HexAngle.BACK)
                return false;

            this.angles = this.angles.Append(nextAngle).ToArray();
            return true;
        }

        public HexCoord[] positions(HexCoord start = null)
        {
            if (start == null)
                start = HexCoord.Origin;

            HexCoord[] output= new HexCoord[0];
            output = output.Append(start).ToArray();
            HexDir compass = new HexDir (this.startDir.Direction);
            HexCoord cursor = start;
            foreach (HexAngle a in this.angles) 
            {
                cursor += compass;
                output = output.Append(cursor).ToArray();
                compass *= a;
            }
            output = output.Append(cursor+compass).ToArray();
            return output;
        }

        public HexDir[] directions()
        {
            HexDir[] output = new HexDir[0];
            output = output.Append(startDir).ToArray();

            HexDir compass = this.startDir;
            foreach (HexAngle a in this.angles)
            {
                compass *= a;
                output = output.Append(compass).ToArray();
            }
            return output;  
        }

        public HexDir finalDir()
        {
            HexDir compass = this.startDir;
            foreach (HexAngle a in this.angles)
            {
                compass *= a;
            }
            return compass;
        }

        public string anglesSignature()
        {
            string output = "";
            foreach (HexAngle a in this.angles)
            {
                if (a == HexAngle.FORWARD)
                {
                    output = new string(output.Append('w').ToArray());
                }
                else if (a == HexAngle.RIGHT)
                {
                    output = new string(output.Append('e').ToArray());
                }
                else if (a == HexAngle.RIGHT_BACK)
                {
                    output = new string(output.Append('d').ToArray());
                }
                else if (a == HexAngle.BACK)
                {
                    output = new string(output.Append('s').ToArray());
                }
                else if (a == HexAngle.LEFT_BACK)
                {
                    output = new string(output.Append('a').ToArray());
                }
                else if (a == HexAngle.LEFT)
                {
                    output = new string(output.Append('q').ToArray());
                }
            }
            return output;
        }

        public string toString()
        {
            return "HexPattern[" + startDir.toString() + "," + anglesSignature() + "]";
        }

        public override string ToString()
        {
            return "HexPattern[" + startDir.toString() + "," + anglesSignature() + "]";
        }

        public static HexPattern fromAngles(string signature, HexDir _startDir)
        {
            HexPattern output = new HexPattern(_startDir);
            HexDir compass = new HexDir(_startDir.Direction);

            foreach (char c in signature) 
            {
                //capi.Logger.Event("Adding " + c +" to pattern");
                HexAngle angle = null;
                switch (c)
                {
                    case 'w':
                        angle = HexAngle.FORWARD;
                        break;
                    case 'e':
                        angle = HexAngle.RIGHT;
                        break;
                    case 'd':
                        angle = HexAngle.RIGHT_BACK;
                        break;
                    case 's':
                        angle = HexAngle.BACK;
                        break;
                    case 'a':
                        angle = HexAngle.LEFT_BACK;
                        break;
                    case 'q':
                        angle = HexAngle.LEFT;
                        break;
                }
                compass *= angle; 
                bool success = output.tryAppendDir(compass);

                //capi.Logger.Event("Sig: "+ output.toString()+ " "+compass.Direction.ToString() +" "+ angle.Angle.ToString() + " " + (success ? "worked" : "Failed"));
                if (!success)
                   return null;

            }
            //capi.Logger.Event("outputing: " + output.anglesSignature());
            return output;
        }

        public void ToBytes(BinaryWriter writer)
        {
            writer.Write((byte)startDir.Direction);
            writer.Write7BitEncodedInt(angles.Length);
            for (int i = 0; i < angles.Length; i += 2)
            {
                EnumHexAngle angle1 = angles[i].Angle;
                EnumHexAngle angle2 = angles.Length <= i + 1 ? 0 : angles[i + 1].Angle;

                byte writen = (byte)(((byte)angle1 << 4) | (byte)angle2);

                writer.Write(writen);
            }
        }

        public void FromBytes(BinaryReader reader, IWorldAccessor resolver)
        {
            HexDir startDir = new HexDir((EnumHexDir)reader.ReadByte());
            HexPattern pattern = new HexPattern(startDir, new HexAngle[reader.Read7BitEncodedInt()]);

            for (int i = 0; i < pattern.angles.Length; i += 2)
            {
                byte Currentbyte = reader.ReadByte();
                pattern.angles[i] = new HexAngle((EnumHexAngle)(Currentbyte >> 4));
                if (pattern.angles.Length > i + 1)
                {
                    pattern.angles[i + 1] = new HexAngle((EnumHexAngle)(Currentbyte & 15));
                }
            }
            startD = pattern.startDir;
            angles = pattern.angles;
        }

    }

    internal class Pair<T, Y>
    {
        public T first;
        public Y second;
        public Pair (T _A, Y _B)
        {
            first = _A;
            second = _B; 
        }
    }
}
