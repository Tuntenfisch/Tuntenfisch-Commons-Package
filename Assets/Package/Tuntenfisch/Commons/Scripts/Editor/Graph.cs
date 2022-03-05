using System;
using System.Linq;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace Tuntenfisch.Commons.Editor
{
    public class Graph
    {
        #region Public Properties
        public float this[int valueIndex]
        {
            get
            {
                if (valueIndex >= ValueCount)
                {
                    throw new IndexOutOfRangeException();
                }
                return GetCoordinate(valueIndex).y;
            }

            set
            {
                if (valueIndex >= ValueCount)
                {
                    throw new IndexOutOfRangeException();
                }
                m_values[valueIndex] = value;
            }
        }

        public int ValueCount => m_values.Length;
        #endregion

        #region Private Fields
        private Rect m_range;
        private float[] m_values;
        private int2 m_gridLineCount;
        private Color m_backgroundColor;
        private Color m_contentColor;
        private Color m_handleColor;
        private float2 m_negativeCoordinateLabelSize;
        private float2 m_positiveCoordinateLabelSize;
        private int m_selectedValueIndex;
        private GraphFlags m_flags;
        #endregion

        #region Public Methods
        public Graph(float2 xRange, float2 yRange, int2 gridLineCount, GraphFlags flags, Color graphColor, Color graphContentColor, Color graphHandleColor)
        {
            m_range = new Rect(xRange.x, yRange.x, math.abs(xRange.y - xRange.x), math.abs(yRange.y - yRange.x));
            m_values = Enumerable.Repeat(m_range.yMin, gridLineCount.x).ToArray();
            m_gridLineCount = gridLineCount;
            m_backgroundColor = graphColor;
            m_contentColor = graphContentColor;
            m_handleColor = graphHandleColor;
            m_selectedValueIndex = -1;
            m_flags = flags;
        }

        public Graph(float2 xRange, float2 yRange, int2 gridLineCount, GraphFlags flags) : this(xRange, yRange, gridLineCount, flags, Color.gray, Color.green, Color.white) { }

        public Graph(float2 xRange, float2 yRange, int2 gridLineCount) : this(xRange, yRange, gridLineCount, GraphFlags.None, Color.gray, Color.green, Color.white) { }

        public void OnGUI()
        {
            if (math.any(m_negativeCoordinateLabelSize == 0.0f))
            {
                m_negativeCoordinateLabelSize = EditorStyles.label.CalcSize(new GUIContent("-" + StaticStyle.CoordinateLabelFormatString));
                m_positiveCoordinateLabelSize = EditorStyles.label.CalcSize(new GUIContent(StaticStyle.CoordinateLabelFormatString));
            }
            OnGraphGUI();
        }

        public bool HasFlags(GraphFlags flags)
        {
            return (m_flags & flags) == flags;
        }

        public void SetFlags(GraphFlags flags, bool set)
        {
            if (set)
            {
                m_flags |= flags;
            }
            else
            {
                m_flags &= ~flags;
            }
        }
        #endregion

        #region Private Methods
        private void OnGraphGUI()
        {
            Rect graphGroupRect = UnityEditor.EditorGUILayout.GetControlRect(GUILayout.ExpandWidth(true), GUILayout.Height(StaticStyle.Height));
            Rect graphRect = new Rect(StaticStyle.Padding, (float2)graphGroupRect.size - 2.0f * StaticStyle.Padding);
            Rect graphViewRect = new Rect((float2)graphRect.min + new float2(m_positiveCoordinateLabelSize.x, 0.0f), (float2)graphRect.size - m_positiveCoordinateLabelSize);
            GUI.BeginGroup(graphGroupRect);

            switch (Event.current.type)
            {
                case EventType.Repaint:
                    UnityEditor.Handles.color = m_backgroundColor;
                    DrawHorizontalGridLines(graphViewRect);
                    DrawVerticalGridLines(graphViewRect);
                    UnityEditor.Handles.color = m_contentColor;
                    DrawGraphContents(graphViewRect);
                    break;
            }

            if ((m_flags & GraphFlags.ReadOnly) != GraphFlags.ReadOnly)
            {
                switch (Event.current.type)
                {
                    case EventType.Repaint:
                        DrawGraphHandles(graphViewRect);
                        break;

                    case EventType.MouseDown:
                        OnMouseDown(graphViewRect);
                        break;

                    case EventType.MouseDrag:
                        OnMouseDrag(graphGroupRect, graphViewRect);
                        break;

                    case EventType.MouseUp:
                        OnMouseUp();
                        break;
                }
            }
            GUI.EndGroup();
        }

        private void DrawHorizontalGridLines(Rect graphViewRect)
        {
            float step = m_range.height / (m_gridLineCount.y - 1);

            for (float y = m_range.yMin; y <= m_range.yMax; y += step)
            {
                float2 startGraphCoordinate = TransformCoordinateToGraphCoordinate(graphViewRect, new float2(m_range.xMin, y)) - new float2(StaticStyle.TickLength, 0.0f);
                float2 endGraphCoordinate = TransformCoordinateToGraphCoordinate(graphViewRect, new float2(m_range.xMax, y));
                UnityEditor.Handles.DrawLine(new float3(startGraphCoordinate, 0.0f), new float3(endGraphCoordinate, 0.0f));

                float2 labelSize = y < 0.0f ? m_negativeCoordinateLabelSize : m_positiveCoordinateLabelSize;
                Rect coordinateLabelRect = new Rect(startGraphCoordinate + new float2(-labelSize.x, -0.5f * labelSize.y), labelSize);
                UnityEditor.EditorGUI.LabelField(coordinateLabelRect, y.ToString(StaticStyle.CoordinateLabelFormatString));
            }
        }

        private void DrawVerticalGridLines(Rect graphViewRect)
        {
            float step = m_range.width / (m_gridLineCount.x - 1);

            for (float x = m_range.xMin; x <= m_range.yMax; x += step)
            {
                float2 startGraphCoordinate = TransformCoordinateToGraphCoordinate(graphViewRect, new float2(x, m_range.yMin)) + new float2(0.0f, StaticStyle.TickLength);
                float2 endGraphCoordinate = TransformCoordinateToGraphCoordinate(graphViewRect, new float2(x, m_range.yMax));
                UnityEditor.Handles.DrawLine(new float3(startGraphCoordinate, 0.0f), new float3(endGraphCoordinate, 0.0f));

                float2 labelSize = x < 0.0f ? m_negativeCoordinateLabelSize : m_positiveCoordinateLabelSize;
                Rect coordinateLabelRect = new Rect(startGraphCoordinate + new float2(-0.5f * labelSize.x, 0.0f), labelSize);
                UnityEditor.EditorGUI.LabelField(coordinateLabelRect, x.ToString(StaticStyle.CoordinateLabelFormatString));
            }
        }

        private void DrawGraphHandles(Rect graphViewRect)
        {
            for (int valueIndex = 0; valueIndex < m_values.Length; valueIndex++)
            {
                Rect handleRect = GetHandleRect(graphViewRect, valueIndex);
                GUI.DrawTexture(handleRect, StaticStyle.HandleTexture, ScaleMode.ScaleToFit, true, 0.0f, m_handleColor, 0.0f, 0.0f);
            }
        }

        private void DrawGraphContents(Rect graphViewRect)
        {
            for (int valueIndex = 0; valueIndex < m_values.Length - 1; valueIndex++)
            {
                float2 startGraphCoordinate = GetGraphCoordinate(graphViewRect, valueIndex);
                float2 endGraphCoordinate = GetGraphCoordinate(graphViewRect, valueIndex + 1);
                UnityEditor.Handles.DrawAAPolyLine(new float3(startGraphCoordinate, 0.0f), new float3(endGraphCoordinate, 0.0f));
            }
        }

        private float2 GetCoordinate(int valueIndex)
        {
            return new float2(m_range.xMin + m_range.width * valueIndex / (m_values.Length - 1), m_values[valueIndex]);
        }

        private float2 GetGraphCoordinate(Rect graphViewRect, int valueIndex)
        {
            return TransformCoordinateToGraphCoordinate(graphViewRect, GetCoordinate(valueIndex));
        }

        private Rect GetHandleRect(Rect graphViewRect, int valueIndex)
        {
            float2 graphCoordinate = GetGraphCoordinate(graphViewRect, valueIndex);
            return new Rect(graphCoordinate - 0.5f * StaticStyle.HandleSize, StaticStyle.HandleSize);
        }

        private float2 TransformCoordinateToGraphCoordinate(Rect graphViewRect, float2 coordinate)
        {
            return new float2
            (
                graphViewRect.min.x + (coordinate.x - m_range.xMin) / m_range.width * graphViewRect.width,
                graphViewRect.min.y + (m_range.height - (coordinate.y - m_range.yMin)) / m_range.height * graphViewRect.height
            );
        }

        private float2 TransformGraphCoordinateToCoordinate(Rect graphViewRect, float2 graphCoordinate)
        {
            return new float2
            (
                (graphCoordinate.x - graphViewRect.min.x) * m_range.width / graphViewRect.width + m_range.xMin,
                -(graphCoordinate.y - graphViewRect.min.y) * m_range.height / graphViewRect.height + m_range.height + m_range.yMin
            );
        }

        private bool TryGetValueIndex(Rect graphViewRect, float2 graphCoordinate, out int valueIndex)
        {
            for (valueIndex = 0; valueIndex < m_values.Length; valueIndex++)
            {
                Rect handleRect = GetHandleRect(graphViewRect, valueIndex);

                if (handleRect.Contains(graphCoordinate))
                {
                    return true;
                }
            }
            return false;
        }

        private void OnMouseDown(Rect graphViewRect)
        {
            if (TryGetValueIndex(graphViewRect, Event.current.mousePosition, out int valueIndex))
            {
                m_selectedValueIndex = valueIndex;
                SetFlags(GraphFlags.Dirty, true);
            }
            Event.current.Use();
        }

        private void OnMouseDrag(Rect graphGroupRect, Rect graphViewRect)
        {
            if (m_selectedValueIndex == -1)
            {
                return;
            }
            float2 mousePosition = Event.current.mousePosition;
            float2 coordinate = TransformGraphCoordinateToCoordinate(graphViewRect, Event.current.mousePosition);
            m_values[m_selectedValueIndex] = math.clamp(coordinate.y, m_range.yMin, m_range.yMax);

            if (math.any(mousePosition < 0.0f) || math.any(mousePosition > graphGroupRect.size))
            {
                m_selectedValueIndex = -1;
            }
            Event.current.Use();
        }

        private void OnMouseUp()
        {
            if (m_selectedValueIndex == -1)
            {
                return;
            }
            m_selectedValueIndex = -1;
            Event.current.Use();
        }
        #endregion

        #region Public Structs, Classes and Enums
        public enum GraphFlags
        {
            None = 0,
            ReadOnly = 1,
            Dirty = 2
        }
        #endregion

        #region Public Structs, Classes and Enums
        private static class StaticStyle
        {
            #region Public Fields
            public const float Height = 300.0f;
            public const float TickLength = 5.0f;
            public const string CoordinateLabelFormatString = "0.00";

            public static readonly float2 Padding = new float2(10.0f, 10.0f);
            public static readonly float2 HandleSize = new float2(20.0f, 20.0f);
            public static readonly Texture2D HandleTexture;
            #endregion

            static StaticStyle()
            {
                HandleTexture = UnityEditor.EditorGUIUtility.FindTexture("CrossIcon");
            }
        }
        #endregion
    }
}