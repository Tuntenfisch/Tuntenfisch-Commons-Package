using System;
using System.IO;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Tuntenfisch.Commons.Graphics
{
    public static class Texture2DExtensions
    {
        #region Public Methods
        public static void InsertColumn(this Texture2D texture, int columnIndex, Color color)
        {
            InsertColumn(texture, columnIndex, Enumerable.Repeat(color, texture.height).ToArray());
        }

        public static void InsertColumn(this Texture2D texture, int columnIndex, Color[] colors)
        {
            if (columnIndex < 0 || columnIndex > texture.width)
            {
                throw new IndexOutOfRangeException();
            }
            // In order to insert a column we need to enlarge the texture. This requires reinitializing it and
            // therefore clearing the texture contents, so we need to make sure we read the texture contents
            // before we reinitialize the texture.

            // First we read the block of pixels that comes before the column to be inserted.
            int pixelBlockBeforeColumnWidth = columnIndex;
            Color[] pixelBlockBeforeColumn = texture.GetPixels(0, 0, pixelBlockBeforeColumnWidth, texture.height);
            // Second we read the block of pixels that comes after the column to be inserted.
            int pixelBlockAfterColumnWidth = texture.width - columnIndex;
            Color[] pixelBlockAfterColumn = texture.GetPixels(columnIndex, 0, pixelBlockAfterColumnWidth, texture.height);

            // Enlarge the texture width by 1 by reinitializing it. Texture contents will be undefined after reinitialization.
            texture.Reinitialize(texture.width + 1, texture.height);
            // Write back the first block of pixels.
            texture.SetPixels(0, 0, pixelBlockBeforeColumnWidth, texture.height, pixelBlockBeforeColumn);
            // Initialize the column with the specified color.
            texture.SetPixels(columnIndex, 0, 1, texture.height, colors);
            // Write back the second block of pixels, offsetting it by 1 to account for the newly inserted column.
            texture.SetPixels(columnIndex + 1, 0, pixelBlockAfterColumnWidth, texture.height, pixelBlockAfterColumn);
        }

        public static void RemoveColumn(this Texture2D texture, int columnIndex)
        {
            if (columnIndex < 0 || columnIndex > texture.width - 1)
            {
                throw new IndexOutOfRangeException();

            }
            // This works very similar to inserting a column, but instead we remove one.
            int pixelBlockBeforeColumnWidth = columnIndex;
            Color[] pixelBlockBeforeColumn = texture.GetPixels(0, 0, pixelBlockBeforeColumnWidth, texture.height);
            int pixelBlockAfterColumnWidth = texture.width - columnIndex - 1;
            Color[] pixelBlockAfterColumn = texture.GetPixels(columnIndex + 1, 0, pixelBlockAfterColumnWidth, texture.height);

            texture.Reinitialize(texture.width - 1, texture.height);
            texture.SetPixels(0, 0, pixelBlockBeforeColumnWidth, texture.height, pixelBlockBeforeColumn);
            texture.SetPixels(columnIndex, 0, pixelBlockAfterColumnWidth, texture.height, pixelBlockAfterColumn);
        }

#if UNITY_EDITOR
        public static void SaveAsset(this Texture2D texture, string assetPath, bool refreshAssetDatabase = true)
        {
            string path = Path.GetFullPath(Path.Join(Application.dataPath, "..", assetPath));
            File.WriteAllBytes(path, texture.EncodeToPNG());

            if (refreshAssetDatabase)
            {
                AssetDatabase.Refresh();
            }
        }
#endif
        #endregion
    }
}