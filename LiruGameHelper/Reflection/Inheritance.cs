using System;

namespace LiruGameHelper.Reflection;

public static class Inheritance
{
    #region Level Functions
    public static int Level(Type child, Type parent)
    {
        ArgumentNullException.ThrowIfNull(child);
        ArgumentNullException.ThrowIfNull(parent);

        // Initialise the count.
        int count = 0;

        // Start looping from the child type.
        Type checkType = child;

        // Keep looping until the check type is as far as it can go.
        do
        {
            // If the base type has been found, return the count.
            if (checkType == parent) return count;

            // Increment the count.
            count++;

            // If the check type has no base type, return -1 to signal no inheritance.
            if (checkType.BaseType == null) return -1;

            // Set the check type to its base type.
            checkType = checkType.BaseType;
        }
        while (checkType != typeof(object));

        // If nothing was found, return -1 to signal no inheritance.
        return -1;
    }
    #endregion

    #region Check Functions
    public static bool DerivesFrom(Type child, Type parent) => Level(child, parent) != -1;
    #endregion
}
