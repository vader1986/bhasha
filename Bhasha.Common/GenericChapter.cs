using System;
using System.Linq;
using System.Text.Json.Serialization;

namespace Bhasha.Common
{
    public class GenericChapter : IEntity, IEquatable<GenericChapter>
    {
        /// <summary>
        /// Unique identifier of the chapter.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Guid Id { get; }

        /// <summary>
        /// Level of difficulty of the chapter.
        /// </summary>
        public int Level { get; }

        /// <summary>
        /// Name of the chapter.
        /// </summary>
        public Guid NameId { get; }

        /// <summary>
        /// Description of the chapter.
        /// </summary>
        public Guid DescriptionId { get; }

        /// <summary>
        /// Sequence of pages of the chapter.
        /// </summary>
        public GenericPage[] Pages { get; }

        [JsonConstructor]
        public GenericChapter(int level, Guid nameId, Guid descriptionId, GenericPage[] pages) : this(default, level, nameId, descriptionId, pages) { }

        public GenericChapter(Guid id, int level, Guid nameId, Guid descriptionId, GenericPage[] pages)
        {
            Id = id;
            Level = level;
            NameId = nameId;
            DescriptionId = descriptionId;
            Pages = pages;
        }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Level)}: {Level}, {nameof(NameId)}: {NameId}, {nameof(DescriptionId)}: {DescriptionId}, {nameof(Pages)}: {string.Join('/', Pages?.Select(x => x.ToString()))}";
        }

        public bool Equals(GenericChapter other)
        {
            return other != null && other.Level == Level && other.NameId == NameId && other.DescriptionId == DescriptionId && other.Pages.SequenceEqual(Pages);
        }
    }
}
