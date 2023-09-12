using Microsoft.Xna.Framework;

namespace SamSer.Control
{
    /// <summary>
    /// Objects focusable by the <see cref="Camera"/>
    /// </summary>
    public interface IFocusable
    {
        /// <summary>
        ///
        /// </summary>
        Vector2 Position { get; set; }
    }
}
