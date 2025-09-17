using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using Prm;
using static Prm.RentalList;
using static Prm.PID;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;

internal class PrmSearcher
{
    static void Main(string[] args)
    {
        do
        {
            Console.WriteLine("\n======================================");
            Console.Write("RentalPokemon Searcher\n");

            if (!File.Exists("Config.txt"))
            {
                Console.WriteLine("setting.txtが見つかりませんでした");
                Console.ReadKey();
                return;
            }

            string outputPath = $"RentalPokemon.txt";
            List<RentalList> PokemonList = PokemonData.GetRentalList();

            using (StreamWriter writer = new(outputPath))
            {
                writer.WriteLine("【RentalPokemon List】");

                HashSet<string> targetSet = new();

                Console.WriteLine("---------------");
                writer.WriteLine("---------------");

                while (targetSet.Count < 6)
                {
                    Console.Write($"{targetSet.Count + 1}：");
                    string target = Console.ReadLine()?.Trim();

                    if (string.IsNullOrWhiteSpace(target))
                    {
                        break;
                    }

                    targetSet.Add(target);

                    writer.WriteLine($"{targetSet.Count}：{target}");
                }

                Console.WriteLine("---------------");
                writer.WriteLine("---------------");

                Console.WriteLine("[年, 月, 秒, 時, 分, 秒, VCount, Timer0, 初期SEED]\n");
                writer.WriteLine("[年, 月, 秒, 時, 分, 秒, VCount, Timer0, 初期SEED]\n");

                for (DateTime Dt = PID.InDt; Dt <= PID.EnDt; Dt = Dt.Add(PID.increment))     //初期SEED
                {
                    for (uint Timer0 = PID.InTimer0; Timer0 < PID.EnTimer0 + 1; Timer0++)
                    {
                        PID.Dt = Dt;
                        PID.Timer0 = Timer0;
                        InSeedData SeedData = PID.GenSeed();
                        ulong Seed = SeedData.Seed;
                        int Flag = 0;

                        for (int Count = 0; Count < PID.PIDCount; Count++, Seed = NextSeed(Seed))   //PID
                        {
                            ulong temp = NextSeed(Seed);

                            List<RentalList> SelectedList = new();
                            HashSet<string> SelectedPokemon = new();
                            HashSet<string> SelectedItem = new();

                            for (int i = 0; i < 6; i++, temp = NextSeed(temp))
                            {
                                ulong ExCount = 0;

                                for (int m = 0; m < 132; m++)
                                {
                                    var Entry = PokemonList[m];

                                    if (SelectedPokemon.Contains(Entry.PokemonName) || SelectedItem.Contains(Entry.ItemName))
                                    {
                                        ExCount++;
                                    }
                                }

                                ulong temp1 = temp >> 48;
                                ulong temp2 = temp1 + ((temp >> 32) - (temp1 << 16));
                                ulong temp3 = temp >> 48;
                                if (temp2 > 0xFFFF) temp3++;

                                var RentalIndex = 0;
                                ulong RawIndex = temp3 % (132 - ExCount);

                                while (RawIndex > 0)
                                {
                                    var Entry = PokemonList[RentalIndex];
                                    RentalIndex++;

                                    if (SelectedPokemon.Contains(Entry.PokemonName) || SelectedItem.Contains(Entry.ItemName))
                                    {
                                        continue;
                                    }

                                    RawIndex--;
                                }

                                if (RawIndex == 0)
                                {
                                    var Entry = PokemonList[RentalIndex];

                                    if (SelectedPokemon.Contains(Entry.PokemonName) || SelectedItem.Contains(Entry.ItemName))
                                    {
                                        RentalIndex++;
                                    }
                                }

                                RentalList SelectedIndex = PokemonList[RentalIndex];

                                SelectedList.Add(SelectedIndex);
                                SelectedPokemon.Add(SelectedIndex.PokemonName);
                                SelectedItem.Add(SelectedIndex.ItemName);

                                if (targetSet.Count == 6 && !targetSet.IsSupersetOf(SelectedPokemon))
                                {
                                    break;
                                }
                            }

                            if (targetSet.Count == 0 || SelectedPokemon.IsSupersetOf(targetSet))
                            {
                                Console.WriteLine($"{SeedData.Year}, {SeedData.Month}, {SeedData.Day}, {SeedData.Hour}, {SeedData.Minute}, {SeedData.Second}, 0x{SeedData.VCount:X2}, 0x{SeedData.InTimer0:X4}, 0x{SeedData.Seed:X16}");
                                writer.WriteLine($"{SeedData.Year}, {SeedData.Month}, {SeedData.Day}, {SeedData.Hour}, {SeedData.Minute}, {SeedData.Second}, 0x{SeedData.VCount:X2}, 0x{SeedData.InTimer0:X4}, 0x{SeedData.Seed:X16}");

                                for (int m = 0; m < 6; m++)
                                {
                                    Console.WriteLine($"{m + 1}：{SelectedList[m].PokemonName}@{SelectedList[m].ItemName}");
                                    writer.WriteLine($"{m + 1}：{SelectedList[m].PokemonName}@{SelectedList[m].ItemName}");
                                }

                                ulong TempSeed = Seed;
                                for (int m = 0; m < 24; m++)
                                {
                                    TempSeed = NextSeed(TempSeed);
                                }

                                Console.WriteLine($"Seed：0x{Seed:X16}");
                                writer.WriteLine($"Seed：0x{Seed:X16}");
                                Console.WriteLine($"Count：{Count}");
                                writer.WriteLine($"Count：{Count}");
                                Console.WriteLine($"Current Seed：0x{TempSeed:X16}");
                                writer.WriteLine($"Current Seed：0x{TempSeed:X16}");
                                Console.WriteLine("");
                                writer.WriteLine("");

                                Flag += 1;

                                if (CountFlag > 0 && CountFlag == Flag) return;
                            }
                        }
                    }

                }
                Console.WriteLine("======================================");
            }

        } while (Console.ReadKey().Key == ConsoleKey.R);
    }

    static ulong NextSeed(ulong PWTSeed)
    {
        ulong a = 0x5D588B656C078965;
        ulong b = 0x269EC3;
        ulong result = (a * PWTSeed + b) & 0xFFFFFFFFFFFFFFFF;
        return result;
    }
}


