using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GameAttempt1.Control
{
    public class TextureManager
    {
        private Dictionary<string, Texture2D> _textureDictionary;
        private ContentManager _content;
        public TextureManager(ContentManager content)
        {
            _textureDictionary = new Dictionary<string, Texture2D>();
            _content = content;
        }

        public Texture2D this[string name]
        {
            get => _textureDictionary[name];
            set { if (!_textureDictionary.ContainsKey(name)) { _textureDictionary[name] = value; } }
        }

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

        public void UnloadTexture(string textureName)
        {
            if (_textureDictionary.ContainsKey(textureName))
            {
                var texture = _textureDictionary[textureName];
                texture.Dispose();
                _textureDictionary.Remove(textureName);
            }
        }
    }
}
