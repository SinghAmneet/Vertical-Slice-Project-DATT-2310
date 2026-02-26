using System;

// Enums for food stats
public enum Stats
{
    Sweet,
    Salty,
    Sour,
    Bitter,
    Spicy,
}

[Serializable] // make the class visible in scripts and inspector
public class Stat
{
    public Stats stat;
    public int value;
}
