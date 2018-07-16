namespace GraphQlGenSharp.Parsers.Models
{
    public enum ScalarType
    {
        [NameAliases("Int")]
        IntType,
        [NameAliases("Float")]
        FloatType,
        [NameAliases("String")]
        StringType,
        [NameAliases("Boolean")]
        BooleanType,
        [NameAliases("ID")]
        IdType,
        [NameAliases("Int!")]
        NullableIntType,
        [NameAliases("Float!")]
        NullableFloatType,
        [NameAliases("String!")]
        NullableStringType,
        [NameAliases("Boolean!")]
        NullableBooleanType,
        [NameAliases("ID!")]
        NullableIdType,
        // Sangria: https://www.howtographql.com/graphql-scala/5-custom_scalars/
        [NameAliases("Long")]
        LongType,
        [NameAliases("BigInt")]
        BigIntType,
        [NameAliases("BigDecimal")]
        BigDecimalType,
        [NameAliases("Long!")]
        NullableLongType,
        [NameAliases("BigInt!")]
        NullableBigIntType,
        [NameAliases("BigDecimal!")]
        NullableBigDecimalType,
        // custom type
        [NameAliases("DateTime")]
        DateTimeType,
        [NameAliases("DateTime!")]
        NullableDateTimeType
    }
}
