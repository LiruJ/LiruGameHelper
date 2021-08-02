using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LiruGameHelperMonoGame.Textures
{
    public static class Split
    {
        #region Splitting Functions
        /// <summary> Splits the given <see cref="Texture2D"/> into an array of textures based on the given <see cref="Point"/> size. </summary>
        /// <param name="spriteSheet"> The <see cref="Texture2D"/> to split. </param>
        /// <param name="spriteDimensions"> The size of each <see cref="Texture2D"/>. </param>
        /// <returns> An array of the split <see cref="Texture2D"/>s. </returns>
        public static Texture2D[] FromSize(Texture2D spriteSheet, Point spriteDimensions)
        {
            //The width and height of the spritesheet in tiles
            int textureWidth = spriteSheet.Width / spriteDimensions.X, textureHeight = spriteSheet.Height / spriteDimensions.Y;

            //The array of tile textures
            Texture2D[] tileTextures = new Texture2D[textureWidth * textureHeight];

            //Goes through the spritesheet and adds each tile to the array
            for (int y = 0; y < textureHeight; y++)
                for (int x = 0; x < textureWidth; x++)
                {
                    //The rectangle of the target tile
                    Rectangle sourceRectangle = new Rectangle(spriteDimensions.X * x, spriteDimensions.Y * y, spriteDimensions.X, spriteDimensions.Y);

                    //Get the data from the target area of the spritesheet
                    Color[] data = new Color[sourceRectangle.Width * sourceRectangle.Height];
                    spriteSheet.GetData(0, sourceRectangle, data, 0, data.Length);

                    //Create a new texture based on the data
                    Texture2D finalTile = new Texture2D(spriteSheet.GraphicsDevice, spriteDimensions.X, spriteDimensions.Y);
                    finalTile.SetData(data);

                    //Add the new texture to the array
                    tileTextures[x + y * textureWidth] = finalTile;
                }

            //Return the final array of tile textures
            return tileTextures;
        }
        #endregion
    }
}
