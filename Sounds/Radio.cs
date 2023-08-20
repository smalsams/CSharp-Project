using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System.Collections;
using System.Collections.Generic;

namespace GameAttempt1.Sounds
{
    public class Radio : ICollection<SoundEffect>
    {
        private ContentManager _contentManager;

        public Radio(ContentManager content)
        {
            content = _contentManager;
        }
        public ICollection<SoundEffect> SoundEffects = new List<SoundEffect>();

        public int Count => SoundEffects.Count;

        public bool IsReadOnly => SoundEffects.IsReadOnly;
        public void Add(string path)
        {
            var item = _contentManager.Load<SoundEffect>(path);
            SoundEffects.Add(item);
        }
        public void Add(SoundEffect item)
        {
            SoundEffects.Add(item);
        }

        public void Clear()
        {
            SoundEffects.Clear();
        }

        public bool Contains(SoundEffect item)
        {
            return SoundEffects.Contains(item);
        }

        public void CopyTo(SoundEffect[] array, int arrayIndex)
        {
            SoundEffects.CopyTo(array, arrayIndex);
        }

        public IEnumerator<SoundEffect> GetEnumerator()
        {
            return SoundEffects.GetEnumerator();
        }

        public bool Remove(SoundEffect item)
        {
            return SoundEffects.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return SoundEffects.GetEnumerator();
        }
    }
}
