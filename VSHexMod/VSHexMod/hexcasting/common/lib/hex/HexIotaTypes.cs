using Cairo;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.Common;
using VSHexMod.hexcasting.api.casting.eval.iota;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VSHexMod.hexcasting.common.lib.hex
{
    public class HexIotaTypes
    {
        public Dictionary<string, Iota> _REGISTRY;
        ICoreAPI Api;


        public HexIotaTypes(ICoreAPI _Api)
        {
            Api = _Api;
            _REGISTRY = new();
            registerType("null", new NullIota());
            registerType("double", new DoubleIota());
            registerType("boolean", new BoolIota());
            registerType("entity", new EntityIota());
            registerType("list", new ListIota());
            registerType("pattern", new PatternIota());
            registerType("vec3", new Vec3Iota());

        }

        public  NullIota NULL => type("null") as NullIota;
        public  DoubleIota DOUBLE => type("dobl") as DoubleIota;
        public  BoolIota BOOLEAN => type("bool") as BoolIota;
        public  EntityIota ENTITY => type("enty") as EntityIota;
        public  ListIota LIST => type("list") as ListIota;
        public  PatternIota PATTERN => type("patt") as PatternIota;
        public  Vec3Iota VEC3 => type("vec3").Copy() as Vec3Iota;

        public void registerType(string key, Iota type)
        {
            _REGISTRY.Add(key, type);
        }

        public string getKey(Iota type)
        {
            var n = _REGISTRY.First((x)=>Iota.typesMatch(x.Value, type));
            return n.Key;
        }

        public Iota type(string name)
        {
            return _REGISTRY[name];
        }
    }
}
