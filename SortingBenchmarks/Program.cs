using System.Diagnostics;
using static System.Console;

namespace SortingBenchmarks;

internal class Program
{
	static Random ran = new();

	static void Main(string[] args)
	{
		const int highestmagnitude = 10;
		Dictionary<string, Func<int[], int[]>> functionmap = new()
		{
			{ "Bubble Sort", BubbleSort },
			{ "Selection Sort", SelectionSort },
			{ "Insertion Sort", InsertionSort },
			{ "Quick Sort", QuickSort }
		};
		Dictionary<Func<int[], int[]>, Dictionary<long, long>> elapsedtime = new();
		foreach ((string name, Func<int[], int[]> function) in functionmap)
		{
			elapsedtime.Add(function, new());
			for (int i = 0; i <= highestmagnitude; ++i)
			{
				int mag = (int)Math.Pow(10, i);
				ForegroundColor = ConsoleColor.White;
				WriteLine($"Press Enter to begin sorting {mag} elements with {name}, or type 'no' to skip to the next sort.");
				if (ReadLine() == "no") break;
				WriteLine($"Sorting {mag} elements...");
				int[] tosort = Enumerable.Range(0, mag).Select(_ => ran.Next()).ToArray();
				Stopwatch time = new();
				time.Start();
				tosort = function(tosort);
				time.Stop();
				TimeSpan result = time.Elapsed;
				ForegroundColor = ConsoleColor.Cyan;
				WriteLine($"Sorting {mag} elements with {name} took {result.TotalNanoseconds} ns ({result.TotalSeconds} seconds).");
			}
		}
	}

	static int[] QuickSort(int[] sort)
	{
		return sort.Order().ToArray();
	}

	static int[] BubbleSort(int[] sort)
	{
		bool swapped = true;
		int pass = 1;
		while (swapped)
		{
			swapped = false;
			for (int i = 0; i < sort.Length - pass; i++)
				if (sort[i] < sort[i + 1])
				{
					(sort[i], sort[i + 1]) = (sort[i + 1], sort[i]);
					swapped = true;
				}
			pass++;
		}
		return sort;
	}

	static int[] SelectionSort(int[] sort)
	{
		for (int i = 0; i < sort.Length - 1; i++)
		{
			int min = i;
			for (int j = 0; j < sort.Length; j++)
				if (sort[j] < sort[min])
					min = j;
			(sort[min], sort[i]) = (sort[i], sort[min]);
		}
		return sort;
	}

	static int[] InsertionSort(int[] sort)
	{
		int current = 1;
		for (int i = 0; i < sort.Length; i++)
		{
			int data = sort[current];
			int comp = 0;
			bool finish = false;
			while (comp < current && !finish)
			{
				if (data < sort[comp])
				{
					for (int shuffle = current; shuffle > comp; shuffle--)
						sort[shuffle] = sort[shuffle - 1];
					sort[comp] = data;
					finish = true;
				}
			}
		}
		return sort;
	}
}
