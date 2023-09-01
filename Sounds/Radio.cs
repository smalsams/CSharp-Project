using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using static SamSer.Utilities.GameUtilities;

namespace SamSer.Sounds
{
    public class Radio : IDictionary<string, SoundEffect>
    {
        private readonly ContentManager _contentManager;

        public Radio(ContentManager content)
        {
            _contentManager = content;
        }
        public Dictionary<string, SoundEffect> SoundEffects = new Dictionary<string, SoundEffect>();

        public int Count => SoundEffects.Count;

        public bool IsReadOnly => false;

        public ICollection<string> Keys => SoundEffects.Keys;

        public ICollection<SoundEffect> Values => SoundEffects.Values;

        public SoundEffect this[string key] 
        {
            get 
            {
                if (!ContainsKey(key))
                {
                    throw new KeyNotFoundException();
                }
                return SoundEffects[key];
            }
            set 
            {
                if (!ContainsKey(key))
                {
                    this[key] = value;
                }
            } 
        }

        public void Add(string itemName)
        {
            var item = _contentManager.Load<SoundEffect>($"{DIR_PATH_RELATIVE}{itemName}");
            SoundEffects.Add(itemName, item);
        }
        public void Add(SoundEffect item)
        {
            SoundEffects.Add(nameof(item),item);
        }

        public bool Contains(SoundEffect item)
        {
            return SoundEffects.ContainsValue(item);
        }
        public bool ContainsKey(string key)
        {
            return SoundEffects.ContainsKey(key);
        }



        IEnumerator IEnumerable.GetEnumerator()
        {
            return SoundEffects.GetEnumerator();
        }

        public void Add(string key, SoundEffect value)
        {
            SoundEffects.Add(key, value);
        }

        public bool Remove(string key)
        {
            return SoundEffects.Remove(key);
        }

        public bool TryGetValue(string key, [MaybeNullWhen(false)] out SoundEffect value)
        {
            return SoundEffects.TryGetValue(key, out value);
        }

        public void Add(KeyValuePair<string, SoundEffect> item)
        {
            SoundEffects.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            SoundEffects.Clear();
        }

        public bool Contains(KeyValuePair<string, SoundEffect> item)
        {
            return (SoundEffects.ContainsKey(item.Key) && SoundEffects[item.Key] == item.Value);
        }

        public void CopyTo(KeyValuePair<string, SoundEffect>[] array, int arrayIndex)
        {
            throw new System.NotImplementedException();
        }

        public bool Remove(KeyValuePair<string, SoundEffect> item)
        {
            throw new System.NotImplementedException();
        }

        IEnumerator<KeyValuePair<string, SoundEffect>> IEnumerable<KeyValuePair<string, SoundEffect>>.GetEnumerator()
        {
            throw new System.NotImplementedException();
        }
    }
}
