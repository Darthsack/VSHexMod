using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Util;
using Vintagestory.Common;

namespace VSHexMod.hexcasting.api.casting.eval
{
    public enum QuickMishap
    {
        TypeMismatch,
        LowMedia,
        LeftHandEmpty,
        LeftHandInvalid,
        NotSaveable,

        ExecuteError,
        ExecuteNotFound,
        ExecuteInvalid,
        ExecuteListNotClosed,
        ExecuteOverload,

    } 

    public class Mishap
    {
        public string langcode;
        public string[] inputs; 

        public Mishap(string langcode = null, params string[] inputs)
        {
            langcode ??= "Mishap-Default";
            inputs ??= [];
            this.langcode = langcode;
            this.inputs = inputs;
        }
        public Mishap(QuickMishap type, params string[] inputs): this(null, inputs)
        {
            switch (type)
            {
                case QuickMishap.TypeMismatch:
                    langcode = "Mishap-TypeMismatch";
                    break;
                case QuickMishap.LowMedia:
                    langcode = "Mishap-LowMedia";
                    break;
                case QuickMishap.LeftHandEmpty:
                    langcode = "Mishap-LeftHandEmpty";
                    break;
                case QuickMishap.LeftHandInvalid:
                    langcode = "Mishap-LeftHandInvalid";
                    break;
                case QuickMishap.NotSaveable:
                    langcode = "Mishap-NotSaveable";
                    break;
                case QuickMishap.ExecuteError:
                    langcode = "Mishap-ExecuteError";
                    break;
                case QuickMishap.ExecuteNotFound:
                    langcode = "Mishap-ExecuteNotFound";
                    break;
                case QuickMishap.ExecuteInvalid:
                    langcode = "Mishap-ExecuteInvalid";
                    break;
                case QuickMishap.ExecuteListNotClosed:
                    langcode = "Mishap-ExecuteListNotClosed";
                    break;
                case QuickMishap.ExecuteOverload:
                    langcode = "Mishap-ExecuteOverload";
                    break;
               
                default:
                    break;
            }
        }


        public override string ToString()
        {
            string[] lang = [];
            
            inputs.Foreach((s) => 
                {
                    if (s.Contains('/'))
                    {
                        string[] n = s.Split('/');
                        string[] temp = [];
                        n.Foreach((st) =>
                        {
                            temp.AddItem(Lang.Get(st));
                        });
                        lang.AddItem(temp.Join(delimiter: "/"));

                    } 
                    else
                        lang.AddItem(Lang.Get(s));
                });

            return Lang.Get(langcode, lang);
        }
    }
}
