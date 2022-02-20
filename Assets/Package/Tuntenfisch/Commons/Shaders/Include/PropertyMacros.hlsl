#ifndef TUNTENFISCH_COMMONS_INCLUDE_PROPERTY_MACROS
#define TUNTENFISCH_COMMONS_INCLUDE_PROPERTY_MACROS

#define PROPERTY_WITH_SEMANTIC(type, name, semantic) \
    type name : semantic; \
    \
    GETTER_SETTER(type, name, name)

#define TWO_PACKED_PROPERTIES_WITH_SEMANTIC(firstType, firstName, secondType, secondName, semantic) \
    float2 firstName ## And ## secondName : semantic; \
    \
    GETTER_SETTER_SWIZZLE(firstType, firstName ## And ## secondName, x, firstName) \
    GETTER_SETTER_SWIZZLE(secondType, firstName ## And ## secondName, y, secondName)

#define THREE_PACKED_PROPERTIES_WITH_SEMANTIC(firstType, firstName, secondType, secondName, thirdType, thirdName, semantic) \
    float3 firstName ## And ## secondName ## And ## thirdName : semantic; \
    \
    GETTER_SETTER_SWIZZLE(firstType, firstName ## And ## secondName ## And ## thirdName, x, firstName) \
    GETTER_SETTER_SWIZZLE(secondType, firstName ## And ## secondName ## And ## thirdName, y, secondName) \
    GETTER_SETTER_SWIZZLE(thirdType, firstName ## And ## secondName ## And ## thirdName, z, thirdName)

#define FOUR_PACKED_PROPERTIES_WITH_SEMANTIC(firstType, firstName, secondType, secondName, thirdType, thirdName, fourthType, fourthName, semantic) \
    float4 firstName ## And ## secondName ## And ## thirdName ## And ## fourthName : semantic; \
    \
    GETTER_SETTER_SWIZZLE(firstType, firstName ## And ## secondName ## And ## thirdName ## And ## fourthName, x, firstName) \
    GETTER_SETTER_SWIZZLE(secondType, firstName ## And ## secondName ## And ## thirdName ## And ## fourthName, y, secondName) \
    GETTER_SETTER_SWIZZLE(thirdType, firstName ## And ## secondName ## And ## thirdName ## And ## fourthName, z, thirdName) \
    GETTER_SETTER_SWIZZLE(fourthType, firstName ## And ## secondName ## And ## thirdName ## And ## fourthName, w, fourthName)



#define ARRAY_PROPERTY(type, name, count) \
    type name[count]; \
    \
    ARRAY_GETTER_SETTER(type, name, count, name)

#define PROPERTY(type, name) \
    type name; \
    \
    GETTER_SETTER(type, name, name)

#define TWO_PACKED_PROPERTIES(firstType, firstName, secondType, secondName) \
    float2 firstName ## And ## secondName; \
    \
    GETTER_SETTER_SWIZZLE(firstType, firstName ## And ## secondName, x, firstName) \
    GETTER_SETTER_SWIZZLE(secondType, firstName ## And ## secondName, y, secondName)

#define THREE_PACKED_PROPERTIES(firstType, firstName, secondType, secondName, thirdType, thirdName) \
    float3 firstName ## And ## secondName ## And ## thirdName; \
    \
    GETTER_SETTER_SWIZZLE(firstType, firstName ## And ## secondName ## And ## thirdName, x, firstName) \
    GETTER_SETTER_SWIZZLE(secondType, firstName ## And ## secondName ## And ## thirdName, y, secondName) \
    GETTER_SETTER_SWIZZLE(thirdType, firstName ## And ## secondName ## And ## thirdName, z, thirdName)

#define FOUR_PACKED_PROPERTIES(firstType, firstName, secondType, secondName, thirdType, thirdName, fourthType, fourthName) \
    float4 firstName ## And ## secondName ## And ## thirdName ## And ## fourthName; \
    \
    GETTER_SETTER_SWIZZLE(firstType, firstName ## And ## secondName ## And ## thirdName ## And ## fourthName, x, firstName) \
    GETTER_SETTER_SWIZZLE(secondType, firstName ## And ## secondName ## And ## thirdName ## And ## fourthName, y, secondName) \
    GETTER_SETTER_SWIZZLE(thirdType, firstName ## And ## secondName ## And ## thirdName ## And ## fourthName, z, thirdName) \
    GETTER_SETTER_SWIZZLE(fourthType, firstName ## And ## secondName ## And ## thirdName ## And ## fourthName, w, fourthName)



#define ARRAY_GETTER_SETTER(fieldType, fieldName, count, getterSetterSuffix) \
    void Get ## getterSetterSuffix(out fieldType values[count]) \
    { \
        values = fieldName; \
    } \
    \
    void Set ## getterSetterSuffix(fieldType values[count]) \
    { \
        fieldName = values; \
    } \
    \
    fieldType Get ## getterSetterSuffix ## Element(int index) \
    { \
        return fieldName[index]; \
    } \
    \
    void Set ## getterSetterSuffix ## Element(int index, fieldType value) \
    { \
        fieldName[index] = value; \
    }

#define GETTER_SETTER(fieldType, fieldName, getterSetterSuffix) \
    fieldType Get ## getterSetterSuffix() \
    { \
        return fieldName; \
    } \
    \
    void Set ## getterSetterSuffix(fieldType value) \
    { \
        fieldName = value; \
    }

#define GETTER_SETTER_SWIZZLE(fieldType, fieldName, swizzle, getterSetterSuffix) \
    fieldType Get ## getterSetterSuffix() \
    { \
        return as ## fieldType(fieldName.swizzle); \
    } \
    \
    void Set ## getterSetterSuffix(fieldType value) \
    { \
        fieldName.swizzle = asfloat(value); \
    }

#endif