namespace SamSer.Entities
{
    /// <summary>
    /// An easily JSON serializable object containing necessary parameters for entity construction
    /// </summary>
    public class EntityInfo
    {
        /// <remarks>Type of the entity, so it is possible to assign entity specific properties</remarks>
        public string Type { get; set; }
        /// <remarks>Entity of type <see cref="Type"/> specific properties.</remarks>
        public string Properties { get; set; }
    }
}
