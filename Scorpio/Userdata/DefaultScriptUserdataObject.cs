﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Scorpio;
using Scorpio.Variable;
using Scorpio.Exception;
using Scorpio.Compiler;

namespace Scorpio.Userdata
{
    /// <summary> 普通Object类型 </summary>
    public class DefaultScriptUserdataObject : ScriptUserdata
    {
        private UserdataType m_Type;
        public DefaultScriptUserdataObject(Script script, object value, UserdataType type) : base(script)
        {
            this.Value = value;
            this.ValueType = value.GetType();
            this.m_Type = type;
        }
        public override ScriptObject GetValue(object key)
        {
            if (!(key is string)) throw new ExecutionException(Script, "Object GetValue只支持String类型");
            return Script.CreateObject(m_Type.GetValue(Value, (string)key));
        }
        public override void SetValue(object key, ScriptObject value)
        {
            if (!(key is string)) throw new ExecutionException(Script, "Object SetValue只支持String类型");
            m_Type.SetValue(Value, (string)key, value);
        }
        public override ScriptObject Compute(TokenType type, ScriptObject obj)
        {
            UserdataMethod method = m_Type.GetComputeMethod(type);
            if (method == null) throw new ExecutionException(Script, "找不到运算符重载 " + type);
            return Script.CreateObject (method.Call(null, new ScriptObject[] { this, obj }));
        }
    }
}
