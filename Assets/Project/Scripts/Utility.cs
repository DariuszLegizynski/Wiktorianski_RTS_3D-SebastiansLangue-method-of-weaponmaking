using System.Collections;

//the explanation is in Sebastians Lagues "unity_create_a_game_series_e09_obstacle_placement"
public static class Utility
{
    public static T[] ShuffleArray<T>(T[] array, int seed)
    {
        System.Random prng = new System.Random(seed);

        for(int i = 0; i < array.Length -1; i++)
        {
            int randomIndex = prng.Next(i, array.Length);
            T tempItem = array[randomIndex];
            array[randomIndex] = array[i];
            array[i] = tempItem;
        }

        return array;
    }
}
