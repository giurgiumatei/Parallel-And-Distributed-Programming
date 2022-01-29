var n = 5;
var k = 3;
var array = new int[10];
Back(array, n, k, 1);



void Print(int[] array, int k)
{
    for (int i = 0; i < k; i++)
    {
        Console.Write(array[i] + " ");
    }
    Console.WriteLine();
}


static bool Distinct(int[] elements, int n)
{
    for (int i = 1; i < n; i++)
    {
        if (elements[i] == elements[n])
        {
            return false;
        }
    }

    return true;
}

void Back(int[] array, int n, int k, int i)
{
    for (int j = 1; j <= n; j++)
    {
        array[i] = j;

        if (i == k)
        {
            if (Distinct(array, i))
            {
                Print(array, k);
            }
        }
        else
        {
            Back(array, n, k, i + 1);
        }
    }
}