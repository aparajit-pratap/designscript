﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace ProtoCore.Lang
{
    public static class TraceUtils
    {
        private const string __TEMP_REVIT_TRACE_ID = "{0459D869-0C72-447F-96D8-08A7FB92214B}-REVIT";
        

        /// <summary>
        /// Get a list of the keys bound to trace elements
        /// This should be extracted from the attribute on the methods
        /// </summary>
        /// <returns></returns>
        public static List<String> TEMP_GetTraceKeys()
        {
            //TODO:Luke Extract this from RequiresTraceAttribute

            return new List<string>() { __TEMP_REVIT_TRACE_ID };
        }

        /// <summary>
        /// Get a map of TraceID -> Objects
        /// </summary>
        /// <returns></returns>
        public static Dictionary<String, Object> GetObjectFromTLS()
        {
            Dictionary<String, Object> objs = new Dictionary<String, Object>();

            foreach (String key in TEMP_GetTraceKeys())
            {
                objs.Add(key, 
                    Thread.GetData(Thread.GetNamedDataSlot(key)));
            }

            return objs;
        }

        /// <summary>
        /// Set the data associated with trace
        /// </summary>
        /// <param name="objs"></param>
        public static void SetObjectToTLS(Dictionary<String, Object> objs)
        {
            foreach (String k in objs.Keys)
                Thread.SetData(Thread.GetNamedDataSlot(k), objs[k]);
        }

    }
}
