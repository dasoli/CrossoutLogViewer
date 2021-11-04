using System;
using System.Collections.Generic;
using System.Reflection;

namespace CrossoutLogView.Database.Reflection
{
    public class VariableInfo
    {
        private static readonly Dictionary<Guid, VariableInfo[]> generatedVarInfos =
            new Dictionary<Guid, VariableInfo[]>();

        public readonly Func<object, object> GetValue;
        public readonly string Name;
        public readonly Action<object, object> SetValue;
        public readonly Type VariableType;

        public VariableInfo(string name, Type variableType, Func<object, object> getValue,
            Action<object, object> setValue)
        {
            Name = name;
            VariableType = variableType;
            GetValue = getValue;
            SetValue = setValue;
        }

        public static VariableInfo FromFieldInfo(FieldInfo info)
        {
            return new VariableInfo(info.Name, info.FieldType, info.GetValue, info.SetValue);
        }

        public static VariableInfo FromPropertyInfo(PropertyInfo info)
        {
            return new VariableInfo(info.Name, info.PropertyType, info.GetValue, info.SetValue);
        }

        public static VariableInfo[] FromType(Type type, bool includeReadonly = false)
        {
            if (!generatedVarInfos.TryGetValue(type.GUID, out var vars))
            {
                var temp = new List<VariableInfo>();
                foreach (var fi in type.GetFields(BindingFlags.Public | BindingFlags.Instance))
                    temp.Add(FromFieldInfo(fi));
                foreach (var pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                    if (pi.CanWrite || includeReadonly)
                        temp.Add(FromPropertyInfo(pi));
                vars = temp.ToArray();
                generatedVarInfos.Add(type.GUID, vars);
            }

            return vars;
        }

        public override string ToString()
        {
            return typeof(VariableInfo).Name + " " + Name + " " + VariableType;
        }
    }
}