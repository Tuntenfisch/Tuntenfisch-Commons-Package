#ifndef TUNTENFISCH_COMMONS_INCLUDE_RANDOM
#define TUNTENFISCH_COMMONS_INCLUDE_RANDOM

// PRNG taken from on https://gamedev.stackexchange.com/questions/32681/random-number-hlsl.
#define RANDOM_IA 16807
#define RANDOM_IM 2147483647
#define RANDOM_AM (1.0f / RANDOM_IM)
#define RANDOM_IQ 127773u
#define RANDOM_IR 2836
#define RANDOM_MASK 123459876

struct PRNG
{
    int seed;

    // Returns a random float.
    float NextFloat()
    {
        Cycle();
        return RANDOM_AM * seed;
    }

    // Returns a random float within the input range.
    float NextFloat(float low, float high)
    {
        float value = NextFloat();
        return low * (1.0f - value) + high * value;
    }

    float NextFloat(float2 range)
    {
        return NextFloat(range.x, range.y);
    }

    // Returns a random int.
    int NextInt()
    {
        Cycle();
        return seed;
    }

    // Generates the next number in the sequence.
    void Cycle()
    {
        seed ^= RANDOM_MASK;
        int k = seed / RANDOM_IQ;
        seed = RANDOM_IA * (seed - k * RANDOM_IQ) - RANDOM_IR * k;
        seed = seed < 0 ? seed + RANDOM_IM : seed;
        seed ^= RANDOM_MASK;
    }

    // Cycles the generator based on the input count. Useful for generating a thread unique seed.
    void Cycle(int count)
    {
        for (int index = 0; index < count; index++)
        {
            Cycle();
        }
    }

    static PRNG Create(int seed)
    {
        PRNG prng;
        prng.seed = seed;

        return prng;
    }
};

#endif