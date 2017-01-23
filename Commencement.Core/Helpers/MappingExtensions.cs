using FluentNHibernate.Mapping;

namespace Commencement.Core.Helpers
{
    public static class MappingExtensions
    {
        public static PropertyPart StringMaxLength(this PropertyPart propertyPart)
        {
            return propertyPart.CustomType("StringClob");
        }
    }
}
