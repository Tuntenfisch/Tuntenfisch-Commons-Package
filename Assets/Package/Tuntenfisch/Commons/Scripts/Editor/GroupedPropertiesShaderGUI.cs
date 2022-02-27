using System.Collections.Generic;
using UnityEditor;

namespace Tuntenfisch.Commons.Editor
{
    public class GroupedPropertiesShaderGUI : ShaderGUI
    {
        #region Private Fields
        private Dictionary<string, MaterialPropertyGroup> m_materialPropertyGroups;
        #endregion

        #region Unity Callbacks
        public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
        {
            if (m_materialPropertyGroups == null)
            {
                InitializeMaterialPropertyGroups(properties);
            }

            foreach (MaterialPropertyGroup group in m_materialPropertyGroups.Values)
            {
                group.DrawPropertiesInGroup(materialEditor, properties);
            }
        }
        #endregion

        #region Private Methods
        private string GetGroupName(MaterialProperty property)
        {
            return property.displayName.Substring(0, property.displayName.IndexOf(' '));
        }

        private void InitializeMaterialPropertyGroups(MaterialProperty[] properties)
        {
            // We perform a grouping of the properties based on the first word of each property's display name.
            //
            // Example: The property with the display name "Blade Texture" would go into the group named "Blade".
            m_materialPropertyGroups = new Dictionary<string, MaterialPropertyGroup>();

            for (int propertyIndex = 0; propertyIndex < properties.Length; propertyIndex++)
            {
                MaterialProperty property = properties[propertyIndex];
                string groupName = GetGroupName(properties[propertyIndex]);

                if (!m_materialPropertyGroups.ContainsKey(groupName))
                {
                    m_materialPropertyGroups[groupName] = new MaterialPropertyGroup(groupName);
                }
                // We do not add the property itself to the group. Instead we add the property index of the property
                // inside the properties array.
                //
                // You might ask why... Well, it turns out that if we add the property directly, any undo/redo actions
                // performed in the editor do not reach those directly added properties for some reason. Even tho
                // MaterialProperty is a reference type.
                //
                // It seems like the properties passed into OnGUI are newly created before every OnGUI call?! 
                //
                // This does mean that we need to rely on the properties array's order not changing from one OnGUI call to the next.
                //
                // We could add the properties directly if we call InitializeMaterialPropertyGroups every time OnGUI is called,
                // but that seems kind of wasteful to me.

                // The HideInInspector attribute doesn't apply if a custom shader GUI is used.
                // So we need to check if the attribute is present ourselves and if it is, we
                // simply ignore the property it is attached to.
                if ((property.flags & MaterialProperty.PropFlags.HideInInspector) != 0)
                {
                    continue;
                }
                m_materialPropertyGroups[groupName].AddPropertyByIndexReference(propertyIndex);
            }
        }
        #endregion

        #region Private Structs, Classes and Enums
        private class MaterialPropertyGroup
        {
            #region Public Properties
            public string GroupName => m_groupName;
            public int Size => m_propertyIndexReferences.Count;
            #endregion

            #region Private Fields
            private readonly string m_groupName;
            private bool m_expanded;
            private readonly List<int> m_propertyIndexReferences;
            #endregion

            #region Public Methods
            public MaterialPropertyGroup(string groupName)
            {
                m_groupName = groupName;
                m_expanded = true;
                m_propertyIndexReferences = new List<int>();
            }

            public int GetPropertyIndexReference(int index)
            {
                return m_propertyIndexReferences[index];
            }

            public void AddPropertyByIndexReference(int propertyIndex)
            {
                m_propertyIndexReferences.Add(propertyIndex);
            }

            public void DrawPropertiesInGroup(MaterialEditor materialEditor, MaterialProperty[] properties)
            {
                if (m_expanded = EditorGUILayout.BeginFoldoutHeaderGroup(m_expanded, m_groupName))
                {
                    foreach (int propertyIndex in m_propertyIndexReferences)
                    {
                        MaterialProperty property = properties[propertyIndex];
                        materialEditor.ShaderProperty(properties[propertyIndex], GetPropertyName(property));
                    }
                }
                EditorGUILayout.EndFoldoutHeaderGroup();
            }
            #endregion

            #region Private Methods
            private string GetPropertyName(MaterialProperty property)
            {
                string propertyName = property.displayName;

                if (propertyName.StartsWith(GroupName))
                {
                    propertyName = propertyName.Remove(0, m_groupName.Length).TrimStart();
                }
                return propertyName;
            }
            #endregion
        }
        #endregion
    }
}