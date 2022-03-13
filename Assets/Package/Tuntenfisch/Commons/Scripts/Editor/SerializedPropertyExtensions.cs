using System;
using System.Collections;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;

namespace Tuntenfisch.Commons.Editor
{
    public static class SerializedPropertyExtensions
    {
        #region Private Fields
        private const BindingFlags c_bindingFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
        #endregion

        #region Public Methods
        public static T GetValue<T>(this SerializedProperty property)
        {
            return (T)GetValue(property, SanitizePropertyPath(property.propertyPath));
        }

        public static void SetValue(this SerializedProperty property, object value)
        {
            string propertyPath = SanitizePropertyPath(property.propertyPath);
            string lastFieldName;

            if (propertyPath.Contains('.'))
            {
                lastFieldName = propertyPath.Substring(propertyPath.LastIndexOf('.') + 1);
                propertyPath = propertyPath.Substring(0, propertyPath.LastIndexOf('.'));
            }
            else
            {
                lastFieldName = propertyPath;
                propertyPath = string.Empty;
            }
            object obj = GetValue(property, propertyPath);

            if (lastFieldName.EndsWith(']'))
            {
                SetArrayValue(obj, lastFieldName, value);
            }
            else
            {
                SetValue(obj, lastFieldName, value);
            }
        }

        public static void InsertArrayElementAtIndex(this SerializedProperty property, int index, object element)
        {
            property.InsertArrayElementAtIndex(index);
            property.serializedObject.ApplyModifiedPropertiesWithoutUndo();
            SerializedProperty elementProperty = property.GetArrayElementAtIndex(index);
            elementProperty.SetValue(element);
        }

        public static void AppendArrayElement(this SerializedProperty property, object element)
        {
            property.arraySize++;
            property.serializedObject.ApplyModifiedPropertiesWithoutUndo();
            SerializedProperty elementProperty = property.GetArrayElementAtIndex(property.arraySize - 1);
            elementProperty.SetValue(element);
        }

        public static T GetAttribute<T>(this SerializedProperty property) where T : Attribute
        {
            FieldInfo fieldInfo = GetFieldInfo(property);

            if (fieldInfo == null)
            {
                return default;
            }
            return fieldInfo.GetCustomAttribute<T>();
        }

        public static FieldInfo GetFieldInfo(this SerializedProperty property)
        {
            FieldInfo fieldInfo = null;
            Type type = property.serializedObject.targetObject.GetType();

            foreach (string fieldName in property.propertyPath.Split('.'))
            {
                while (type != null)
                {
                    fieldInfo = type.GetField(fieldName, c_bindingFlags);

                    if (fieldInfo != null)
                    {
                        type = fieldInfo.FieldType;

                        break;
                    }
                    type = type.BaseType;
                }
            }
            return fieldInfo;
        }
        #endregion

        #region Private Methods
        private static object GetValue(SerializedProperty property, string sanitizedPropertyPath)
        {
            object obj = property.serializedObject.targetObject;

            if (string.IsNullOrEmpty(sanitizedPropertyPath))
            {
                return obj;
            }
            string[] fieldNames = sanitizedPropertyPath.Split('.');

            for (int fieldNameIndex = 0; fieldNameIndex < fieldNames.Length; fieldNameIndex++)
            {
                string fieldName = fieldNames[fieldNameIndex];

                if (fieldName.EndsWith(']'))
                {
                    obj = GetArrayValue(obj, fieldName);
                }
                else
                {
                    obj = GetValue(obj, fieldName);
                }
            }
            return obj;
        }


        private static string SanitizePropertyPath(string propertyPath)
        {
            return propertyPath.Replace(".Array.data[", "[");
        }

        private static object GetValue(object obj, string fieldName)
        {
            Type type = obj.GetType();
            FieldInfo field = type.GetField(fieldName, c_bindingFlags);
            return field.GetValue(obj);
        }

        private static object GetArrayValue(object obj, string fieldName)
        {
            Type type = obj.GetType();
            (string arrayFieldName, int index) = ExtractArrayFieldNameAndIndex(fieldName);
            FieldInfo field = type.GetField(arrayFieldName, c_bindingFlags);
            IEnumerable enumerable = field.GetValue(obj) as IEnumerable;
            IEnumerator enumerator = enumerable.GetEnumerator();
            do { enumerator.MoveNext(); } while (index-- > 0);
            return enumerator.Current;
        }

        private static void SetValue(object obj, string fieldName, object value)
        {
            obj.GetType().GetField(fieldName, c_bindingFlags).SetValue(obj, value);
        }

        private static void SetArrayValue(object obj, string fieldName, object value)
        {
            (string arrayFieldName, int index) = ExtractArrayFieldNameAndIndex(fieldName);
            FieldInfo field = obj.GetType().GetField(arrayFieldName, c_bindingFlags);
            IList list = field.GetValue(obj) as IList;
            list[index] = value;
        }

        private static (string, int) ExtractArrayFieldNameAndIndex(string fieldName)
        {
            Match match = Regex.Match(fieldName, @"(\w+)\[(\d+)\]");
            return (match.Groups[1].Value, int.Parse(match.Groups[2].Value));
        }
        #endregion
    }
}