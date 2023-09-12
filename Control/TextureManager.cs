using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace SamSer.Control
{
    /// <summary>
    /// Data structure used for storing textures in a level
    /// </summary>
    public class TextureManager
    {
        private readonly Dictionary<string, Texture2D> _textureDictionary;
        private readonly ContentManager _content;
        public TextureManager(ContentManager content)
        {
            _textureDictionary = new Dictionary<string, Texture2D>();
            _content = content;
        }
        public Texture2D this[string name]
        {
            get => _textureDictionary[name];
            set => _textureDictionary.TryAdd(name, value);
        }
        /// <summary>
        /// Adds the texture name - texture pair into the dictionary
        /// </summary>
        /// <param name="textureName"></param>
        /// <returns></returns>
        public Texture2D LoadTexture(string textureName)
        {
            if (_textureDictionary.ContainsKey(textureName))
            {
                return this[textureName];
            }
            var texture = _content.Load<Texture2D>($"Sprites/{textureName}");
            _textureDictionary.Add(textureName, texture);
            return texture;
        }
    }
}
