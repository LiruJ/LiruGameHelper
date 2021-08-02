using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace LiruGameHelperMonoGame.Textures
{
    [Obsolete]
    public class DrawableTexture
    {
        #region Private Fields
        /// <summary> Is <c>true</c> when the internal colour data has changed, and the <see cref="Texture2D"/> needs to be updated. </summary>
        private bool hasChanged = false;

        /// <summary> Holds each pixel of the <see cref="Texture2D"/>. </summary>
        private Color[] colourData;

        /// <summary> Holds the texture that has been calculated from previous changes. </summary>
        private Texture2D calculatedTexture;
        #endregion

        #region Public Properties
        /// <summary>  </summary>
        public Texture2D Texture
        {
            get
            {
                // If the DrawableTexture has changed since it was last needed, calculate again.
                if (hasChanged) recalculateTexture();

                // Return the calculated texture.
                return calculatedTexture;
            }
        }

        public Point Size { get => calculatedTexture.Bounds.Size; }

        public int Width { get => Size.X; }

        public int Height { get => Size.Y; }
        #endregion

        #region Public Constructors
        /// <summary> Creates a new <see cref="DrawableTexture"/> based on the given <paramref name="baseTexture"/>. </summary>
        /// <param name="baseTexture"> The <see cref="Texture2D"/> on which this <see cref="DrawableTexture"/>'s data is based. </param>
        /// <exception cref="ArgumentNullException"> Thrown when the given <paramref name="baseTexture"/> is <c>null</c>. </exception>
        public DrawableTexture(Texture2D baseTexture)
        {
            // If the given texture is null, throw an exception.
            if (baseTexture is null) throw new ArgumentNullException("baseTexture", "Given Texture2D cannot be null.");

            // Set the calculated texture to the given texture.
            calculatedTexture = baseTexture;

            // Get the data from the given texture and set colour data.
            baseTexture.GetData(colourData);
        }

        /// <summary> Creates a copy of the given <see cref="DrawableTexture"/>. </summary>
        /// <param name="drawableTexture"> The <see cref="DrawableTexture"/> to make a copy of. </param>
        /// <exception cref="ArgumentNullException"> Thrown when the given <paramref name="drawableTexture"/> is <c>null</c>. </exception>
        public DrawableTexture(DrawableTexture drawableTexture)
        {
            // If the given drawable texture is null, throw an exception.
            if (drawableTexture is null) throw new ArgumentNullException("drawableTexture", "Given DrawableTexture cannot be null.");

            // Create a new texture using the given drawable texture's data.
            calculatedTexture = new Texture2D(drawableTexture.calculatedTexture.GraphicsDevice, drawableTexture.Width, drawableTexture.Height);

            // Create a copy of the given drawable texture's colour data.
            drawableTexture.colourData.CopyTo(colourData, 0);

            // Copy the hasChanged from the given drawable texture, if it needs to redraw it's best to do it when the texture is needed.
            hasChanged = drawableTexture.hasChanged;
        }

        /// <summary>  </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="data"></param>
        /// <exception cref="ArgumentNullException"> Thrown when the given <paramref name="data"/> or <paramref name="graphicsDevice"/> is <c>null</c>. </exception>
        public DrawableTexture(GraphicsDevice graphicsDevice, int width, int height, Color[] data)
            : this(graphicsDevice, new Point(width, height))
        {
            // If the given data is null, throw an exception.
            if (data is null) throw new ArgumentNullException("data", "Given colour array cannot be null.");

            // Since the base constructor already created the colour data array, just copy the given array over to create a new reference.
            data.CopyTo(colourData, 0);
        }

        /// <summary>  </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public DrawableTexture(GraphicsDevice graphicsDevice, int width, int height) : this(graphicsDevice, new Point(width, height)) { } 

        /// <summary> Creates an empty <see cref="DrawableTexture"/> using the given <paramref name="graphicsDevice"/> and <paramref name="size"/> to create the <see cref="Texture2D"/>. </summary>
        /// <param name="graphicsDevice"> The <see cref="GraphicsDevice"/> on which the <see cref="Texture2D"/> is stored. </param>
        /// <param name="size"> The size of the underlying <see cref="Texture2D"/> in pixels. </param>
        /// <exception cref="ArgumentNullException"> Thrown when the given <paramref name="graphicsDevice"/> is <c>null</c>. </exception>
        public DrawableTexture(GraphicsDevice graphicsDevice, Point size)
        {
            // If the given graphics device is null, throw an exception.
            if (graphicsDevice is null) throw new ArgumentNullException("graphicsDevice", "Given GraphicsDevice cannot be null.");

            // Create an empty texture with the given size.
            calculatedTexture = new Texture2D(graphicsDevice, size.X, size.Y);

            // Initialise the data to be empty.
            colourData = new Color[Size.X * Size.Y];
        }
        #endregion

        #region Calculation Functions
        /// <summary>  </summary>
        private void recalculateTexture()
        {
            // If this function was called but the texture does not need to update, throw an exception.
            if (!hasChanged) throw new Exception("DrawableTexture was not marked to update, yet recalculation function was called.");

            // Set the data of the underlying texture.
            calculatedTexture.SetData(colourData);

            // Set hasChanged to false, as the texture was recalculated.
            hasChanged = false;
        }
        #endregion
        
        #region Drawing Functions
        public void DrawLine(Point start, Point end, Color colour)
        {

        }

        public void DrawCircle(Point centre, double radius, Color colour, bool fill = false)
        {

        }

        public void DrawRectangle(Rectangle rectangle, Color colour, bool fill = false)
        {

        }
        #endregion
    }
}
